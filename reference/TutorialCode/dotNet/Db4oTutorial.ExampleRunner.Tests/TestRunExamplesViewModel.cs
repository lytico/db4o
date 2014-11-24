using System;
using NUnit.Framework;

namespace Db4oTutorial.ExampleRunner.Tests
{
    [TestFixture]
    public class TestRunExamplesViewModel
    {
        private RunExamplesViewModel toTest;

        [SetUp]
        public void Setup()
        {
            this.toTest = new RunExamplesViewModel();
        }

        [Test]
        public void ModifyCode()
        {
            toTest.Code = "New Code";
            Assert.AreEqual("New Code", toTest.Code);
        }


        [Test]
        public void ChangingCodeFiresEvent()
        {
            var fired = false;
            toTest.PropertyChanged += (sender, args) => fired = true;
            toTest.Code = "New Code";
            Assert.IsTrue(fired);
        }
    }
}
