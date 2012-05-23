using System.Collections.Generic;
using TankTop.Dto;

namespace TankTop.Extensions
{
    public static class IndexExtensions
    {
        public static void AddDocument(this Index index, Document document)
        {
            index.TankTopClient.CreateDocument(index.Name, document);
        }

        public static void AddDocuments(this Index index, IEnumerable<Document> documents)
        {
            index.TankTopClient.CreateDocuments(index.Name, documents);
        }

        public static void DeleteDocument(this Index index, string docId)
        {
            index.TankTopClient.DeleteDocument(index.Name, docId);
        }

        public static void DeleteDocuments(this Index index, IEnumerable<string> docIds)
        {
            index.TankTopClient.DeleteDocuments(index.Name, docIds);
        }

        public static IDictionary<int, string> ReadFunctions(this Index index)
        {
            return index.TankTopClient.ReadIndexFunctions(index.Name);
        }

        public static void CreateFunction(this Index index, int functionNumber, string functionDefinition)
        {
            index.TankTopClient.CreateIndexFunction(index.Name, functionNumber, functionDefinition);
        }

        public static void DeleteFunction(this Index index, int functionNumber)
        {
            index.TankTopClient.DeleteIndexFunction(index.Name, functionNumber);
        }

        public static SearchResult Search(this Index index, Search search)
        {
            return index.TankTopClient.Search(index.Name, search);
        }

        public static void UpdateDocumentVariables(this Index index, string docId, IDictionary<int, float> variables)
        {
            index.TankTopClient.UpdateDocumentVariables(index.Name, docId, variables);
        }

        public static void UpdateDocumentCategories(this Index index, string docId, IDictionary<string, string> categories)
        {
            index.TankTopClient.UpdateDocumentCategories(index.Name, docId, categories);
        }
    }
}