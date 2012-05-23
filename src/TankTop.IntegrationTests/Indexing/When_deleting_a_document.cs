using System.Collections.Generic;
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
            var document = new Document { DocId = "id", Fields = new Dictionary<string, string> { { "key", "value" } } };
            index.AddDocument(document);
            var search = new Search { QueryString = "key:value", Fetch = new[] { "*" } };
            var searchResult = index.Search(search);
            searchResult.Results.Count().Should().Be(1);

            index.DeleteDocument("id");
            searchResult = index.Search(search);
            searchResult.Results.Count().Should().Be(0);
        }
    }
}