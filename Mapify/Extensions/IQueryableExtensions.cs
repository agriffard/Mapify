using System.Linq.Expressions;
using Gridify;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Mapify.Extensions;

public static class IQueryableExtensions
{
    /// <summary>
    /// Finds an element by identifier.
    /// </summary>
    /// <param name="query">The IQueryable.</param>
    /// <param name="id">The id.</param>
    /// <returns></returns>
    public static async Task<T> FindByIdAsync<T, TId>(this IQueryable query, TId id)
    {
        var result = default(T);

        var projectedQuery = query.ProjectToType<T>();
        projectedQuery = projectedQuery.ApplyFiltering($"Id = {id}");
        result = await projectedQuery.SingleOrDefaultAsync();

        return result;
    }

    /// <summary>
    /// Finds a list of elements from a filter.
    /// </summary>
    /// <param name="query">The IQueryable.</param>
    /// <param name="filter">The filter.</param>
    /// <param name="orderBy">The sort.</param>
    /// <returns></returns>
    public static async Task<List<T>> FindListAsync<T>(this IQueryable query, string filter, string? orderBy = null)
    {
        var result = new List<T>();

        var projectedQuery = query.ProjectToType<T>();
        if (filter != null)
        {
            projectedQuery = projectedQuery.ApplyFiltering(filter);
        }
        if (orderBy != null)
        {
            projectedQuery = projectedQuery.ApplyOrdering(orderBy);
        }
        result = await projectedQuery.ToListAsync();

        return result;
    }

    /// <summary>
    /// Finds a list of elements from a filter.
    /// </summary>
    /// <param name="query">The IQueryable.</param>
    /// <param name="filterExpression">The filter expression.</param>
    /// <param name="orderByExpression">The sort expression.</param>
    /// <returns></returns>
    public static async Task<List<T>> FindListAsync<T, TOrderBy>(this IQueryable query, Expression<Func<T, bool>> filterExpression, Expression<Func<T, TOrderBy>> orderByExpression)
    {
        var result = new List<T>();

        var projectedQuery = query.ProjectToType<T>();
        if (filterExpression != null)
        {
            projectedQuery = projectedQuery.Where(filterExpression);
        }
        if (orderByExpression != null)
        {
            projectedQuery = projectedQuery.OrderBy(orderByExpression);
        }
        result = await projectedQuery.ToListAsync();

        return result;
    }

    /// <summary>
    /// Finds one element from a filter.
    /// </summary>
    /// <param name="query">The IQueryable.</param>
    /// <param name="filter">The filter.</param>
    /// <returns></returns>
    public static async Task<T> FindOneAsync<T>(this IQueryable query, string filter)
    {
        var result = default(T);

        var projectedQuery = query.ProjectToType<T>();
        projectedQuery = projectedQuery.ApplyFiltering(filter);
        result = await projectedQuery.SingleOrDefaultAsync();

        return result;
    }

    /// <summary>
    /// Finds one element from a filter expression.
    /// </summary>
    /// <param name="query">The IQueryable.</param>
    /// <param name="filterExpression">The filter expression.</param>
    /// <returns></returns>
    public static async Task<T> FindOneAsync<T>(this IQueryable query, Expression<Func<T, bool>> filterExpression)
    {
        var result = default(T);

        var projectedQuery = query.ProjectToType<T>();
        projectedQuery = projectedQuery.Where(filterExpression);
        result = await projectedQuery.SingleOrDefaultAsync();

        return result;
    }

    /// <summary>
    /// Gets a paged list and a total count of entities.
    /// </summary>
    public static async Task<(List<T> results, int totalCount)> GetPagedListAsync<T>(this IQueryable query, int page, int pageSize, string filter, string orderBy)
    {
        var result = new List<T>();

        var data = query.ProjectToType<T>();

        if (filter != null)
        {
            data = data.ApplyFiltering(filter);
        }

        int totalCount = await data.CountAsync();

        if (orderBy != null)
        {
            data = data.ApplyOrdering(orderBy);
        }

        if (page > 0 && pageSize > 0)
        {
            data = data.ApplyPaging(page, pageSize);
        }

        result = await data.ToListAsync();

        return new(result, totalCount);
    }

    /// <summary>
    /// Gets a paged result of entities.
    /// </summary>
    public static async Task<PagedResult<T>> GetPagedResultAsync<T>(this IQueryable query, int page, int pageSize, string filter, string orderBy)
    {
        var (results, totalCount) = await GetPagedListAsync<T>(query, page, pageSize, filter, orderBy);
        var result = new PagedResult<T>(results, page, pageSize, totalCount);

        return result;
    }
}
