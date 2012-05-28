using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TankTop.Dto;
using TankTop.Extensions;

namespace TankTop.IntegrationTests.Indexing
{
    [TestFixture]
    class When_searching_for_a_document_and_converting_fields : IntegrationTest
    {
        [Test]
        public void Then_document_should_be_returned()
        {
            var document = new Document<TestClass>("id", new TestClass { Key = "value" }).AddAllFields();
            Index.AddDocument(document);
            var query = new Query<TestClass>("value").WithSearchFields(x => x.Foo, x => x.Key).WithAllFields();
            var searchResult = Index.Search(query);
            var testClass = searchResult.Results.First().Obj;
            testClass.Key.Should().Be("value");
        }
    }
}