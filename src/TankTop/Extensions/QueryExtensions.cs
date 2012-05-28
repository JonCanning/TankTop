using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
                                        q = query.QueryText,
                                        start = query.Start,
                                        len = query.Len,
                                        function = query.Function,
                                        fetch = query.Fetch.IsNull() ? null : String.Join(",", query.Fetch),
                                        fetch_variables = query.FetchVariables.IsNull() ? null : query.FetchVariables.ToString().ToLower(),
                                        fetch_categories = query.FetchCategories.IsNull() ? null : query.FetchCategories.ToString().ToLower(),
                                        snippet = query.Snippet.IsNull() ? null : String.Join(",", query.Snippet),
                                        category_filters = query.CategoryFilters.SerializeToString(),
                                        match_any_field = query.MatchAnyField
                                    };
            var queryString = QueryStringSerializer.SerializeToString(searchQueryString);
            var stringBuilder = new StringBuilder(queryString);
            if (query.Var.IsNotNull())
            {
                foreach (var @var in query.Var)
                {
                    stringBuilder.Append("&var{0}={1}".FormatWith(var.Key, var.Value));
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
            query.Check(x => x.QueryText);
        }

        public static Query WithCategories(this Query query)
        {
            query.FetchCategories = true;
            return query;
        }

        public static Query<T> WithCategories<T>(this Query<T> query)
        {
            return query.CastTo<Query>().WithCategories().CastTo<Query<T>>();
        }

        public static Query WithVariables(this Query query)
        {
            query.FetchVariables = true;
            return query;
        }

        public static Query<T> WithVariables<T>(this Query<T> query)
        {
            return query.CastTo<Query>().WithVariables().CastTo<Query<T>>();
        }

        public static Query WithFields(this Query query, params string[] fields)
        {
            query.Fetch = query.Fetch ?? new string[0];
            query.Fetch = fields;
            return query;
        }

        public static Query<T> WithFields<T>(this Query<T> query, params Expression<Func<T, object>>[] fields)
        {
            return query.WithFields(fields.Select(x => x.PropertyName().ToLower()).ToArray()).CastTo<Query<T>>();
        }

        public static Query WithAllFields(this Query query)
        {
            return query.WithFields("*");
        }

        public static Query<T> WithAllFields<T>(this Query<T> query)
        {
            return query.WithFields("*").CastTo<Query<T>>();
        }

        public static Query WithSnippetFromFields(this Query query, params string[] fields)
        {
            query.Snippet = fields;
            return query;
        }

        public static Query<T> WithSnippetFromFields<T>(this Query<T> query, params string[] fields)
        {
            return query.CastTo<Query>().WithSnippetFromFields(fields).CastTo<Query<T>>();
        }

        public static Query Skip(this Query query, int skip)
        {
            query.Start = skip;
            return query;
        }

        public static Query<T> Skip<T>(this Query<T> query, int skip)
        {
            return query.CastTo<Query>().Skip(skip).CastTo<Query<T>>();
        }

        public static Query Take(this Query query, int take)
        {
            query.Len = take;
            return query;
        }

        public static Query<T> Take<T>(this Query<T> query, int take)
        {
            return query.CastTo<Query>().Take(take).CastTo<Query<T>>();
        }

        public static Query WithScoringFunction(this Query query, int functionNum)
        {
            query.Function = functionNum;
            return query;
        }

        public static Query<T> WithScoringFunction<T>(this Query<T> query, int functionNum)
        {
            return query.CastTo<Query>().WithScoringFunction(functionNum).CastTo<Query<T>>();
        }

        public static Query WithQueryVariable(this Query query, int index, float value)
        {
            query.Var = query.Var ?? new Dictionary<int, float>();
            query.Var.Add(index, value);
            return query;
        }

        public static Query<T> WithQueryVariable<T>(this Query<T> query, int index, float value)
        {
            return query.CastTo<Query>().WithQueryVariable(index, value).CastTo<Query<T>>();
        }

        public static Query WithCategoryFilter(this Query query, string category, params string[] matches)
        {
            query.CategoryFilters = query.CategoryFilters ?? new Dictionary<string, IEnumerable<string>>();
            query.CategoryFilters.Add(category, matches);
            return query;
        }

        public static Query<T> WithCategoryFilter<T>(this Query<T> query, string category, params string[] matches)
        {
            return query.CastTo<Query>().WithCategoryFilter(category, matches).CastTo<Query<T>>();
        }

        public static Query MatchAnyField(this Query query)
        {
            query.MatchAnyField = true;
            return query;
        }

        public static Query<T> MatchAnyField<T>(this Query<T> query)
        {
            return query.CastTo<Query>().MatchAnyField().CastTo<Query<T>>();
        }

        public static Query WithSearchFields(this Query query, params string[] fieldsToSearch)
        {
            if (fieldsToSearch.Any()) query.QueryText = QueryText(query, fieldsToSearch);
            return query;
        }

        public static Query<T> WithSearchFields<T>(this Query<T> query, params Expression<Func<T, object>>[] fieldsToSearch)
        {
            query.QueryText = QueryText(query, fieldsToSearch.Select(x => x.PropertyName().ToLower()));
            return query;
        }

        static string QueryText(Query query, IEnumerable<string> fieldsToSearch)
        {
            return String.Join(" OR ", fieldsToSearch.Select(x => "{0}:{1}".FormatWith(x, query.QueryText)));
        }
    }
}