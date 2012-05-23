using System;
using System.Linq;
using System.Linq.Expressions;

namespace TankTop.Extensions
{
    public static class Extensions
    {
        public static bool IsNull(this object obj)
        {
            return obj == null;
        }

        public static bool IsNotNull(this object obj)
        {
            return !obj.IsNull();
        }

        public static void Check<T, TProperty>(this T obj, Expression<Func<T, TProperty>> expression, params Predicate<TProperty>[] predicates)
        {
            var propertyName = expression.PropertyName();
            var property = expression.Compile().Invoke(obj);
            if (property.IsNull() || string.IsNullOrWhiteSpace(property.ToString()))
            {
                throw new NullReferenceException(propertyName);
            }
            if (predicates.Any(x => !x(property)))
            {
                throw new NullReferenceException(propertyName);
            }
        }

        static string PropertyName(this LambdaExpression expression)
        {
            return (expression.Body is UnaryExpression ?
                expression.Body.CastTo<UnaryExpression>().Operand.CastTo<MemberExpression>() :
                expression.Body.CastTo<MemberExpression>()).Member.Name;
        }

        public static T CastTo<T>(this object obj)
        {
            return (T)obj;
        }
    }
}