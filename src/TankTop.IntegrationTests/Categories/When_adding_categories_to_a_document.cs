using System.Collections.Generic;
using System.Linq;
using System.Net;
using FluentAssertions;
using NUnit.Framework;
using TankTop.Dto;
using TankTop.Extensions;

namespace TankTop.IntegrationTests.Categories
{
    [TestFixture]
    class When_adding_categories_to_a_document : IntegrationTest
    {
        [Test]
        public void Then_categories_should_be_returned()
        {
            var index = TankTopClient.CreateIndex("TankTop");
            var document = new Document("id") { Fields = new Dictionary<string, string> { { "key", "value" } } };
            index.AddDocument(document);
            TankTopClient.StatusCode.Should().Be(HttpStatusCode.OK);
            var search = new Query("key:value").WithCategories();
            var searchResult = index.Search(search);
            searchResult.Results.First().Categories.Should().BeNull();

            index.UpdateCategories("id", new Dictionary<string, string> { { "cat", "val" } });

            searchResult = index.Search(search);
            searchResult.Results.First().Categories["cat"].Should().Be("val");
            searchResult.Facets["cat"]["val"].Should().Be(1);
        }
    }
}
