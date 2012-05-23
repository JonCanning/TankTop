using System.Collections.Generic;
using System.Linq;
using System.Net;
using FluentAssertions;
using NUnit.Framework;
using TankTop.Dto;
using TankTop.Extensions;

namespace TankTop.IntegrationTests.ScoringVariables
{
    [TestFixture]
    class When_adding_scoring_variables_to_a_document : IntegrationTest
    {
        [Test]
        public void Then_variables_should_be_returned()
        {
            var index = TankTopClient.CreateIndex("TankTop");
            var document = new Document { DocId = "id", Fields = new Dictionary<string, string> { { "key", "value" } } };
            index.AddDocument(document);
            TankTopClient.StatusCode.Should().Be(HttpStatusCode.OK);
            var search = new Search { QueryString = "key:value", FetchVariables = true };
            var searchResult = index.Search(search);
            searchResult.Results.First().Variables.Should().BeNull();

            index.UpdateDocumentVariables("id", new Dictionary<int, float> { { 3, 5 } });

            searchResult = index.Search(search);
            searchResult.Results.First().Variables[3].Should().BeInRange(5, 5);
        }
    }
}
