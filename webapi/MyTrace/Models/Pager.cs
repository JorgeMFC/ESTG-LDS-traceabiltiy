using Nethereum.JsonRpc.Client;

namespace MyTrace.Models
{
    public class Pager<T>
    {
        public IEnumerable<T> results { get; set; }
        public int totalResults { get; set; }
        public int limit { get; set; }
        public int page { get; set; }
        public int totalPages { get; set; }
        public bool hasPrevPage { get; set; }
        public bool hasNextPage { get; set; }
        public int? nextPage { get; set; }

        public Pager(IEnumerable<T> allResults, int limit, int page)
        {
            if (page != 1)
            {
                results = allResults.Skip((limit * page) - limit).Take(limit);
            }
            else
            {
                results = allResults.Take(limit);
            }

            totalResults = allResults.Count();
            this.limit = limit;
            this.page = page;

            totalPages = (int)Math.Ceiling((double)totalResults / limit);

            hasPrevPage = page != 1;
            hasNextPage = (page != totalPages);

            if (page != totalPages)
            {
                nextPage = page + 1;
            }
            else
            {
                nextPage = null;
            }
        }
    }
}
