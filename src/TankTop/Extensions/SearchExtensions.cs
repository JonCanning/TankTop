using System.Text;
using ServiceStack.Text;
using TankTop.Dto;

namespace TankTop.Extensions
{
    public static class SearchExtensions
    {
        public static string ToQueryString(this Search search)
        {
            var searchQueryString = new
                                    {
                                        q = search.QueryString,
                                        start = search.Start,
                                        len = search.Len,
                                        function = search.Function,
                                        fetch = search.Fetch.IsNull() ? null : string.Join(",", search.Fetch),
                                        fetch_variables = search.FetchVariables.IsNull() ? null : search.FetchVariables.ToString().ToLower(),
                                        fetch_categories = search.FetchCategories.IsNull() ? null : search.FetchCategories.ToString().ToLower(),
                                        snippet = search.Snippet.IsNull() ? null : string.Join(",", search.Snippet),
                                        category_filters = search.CategoryFilters
                                    };
            var queryString = QueryStringSerializer.SerializeToString(searchQueryString);
            var stringBuilder = new StringBuilder(queryString);
            if (search.Var.IsNotNull())
            {
                foreach (var @var in search.Var)
                {
                    stringBuilder.Append("&var{0}={1}".FormatWith(@var.Key, @var.Value));
                }
            }
            if (search.VariableFilters.IsNotNull())
            {
                foreach (var variableFilter in search.VariableFilters)
                {
                    stringBuilder.Append("&filter_docvar{0}={1}".FormatWith(variableFilter.Key, variableFilter.Value));
                }
            }
            if (search.FunctionFilters.IsNotNull())
            {
                foreach (var functionFilter in search.FunctionFilters)
                {
                    stringBuilder.Append("&filter_function{0}={1}".FormatWith(functionFilter.Key, functionFilter.Value));
                }
            }
            return stringBuilder.ToString();
        }

        public static void Check(this Search search)
        {
            search.Check(x => x.QueryString);
        }
    }
}