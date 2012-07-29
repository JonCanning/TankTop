using NUnit.Framework;
using TankTop.Dto;

namespace TankTop.IntegrationTests
{
    [SetUpFixture]
    class SetUpFixture
    {
       // public static readonly TankTopClient TankTopClient = new TankTopClient("http://:begyhuzatybu@vehehu.api.indexden.com");
        public static readonly TankTopClient TankTopClient = new TankTopClient("http://:nygutusevypy@jerate.api.indexden.com");

        public static Index Index;

        [SetUp]
        public void SetUp()
        {
            DeleteIndex();
            Index = TankTopClient.CreateIndex("TankTop", true);
        }

        [TearDown]
        public void TearDown()
        {
            DeleteIndex();
        }

        static void DeleteIndex()
        {
            TankTopClient.Try(x => x.DeleteIndex("TankTop"));
        }
    }
}
