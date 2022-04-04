namespace InvestTeam.AutoBox.Application.AppServices
{
    using InvestTeam.AutoBox.Application.Common.Expressions;
    using InvestTeam.AutoBox.Domain.Attributes;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    /// <summary>    
    /// 
    /// </summary>
    public class ExpressionService<TEntity, TEntityDTO>
    {
        private readonly TEntityDTO dto;

        public ExpressionService(TEntityDTO dto)
        {
            this.dto = dto;
        }

        public virtual Expression<Func<TEntity, bool>> GetFilter()
        {
            List<Filter> subFilters = new List<Filter>();

            var type = typeof(TEntityDTO);
            var dtoPropertiesWithFilterAttributeApplied = type
                    .GetProperties()
                    .Where(prop => prop.IsDefined(typeof(FilterAttribute), true));

            foreach (PropertyInfo dtoFilterProperty in dtoPropertiesWithFilterAttributeApplied)
            {
                var fieldValue = dtoFilterProperty.GetValue(dto);

                if (fieldValue is null)
                {
                    continue;
                }

                var fieldName = dtoFilterProperty.Name;
                var filterAttribute = (FilterAttribute)dtoFilterProperty.GetCustomAttributes(true)[0];

                if (!string.IsNullOrEmpty(filterAttribute.EntityFieldName))
                {
                    fieldName = filterAttribute.EntityFieldName;
                }

                if (fieldValue is string)
                {
                    subFilters.Add(ExpressionBuilder.GetComparer(fieldName, (string)fieldValue));
                }
                else if (fieldValue is ICollection<string>)
                {
                    foreach (var filedSubValue in (fieldValue as ICollection<string>))
                    {
                        subFilters.Add(ExpressionBuilder.GetComparer(fieldName, filedSubValue));
                    }

                }
                else if (fieldValue is Enum || fieldValue is int)
                {
                    Filter numericFilter = ExpressionBuilder.GetComparer(fieldName,
                        Convert.ToInt32(fieldValue),
                        filterAttribute.ExpressionComparer);

                    subFilters.Add(numericFilter);
                }
                else if (fieldValue is bool)
                {
                    subFilters.Add(ExpressionBuilder.GetComparer(fieldName, (bool)fieldValue));
                }
                else if (fieldValue is DateTime)
                {
                    Filter dateTimeFilter = ExpressionBuilder.GetComparer(fieldName,
                        (DateTime)fieldValue,
                        filterAttribute.ExpressionComparer);

                    subFilters.Add(dateTimeFilter);
                }
                else
                {
                    throw new NotImplementedException($"Target DTO type { typeof(TEntityDTO) }. " +
                        $"DTO property name `{ fieldName }`: " +
                        $"property value type `{ fieldValue.GetType()  }` is NOT supported for use as expression filter.");
                }
            }

            var queryFilter = ExpressionBuilder.GetExpression<TEntity>(subFilters);

            return queryFilter;
        }
    }
}
