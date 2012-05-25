using System.Linq;
using System.Net;
using FluentAssertions;
using NUnit.Framework;
using TankTop.Dto;
using TankTop.Extensions;

namespace TankTop.IntegrationTests.Indexing
{
    [TestFixture]
    class When_deleting_a_document : IntegrationTest
    {
        [Test]
        public void Then_document_should_be_deleted()
        {
            var document = new Document("1").AddField("key", "value");
            Index.AddDocument(document);
            TankTopClient.StatusCode.Should().Be(HttpStatusCode.OK);
            var search = new Query("key:value").WithReturnedFields("*");
            var searchResult = Index.Search(search);
            searchResult.Results.Count().Should().Be(1);
            Index.DeleteDocument("1");
            TankTopClient.StatusCode.Should().Be(HttpStatusCode.OK);
            searchResult = Index.Search(search);
            searchResult.Results.Count().Should().Be(0);
        }
    }
}