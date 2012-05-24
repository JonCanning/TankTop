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
            var document = new Document("id").AddField("key", "value");
            index.AddDocument(document);
            TankTopClient.StatusCode.Should().Be(HttpStatusCode.OK);
            var search = new Query("key:value").WithVariables();
            var searchResult = index.Search(search);
            //searchResult.Results.First().Variables.Should().BeNull();

            index.UpdateVariables("id", 0, 0, 5);

            searchResult = index.Search(search);
            searchResult.Results.First().Variables[2].Should().BeInRange(5, 5);
        }
    }
}
