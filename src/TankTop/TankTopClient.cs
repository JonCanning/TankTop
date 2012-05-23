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
        }

        public TankTopClient(string baseAddress) : this(new WebClient(baseAddress)) { }

        public HttpStatusCode StatusCode
        {
            get { return webClient.StatusCode; }
        }

        public IEnumerable<Index> ReadIndexes()
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

        public Index ReadIndex(string indexName)
        {
            var resource = Resources.Indexes_Name.FormatWith(indexName);
            var index = webClient.Get<Index>(resource);
            index.Name = indexName;
            return index;
        }

        public void CreateDocument(string indexName, Document document)
        {
            document.Check();
            var resource = Resources.Indexes_Name_Docs.FormatWith(indexName);
            webClient.Put(resource, document.ToSerializable());
        }

        public void CreateDocuments(string indexName, IEnumerable<Document> documents)
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

        public void DeleteDocuments(string indexName, IEnumerable<string> docIds)
        {
            var resource = Resources.Indexes_Name_Docs.FormatWith(indexName);
            var deleteDocuments = docIds.Select(x => new { docid = x });
            webClient.Delete(resource, deleteDocuments);
        }

        public void UpdateDocumentVariables(string indexName, string docId, IDictionary<int, float> variables)
        {
            var resource = Resources.Indexes_Name_Docs_Variables.FormatWith(indexName);
            webClient.Put(resource, new { docid = docId, variables = variables });
        }

        public void UpdateDocumentCategories(string indexName, string docId, IDictionary<string, string> categories)
        {
            var resource = Resources.Indexes_Name_Docs_Categories.FormatWith(indexName);
            webClient.Put(resource, new { docid = docId, categories = categories });
        }

        public IDictionary<int, string> ReadIndexFunctions(string indexName)
        {
            var resource = Resources.Indexes_Name_Functions.FormatWith(indexName);
            var functions = webClient.Get<IEnumerable<FunctionDefinition>>(resource);
            return functions.ToDictionary(x => x.Num, x => x.Definition);
        }

        public void CreateIndexFunction(string indexName, int functionNumber, string functionDefinition)
        {
            var resource = Resources.Indexes_Name_Functions_Num.FormatWith(indexName, functionNumber);

            webClient.Put(resource, new { definition = functionDefinition });
        }

        public void DeleteIndexFunction(string indexName, int functionNumber)
        {
            var resource = Resources.Indexes_Name_Functions_Num.FormatWith(indexName, functionNumber);
            webClient.Delete(resource);
        }

        public SearchResult Search(string indexName, Search search)
        {
            search.Check();
            var resource = Resources.Indexes_Name_Search.FormatWith(indexName);
            var queryString = search.ToQueryString();
            var resourceAndQueryString = "{0}?{1}".FormatWith(resource, queryString);
            var searchResult = webClient.Get<SearchResult>(resourceAndQueryString);
            if (searchResult.IsNotNull() && searchResult.Results.IsNotNull() && searchResult.Results.Any())
            {
                var resultDocuments = JsonObject.Parse(webClient.Response).ArrayObjects("results").ConvertAll(x => ResultDocument(searchResult, x));
                searchResult.Results = resultDocuments;
            }
            return searchResult;
        }

        ResultDocument ResultDocument(SearchResult searchResult, JsonObject jsonObject)
        {
            var resultDocument = searchResult.Results.Single(y => y.DocId == jsonObject.Get("docid"));
            var fields = jsonObject.Select(y => y.Key).Where(y => !ResultDocumentProperties.Contains(y)).ToList();

            const string variable = "variable_";
            if (webClient.Response.Contains(variable))
            {
                resultDocument.Variables = new Dictionary<int, float>();
                foreach (var field in fields)
                {
                    if (!field.StartsWith(variable)) continue;
                    var intString = field.Replace(variable, string.Empty);
                    var i = int.Parse(intString);
                    var f = jsonObject.Get<float>(field);
                    resultDocument.Variables.Add(i, f);
                }
                fields.RemoveAll(x => x.StartsWith(variable));
            }

            const string category = "category_";
            if (webClient.Response.Contains(category))
            {
                resultDocument.Categories = new Dictionary<string, string>();
                foreach (var field in fields)
                {
                    if (!field.StartsWith(category)) continue;
                    var categoryName = field.Replace(category, string.Empty);
                    var categoryValue = jsonObject.Get<string>(field);
                    resultDocument.Categories.Add(categoryName, categoryValue);
                }
                fields.RemoveAll(x => x.StartsWith(category));
            }

            if (fields.Any())
            {
                resultDocument.Fields = new Dictionary<string, string>();
                foreach (var field in fields)
                {
                    resultDocument.Fields.Add(field, jsonObject.Get(field));
                }
            }
            return resultDocument;
        }
    }
}