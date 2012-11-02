using System;
using System.Collections.Generic;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using ServiceStack.Text;
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
            var query = new Query("value").WithSearchFields("field1", "field2");
            index.Search(query);
            query.QueryText.Should().Be("field1:value OR field2:value");
        }

        [Test]
        public void When_using_Search_overload_with_expressions()
        {
            var index = new Index();
            index.TankTopClient = Substitute.For<ITankTopClient>();
            var query = new Query<SearchTest>("value").WithSearchFields(x => x.Field1, x => x.Field2);
            index.Search(query);
            query.QueryText.Should().Be("field1:value OR field2:value");
        }

        [Test]
        public void When_searching_and_Var_requires_url_encoding()
        {
            var query = new Query("test:Something").WithQueryVariable(0, new DateTime(2000, 1, 1).Ticks/TimeSpan.TicksPerSecond);
            query.ToQueryString().Should().Be("q=test%3aSomething&var0=6.308228E%2b10");
        }
    }

    public class SearchTest
    {
        public string Field1 { get; set; }
        public string Field2 { get; set; }
    }
}
