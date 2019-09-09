using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KE_PDC.API
{
    public class PaginatedList<T> : List<T>
    {
        public int PerPage { get; private set; }
        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }
        public int From { get; private set; }
        public int To { get; private set; }
        public int Total { get; private set; }

        public PaginatedList(List<T> data, int currentPage, int perPage, int from, int to, int count)
        {
            PerPage = perPage;
            From = from;
            To = to;
            CurrentPage = currentPage;
            TotalPages = (int)Math.Ceiling(count / (double)perPage);
            Total = count;

            AddRange(data);
        }

        public bool HasPreviousPage
        {
            get
            {
                return (CurrentPage > 1);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (CurrentPage < Total);
            }
        }

        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int currentPage, int perPage = 15)
        {
            if (currentPage < 1)
            {
                currentPage = 1;
            }

            if (perPage > 0)
            {
                source = source.Skip((currentPage - 1) * perPage).Take(perPage);
            }

            int from = ((currentPage - 1) * perPage) + 1;
            int to = from + perPage - 1;
            int total = await source.CountAsync();
            List<T> data = await source.ToListAsync();
            return new PaginatedList<T>(
                data: data,
                currentPage: currentPage,
                perPage: perPage,
                from: from,
                to: to,
                count: total
            );
        }
    }
}