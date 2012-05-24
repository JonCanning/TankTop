using System;
using System.Net;
using FluentAssertions;
using NUnit.Framework;

namespace TankTop.IntegrationTests.IndexManagement
{
    [TestFixture]
    class When_reading_indexes : IntegrationTest
    {
        [Test]
        public void Then_reading_indexes_should_return_ok()
        {
            TankTopClient.GetIndexes();
            TankTopClient.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public void Then_indexes_should_be_returned()
        {
            TankTopClient.GetIndexes().Should().NotBeEmpty();
        }
    }
}