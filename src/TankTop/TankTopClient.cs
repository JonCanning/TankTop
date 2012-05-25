using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using ServiceStack.Text;
using TankTop.Dto;
using TankTop.Extensions;

namespace TankTop
{
    public class TankTopClient : ITankTopClient
    {
        readonly IWebClient webClient;
        static readonly IEnumerable<string> ResultDocumentProperties = typeof(ResultDocument).GetProperties().Select(x => x.Name.ToLower());

        public TankTopClient(IWebClient webClient)
        {
            this.webClient = webClient;
            JsConfig.ExcludeTypeInfo = true;
            JsConfig.EmitCamelCaseNames = true;
        }

        public TankTopClient(string baseAddress) : this(new WebClient(baseAddress)) { }

        public HttpStatusCode StatusCode
        {
            get { return webClient.StatusCode; }
        }

        public IEnumerable<Index> GetIndexes()
        {
            var resource = Resources.Indexes;
            var dictionary = webClient.Get<IDictionary<string, Index>>(resource);
            return dictionary.Select(x =>
                                     {
                                         x.Value.Name = x.Key;
                                         return x.Value;
                                     });
        }

        public void DeleteIndex(string indexName)
        {
            var resource = Resources.Indexes_Name.FormatWith(indexName);
            webClient.Delete(resource);
        }

        public Index CreateIndex(string indexName, bool publicSearch = false)
        {
            var resource = Resources.Indexes_Name.FormatWith(indexName);
            var index = webClient.Put<Index>(resource, new { public_search = publicSearch });
            if (webClient.StatusCode == HttpStatusCode.NoContent)
            {
                index = webClient.Get<Index>(resource);
            }
            index.Name = indexName;
            index.TankTopClient = this;
            return index;
        }

        public Index GetIndex(string indexName)
        {
            var resource = Resources.Indexes_Name.FormatWith(indexName);
            var index = webClient.Get<Index>(resource);
            index.Name = indexName;
            index.TankTopClient = this;
            return index;
        }

        public void AddDocument(string indexName, Document document)
        {
            document.Check();
            var resource = Resources.Indexes_Name_Docs.FormatWith(indexName);
            webClient.Put(resource, document.ToSerializable());
        }

        public void AddDocuments(string indexName, params Document[] documents)
        {
            foreach (var document in documents)
            {
                document.Check();
            }
            var resource = Resources.Indexes_Name_Docs.FormatWith(indexName);
            webClient.Put(resource, documents.Select(x => x.ToSerializable()));
        }

        public void DeleteDocument(string indexName, string docId)
        {
            var resource = Resources.Indexes_Name_Docs.FormatWith(indexName);
            webClient.Delete(resource, new { docid = docId });
        }

        public void DeleteDocuments(string indexName, params string[] docIds)
        {
            var resource = Resources.Indexes_Name_Docs.FormatWith(indexName);
            var deleteDocuments = docIds.Select(x => new { docid = x });
            webClient.Delete(resource, deleteDocuments);
        }

        public void DeleteDocuments(string indexName, Query query)
        {
            var searchQueryString = SearchQueryString(indexName, query);
            webClient.Delete(searchQueryString);
        }

        public void UpdateVariables(string indexName, string docId, IDictionary<int, float> variables)
        {
            var resource = Resources.Indexes_Name_Docs_Variables.FormatWith(indexName);
            webClient.Put(resource, new { docid = docId, variables });
        }

        public void UpdateCategories(string indexName, string docId, IDictionary<string, string> categories)
        {
            var resource = Resources.Indexes_Name_Docs_Categories.FormatWith(indexName);
            webClient.Put(resource, new { docid = docId, categories = categories });
        }

        public IDictionary<int, string> GetFunctions(string indexName)
        {
            var resource = Resources.Indexes_Name_Functions.FormatWith(indexName);
            return webClient.Get<IDictionary<int, string>>(resource);
        }

        public void AddFunction(string indexName, int functionNumber, string functionDefinition)
        {
            var resource = Resources.Indexes_Name_Functions_Num.FormatWith(indexName, functionNumber);
            webClient.Put(resource, new { definition = functionDefinition });
        }

        public void DeleteFunction(string indexName, int functionNumber)
        {
            var resource = Resources.Indexes_Name_Functions_Num.FormatWith(indexName, functionNumber);
            webClient.Delete(resource);
        }

        public SearchResult Search(string indexName, Query query)
        {
            var searchQueryString = SearchQueryString(indexName, query);
            var searchResult = webClient.Get<SearchResult>(searchQueryString);
            if (searchResult.IsNotNull() && searchResult.Results.IsNotNull() && searchResult.Results.Any())
            {
                var resultDocuments = JsonObject.Parse(webClient.Response).ArrayObjects("results").ConvertAll(x => ResultDocument(searchResult, x));
                searchResult.Results = resultDocuments;
            }
            return searchResult;
        }

        public SearchResult<T> Search<T>(string indexName, Query query, params Action<T, JsonObject>[] mappingActions)
        {
            var searchQueryString = SearchQueryString(indexName, query);
            var searchResult = webClient.Get<SearchResult<T>>(searchQueryString);
            if (searchResult.IsNotNull() && searchResult.Results.IsNotNull() && searchResult.Results.Any())
            {
                var resultDocuments = JsonObject.Parse(webClient.Response).ArrayObjects("results").ConvertAll(x => ResultDocument(searchResult, x, mappingActions));
                searchResult.Results = resultDocuments;
            }
            return searchResult;
        }

        public void Promote(string indexName, string docId, string query)
        {
            var resource = Resources.Indexes_Name_Promote.FormatWith(indexName);
            webClient.Put(resource, new { docid = docId, query });
        }

        static string SearchQueryString(string indexName, Query query)
        {
            var resource = Resources.Indexes_Name_Search.FormatWith(indexName);
            query.Check();
            var queryString = query.ToQueryString();
            return "{0}?{1}".FormatWith(resource, queryString);
        }

        IList<string> MapProperties(BaseResultDocument resultDocument, JsonObject jsonObject)
        {
            var fields = jsonObject.Select(y => y.Key).Where(y => !ResultDocumentProperties.Contains(y)).ToList();

            const string variable = "variable_";
            if (webClient.Response.Contains(variable))
            {
                resultDocument.Variables = new List<float>();
                foreach (var field in fields.Where(x => x.StartsWith(variable)).OrderBy(x => x))
                {
                    var f = jsonObject.Get<float>(field);
                    resultDocument.Variables.Add(f);
                }
                fields.RemoveAll(x => x.StartsWith(variable));
            }

            const string category = "category_";
            if (webClient.Response.Contains(category))
            {
                resultDocument.Categories = new Dictionary<string, string>();
                foreach (var field in fields.Where(x => x.StartsWith(category)))
                {
                    var categoryName = field.Replace(category, string.Empty);
                    var categoryValue = jsonObject.Get<string>(field);
                    resultDocument.Categories.Add(categoryName, categoryValue);
                }
                fields.RemoveAll(x => x.StartsWith(category));
            }

            const string snippet = "snippet_";
            if (webClient.Response.Contains(snippet))
            {
                resultDocument.Snippets = new Dictionary<string, string>();
                foreach (var field in fields.Where(x => x.StartsWith(snippet)))
                {
                    var snippetName = field.Replace(snippet, string.Empty);
                    var snippetValue = jsonObject.Get<string>(field);
                    resultDocument.Snippets.Add(snippetName, snippetValue);
                }
                fields.RemoveAll(x => x.StartsWith(category));
            }
            return fields;
        }

        ResultDocument<T> ResultDocument<T>(SearchResult<T> searchResult, JsonObject jsonObject, IEnumerable<Action<T, JsonObject>> mappingActions)
        {
            var resultDocument = searchResult.Results.Single(y => y.DocId == jsonObject.Get("docid"));

            var fields = MapProperties(resultDocument, jsonObject);

            if (fields.Any())
            {
                resultDocument.Fields = fields.ToDictionary(x => x, x => jsonObject.Get(x)).FromDictionary<T>();
                foreach (var field in fields)
                {
                    foreach (var mappingAction in mappingActions)
                    {
                        mappingAction(resultDocument.Fields, jsonObject);
                    }
                }
            }

            return resultDocument;
        }

        ResultDocument ResultDocument(SearchResult searchResult, JsonObject jsonObject)
        {
            var resultDocument = searchResult.Results.Single(y => y.DocId == jsonObject.Get("docid"));

            var fields = MapProperties(resultDocument, jsonObject);

            if (fields.Any())
            {
                resultDocument.Fields = fields.ToDictionary(x => x, x => jsonObject.Get(x));
            }
            return resultDocument;
        }
    }
}