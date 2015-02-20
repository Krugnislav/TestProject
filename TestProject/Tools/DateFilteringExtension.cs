using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;

namespace TestProject.Tools
{
    public static class DateFilteringExtension
    {
        public static IQueryable<T> FilterByDate<T>
            (this IQueryable<T> query, string fieldName, string begin, string end, bool isSortAscending)
        {
            // Find the field we're sorting on and get it's type
            Type fieldType = typeof(T).GetProperty(fieldName).PropertyType;

            // Find the generic sort method. Note the binding flags 
            // since our particular sort method is marked as private & static
            MethodInfo sortItMethod = typeof(SortingExtensions)
                .GetMethod("SortIt", BindingFlags.Static | BindingFlags.NonPublic);

            // The method returned is an open generic, 
            // so let's close it with the types we need for this operation
            sortItMethod = sortItMethod.MakeGenericMethod(typeof(T), fieldType);

            // Now that we have our closed generic type, 
            // we can invoke it with the specified parameters 
            query = (IQueryable<T>)sortItMethod
                .Invoke(null, new object[] { query, fieldName, isSortAscending });

            return query;
        }

        private static IQueryable<T> SortIt<T, K>
            (IQueryable<T> query, string sortFieldName, bool isSortAscending)
        {
            //тут собственно и подисходит формирование выражения x => x.sortFieldName
            ParameterExpression param = Expression.Parameter(typeof(T), "x");
            MemberExpression body = Expression.Property(param, sortFieldName);
            var sortExpression = Expression.Lambda<Func<T, K>>(body, param);
            if (isSortAscending)
                return query.OrderBy<T, K>(sortExpression);
            return query.OrderByDescending<T, K>(sortExpression);
        }
    }
}