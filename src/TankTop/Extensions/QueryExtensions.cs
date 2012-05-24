using System.Collections.Generic;
using System.Text;
using ServiceStack.Text;
using TankTop.Dto;

namespace TankTop.Extensions
{
    public static class QueryExtensions
    {
        public static string ToQueryString(this Query query)
        {
            var searchQueryString = new
                                    {
                                        q = query.QueryString,
                                        start = query.Start,
                                        len = query.Len,
                                        function = query.Function,
                                        fetch = query.Fetch.IsNull() ? null : string.Join(",", query.Fetch),
                                        fetch_variables = query.FetchVariables.IsNull() ? null : query.FetchVariables.ToString().ToLower(),
                                        fetch_categories = query.FetchCategories.IsNull() ? null : query.FetchCategories.ToString().ToLower(),
                                        snippet = query.Snippet.IsNull() ? null : string.Join(",", query.Snippet),
                                        category_filters = JsonSerializer.SerializeToString(query.CategoryFilters),
                                        match_any_field = query.MatchAnyField
                                    };
            var queryString = QueryStringSerializer.SerializeToString(searchQueryString);
            var stringBuilder = new StringBuilder(queryString);
            if (query.Var.IsNotNull())
            {
                foreach (var @var in query.Var)
                {
                    stringBuilder.Append("&var{0}={1}".FormatWith(@var.Key, @var.Value));
                }
            }
            if (query.VariableFilters.IsNotNull())
            {
                foreach (var variableFilter in query.VariableFilters)
                {
                    stringBuilder.Append("&filter_docvar{0}={1}".FormatWith(variableFilter.Key, variableFilter.Value));
                }
            }
            if (query.FunctionFilters.IsNotNull())
            {
                foreach (var functionFilter in query.FunctionFilters)
                {
                    stringBuilder.Append("&filter_function{0}={1}".FormatWith(functionFilter.Key, functionFilter.Value));
                }
            }
            return stringBuilder.ToString();
        }

        public static void Check(this Query query)
        {
            query.Check(x => x.QueryString);

        }

        public static Query WithCategories(this Query query)
        {
            query.FetchCategories = true;
            return query;
        }

        public static Query WithVariables(this Query query)
        {
            query.FetchVariables = true;
            return query;
        }

        public static Query WithFields(this Query query, params string[] fields)
        {
            query.Fetch = fields;
            return query;
        }

        public static Query WithSnippetFromFields(this Query query, params string[] fields)
        {
            query.Snippet = fields;
            return query;
        }

        public static Query Skip(this Query query, int skip)
        {
            query.Start = skip;
            return query;
        }

        public static Query Take(this Query query, int take)
        {
            query.Len = take;
            return query;
        }

        public static Query WithScoringFunction(this Query query, int functionNum)
        {
            query.Function = functionNum;
            return query;
        }

        public static Query WithQueryVariable(this Query query, int index, float value)
        {
            query.Var = query.Var ?? new Dictionary<int, float>();
            query.Var.Add(index, value);
            return query;
        }

        public static Query WithCategoryFilter(this Query query, string category, params string[] matches)
        {
            query.CategoryFilters = query.CategoryFilters ?? new Dictionary<string, IEnumerable<string>>();
            query.CategoryFilters.Add(category, matches);
            return query;
        }

        public static Query MatchAnyField(this Query query)
        {
            query.MatchAnyField = true;
            return query;
        }
    }
}