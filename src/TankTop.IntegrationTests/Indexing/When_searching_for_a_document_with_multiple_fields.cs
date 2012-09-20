using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TankTop.Dto;
using TankTop.Extensions;

namespace TankTop.IntegrationTests.Indexing
{
    [TestFixture]
    class When_searching_for_a_document_with_multiple_fields : IntegrationTest
    {
        [Test]
        public void Then_document_should_be_returned()
        {
            var document = new Document("id").AddField("key1", "value").AddField("key2", "value");
            Index.AddDocument(document);
            var query = new Query("key1:value").WithFields("key1", "key2");
            var searchResult = Index.Search(query);
            searchResult.Results.Count().Should().Be(1);
            searchResult.Results.First().Fields["key1"].Should().Be("value");
            searchResult.Results.First().Fields["key2"].Should().Be("value");
        }
    }
}