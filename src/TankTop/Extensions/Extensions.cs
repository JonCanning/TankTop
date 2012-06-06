using System;
using System.Linq;
using System.Linq.Expressions;

namespace TankTop.Extensions
{
    static class Extensions
    {
        internal static bool IsNull(this object obj)
        {
            return obj == null;
        }

        internal static bool IsNotNull(this object obj)
        {
            return !obj.IsNull();
        }

        internal static void Check<T, TProperty>(this T obj, Expression<Func<T, TProperty>> expression, params Predicate<TProperty>[] predicates)
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

        internal static string PropertyName(this LambdaExpression expression)
        {
            return (expression.Body is UnaryExpression ?
                expression.Body.CastTo<UnaryExpression>().Operand.CastTo<MemberExpression>() :
                expression.Body.CastTo<MemberExpression>()).Member.Name;
        }

        internal static T CastTo<T>(this object obj)
        {
            return (T)obj;
        }

        internal static float ToUnixEpoch(this DateTime dateTime)
        {
            var t = dateTime - new DateTime(1970, 1, 1);
            return (float)t.TotalSeconds;
        }
    }
}