using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using ServiceStack.Text;
using TankTop.Dto;
using TankTop.Extensions;

namespace TankTop.IntegrationTests.Indexing
{
    [TestFixture]
    class When_searching_for_a_document : IntegrationTest
    {
        [Test]
        public void Then_document_should_be_returned()
        {
            var document = new Document("id").AddField("key", "value");
            Index.AddDocument(document);
            var query = new Query("key:value").WithReturnedFields("*");
            var searchResult = Index.Search(query);
            searchResult.Results.Count().Should().Be(1);
            searchResult.Results.First().Fields["key"].Should().Be("value");
        }
    }

    [TestFixture]
    class When_searching_for_a_document_and_converting_fields : IntegrationTest
    {
        [Test]
        public void Then_document_should_be_returned()
        {
            var document = new Document<TestClass>("id", new TestClass { Key = "value" });
            Index.AddDocument(document);
            var query = new Query<TestClass>("value").WithSearchFields(x => x.Foo, x => x.Key).WithAllFields();
            var searchResult = Index.Search(query, (x, y) => x.Foo = y.Get("key"));
            var testClass = searchResult.Results.First().Fields;
            testClass.Key.Should().Be("value");
            testClass.Foo.Should().Be("value");
        }
    }

    public class TestClass
    {
        public string Key { get; set; }
        public string Foo { get; set; }
    }
}