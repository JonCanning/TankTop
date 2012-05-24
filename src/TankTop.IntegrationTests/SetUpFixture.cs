using NUnit.Framework;
using TankTop.Dto;

namespace TankTop.IntegrationTests
{
    [SetUpFixture]
    class SetUpFixture
    {
        public static readonly TankTopClient TankTopClient = new TankTopClient("");
        public static Index Index;

        [SetUp]
        public void SetUp()
        {
            DeleteIndex();
            Index = TankTopClient.CreateIndex("TankTop");

        }

        [TearDown]
        public void TearDown()
        {
            DeleteIndex();
        }

        void DeleteIndex()
        {
            Extensions.Try(() => TankTopClient.DeleteIndex("TankTop"));
        }
    }
}
