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
            var index = TankTopClient.CreateIndex("TankTop");
            var documents = new[] {
                                      new Document("1").AddField("key", "value"),
                                      new Document("2").AddField("key", "value")
                                  };
            index.AddDocuments(documents);
            TankTopClient.StatusCode.Should().Be(HttpStatusCode.OK);
            var search = new Query("key:value").WithFields("*");
            var searchResult = index.Search(search);
            searchResult.Results.Count().Should().Be(2);

            index.DeleteDocuments("1", "2");
            TankTopClient.StatusCode.Should().Be(HttpStatusCode.OK);
            searchResult = index.Search(search);
            searchResult.Results.Count().Should().Be(0);
        }
    }
}