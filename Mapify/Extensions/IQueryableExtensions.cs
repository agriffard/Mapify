namespace Mapify.Extensions;

public static class IQueryableExtensions
{
    /// <summary>
    /// Finds an element by identifier.
    /// </summary>
    /// <param name="query">The IQueryable.</param>
    /// <param name="id">The id.</param>
    /// <returns></returns>
    public static T FindById<T, TId>(this IQueryable query, TId id)
    {
        var result = default(T);

        var projectedQuery = query.ProjectToType<T>();
        projectedQuery = projectedQuery.ApplyFiltering($"Id = {id}");
        result = projectedQuery.FirstOrDefault();

        return result;
    }

    /// <summary>
    /// Finds a list of elements from a filter.
    /// </summary>
    /// <param name="query">The IQueryable.</param>
    /// <param name="filter">The filter.</param>
    /// <param name="orderBy">The sort.</param>
    /// <returns></returns>
    public static List<T> FindList<T>(this IQueryable query, string filter, string? orderBy = null)
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
        result = projectedQuery.ToList();

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
    public static List<T> FindList<T, TOrderBy>(this IQueryable query, Expression<Func<T, bool>> filterExpression, Expression<Func<T, TOrderBy>> orderByExpression, bool descending = false)
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
        result = projectedQuery.ToList();

        return result;
    }

    /// <summary>
    /// Finds one element from a filter.
    /// </summary>
    /// <param name="query">The IQueryable.</param>
    /// <param name="filter">The filter.</param>
    /// <returns></returns>
    public static T FindOne<T>(this IQueryable query, string filter)
    {
        var result = default(T);

        var projectedQuery = query.ProjectToType<T>();
        projectedQuery = projectedQuery.ApplyFiltering(filter);
        result = projectedQuery.FirstOrDefault();

        return result;
    }

    /// <summary>
    /// Finds one element from a filter expression.
    /// </summary>
    /// <param name="query">The IQueryable.</param>
    /// <param name="filterExpression">The filter expression.</param>
    /// <returns></returns>
    public static T FindOne<T>(this IQueryable query, Expression<Func<T, bool>> filterExpression)
    {
        var result = default(T);

        var projectedQuery = query.ProjectToType<T>();
        projectedQuery = projectedQuery.Where(filterExpression);
        result = projectedQuery.FirstOrDefault();

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
    public static (List<T> results, int totalCount) GetPagedList<T>(this IQueryable query, int page, int pageSize, string filter, string orderBy)
    {
        var result = new List<T>();

        var data = query.ProjectToType<T>();

        if (filter != null)
        {
            data = data.ApplyFiltering(filter);
        }

        var totalCount = data.Count();

        if (orderBy != null)
        {
            data = data.ApplyOrdering(orderBy);
        }

        if (page > 0 && pageSize > 0)
        {
            data = data.ApplyPaging(page, pageSize);
        }

        result = data.ToList();

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
    public static (List<T> results, int totalCount) GetPagedList<T, TOrderBy>(this IQueryable query, int page, int pageSize, Expression<Func<T, bool>> filterExpression, Expression<Func<T, TOrderBy>> orderByExpression, bool descending = false)
    {
        var result = new List<T>();

        var data = query.ProjectToType<T>();

        if (filterExpression != null)
        {
            data = data.Where(filterExpression);
        }

        var totalCount = data.Count();

        if (orderByExpression != null)
        {
            data = descending ? data.OrderByDescending(orderByExpression) : data.OrderBy(orderByExpression);
        }

        if (page > 0 && pageSize > 0)
        {
            data = data.ApplyPaging(page, pageSize);
        }

        result = data.ToList();

        return new(result, totalCount);
    }
}
