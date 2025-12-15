using System.Linq.Expressions;

namespace CleanArchMonolit.Shared.Extensions
{
    public static class LinqExpression
    {
        public static IQueryable<TSource> LinqOrderBy<TSource>(this IQueryable<TSource> query, string key, bool ascending = true)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return query;
            }

            var prop = typeof(TSource).GetProperty(key);
            var lambda = (dynamic)CreateExpression(typeof(TSource), prop.Name);

            return ascending ? Queryable.OrderBy(query, lambda) : Queryable.OrderDescending(query, lambda);
        }

        public static LambdaExpression CreateExpression(Type type, string propertyName)
        {
            var param = Expression.Parameter(type, "x");
            Expression body = param;
            foreach (var member in propertyName.Split('.'))
            {
                body = Expression.PropertyOrField(body, member);
            }
            return Expression.Lambda(body, param);
        }
    }
}
