using System.Net;
using FluentAssertions;
using NUnit.Framework;

namespace TankTop.IntegrationTests.IndexManagement
{
    [TestFixture]
    class When_reading_an_index : IntegrationTest
    {
        [TestFixtureSetUp]
        public override void TestFixtureSetUp()
        {
            TankTopClient.CreateIndex("TankTop");
        }

        [Test]
        public void Then_reading_index_should_return_ok()
        {
            TankTopClient.GetIndex("TankTop");
            TankTopClient.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}