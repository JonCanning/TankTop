using System.Linq;
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
            var index = TankTopClient.CreateIndex("TankTop");
            var search = new Query("key:value").WithFields("*");
            var searchResult = index.Search(search);
            searchResult.Results.Count().Should().Be(0);
            var document = new Document("1)").AddField("key", "value");
            index.AddDocument(document);
            searchResult = index.Search(search);
            searchResult.Results.Count().Should().Be(1);

            index.DeleteDocument("1");
            searchResult = index.Search(search);
            searchResult.Results.Count().Should().Be(0);
        }
    }
}