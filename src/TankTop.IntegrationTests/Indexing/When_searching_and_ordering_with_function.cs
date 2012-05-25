using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TankTop.Dto;
using TankTop.Extensions;

namespace TankTop.IntegrationTests.Indexing
{
    [TestFixture]
    class When_searching_and_ordering_with_function : IntegrationTest
    {
        [Test]
        public void Then_results_should_be_ordered()
        {
            Index.AddFunction(1, "-km(query.var[0], query.var[1], doc.var[0], doc.var[1])");
            var doc1 = new Document("id1").AddField("key", "value").AddVariable(0, 2).AddVariable(1, 2);
            var doc2 = new Document("id2").AddField("key", "value").AddVariable(0, 1).AddVariable(1, 1);
            Index.AddDocuments(doc1, doc2);
            var query = new Query("key:value").WithReturnedFields("*").WithScoringFunction(1).WithQueryVariable(0, 0).WithQueryVariable(1, 0);
            var searchResult = Index.Search(query);
            searchResult.Results.Count().Should().Be(2);
            searchResult.Results.First().DocId.Should().Be("id2");
        }
    }
}