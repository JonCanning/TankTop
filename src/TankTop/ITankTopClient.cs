using System.Collections.Generic;
using System.Net;
using TankTop.Dto;

namespace TankTop
{
    public interface ITankTopClient
    {
        HttpStatusCode StatusCode { get; }

        IEnumerable<Index> GetIndexes();
        void DeleteIndex(string indexName);
        Index CreateIndex(string indexName, bool publicSearch = false);
        Index GetIndex(string indexName);
        IDictionary<int, string> GetFunctions(string indexName);
        void AddFunction(string indexName, int functionNumber, string functionDefinition);
        void DeleteFunction(string indexName, int functionNumber);

        void AddDocument(string indexName, Document document);
        void AddDocuments(string indexName, params Document[] documents);
        void DeleteDocument(string indexName, string docId);
        void DeleteDocuments(string indexName, params string[] docIds);
        void DeleteDocuments(string indexName, Query query);
        void UpdateVariables(string indexName, string docId, IDictionary<int, float> variables);
        void UpdateCategories(string indexName, string docId, IDictionary<string, string> categories);
        SearchResult Search(string indexName, Query query);
        SearchResult<T> Search<T>(string indexName, Query<T> query);
        void Promote(string indexName, string docId, string query);
    }
}