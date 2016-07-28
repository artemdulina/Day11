using System;
using System.Linq.Expressions;

namespace MathematicalEntities.Helpers
{
    public static class OperationHelper<T>
    {
        private static readonly ParameterExpression paramX = Expression.Parameter(typeof(T), "x");
        private static readonly ParameterExpression paramY = Expression.Parameter(typeof(T), "y");

        private static Func<T, T, T> MultiplyF;
        private static Func<T, T, T> AdditionF;

        public static Func<T, T, T> MultiplyFunction
        {
            get
            {
                if (MultiplyF != null)
                {
                    return MultiplyF;
                }

                return MultiplyF = Expression.Lambda<Func<T, T, T>>(Expression.Multiply(paramX, paramY), paramX, paramY).Compile();
            }
        }

        public static Func<T, T, T> AdditionFunction
        {
            get
            {
                if (AdditionF != null)
                {
                    return AdditionF;
                }

                return AdditionF = Expression.Lambda<Func<T, T, T>>(Expression.Add(paramX, paramY), paramX, paramY).Compile();
            }
        }
    }
}
