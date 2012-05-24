using NUnit.Framework;
using TankTop.Dto;

namespace TankTop.IntegrationTests
{
    abstract class IntegrationTest
    {
        protected readonly ITankTopClient TankTopClient;

        protected IntegrationTest()
        {
            TankTopClient = new TankTopClient("");
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