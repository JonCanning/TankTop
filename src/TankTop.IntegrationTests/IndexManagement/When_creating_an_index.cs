using FluentAssertions;
using NUnit.Framework;
using TankTop.Dto;

namespace TankTop.IntegrationTests.IndexManagement
{
    [TestFixture]
    class When_creating_an_index : IntegrationTest
    {
        Index index;
        public override void TestFixtureSetUp()
        {
            base.TestFixtureSetUp();
            index = TankTopClient.CreateIndex("TankTop", true);
        }

        [Test]
        public void Then_index_should_have_publicsearch_true()
        {
            index.Public_Search.Should().BeTrue();
        }

        [Test]
        public void Then_index_should_have_name()
        {
            index.Name.Should().Be("TankTop");
        }
    }
}
