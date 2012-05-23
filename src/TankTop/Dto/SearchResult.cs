using System.Collections.Generic;

namespace TankTop.Dto
{
    public class SearchResult
    {
        public int Matches { get; set; }
        public decimal Search_Time { get; set; }
        public string QueryText { get; set; }
        public string DidYouMean { get; set; }
        public IEnumerable<ResultDocument> Results { get; set; }
        public IDictionary<string, IDictionary<string, int>> Facets { get; set; }
    }

    public class SearchResultJson
    {
        public int Matches { get; set; }
        public decimal Search_Time { get; set; }
        public string QueryText { get; set; }
        public string DidYouMean { get; set; }
        public IEnumerable<dynamic> Results { get; set; }
        public IDictionary<string, IDictionary<string, int>> Facets { get; set; }
    }
}