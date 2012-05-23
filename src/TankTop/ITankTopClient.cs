using System.Collections.Generic;
using System.Net;
using TankTop.Dto;

namespace TankTop
{
    public interface ITankTopClient
    {
        HttpStatusCode StatusCode { get; }
        IEnumerable<Index> ReadIndexes();
        void DeleteIndex(string indexName);
        Index CreateIndex(string indexName, bool publicSearch = false);
        Index ReadIndex(string indexName);
        void CreateDocument(string indexName, Document document);
        void CreateDocuments(string indexName, IEnumerable<Document> documents);
        void DeleteDocument(string indexName, string docId);
        void DeleteDocuments(string indexName, IEnumerable<string> docIds);
        void UpdateDocumentVariables(string indexName, string docId, IDictionary<int, float> variables);
        void UpdateDocumentCategories(string indexName, string docId, IDictionary<string, string> categories);
        IDictionary<int, string> ReadIndexFunctions(string indexName);
        void CreateIndexFunction(string indexName, int functionNumber, string functionDefinition);
        void DeleteIndexFunction(string indexName, int functionNumber);
        SearchResult Search(string indexName, Search search);
    }
}