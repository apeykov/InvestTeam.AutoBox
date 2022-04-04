namespace InvestTeam.AutoBox.Application.Common
{
    using InvestTeam.AutoBox.Domain.Entities;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    /// <summary>
    /// This class serves as an output from the Services and is designed for:
    ///     1) UI Post-Operation Report
    ///     2) UI Post-Operation Error Report
    ///     3) Logging
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    public class OperationResult<TResult> : CommonResult
    {
        protected readonly Dictionary<TResult, IList<Exception>> exceptionsMap = new Dictionary<TResult, IList<Exception>>();
        protected List<TResult> entities;

        public OperationResult()
        {
            entities = new List<TResult>();
        }

        public OperationResult(IList<TResult> targetEntities)
        {
            entities = new List<TResult>(targetEntities);
        }

        public OperationResult(TResult targetEntity)
        {
            entities = new List<TResult>() { targetEntity };
        }

        public OperationResult(params OperationResult<TResult>[] subResults) : this()
        {
            foreach (var subResult in subResults)
            {
                entities = entities.Union(subResult.entities).ToList();

                foreach (var subResultException in subResult.Exceptions)
                {
                    var relatedEntity = subResult.exceptionsMap
                        .First(entityToExceptionsMap => entityToExceptionsMap.Value.Contains(subResultException))
                        .Key;

                    this.AddException(subResultException, relatedEntity);
                }

                warnings = warnings.Union(subResult.warnings).ToList();
                infos = infos.Union(subResult.infos).ToList();
            }
        }

        public OperationResult(IEqualityComparer<TResult> comparer, params OperationResult<TResult>[] subResults) : this()
        {
            foreach (var subResult in subResults)
            {
                entities = entities.Union(subResult.entities, comparer).ToList();

                foreach (var subResultException in subResult.Exceptions)
                {
                    var relatedEntity = subResult.exceptionsMap
                        .First(entityToExceptionsMap => entityToExceptionsMap.Value.Contains(subResultException))
                        .Key;

                    this.AddException(subResultException, relatedEntity);
                }

                warnings = warnings.Union(subResult.warnings).ToList();
                infos = infos.Union(subResult.infos).ToList();
            }
        }

        /// <summary>
        /// Operation target entities
        /// </summary>
        public virtual IReadOnlyList<TResult> Entities
        {
            get
            {
                return new ReadOnlyCollection<TResult>(entities);
            }
        }

        /// <summary>
        /// The real number of all affected entities into the data store, after operation exec
        /// (including log/service related)
        /// </summary>
        public virtual int Affected { get; set; }

        public IReadOnlyList<TResult> ErrFreeEntities
        {
            get
            {
                IList<TResult> errFreeEntities = entities.Where(e => !exceptionsMap.ContainsKey(e)).ToList();

                return new ReadOnlyCollection<TResult>(errFreeEntities);
            }
        }

        /// <summary>
        /// Add a Map entry: entity -> List of Exceptions
        /// </summary>
        /// <param name="entity">Key for a Map</param>
        /// <param name="ex">One exception in the List of Exceptions (value in the Map)</param>       
        public virtual void AddException(Exception operationException, TResult operationTargetEntity)
        {
            if (operationTargetEntity == null)
            {
                throw new ArgumentNullException("Operation Target Object is NULL and can't be mapped to exception.");
            }

            if (operationException == null)
            {
                throw new ArgumentNullException("Invalid Operation Exception (NULL)");
            }

            exceptions.Add(operationException);

            if (!entities.Contains(operationTargetEntity))
            {
                entities.Add(operationTargetEntity);
            }

            if (exceptionsMap.ContainsKey(operationTargetEntity))
            {
                exceptionsMap[operationTargetEntity].Add(operationException);
            }
            else
            {
                exceptionsMap.Add(operationTargetEntity, new List<Exception>() { operationException });
            }
        }


        internal void AddError(string message, TResult operationTargetEntity)
        {
            AddException(new Exception(message), operationTargetEntity);
        }

        public virtual ICollection<Error> GetEntityErrors(TResult entity)
        {
            if (!exceptionsMap.ContainsKey(entity))
            {
                return new List<Error>() { };
            }

            var entityErrors = new List<Error>();

            IList<Exception> entityExceptions = exceptionsMap[entity];

            foreach (var ex in entityExceptions)
            {
                var error = new Error()
                {
                    ErrorText = ex.ToString(),
                    ErrorType = ex.GetType().ToString(),
                };

                entityErrors.Add(error);
            }

            return entityErrors;
        }
    }
}