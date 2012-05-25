using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TankTop.Dto;
using TankTop.Extensions;

namespace TankTop.IntegrationTests.Indexing
{
    [TestFixture]
    class When_searching_and_filtering_using_categories : IntegrationTest
    {
        [Test]
        public void Then_document_should_be_returned()
        {
            var doc1 = new Document("id1").AddField("key", "value").AddCategory("cat", "1");
            var doc2 = new Document("id2").AddField("key", "value").AddCategory("cat", "2");
            Index.AddDocuments(doc1, doc2);
            var query = new Query("key:value").WithReturnedFields("*").WithCategoryFilter("cat", "1");
            var searchResult = Index.Search(query);
            searchResult.Results.Count().Should().Be(1);
            searchResult.Results.First().DocId.Should().Be("id1");
        }
    }
}