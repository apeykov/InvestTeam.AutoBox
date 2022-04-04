namespace InvestTeam.AutoBox.Application.Common
{
    using System.Collections.Generic;

    public class QueryOperationResult<TItemResult> : CommonResult
    {
        /// <summary>
        /// Results of a query against the data store
        /// </summary>
        public IList<TItemResult> Entities { get; set; }
    }
}
