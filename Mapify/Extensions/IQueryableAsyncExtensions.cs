namespace Mapify.Extensions;

public static class IQueryableAsyncExtensions
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
        result = await projectedQuery.FirstOrDefaultAsync();

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
    /// <param name="descending">If the sort is descending.</param>
    /// <returns></returns>
    public static async Task<List<T>> FindListAsync<T, TOrderBy>(this IQueryable query, Expression<Func<T, bool>> filterExpression, Expression<Func<T, TOrderBy>> orderByExpression, bool descending = false)
    {
        var result = new List<T>();

        var projectedQuery = query.ProjectToType<T>();
        if (filterExpression != null)
        {
            projectedQuery = projectedQuery.Where(filterExpression);
        }
        if (orderByExpression != null)
        {
            projectedQuery = descending ? projectedQuery.OrderByDescending(orderByExpression) : projectedQuery.OrderBy(orderByExpression);
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
        result = await projectedQuery.FirstOrDefaultAsync();

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
        result = await projectedQuery.FirstOrDefaultAsync();

        return result;
    }

    /// <summary>
    /// Gets a paged list and a total count of entities.
    /// </summary>
    /// <param name="query">The IQueryable.</param>
    /// <param name="page">The page.</param>
    /// <param name="pageSize">The page size.</param>
    /// <param name="filter">The filter.</param>
    /// <param name="orderBy">The sort.</param>
    public static async Task<(List<T> results, int totalCount)> GetPagedListAsync<T>(this IQueryable query, int page, int pageSize, string filter, string orderBy)
    {
        var result = new List<T>();

        var data = query.ProjectToType<T>();

        if (filter != null)
        {
            data = data.ApplyFiltering(filter);
        }

        var totalCount = await data.CountAsync();

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
    /// Gets a paged list and a total count of entities.
    /// </summary>
    /// <param name="query">The IQueryable.</param>
    /// <param name="page">The page.</param>
    /// <param name="pageSize">The page size.</param>
    /// <param name="filterExpression">The filter expression.</param>
    /// <param name="orderByExpression">The sort expression.</param>
    /// <param name="descending">If the sort is descending.</param>
    public static async Task<(List<T> results, int totalCount)> GetPagedListAsync<T, TOrderBy>(this IQueryable query, int page, int pageSize, Expression<Func<T, bool>> filterExpression, Expression<Func<T, TOrderBy>> orderByExpression, bool descending = false)
    {
        var result = new List<T>();

        var data = query.ProjectToType<T>();

        if (filterExpression != null)
        {
            data = data.Where(filterExpression);
        }

        var totalCount = await data.CountAsync();

        if (orderByExpression != null)
        {
            data = descending ? data.OrderByDescending(orderByExpression) : data.OrderBy(orderByExpression);
        }

        if (page > 0 && pageSize > 0)
        {
            data = data.ApplyPaging(page, pageSize);
        }

        result = await data.ToListAsync();

        return new(result, totalCount);
    }
}
