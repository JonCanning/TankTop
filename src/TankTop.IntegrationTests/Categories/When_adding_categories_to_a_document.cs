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
            var document = new Document("id") { Fields = new Dictionary<string, string> { { "key", "value" } } };
            Index.AddDocument(document);
            TankTopClient.StatusCode.Should().Be(HttpStatusCode.OK);
            var search = new Query("key:value").WithCategories();
            var searchResult = Index.Search(search);
            searchResult.Results.First().Categories.Should().BeNull();

            Index.UpdateCategories("id", new Dictionary<string, string> { { "cat", "val" } });

            searchResult = Index.Search(search);
            searchResult.Results.First().Categories["cat"].Should().Be("val");
            searchResult.Facets["cat"]["val"].Should().Be(1);
        }
    }
}
