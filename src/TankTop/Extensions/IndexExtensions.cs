using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ServiceStack.Text;
using TankTop.Dto;

namespace TankTop.Extensions
{
    public static class IndexExtensions
    {
        public static void AddDocument(this Index index, Document document)
        {
            index.TankTopClient.AddDocument(index.Name, document);
        }

        public static void AddDocuments(this Index index, params Document[] documents)
        {
            index.TankTopClient.AddDocuments(index.Name, documents);
        }

        public static void DeleteDocument(this Index index, string docId)
        {
            index.TankTopClient.DeleteDocument(index.Name, docId);
        }

        public static void DeleteDocuments(this Index index, params string[] docIds)
        {
            index.TankTopClient.DeleteDocuments(index.Name, docIds);
        }

        public static void DeleteDocuments(this Index index, Query query)
        {
            index.TankTopClient.DeleteDocuments(index.Name, query);
        }

        public static IDictionary<int, string> GetFunctions(this Index index)
        {
            return index.TankTopClient.GetFunctions(index.Name);
        }

        public static void AddFunction(this Index index, int functionNumber, string functionDefinition)
        {
            index.TankTopClient.AddFunction(index.Name, functionNumber, functionDefinition);
        }

        public static void DeleteFunction(this Index index, int functionNumber)
        {
            index.TankTopClient.DeleteFunction(index.Name, functionNumber);
        }

        public static SearchResult Search(this Index index, string queryText)
        {
            var query = new Query(queryText);
            return index.Search(query);
        }

        public static SearchResult Search(this Index index, Query query, params string[] fieldsToSearch)
        {
            SetQueryTextFields(query, fieldsToSearch);
            return index.TankTopClient.Search(index.Name, query);
        }

        static void SetQueryTextFields(Query query, IEnumerable<string> fieldsToSearch)
        {
            if (fieldsToSearch.Any()) query.QueryText = string.Join(" OR ", fieldsToSearch.Select(x => "{0}:{1}".FormatWith(x, query.QueryText)));
        }

        public static SearchResult<T> Search<T>(this Index index, Query query, params Expression<Func<T, object>>[] expressions)
        {
            SetQueryTextFields(query, expressions.Select(x => x.PropertyName().ToLower()));
            return index.TankTopClient.Search<T>(index.Name, query);
        }

        public static SearchResult<T> Search<T>(this Index index, string queryText)
        {
            var query = new Query(queryText);
            return index.Search<T>(query);
        }

        public static void UpdateVariables(this Index index, string docId, IDictionary<int, float> variables)
        {
            index.TankTopClient.UpdateVariables(index.Name, docId, variables);
        }

        public static void UpdateCategories(this Index index, string docId, IDictionary<string, string> categories)
        {
            index.TankTopClient.UpdateCategories(index.Name, docId, categories);
        }

        public static void Promote(this Index index, string docId, string query)
        {
            index.TankTopClient.Promote(index.Name, docId, query);
        }
    }
}