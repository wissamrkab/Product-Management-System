using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace PMS.Application.Helpers;

public static class ExtensionsMethods
{
    public static string RemoveHyphens(this string str)
    {
        return str.Replace("-", "");
    } 
    public static IQueryable<TEntity> GetAllIncluding<TEntity>(this IQueryable<TEntity> queryable, params Expression<Func<TEntity, object>>[] includeProperties) where TEntity : class
    {
        return includeProperties.Aggregate(queryable, (current, includeProperty) => current.Include(includeProperty));
    }

    public static IQueryable<TEntity> GetAllIncludingWithFunc<TEntity>(this IQueryable<TEntity> queryable, params Expression<Func<TEntity, object>>[] includeProperties) where TEntity : class
    {
        return includeProperties.Aggregate(queryable, (current, includeProperty) => current.Include(includeProperty));
    }
}
