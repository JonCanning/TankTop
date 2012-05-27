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
            var document = new Document("id").AddField("key", "value");
            Index.AddDocument(document);
            var query = new Query("key:value").WithReturnedFields("*");
            var searchResult = Index.Search(query);
            searchResult.Results.Count().Should().Be(1);
            searchResult.Results.First().Fields["key"].Should().Be("value");
        }
    }

    public class TestClass
    {
        public string Key { get; set; }
        public string Foo { get; set; }
    }
}