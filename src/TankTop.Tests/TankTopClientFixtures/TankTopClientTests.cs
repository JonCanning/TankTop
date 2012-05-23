using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using ServiceStack.Text;
using TankTop.Dto;
using TankTop.Extensions;

namespace TankTop.Tests.TankTopClientFixtures
{
    [TestFixture]
    class TankTopClientTests
    {
        IWebClient webClient;
        ITankTopClient tankTopClient;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            webClient = Substitute.For<IWebClient>();
            tankTopClient = new TankTopClient(webClient);
        }

        [Test]
        public void When_creating_an_index()
        {
            var resource = Resources.Indexes_Name.FormatWith("Index");
            webClient.Put<Index>(resource, Arg.Any<object>()).Returns(new Index());
            var index = tankTopClient.CreateIndex("Index", true);
            webClient.Received().Put<Index>(resource, Arg.Is<object>(x => x.GetValue<bool>("public_search")));
            index.Name.Should().Be("Index");
            index.TankTopClient.Should().Be(tankTopClient);
        }

        [Test]
        public void When_creating_an_index_that_already_exists()
        {
            var resource = Resources.Indexes_Name.FormatWith("Index");
            webClient.Get<Index>(resource).Returns(new Index());
            webClient.StatusCode.Returns(HttpStatusCode.NoContent);
            var index = tankTopClient.CreateIndex("Index", true);
            webClient.Received().Put<Index>(resource, Arg.Is<object>(x => x.GetValue<bool>("public_search")));
            index.Name.Should().Be("Index");
            index.TankTopClient.Should().Be(tankTopClient);
        }

        [Test]
        public void When_reading_indexes()
        {
            var resource = Resources.Indexes;
            webClient.Get<IDictionary<string, Index>>(resource).Returns(new Dictionary<string, Index> {
                                                                                                                         {"One", new Index()},
                                                                                                                         {"Two", new Index()}
                                                                                                                     });

            var indexes = tankTopClient.ReadIndexes();
            webClient.Received().Get<IDictionary<string, Index>>(resource);
            indexes.ElementAt(0).Name.Should().Be("One");
            indexes.ElementAt(1).Name.Should().Be("Two");
        }

        [Test]
        public void When_reading_an_index()
        {
            var resource = Resources.Indexes_Name.FormatWith("Index");
            webClient.Get<Index>(resource).Returns(new Index());

            var index = tankTopClient.ReadIndex("Index");

            webClient.Received().Get<Index>(resource);
            index.Name.Should().Be("Index");
        }

        [Test]
        public void When_deleting_an_index()
        {
            var resource = Resources.Indexes_Name.FormatWith("Index");
            tankTopClient.DeleteIndex("Index");
            webClient.Received().Delete(resource);
        }

        [Test]
        public void When_indexing_a_document()
        {
            var index = new Index { Name = "Index", TankTopClient = tankTopClient };
            var document = new Document { DocId = "id", Fields = new Dictionary<string, string> { { "key", "value" } } };
            index.AddDocument(document);
            var resource = Resources.Indexes_Name_Docs.FormatWith(index.Name);
            webClient.Received().Put(resource, document.ToSerializable());
        }

        [Test]
        public void When_indexing_a_document_without_docid()
        {
            var index = new Index { Name = "Index" };
            var document = new Document();

            Action action = () => index.AddDocument(document);
            action.ShouldThrow<NullReferenceException>("DocId");
        }

        [Test]
        public void When_indexing_a_document_without_fields()
        {
            var index = new Index { Name = "Index" };
            var document = new Document { DocId = "id" };

            Action action = () => index.AddDocument(document);
            action.ShouldThrow<NullReferenceException>("DocId");
        }

        [Test]
        public void When_indexing_a_document_batch()
        {
            var index = new Index { Name = "Index", TankTopClient = tankTopClient };
            var documents = new[] { new Document { DocId = "id", Fields = new Dictionary<string, string> { { "key", "value" } } }, new Document { DocId = "id", Fields = new Dictionary<string, string> { { "key", "value" } } } };
            index.AddDocuments(documents);
            var resource = Resources.Indexes_Name_Docs.FormatWith(index.Name);
            webClient.Received().Put(resource, Arg.Is<IEnumerable<object>>(x => x.SequenceEqual(documents.Select(y => y.ToSerializable()))));
        }

        [Test]
        public void When_deleting_a_document()
        {
            var resource = Resources.Indexes_Name_Docs.FormatWith("Index");

            var index = new Index { Name = "Index", TankTopClient = tankTopClient };
            index.DeleteDocument("id");

            webClient.Received().Delete(resource, Arg.Is<object>(x => x.GetValue<string>("docid") == "id"));
        }

        [Test]
        public void When_deleting_a_document_batch()
        {
            var resource = Resources.Indexes_Name_Docs.FormatWith("Index");

            var index = new Index { Name = "Index", TankTopClient = tankTopClient };
            var deleteDocuments = new[] { "1", "2" };
            index.DeleteDocuments(deleteDocuments);

            webClient.Received().Delete(resource, Arg.Is<IEnumerable<object>>(x => x.ElementAt(0).GetValue<string>("docid") == "1" && x.ElementAt(1).GetValue<string>("docid") == "2"));
        }

        [Test]
        public void When_adding_variables_to_a_document()
        {
            var resource = Resources.Indexes_Name_Docs_Variables.FormatWith("Index");

            tankTopClient.UpdateDocumentVariables("Index", "id", new Dictionary<int, float>());

            webClient.Received().Put(resource, Arg.Is<object>(x => x.GetValue<string>("docid") == "id"));
        }

        [Test]
        public void When_adding_variables_to_a_document_by_extension()
        {
            var resource = Resources.Indexes_Name_Docs_Variables.FormatWith("Index");
            var index = new Index { Name = "Index", TankTopClient = tankTopClient };

            index.UpdateDocumentVariables("id", new Dictionary<int, float>());

            webClient.Received().Put(resource, Arg.Is<object>(x => x.GetValue<string>("docid") == "id"));
        }

        [Test]
        public void When_adding_categories_to_a_document()
        {
            var resource = Resources.Indexes_Name_Docs_Categories.FormatWith("Index");

            tankTopClient.UpdateDocumentCategories("Index", "id", new Dictionary<string, string>());

            webClient.Received().Put(resource, Arg.Is<object>(x => x.GetValue<string>("docid") == "id"));
        }

        [Test]
        public void When_adding_categories_to_a_document_by_extension()
        {
            var resource = Resources.Indexes_Name_Docs_Categories.FormatWith("Index");
            var index = new Index { Name = "Index", TankTopClient = tankTopClient };

            index.UpdateDocumentCategories("id", new Dictionary<string, string>());

            webClient.Received().Put(resource, Arg.Is<object>(x => x.GetValue<string>("docid") == "id"));
        }

        [Test]
        public void When_reading_index_functions()
        {
            var resource = Resources.Indexes_Name_Functions.FormatWith("Index");

            tankTopClient.ReadIndexFunctions("Index");

            webClient.Received().Get<IEnumerable<FunctionDefinition>>(resource);
        }

        [Test]
        public void When_reading_index_functions_by_extension()
        {
            var resource = Resources.Indexes_Name_Functions.FormatWith("Index");
            var index = new Index { Name = "Index", TankTopClient = tankTopClient };

            index.ReadFunctions();

            webClient.Received().Get<IEnumerable<FunctionDefinition>>(resource);
        }

        [Test]
        public void When_creating_a_function()
        {
            var resource = Resources.Indexes_Name_Functions_Num.FormatWith("Index", 0);

            tankTopClient.CreateIndexFunction("Index", 0, "def");

            webClient.Received().Put(resource, Arg.Is<object>(x => x.GetValue<string>("definition") == "def"));
        }

        [Test]
        public void When_creating_a_function_by_extension()
        {
            var resource = Resources.Indexes_Name_Functions_Num.FormatWith("Index", 0);
            var index = new Index { Name = "Index", TankTopClient = tankTopClient };

            index.CreateFunction(0, "def");

            webClient.Received().Put(resource, Arg.Is<object>(x => x.GetValue<string>("definition") == "def"));
        }

        [Test]
        public void When_deleting_a_function()
        {
            var resource = Resources.Indexes_Name_Functions_Num.FormatWith("Index", 0);

            tankTopClient.DeleteIndexFunction("Index", 0);

            webClient.Received().Delete(resource);
        }

        [Test]
        public void When_deleting_a_function_by_extension()
        {
            var resource = Resources.Indexes_Name_Functions_Num.FormatWith("Index", 0);

            var index = new Index { Name = "Index", TankTopClient = tankTopClient };

            index.DeleteFunction(0);

            webClient.Received().Delete(resource);
        }

        [Test]
        public void When_searching()
        {
            var resource = Resources.Indexes_Name_Search.FormatWith("Index");

            var search = new Search
            {
                QueryString = "field:value",
                Var = new Dictionary<int, float> { { 1, 1 } },
                CategoryFilters = new Dictionary<string, IEnumerable<string>> { { "category", new[] { "one", "two" } } },
                VariableFilters = new Dictionary<int, string> { { 1, "2:3" } },
                FunctionFilters = new Dictionary<int, string> { { 2, "3:4" } },
                FetchVariables = true
            };
            webClient.Get<SearchResult>(Arg.Any<string>()).Returns(new SearchResult());

            tankTopClient.Search("Index", search);

            var resourceQuery = "{0}?q={1}&fetch_variables=true&category_filters={2}&var1=1&filter_docvar1=2:3&filter_function2=3:4".FormatWith(resource, search.QueryString.UrlEncode(), "{category:[one,two]}");
            webClient.Received().Get<SearchResult>(resourceQuery);
        }

        [Test]
        public void When_searching_by_extension()
        {
            var resource = Resources.Indexes_Name_Search.FormatWith("Index");

            var search = new Search
            {
                QueryString = "field:value",
                Var = new Dictionary<int, float> { { 1, 1 } },
                CategoryFilters = new Dictionary<string, IEnumerable<string>> { { "category", new[] { "one", "two" } } },
                VariableFilters = new Dictionary<int, string> { { 1, "2:3" } },
                FunctionFilters = new Dictionary<int, string> { { 2, "3:4" } },
                FetchVariables = true
            };
            webClient.Get<SearchResult>(Arg.Any<string>()).Returns(new SearchResult());
            var index = new Index { Name = "Index", TankTopClient = tankTopClient };
            index.Search(search);
            var resourceQuery = "{0}?q={1}&fetch_variables=true&category_filters={2}&var1=1&filter_docvar1=2:3&filter_function2=3:4".FormatWith(resource, search.QueryString.UrlEncode(), "{category:[one,two]}");
            webClient.Received().Get<SearchResult>(resourceQuery);
        }
    }

    public static class TestExtensions
    {
        public static T GetValue<T>(this object obj, string propertyName)
        {
            return (T)obj.GetType().GetProperty(propertyName).GetValue(obj, null);
        }
    }
}
