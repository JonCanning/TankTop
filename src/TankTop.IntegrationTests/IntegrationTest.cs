using NUnit.Framework;
using TankTop.Dto;

namespace TankTop.IntegrationTests
{
    abstract class IntegrationTest
    {
        protected readonly ITankTopClient TankTopClient;

        protected IntegrationTest()
        {
            //TankTopClient = new TankTopClient("http://:begyhuzatybu@vehehu.api.indexden.com");
            TankTopClient = new TankTopClient("http://:f8XQxRl2B5xQxC@d3px3.api.searchify.com");
        }

        [TestFixtureSetUp]
        public virtual void TestFixtureSetUp()
        {
            DeleteIndex();
        }

        void DeleteIndex()
        {
            TankTopClient.DeleteDocuments("TankTop", new Query("key:v*"));
            //Extensions.Try(() => TankTopClient.DeleteIndex("TankTop"));
        }

        [TestFixtureTearDown]
        public virtual void TestFixtureTearDown()
        {
            DeleteIndex();
        }
    }
}