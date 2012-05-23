using System.Net;
using FluentAssertions;
using NUnit.Framework;
using TankTop.Extensions;

namespace TankTop.IntegrationTests.ScoringFunctions
{
    [TestFixture]
    class When_deleting_scoring_functions_from_an_index : IntegrationTest
    {
        [Test]
        public void Then_functions_should_be_deleted()
        {
            var index = TankTopClient.CreateIndex("TankTop");
            index.CreateFunction(1, "age");
            TankTopClient.StatusCode.Should().Be(HttpStatusCode.OK);

            var functions = index.ReadFunctions();
            TankTopClient.StatusCode.Should().Be(HttpStatusCode.OK);
            functions[1].Should().Be("age");

            index.DeleteFunction(1);

            functions = index.ReadFunctions();
            TankTopClient.StatusCode.Should().Be(HttpStatusCode.OK);
            functions.ContainsKey(1).Should().BeFalse();
        }
    }
}