using System.Collections.Generic;
using System.Net;
using FluentAssertions;
using NUnit.Framework;
using TankTop.Dto;
using TankTop.Extensions;

namespace TankTop.IntegrationTests.Indexing
{
    [TestFixture]
    class When_indexing_a_document : IntegrationTest
    {
        [Test]
        public void Then_document_should_be_indexed()
        {
            var index = TankTopClient.CreateIndex("TankTop");
            var document = new Document { DocId = "id", Fields = new Dictionary<string, string> { { "key", "value" } } };
            index.AddDocument(document);
            TankTopClient.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
