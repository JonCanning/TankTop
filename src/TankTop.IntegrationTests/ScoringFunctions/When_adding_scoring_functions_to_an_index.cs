using System.Net;
using FluentAssertions;
using NUnit.Framework;
using TankTop.Extensions;

namespace TankTop.IntegrationTests.ScoringFunctions
{
    [TestFixture]
    class When_adding_scoring_functions_to_an_index : IntegrationTest
    {
        [Test]
        public void Then_functions_should_be_stored()
        {
            var index = TankTopClient.CreateIndex("TankTop");
            index.CreateFunction(1, "age");
            TankTopClient.StatusCode.Should().Be(HttpStatusCode.OK);

            var functions = index.GetFunctions();
            TankTopClient.StatusCode.Should().Be(HttpStatusCode.OK);
            functions[1].Should().Be("age");
        }
    }
}
