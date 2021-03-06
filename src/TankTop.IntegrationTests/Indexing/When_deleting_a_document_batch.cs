using System.Linq;
using System.Net;
using FluentAssertions;
using NUnit.Framework;
using TankTop.Dto;
using TankTop.Extensions;

namespace TankTop.IntegrationTests.Indexing
{
    [TestFixture]
    class When_deleting_a_document_batch : IntegrationTest
    {
        [Test]
        public void Then_documents_should_be_deleted()
        {
            var documents = new[] {
                                      new Document("1").AddField("key", "value"),
                                      new Document("2").AddField("key", "value")
                                  };
            Index.AddDocuments(documents);
            TankTopClient.StatusCode.Should().Be(HttpStatusCode.OK);
            var search = new Query("key:value").WithFields("*");
            var searchResult = Index.Search(search);
            searchResult.Results.Count().Should().Be(2);

            Index.DeleteDocuments("1", "2");
            TankTopClient.StatusCode.Should().Be(HttpStatusCode.OK);
            searchResult = Index.Search(search);
            searchResult.Results.Count().Should().Be(0);
        }
    }
}