using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TankTop.Dto;
using TankTop.Extensions;

namespace TankTop.IntegrationTests.Indexing
{
    [TestFixture]
    class When_searching_for_documents_using_match_any : IntegrationTest
    {
        [Test]
        public void Then_document_should_be_returned()
        {
            var document = new Document("id").AddField("key", "value");
            Index.AddDocument(document);
            var search = new Query("value").WithFields("key").MatchAnyField();
            var searchResult = Index.Search(search);
            searchResult.Results.Count().Should().Be(1);
            searchResult.Results.First().Fields["key"].Should().Be("value");
        }
    }
}