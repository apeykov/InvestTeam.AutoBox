namespace InvestTeam.AutoBox.Domain.Attributes
{
    using InvestTeam.AutoBox.Domain.Comparers;
    using System;

    /// <summary>
    /// Used in Get for filtering
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class FilterAttribute : Attribute
    {
        private readonly ExpressionComparer expressionComparer = ExpressionComparer.Equals;

        public FilterAttribute()
        {
        }

        public FilterAttribute(string entityFieldName)
        {
            EntityFieldName = entityFieldName;
        }

        public FilterAttribute(string entityFieldName, ExpressionComparer expressionComparer) : this(entityFieldName)
        {
            this.expressionComparer = expressionComparer;
        }

        public string EntityFieldName { get; private set; }

        public ExpressionComparer ExpressionComparer
        {
            get
            {
                return this.expressionComparer;
            }
        }
    }
}
