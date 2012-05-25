using System.Collections.Generic;

namespace TankTop.Dto
{
    public class SearchResult : BaseSearchResult
    {
        public IEnumerable<ResultDocument> Results { get; set; }
    }

    public class SearchResult<T> : BaseSearchResult
    {
        public IEnumerable<ResultDocument<T>> Results { get; set; }
    }

    public abstract class BaseSearchResult
    {
        public int Matches { get; set; }
        public decimal Search_Time { get; set; }
        public string QueryText { get; set; }
        public string DidYouMean { get; set; }
        public IDictionary<string, IDictionary<string, int>> Facets { get; set; }
    }
}