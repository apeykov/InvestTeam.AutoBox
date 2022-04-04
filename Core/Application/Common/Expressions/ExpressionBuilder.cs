namespace InvestTeam.AutoBox.Application.Common.Expressions
{
    using InvestTeam.AutoBox.Domain.Comparers;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    /// <summary>
    /// 
    /// </summary>
    internal static class ExpressionBuilder
    {
        private static MethodInfo containsMethod = typeof(string).GetMethod("Contains", new Type[] { typeof(string) });
        private static MethodInfo startsWithMethod = typeof(string).GetMethod("StartsWith", new Type[] { typeof(string) });
        private static MethodInfo endsWithMethod = typeof(string).GetMethod("EndsWith", new Type[] { typeof(string) });

        public static Expression<Func<T, bool>> GetExpression<T>(IList<Filter> filters)
        {
            if (filters.Count == 0)
                return e => true;

            ParameterExpression param = Expression.Parameter(typeof(T), "t");
            Expression exp = null;

            if (filters.Count == 1)
                exp = GetExpression<T>(param, filters[0]);
            else if (filters.Count == 2)
                exp = GetExpression<T>(param, filters[0], filters[1]);
            else
            {
                while (filters.Count > 0)
                {
                    var f1 = filters[0];
                    var f2 = filters[1];

                    if (exp == null)
                        exp = GetExpression<T>(param, filters[0], filters[1]);
                    else
                        exp = Expression.AndAlso(exp, GetExpression<T>(param, filters[0], filters[1]));

                    filters.Remove(f1);
                    filters.Remove(f2);

                    if (filters.Count == 1)
                    {
                        exp = Expression.AndAlso(exp, GetExpression<T>(param, filters[0]));
                        filters.RemoveAt(0);
                    }
                }
            }

            return Expression.Lambda<Func<T, bool>>(exp, param);
        }

        private static Expression GetExpression<T>(ParameterExpression expressionParameter, Filter filter)
        {
            Expression entityMember;

            if (filter.IsForNavigationProperty())
            {
                Expression expressionBody = expressionParameter;

                foreach (var propAccessor in filter.GetPropertyAccessorSegments())
                {
                    var isCollection = typeof(IEnumerable).IsAssignableFrom(expressionBody.Type);

                    if (isCollection)
                    {
                        Expression rootCollection = expressionBody;

                        Type collectionEntityType = rootCollection.Type.GetGenericArguments()[0];
                        MethodInfo anyMethod = typeof(Enumerable)
                            .GetMethods()
                            .Single(m => m.Name == "Any" && m.GetParameters().Length == 2)
                            .MakeGenericMethod(collectionEntityType);

                        ParameterExpression lambdaParamForAny = Expression.Parameter(collectionEntityType, "x");
                        var subFilter = new Filter()
                        {
                            PropertyName = propAccessor,
                            Operation = filter.Operation,
                            Value = filter.Value
                        };

                        var internalLambdaPropAccessor = Expression.PropertyOrField(lambdaParamForAny, propAccessor);

                        var internalLambdaBody = GetExpression(filter, internalLambdaPropAccessor);

                        var internalLambda = MakeLambda(lambdaParamForAny, internalLambdaBody);

                        MethodCallExpression filterAsExpression = Expression.Call(anyMethod, rootCollection, internalLambda);

                        return filterAsExpression;
                    }
                    else
                    {
                        expressionBody = Expression.PropertyOrField(expressionBody, propAccessor);
                    }
                }

                entityMember = expressionBody;
            }
            else
            {
                entityMember = Expression.Property(expressionParameter, filter.PropertyName);
            }

            return GetExpression(filter, entityMember);
        }

        private static Expression GetExpression(Filter filter, Expression entityMember)
        {
            UnaryExpression constant = null;

            if (entityMember.Type == typeof(Decimal?))
            {
                constant = Expression.Convert(Expression.Constant(Decimal.Parse(filter.Value.ToString())), entityMember.Type);
            }
            else if (entityMember.Type == typeof(DateTime?))
            {
                constant = Expression.Convert(Expression.Constant(DateTime.Parse(filter.Value.ToString())), entityMember.Type);
            }
            else if (entityMember.Type == typeof(Int32?))
            {
                constant = Expression.Convert(Expression.Constant(Int32.Parse(filter.Value.ToString())), entityMember.Type);
            }
            else if (entityMember.Type == typeof(bool?))
            {
                constant = Expression.Convert(Expression.Constant(bool.Parse(filter.Value.ToString())), entityMember.Type);
            }
            else
            {
                constant = Expression.Convert(Expression.Constant(filter.Value), entityMember.Type);
            }

            switch (filter.Operation)
            {
                case ExpressionComparer.Equals:
                    return Expression.Equal(entityMember, constant);

                case ExpressionComparer.GreaterThan:
                    return Expression.GreaterThan(entityMember, constant);

                case ExpressionComparer.GreaterThanOrEqual:
                    return Expression.GreaterThanOrEqual(entityMember, constant);

                case ExpressionComparer.LessThan:
                    return Expression.LessThan(entityMember, constant);

                case ExpressionComparer.LessThanOrEqual:
                    return Expression.LessThanOrEqual(entityMember, constant);

                case ExpressionComparer.Contains:
                    return Expression.Call(entityMember, containsMethod, constant);

                case ExpressionComparer.StartsWith:
                    return Expression.Call(entityMember, startsWithMethod, constant);

                case ExpressionComparer.EndsWith:
                    return Expression.Call(entityMember, endsWithMethod, constant);

                case ExpressionComparer.And:
                    return Expression.And(entityMember, constant);
            }

            return null;
        }

        private static Expression MakeLambda(Expression parameter, Expression predicate)
        {
            var resultParameterVisitor = new ParameterVisitor();
            resultParameterVisitor.Visit(parameter);
            var resultParameter = resultParameterVisitor.Parameter;

            return Expression.Lambda(predicate, (ParameterExpression)resultParameter);
        }

        private static BinaryExpression GetExpression<T>(ParameterExpression param, Filter filter1, Filter filter2)
        {
            Expression bin1 = GetExpression<T>(param, filter1);
            Expression bin2 = GetExpression<T>(param, filter2);

            return Expression.AndAlso(bin1, bin2);
        }

        public static Filter GetComparer(string PropertyName, string FilterValue)
        {
            Filter tmpFilter = new Filter();

            if (FilterValue.Contains("*"))
            {
                if (FilterValue.StartsWith("*") && (FilterValue.EndsWith("*")))
                {
                    tmpFilter.Operation = ExpressionComparer.Contains;
                    tmpFilter.Value = FilterValue.Replace("*", "");
                }
                else
                {
                    if (FilterValue.StartsWith("*"))
                    {
                        tmpFilter.Operation = ExpressionComparer.EndsWith;
                        tmpFilter.Value = FilterValue.Replace("*", "");
                    }
                    if (FilterValue.EndsWith("*"))
                    {
                        tmpFilter.Operation = ExpressionComparer.StartsWith;
                        tmpFilter.Value = FilterValue.Replace("*", "");
                    }
                }
            }
            else
            {
                if (FilterValue.Contains(">"))
                {
                    if (FilterValue.Contains(">="))
                    {
                        tmpFilter.Operation = ExpressionComparer.GreaterThanOrEqual;
                        tmpFilter.Value = FilterValue.Replace(">=", "");
                    }
                    else
                    {
                        tmpFilter.Operation = ExpressionComparer.GreaterThan;
                        tmpFilter.Value = FilterValue.Replace(">", "");
                    }
                }
                else if (FilterValue.Contains("<"))
                {
                    if (FilterValue.Contains("<="))
                    {
                        tmpFilter.Operation = ExpressionComparer.LessThanOrEqual;
                        tmpFilter.Value = FilterValue.Replace("<=", "");
                    }
                    else
                    {
                        tmpFilter.Operation = ExpressionComparer.LessThan;
                        tmpFilter.Value = FilterValue.Replace("<", "");
                    }
                }
                else
                {
                    tmpFilter.Operation = ExpressionComparer.Equals;
                    tmpFilter.Value = FilterValue;
                }
            }

            tmpFilter.PropertyName = PropertyName;

            return tmpFilter;
        }

        public static Filter GetComparer(string PropertyName, int FilterValue,
            ExpressionComparer comparisonOperator)
        {
            Filter tmpFilter = new Filter();

            tmpFilter.Operation = comparisonOperator;
            tmpFilter.Value = FilterValue;
            tmpFilter.PropertyName = PropertyName;

            return tmpFilter;
        }

        public static Filter GetComparer(string PropertyName, bool FilterValue)
        {
            Filter tmpFilter = new Filter();

            tmpFilter.Operation = ExpressionComparer.Equals;
            tmpFilter.Value = FilterValue;
            tmpFilter.PropertyName = PropertyName;

            return tmpFilter;
        }

        public static Filter GetComparer(string PropertyName, DateTime FilterValue,
            ExpressionComparer comparisonOperator)
        {
            Filter tmpFilter = new Filter();

            tmpFilter.Operation = comparisonOperator;
            tmpFilter.Value = FilterValue;
            tmpFilter.PropertyName = PropertyName;

            return tmpFilter;
        }

        private class ParameterVisitor : ExpressionVisitor
        {
            public Expression Parameter
            {
                get;
                private set;
            }
            protected override Expression VisitParameter(ParameterExpression node)
            {
                Parameter = node;
                return node;
            }
        }
    }
}