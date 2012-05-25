using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using TankTop.Dto;
using TankTop.Extensions;

namespace TankTop.Tests.TankTopClientFixtures
{
    [TestFixture]
    class ExtensionTests
    {
        [Test]
        public void When_using_Search_overload_with_fields()
        {
            var index = new Index();
            index.TankTopClient = Substitute.For<ITankTopClient>();
            var query = new Query("value");
            index.Search(query, "field1", "field2");
            query.QueryText.Should().Be("field1:value OR field2:value");
        }

        [Test]
        public void When_using_Search_overload_with_expressions()
        {
            var index = new Index();
            index.TankTopClient = Substitute.For<ITankTopClient>();
            var query = new Query("value");
            index.Search<SearchTest>(query, x => x.Field1, x => x.Field2);
            query.QueryText.Should().Be("field1:value OR field2:value");
        }
    }

    public class SearchTest
    {
        public string Field1 { get; set; }
        public string Field2 { get; set; }
    }
}
