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
            var document = new Document("id").AddField("key", "value");
            Index.AddDocument(document);
            TankTopClient.StatusCode.Should().Be(HttpStatusCode.OK);
            var search = new Query("key:value").WithVariables();

            Index.UpdateVariables("id", new Dictionary<int, float> { { 2, 5 } });

            var searchResult = Index.Search(search);
            searchResult.Results.First().Variables[2].Should().BeInRange(5, 5);
        }
    }
}
