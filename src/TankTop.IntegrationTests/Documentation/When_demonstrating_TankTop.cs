using System.Collections.Generic;
using System.Linq;
using System.Threading;
using NSubstitute;
using NUnit.Framework;
using TankTop.Dto;
using TankTop.Extensions;

namespace TankTop.IntegrationTests.Documentation
{
    [TestFixture]
    class When_demonstrating_TankTop
    {
        TankTopClient tankTopClient;
        Index index;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            tankTopClient = new TankTopClient("http://:begyhuzatybu@vehehu.api.indexden.com");
            index = tankTopClient.CreateIndex("Stock");
            while (!index.Started)
            {
                Thread.Sleep(1000);
                index = tankTopClient.GetIndex("Stock");
            }
        }

        [Test]
        public void Index_a_document_using_non_generic_syntax()
        {
            var stockItem = new StockItem
                            {
                                Id = 1,
                                Title = "Edam",
                                Description = "Tasty spherical cheese from Holland",
                                Price = 12.34M,
                                Categories = new Dictionary<string, string> {
                                                                                {"Country", "Holland"},
                                                                                {"PriceRange", "11-20"}
                                                                            }
                            };

            var document = new Document(stockItem.Id.ToString())
                .AddField("title", stockItem.Title)
                .AddField("description", stockItem.Description)
                .AddVariable(0, stockItem.Price)
                .AddCategories(stockItem.Categories);

            index.AddDocument(document);

            var query = new Query("title:Edam").WithAllFields().WithVariables();
            var searchResult = index.Search(query);
            var resultDocument = searchResult.Results.First();

            var resultStockItem = new StockItem
                                  {
                                      Id = int.Parse(resultDocument.DocId),
                                      Title = resultDocument.Fields["title"],
                                      Description = resultDocument.Fields["description"],
                                      Price = (decimal)resultDocument.Variables[0],
                                      Categories = resultDocument.Categories
                                  };

            Assert.AreEqual("Edam", resultStockItem.Title);
            Assert.AreEqual(12.34, resultStockItem.Price);
        }

        [Test]
        public void Index_a_document_using_generic_syntax()
        {
            var stockItem = new StockItem
                            {
                                Id = 1,
                                Title = "Edam",
                                Description = "Tasty spherical cheese from Holland",
                                Price = 12.34M,
                                Categories = new Dictionary<string, string> {
                                                                                {"Country", "Holland"},
                                                                                {"PriceRange", "11-20"}
                                                                            }
                            };

            var document = new Document<StockItem>(stockItem.Id.ToString(), stockItem)
                .AddFields(x => x.Title, x => x.Description)
                .AddVariable(0, stockItem.Price)
                .AddCategories(stockItem.Categories);

            index.AddDocument(document);

            var query = new Query<StockItem>("title:Edam");
            var searchResult = index.Search(query);

            var resultStockItem = searchResult.Results.First().Obj;

            Assert.AreEqual("Edam", resultStockItem.Title);
            Assert.AreEqual(12.34, resultStockItem.Price);
        }

        [Test]
        public void Then_mock_ITankTopClient()
        {
            var mockTankTopClient = Substitute.For<ITankTopClient>();
            var myIndex = new Index { Name = "MyIndex", TankTopClient = mockTankTopClient };
            var document = new Document("id").AddField("key", "value");
            myIndex.AddDocument(document);
            mockTankTopClient.Received().AddDocument("MyIndex", document);
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            index.Delete();
        }
    }
}