using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TankTop.Dto;
using TankTop.Extensions;

namespace TankTop.IntegrationTests.Indexing
{
    [TestFixture]
    class When_searching_with_snippet : IntegrationTest
    {
        [Test]
        public void Then_result_should_contain_highlighted_snippet()
        {
            Index.AddFunction(1, "-km(query.var[0], query.var[1], doc.var[0], doc.var[1])");
            var doc = new Document("id").AddField("key", "value of the market is over two thousand pounds");
            Index.AddDocument(doc);
            var query = new Query("key:market").WithFields("*").WithSnippetFromFields("key");
            var searchResult = Index.Search(query);
            searchResult.Results.First().Snippets["key"].Should().Contain("<b>market</b>");
        }
    }
}