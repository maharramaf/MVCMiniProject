using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace MVCMiniProject.Admin
{
    public static class AdminQueryHelper
    {
        public static IQueryable GetSet(DbContext context, Type entityType)
        {
            var method = typeof(DbContext).GetMethods()
                .First(m => m.Name == nameof(DbContext.Set) && m.IsGenericMethodDefinition && m.GetParameters().Length == 0);
            return (IQueryable)method.MakeGenericMethod(entityType).Invoke(context, null)!;
        }

        public static IQueryable Include(IQueryable source, Type entityType, IEnumerable<string> navigations)
        {
            var include = typeof(EntityFrameworkQueryableExtensions)
                .GetMethods()
                .First(m => m.Name == nameof(EntityFrameworkQueryableExtensions.Include)
                            && m.GetParameters().Length == 2
                            && m.GetParameters()[1].ParameterType == typeof(string));

            foreach (var navigation in navigations.Where(n => !string.IsNullOrWhiteSpace(n)))
            {
                source = (IQueryable)include.MakeGenericMethod(entityType).Invoke(null, new object[] { source, navigation })!;
            }

            return source;
        }

        public static IQueryable WhereSearch(IQueryable source, Type entityType, string term, IEnumerable<string> stringProperties)
        {
            var props = stringProperties.Where(p => entityType.GetProperty(p) != null).ToArray();
            if (props.Length == 0)
            {
                return source;
            }

            var parameter = Expression.Parameter(entityType, "x");
            var contains = typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(string) })!;
            var termConstant = Expression.Constant(term);
            Expression? body = null;

            foreach (var name in props)
            {
                var member = Expression.Property(parameter, name);
                var notNull = Expression.NotEqual(member, Expression.Constant(null, typeof(string)));
                var call = Expression.Call(member, contains, termConstant);
                var clause = Expression.AndAlso(notNull, call);
                body = body == null ? clause : Expression.OrElse(body, clause);
            }

            var lambda = Expression.Lambda(body!, parameter);
            return CallQueryable(source, entityType, "Where", lambda);
        }

        public static IQueryable WhereEquals(IQueryable source, Type entityType, string propertyName, object value)
        {
            var property = entityType.GetProperty(propertyName);
            if (property == null)
            {
                return source;
            }

            var parameter = Expression.Parameter(entityType, "x");
            var member = Expression.Property(parameter, property);
            var constant = Expression.Constant(Convert.ChangeType(value, Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType));
            var equal = Expression.Equal(member, Expression.Convert(constant, property.PropertyType));
            var lambda = Expression.Lambda(equal, parameter);
            return CallQueryable(source, entityType, "Where", lambda);
        }

        public static IQueryable OrderByProperty(IQueryable source, Type entityType, string propertyName, bool descending)
        {
            var property = entityType.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (property == null)
            {
                property = entityType.GetProperty("Id");
            }

            if (property == null)
            {
                return source;
            }

            var parameter = Expression.Parameter(entityType, "x");
            var member = Expression.Property(parameter, property);
            var lambda = Expression.Lambda(member, parameter);
            var methodName = descending ? "OrderByDescending" : "OrderBy";
            return CallQueryable(source, entityType, methodName, lambda, property.PropertyType);
        }

        public static IQueryable SkipTake(IQueryable source, Type entityType, int skip, int take)
        {
            source = CallQueryableValue(source, entityType, "Skip", skip);
            return CallQueryableValue(source, entityType, "Take", take);
        }

        public static async Task<int> CountAsync(IQueryable source)
        {
            return await EntityFrameworkQueryableExtensions.CountAsync((dynamic)source);
        }

        public static async Task<List<object>> ToListAsync(IQueryable source)
        {
            var list = await EntityFrameworkQueryableExtensions.ToListAsync((dynamic)source);
            return ((IEnumerable<object>)list).ToList();
        }

        private static IQueryable CallQueryable(IQueryable source, Type entityType, string methodName, LambdaExpression lambda, Type? resultType = null)
        {
            var methods = typeof(Queryable).GetMethods().Where(m => m.Name == methodName);
            MethodInfo method;
            if (resultType == null)
            {
                method = methods.First(m => m.GetParameters().Length == 2).MakeGenericMethod(entityType);
            }
            else
            {
                method = methods.First(m => m.GetParameters().Length == 2).MakeGenericMethod(entityType, resultType);
            }

            return (IQueryable)method.Invoke(null, new object[] { source, lambda })!;
        }

        private static IQueryable CallQueryableValue(IQueryable source, Type entityType, string methodName, int value)
        {
            var method = typeof(Queryable).GetMethods()
                .First(m => m.Name == methodName && m.GetParameters().Length == 2)
                .MakeGenericMethod(entityType);
            return (IQueryable)method.Invoke(null, new object[] { source, value })!;
        }
    }
}
