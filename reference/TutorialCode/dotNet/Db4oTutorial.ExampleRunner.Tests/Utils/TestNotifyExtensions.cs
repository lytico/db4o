using System.ComponentModel;
using Db4oTutorial.ExampleRunner.Utils;
using NUnit.Framework;

namespace Db4oTutorial.ExampleRunner.Tests.Utils
{
    [TestFixture]
    public class TestNotifyExtensions
    {
        private class ClassWithSomeProperties : INotifyPropertyChanged
        {
            private string name;
            public event PropertyChangedEventHandler PropertyChanged;

            public string Name
            {
                set
                {
                    name = value;
                    PropertyChanged.Fire(this, t => t.Name);
                }
                get { return name; }
            }
        }

        [Test]
        public void RunsWithNoListener()
        {
            new ClassWithSomeProperties().Name = "new-Name";
        }

        [Test]
        public void IsNotifiedName()
        {
            var expectCall = false;
            var toTest = new ClassWithSomeProperties();
            toTest.PropertyChanged += (s, e) =>
            {
                Assert.AreSame(toTest, s);
                Assert.AreEqual("Name", e.PropertyName);
                expectCall = true;
            };
            toTest.Name = "Fun";
            Assert.IsTrue(expectCall);
        }

        [Test]
        public void IsNotifiedWithoutDereference()
        {
            var expectCall = false;
            PropertyChangedEventHandler handlers = null;
            handlers += (s, e) =>
            {
                Assert.AreSame(this, s);
                Assert.AreEqual("DummyProperty", e.PropertyName);
                expectCall = true;
            };
            handlers.Fire(this, t => DummyProperty);
            Assert.IsTrue(expectCall);
        }

        [Test]
        public void IsNotifiedWithoutArgument()
        {
            var expectCall = false;
            PropertyChangedEventHandler handlers = null;
            handlers += (s, e) =>
            {
                Assert.AreSame(this, s);
                Assert.AreEqual("DummyProperty", e.PropertyName);
                expectCall = true;
            };
            handlers.Fire(this, () => DummyProperty);
            Assert.IsTrue(expectCall);
        }

        public string DummyProperty { get; set; }
    }
}