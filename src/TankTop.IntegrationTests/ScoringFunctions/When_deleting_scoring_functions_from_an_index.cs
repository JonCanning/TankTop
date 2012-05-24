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
            Index.CreateFunction(1, "age");
            TankTopClient.StatusCode.Should().Be(HttpStatusCode.OK);

            var functions = Index.GetFunctions();
            TankTopClient.StatusCode.Should().Be(HttpStatusCode.OK);
            functions[1].Should().Be("age");

            Index.DeleteFunction(1);

            functions = Index.GetFunctions();
            TankTopClient.StatusCode.Should().Be(HttpStatusCode.OK);
            functions.ContainsKey(1).Should().BeFalse();
        }
    }
}