namespace InvestTeam.AutoBox.Application.Common
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Text;

    public abstract class CommonResult
    {
        protected IList<Exception> exceptions;
        protected IList<string> warnings;
        protected IList<string> infos;

        protected CommonResult()
        {
            exceptions = new List<Exception>();
            warnings = new List<string>();
            infos = new List<string>();
        }

        /// <summary>
        /// Collects Warning messages (part of the `ResultOutput` property)
        /// </summary>
        public virtual IReadOnlyList<string> Warnings
        {
            get
            {
                return new ReadOnlyCollection<string>(warnings);
            }
        }

        /// <summary>
        /// Collects Infomation messages (part of the `ResultOutput` property)
        /// </summary>
        public virtual IReadOnlyList<string> Infos
        {
            get
            {
                return new ReadOnlyCollection<string>(infos);
            }
        }

        /// <summary>
        /// Collection of exceptions (if any), occured during the operation.
        /// </summary>
        public virtual IReadOnlyCollection<Exception> Exceptions
        {
            get
            {
                return new ReadOnlyCollection<Exception>(exceptions);
            }
        }

        /// <summary>
        /// Flag indicating existence of errors during the operation
        /// </summary>
        public virtual bool HasErrors
        {
            get
            {
                return exceptions.Count > 0;
            }
        }

        public virtual string ResultOutput
        {
            get
            {
                var resultOutput = new StringBuilder("[Result Output]");

                if (exceptions.Count > 0)
                {
                    resultOutput.AppendLine();
                    resultOutput.Append("[Exceptions]");
                    resultOutput.AppendLine();

                    foreach (var ex in exceptions)
                    {
                        resultOutput.Append(ex.ToString());
                    }
                }

                AddToOutput(resultOutput, warnings, "[Warnings]");
                AddToOutput(resultOutput, infos, "[Information]");

                return resultOutput.ToString();
            }
        }

        private void AddToOutput(StringBuilder resultOutput, IList<string> messages, string header)
        {
            if (messages.Count > 0)
            {
                resultOutput.AppendLine();
                resultOutput.Append(header);
                resultOutput.AppendLine();
                resultOutput.Append(string.Join("\n", messages));
            }
        }

        public virtual void AddWarning(string warning)
        {
            warnings.Add(warning);
        }

        public virtual void AddInfo(string info)
        {
            infos.Add(info);
        }
    }
}
