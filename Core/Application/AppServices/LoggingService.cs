namespace InvestTeam.AutoBox.Application.AppServices
{
    using InvestTeam.AutoBox.Application.Common;
    using InvestTeam.AutoBox.Application.Common.Interfaces;
    using InvestTeam.AutoBox.Application.Data.DTOs;
    using InvestTeam.AutoBox.Domain.Common;
    using InvestTeam.AutoBox.Domain.Entities;
    using InvestTeam.AutoBox.Domain.Enums;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    public class LoggingService
    {
        private const long MaxLogFileSizeInBytes = 10000000;

        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;
        private readonly IDateTime dateTimeService;
        private readonly IRunId runIdService;
        private readonly ISource sourceService;

        public LoggingService(IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,
            IDateTime dateTimeService,
            IRunId runIdService,
            ISource sourceService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
            this.dateTimeService = dateTimeService;
            this.runIdService = runIdService;
            this.sourceService = sourceService;
        }

        public Guid RunId
        {
            get
            {
                return runIdService.RunId;
            }
        }

        private void LogDetails<TEntity>(History logActivity,
            OperationResult<TEntity> loggedOperationResult)
        {
            IDataRepository<HistoryDetails> logDetails = unitOfWork.Repository<HistoryDetails>();
            string information = string.Empty;
            string warning = string.Empty;
            string newLine = char.ConvertFromUtf32(13) + char.ConvertFromUtf32(10);

            if (loggedOperationResult.Infos.Any())
            {
                information = string.Join(newLine, loggedOperationResult.Infos);
            }

            if (loggedOperationResult.Warnings.Any())
            {
                warning = string.Join(newLine, loggedOperationResult.Warnings);
            }

            if (!string.IsNullOrWhiteSpace(information) || !string.IsNullOrWhiteSpace(warning))
            {
                logDetails.Add(new HistoryDetails()
                {
                    History = logActivity,
                    Information = information,
                    Warning = warning
                });
            }
        }

        private async Task Done(LogResult logResult)
        {
            try
            {
                await unitOfWork.DoneAsync();
            }
            catch (AggregateException ex)
            {
                logResult.AddException(ex);

                foreach (var innerEx in ex.InnerExceptions)
                {
                    await LogToFile(innerEx);
                }
            }
            catch (Exception ex)
            {
                await LogToFile(ex);

                logResult.AddException(ex);
            }
        }

        private async Task LogToFile(Exception ex)
        {
            string logFilePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\AutoBox.log";
            string logEntry = $"[{ dateTimeService.Now }][{ currentUserService.UserId }]\n{ ex }\n\n";
            long logFileLength = 0;

            if (File.Exists(logFilePath))
            {
                logFileLength = new FileInfo(logFilePath).Length;
            }

            if (logFileLength > MaxLogFileSizeInBytes)
            {
                await File.WriteAllTextAsync(logFilePath, logEntry);

                return;
            }

            await File.AppendAllTextAsync(logFilePath, logEntry);
        }

        /// <summary>
        /// This method is meant for logging REST Web API operation related to one operation target object
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="logEntry">Data container for the Web API request</param>
        /// <param name="restOperationResult">Result output related to the logged operation</param>        
        public async Task<LogResult> RestLog<TEntity>(WebApiLogEntryDTO logEntry,
            OperationResult<TEntity> restOperationResult)
          where TEntity : BaseEntity
        {
            var logResult = new LogResult();

            this.unitOfWork.Clear();

            IDataRepository<RESTLog> restLogs = unitOfWork.Repository<RESTLog>();

            if (restOperationResult.Entities.Count == 0)
            {
                //throw new InvalidTargetOperationException(logEntry.Endpoint);
            }

            foreach (var restEntity in restOperationResult.Entities)
            {
                ICollection<Error> restEntityErrors = restOperationResult.GetEntityErrors(restEntity);
                bool thisEntityHasErrors = restEntityErrors.Count > 0;

                History logActivity = new History()
                {
                    Commandlet = logEntry.Endpoint,
                    Parameters = logEntry.RequestParams,

                    RunID = runIdService.RunId,
                    User = currentUserService.UserId,
                    Time = dateTimeService.Now,
                    Source = sourceService.Source,

                    Result = thisEntityHasErrors ? ServiceResult.Failed : ServiceResult.Success,
                    Errors = restEntityErrors
                };

                RESTLog restLog = new RESTLog()
                {
                    Request = logEntry.Request,
                    Response = logEntry.Response,
                    Type = logEntry.HttpVerb,
                    History = logActivity
                };
                restLogs.Add(restLog);

                LogDetails(logActivity, restOperationResult);
            }

            await Done(logResult);

            foreach (var ex in restOperationResult.Exceptions)
            {
                logResult.AddException(ex);
            }

            return logResult;
        }
    }
}