using System;
using Db4objects.Db4o;
using Db4objects.Db4o.CS;
using Db4objects.Db4o.CS.Config;
using Db4objects.Db4o.IO;
using NUnit.Framework;

namespace Db4oDoc.Code.ClientServer.Pooling
{
    [TestFixture]
    public class TestConnectionPool
    {
        private const int Port = 1337;
        private const string UserAndPassword = "sa";

        private IObjectServer server;
        private ConnectionPool toTest;
        private AssertionConnectionFactory assertConnectionFactory;

        [SetUp]
        public void Setup()
        {
            server = CreateInMemoryServer();
            StoreTestObjects();
            assertConnectionFactory = NewFactory();
            toTest = new ConnectionPool(assertConnectionFactory.Connect);
        }


        [TearDown]
        public void TearDown()
        {
            server.Close();
        }

        [Test]
        public void ReturnsConnection()
        {
            ConnectionPool toTest = new ConnectionPool(NewFactory().Connect);
            IObjectContainer container = toTest.Acquire();
            Assert.NotNull(container);
        }

        [Test]
        public void CreatesConnectionForSecondAquire()
        {
            IObjectContainer container1 = toTest.Acquire();
            IObjectContainer container2 = toTest.Acquire();
            assertConnectionFactory.AssertWasCalledTimes(2);
        }


        [Test]
        public void CloseAndRelease()
        {
            IObjectContainer container1 = toTest.Acquire();
            container1.Store(new AStoredObject());
            toTest.CloseAndRelease(container1);
            Assert.AreEqual(2, server.OpenClient().Query<AStoredObject>().Count);
        }

        [Test]
        public void ReusesContainerIfReleased()
        {
            IObjectContainer container1 = toTest.Acquire();
            toTest.Release(container1);
            IObjectContainer container2 = toTest.Acquire();
            assertConnectionFactory.AssertWasCalledTimes(1);
        }

        [Test]
        public void CannotMultipleReturnClient()
        {
            IObjectContainer container1 = toTest.Acquire();
            toTest.Release(container1);
            Assert.Throws(typeof (ArgumentException), delegate { toTest.Release(container1); });
        }

        [Test]
        public void ReusedClientIsNotTainted()
        {
            IObjectContainer container1 = toTest.Acquire();
            AStoredObject fromContainer1 = TestObjectFromContainer(container1);
            toTest.Release(container1);
            AStoredObject fromContainer2 = TestObjectFromContainer(toTest.Acquire());
            Assert.AreNotSame(fromContainer1, fromContainer2);
        }

        [Test]
        public void NowGhostCommits()
        {
            IObjectContainer container1 = toTest.Acquire();
            container1.Store(new AStoredObject());
            toTest.Release(container1);
            IObjectContainer container2 = toTest.Acquire();
            container2.Commit();

            int countStoredObjects = server.OpenClient().Query<AStoredObject>().Count;
            Assert.AreEqual(1, countStoredObjects);
        }

        [Test]
        public void CanPassClosedContainers()
        {
            IObjectContainer container = toTest.Acquire();
            container.Close();
            toTest.Release(container);
        }


        [Test]
        public void NoTransactionSharing()
        {
            IObjectContainer session1 = toTest.Acquire();
            IObjectContainer session2 = toTest.Acquire();
            session1.Store(new AStoredObject());
            int count1 = session1.Query<AStoredObject>().Count;
            session2.Rollback();

            int count2 = session1.Query<AStoredObject>().Count;
            Assert.AreEqual(2, count1);
            Assert.AreEqual(2, count2);
        }

        private static AStoredObject TestObjectFromContainer(IObjectContainer container1)
        {
            return container1.Query<AStoredObject>()[0];
        }


        private void StoreTestObjects()
        {
            IObjectContainer objectContainer = server.OpenClient();
            objectContainer.Store(new AStoredObject());
            objectContainer.Close();
        }

        private static AssertionConnectionFactory NewFactory()
        {
            return new AssertionConnectionFactory();
        }

        private class AssertionConnectionFactory
        {
            private int wasCalled;

            public IObjectContainer Connect()
            {
                wasCalled++;
                return Db4oClientServer.OpenClient("localhost", Port, UserAndPassword, UserAndPassword);
            }


            public void AssertWasCalledTimes(int times)
            {
                Assert.AreEqual(times, wasCalled);
            }
        }


        private static IObjectServer CreateInMemoryServer()
        {
            IServerConfiguration config = Db4oClientServer.NewServerConfiguration();
            config.File.Storage = new MemoryStorage();
            IObjectServer server = Db4oClientServer.OpenServer(config, "In:Memory", Port);
            server.GrantAccess(UserAndPassword, UserAndPassword);
            return server;
        }


        private class AStoredObject
        {
        }
    }
}