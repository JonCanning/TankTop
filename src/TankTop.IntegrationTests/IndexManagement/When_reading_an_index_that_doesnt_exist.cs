using System;
using System.Net;
using FluentAssertions;
using NUnit.Framework;

namespace TankTop.IntegrationTests.IndexManagement
{
    [TestFixture]
    class When_reading_an_index_that_doesnt_exist : IntegrationTest
    {
        [Test]
        public void Then_reading_index_should_return_ok()
        {
            Action action = () => TankTopClient.ReadIndex("Foo");
            action.ShouldThrow<WebException>();
        }
    }
}