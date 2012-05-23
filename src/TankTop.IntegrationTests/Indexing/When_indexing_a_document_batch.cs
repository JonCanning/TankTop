using System.Collections.Generic;
using System.Net;
using FluentAssertions;
using NUnit.Framework;
using TankTop.Dto;
using TankTop.Extensions;

namespace TankTop.IntegrationTests.Indexing
{
    [TestFixture]
    class When_indexing_a_document_batch : IntegrationTest
    {
        [Test]
        public void Then_documents_should_be_indexed()
        {
            var index = TankTopClient.CreateIndex("TankTop");
            var documents = new[]{ 
                                     new Document { DocId = "1", Fields = new Dictionary<string, string> { { "key", "value" } } },
                                     new Document { DocId = "2", Fields = new Dictionary<string, string> { { "key", "value" } } }};
            index.AddDocuments(documents);
            TankTopClient.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}