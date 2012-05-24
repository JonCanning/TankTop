using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TankTop.Dto;
using TankTop.Extensions;

namespace TankTop.IntegrationTests.Indexing
{
    [TestFixture]
    class When_searching_for_a_document : IntegrationTest
    {
        [Test]
        public void Then_document_should_be_returned()
        {
            var index = TankTopClient.CreateIndex("TankTop");
            var document = new Document("id").AddField("key", "value");
            index.AddDocument(document);
            var search = new Query("key:value").WithFields("*");
            var searchResult = index.Search(search);
            searchResult.Results.Count().Should().Be(1);
            searchResult.Results.First().Fields["key"].Should().Be("value");
        }
    }

    /*    [TestFixture]
        class When_searching_for_documents_using_match_any : IntegrationTest
        {
            [Test]
            public void Then_document_should_be_returned()
            {
                var index = TankTopClient.CreateIndex("TankTop");
                var document = new Document("id").AddField("key", "value");
                index.AddDocument(document);
                var search = new Query("").WithQueryString("value").WithFields("key");
                var searchResult = index.Search(search);
                searchResult.Results.Count().Should().Be(1);
                searchResult.Results.First().Fields["key"].Should().Be("value");
            }
        }*/

    [TestFixture]
    class When_searching_and_filtering_using_categories : IntegrationTest
    {
        [Test]
        public void Then_document_should_be_returned()
        {
            var index = TankTopClient.CreateIndex("TankTop");
            var doc1 = new Document("id1").AddField("key", "value1").AddCategory("cat", "1");
            var doc2 = new Document("id2").AddField("key", "value2").AddCategory("cat", "2");
            index.AddDocuments(doc1, doc2);
            var search = new Query("key:value*").WithFields("*").WithCategoryFilter("cat", "1");
            var searchResult = index.Search(search);
            searchResult.Results.Count().Should().Be(1);
            searchResult.Results.First().Fields["key"].Should().Be("value1");
        }
    }
}