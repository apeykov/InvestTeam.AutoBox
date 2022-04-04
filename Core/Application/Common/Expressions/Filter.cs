namespace InvestTeam.AutoBox.Application.Common.Expressions
{
    using InvestTeam.AutoBox.Domain.Comparers;

    /// <summary>
    /// 
    /// </summary>
    public class Filter
    {
        public string PropertyName { get; set; }

        public ExpressionComparer Operation { get; set; }

        public object Value { get; set; }

        /// <summary>
        /// Answers the question if this filter is applied to a navigation property in the context of EF Core
        /// https://docs.microsoft.com/en-us/ef/core/modeling/relationships?tabs=fluent-api%2Cfluent-api-simple-key%2Csimple-key#conventions
        /// </summary>
        /// <returns></returns>
        public bool IsForNavigationProperty()
        {
            return PropertyName.Contains(".");
        }

        /// <summary>
        /// Example: NumberRanges.Identity => ['NumberRanges', 'Identity']
        /// </summary>
        /// <returns></returns>
        public string[] GetPropertyAccessorSegments()
        {
            return PropertyName.Split('.');
        }
    }
}
