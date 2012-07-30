using NUnit.Framework;
using TankTop.Dto;
using TankTop.Extensions;

namespace TankTop.IntegrationTests
{
    [SetUpFixture]
    class SetUpFixture
    {
        public static readonly TankTopClient TankTopClient = new TankTopClient("http://:begyhuzatybu@vehehu.api.indexden.com");

        public static Index Index;

        [SetUp]
        public void SetUp()
        {
            Index = TankTopClient.CreateIndex("TankTop", true);
            DeleteIndex();
        }

        [TearDown]
        public void TearDown()
        {
            DeleteIndex();
        }

        void DeleteIndex()
        {
            Index.DeleteDocuments(new Query("key:v*"));
        }
    }
}
