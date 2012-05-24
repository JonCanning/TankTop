using System.Threading;
using NUnit.Framework;
using TankTop.Dto;
using TankTop.Extensions;

namespace TankTop.IntegrationTests
{
    abstract class IntegrationTest
    {
        protected Index Index = SetUpFixture.Index;
        protected readonly ITankTopClient TankTopClient = SetUpFixture.TankTopClient;

        [TestFixtureSetUp]
        protected void TestFixtureSetUp()
        {
            while (!Index.Started)
            {
                Thread.Sleep(1000);
                Index = TankTopClient.GetIndex("TankTop");
            }
            Index.DeleteDocuments(new Query("key:v*"));
        }
    }
}