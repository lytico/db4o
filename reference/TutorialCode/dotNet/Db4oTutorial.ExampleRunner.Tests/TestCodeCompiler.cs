using System;
using Db4objects.Db4o;
using Moq;
using NUnit.Framework;

namespace Db4oTutorial.ExampleRunner.Tests
{
    [TestFixture]
    public class TestCodeCompiler
    {
        private CodeCompiler toTest;

        [SetUp]
        public void Setup()
        {
            this.toTest = new CodeCompiler();
        }
        
        [Test]
        public void ReturnsAction()
        {
            var toRun = toTest.Compile("");
            Assert.NotNull(toRun);
        }

        [Test]
        public void CanRunAction()
        {
            var containerMock = new Mock<IObjectContainer>();
            var toRun = toTest.Compile("container.Close();");
            toRun.Result(containerMock.Object);
            containerMock.Verify(c=>c.Close());
        }

        [Test]
        public void CanUseDemoDomainmodel()
        {
            var result = toTest.Compile("typeof(Car).ToString();");
            Assert.IsFalse(result.IsFailure);
        }
        [Test]
        public void IndependedActions()
        {
            var container1 = new Mock<IObjectContainer>();
            var container2 = new Mock<IObjectContainer>();
            var toRun1 = toTest.Compile("container.Ext();");
            var toRun2 = toTest.Compile("container.Close();");
            toRun1.Result(container1.Object);
            toRun2.Result(container2.Object);
            container1.Verify(c => c.Ext());
            container1.Verify(c=>c.Close(),Times.Never());
            container2.Verify(c => c.Close());
            container2.Verify(c => c.Ext(), Times.Never());
        }
        [Test]
        public void GetCompileError()
        {
            var toRun = toTest.Compile("container.");
            Assert.IsTrue(toRun.IsFailure);
            Assert.IsTrue(toRun.Exception.Message.Contains("error"));
        }

        [Test]
        public void ReThrows()
        {
            var container = new Mock<IObjectContainer>();
            var toRun = toTest.Compile("throw new Exception(\"Ex\");");
            Assert.Throws<Exception>(() => toRun.Result(container.Object));
        }



        [Test]
        public void RedirectsConsoleWrites()
        {
            var toRun = toTest.Compile("Console.Out.WriteLine(\"11\");"
                + "Console.WriteLine(\"22\");"
                + "System.Console.WriteLine(\"33\");"
                + "System.Console.Write(\"44\");");
            
            
            
            var consoleOut = toRun.Result(null);
            Assert.IsTrue(consoleOut.Contains("11"));
            Assert.IsTrue(consoleOut.Contains("22"));
            Assert.IsTrue(consoleOut.Contains("33"));
            Assert.IsTrue(consoleOut.Contains("44"));
        }


        
    }
}