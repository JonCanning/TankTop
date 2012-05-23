using System.Collections.Generic;

namespace TankTop.Dto
{
    public class Search
    {
        public string QueryString { get; set; }
        public int? Start { get; set; }
        public int? Len { get; set; }
        public int? Function { get; set; }
        public IEnumerable<string> Fetch { get; set; }
        public bool? FetchVariables { get; set; }
        public bool? FetchCategories { get; set; }
        public IEnumerable<string> Snippet { get; set; }
        public IDictionary<int, float> Var { get; set; }
        public IDictionary<string, IEnumerable<string>> CategoryFilters { get; set; }
        public IDictionary<int, string> VariableFilters { get; set; }
        public IDictionary<int, string> FunctionFilters { get; set; }
    }
}
