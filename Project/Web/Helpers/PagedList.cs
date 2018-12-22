using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntityFramework.Extensions;

namespace Web.Helpers
{
    public interface IPagedList
    {
        int TotalCount { get; set; }
        int PageIndex { get; set; }
        int PageSize { get; set; }
        bool IsLastPage { get; set; }
    }

    public class PagedList<T> : List<T>, IPagedList where T : class
    {
        public PagedList(IQueryable<T> source, int index, int pageSize, bool Future)
        {
            IsLastPage = false;
            if (Future)
            {
                var data = source.Skip((index - 1) * pageSize).Take(pageSize).Future();
                TotalCount = source.FutureCount();
                AddRange(data);
            }
            else
            {
                var data = source.Skip((index - 1) * pageSize).Take(pageSize);
                TotalCount = source.Count();
                AddRange(data);
            }

            PageSize = pageSize;

            PageIndex = index;

            if (TotalCount / pageSize < PageIndex - 1)
            {
                index = 1;
                PageIndex = index;
                IsLastPage = true;
            }

        }

        public int TotalCount { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public bool IsLastPage { get; set; }

    }

    public static class Pagination
    {
        public static PagedList<T> ToPagedList<T>(this IQueryable<T> source, int index = 1, int pageSize = 20, bool future = true) where T : class
        {
            return new PagedList<T>(source, index, pageSize, future);
        }

        public static async Task<PagedList<T>> ToPagedListAsync<T>(this IQueryable<T> source, int index = 1, int pageSize = 20) where T : class
        {
            return await Task.Run(() => new PagedList<T>(source, index, pageSize, true));
        }

    }
}