using NUnit.Framework;

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
            Extensions.Try(() => TankTopClient.DeleteIndex("TankTop"));
        }

        [TestFixtureTearDown]
        public virtual void TestFixtureTearDown()
        {
            DeleteIndex();
        }
    }
}