using System;
using System.Collections.Generic;
using System.Linq;
using Db4oTutorial.ExampleRunner.Demos;
using NUnit.Framework;

namespace Db4oTutorial.ExampleRunner.Tests
{
    public class TestSnippetExecutor
    {
        private SnippetExecutor toTest;

        [SetUp]
        public void Setup()
        {
            this.toTest = new SnippetExecutor();
        }

        [Test]
        public void RunSnippet()
        {
            var result = toTest.RunSnippet("Console.Out.WriteLine(\"Hello World\");");
            Assert.IsTrue(result.Contains("Hello World"));
        }


        [Test]
        public void QueryForCars()
        {
            IEnumerable<Car> cars = new Car[0];
            Console.Out.Write(cars.Any());
            var result = toTest.RunSnippet("IEnumerable<Car> cars = new Car[0];" +
                                           "Console.Out.Write(cars.Any());\n"
                                           +"return;\n");
            Console.Out.WriteLine(result);
            Assert.AreEqual("0",result);
        }


        [Test]
        public void HasObjectContainer()
        {
            var result = toTest.RunSnippet("container.Close();");
            Assert.AreEqual("",result);
        }
        [Test]
        public void PrintsResult()
        {
            var result = toTest.RunSnippet("Console.Out.Write(42);");
            Assert.AreEqual("42", result);
        }
        [Test]
        public void ReturnsException()
        {
            var result = toTest.RunSnippet("throw new Exception();");
            Assert.IsTrue(result.Contains("Exception"));
        }
        [Test]
        public void ReturnsCompileError()
        {
            var result = toTest.RunSnippet("container.Close()");
            Assert.IsTrue(result.Contains(";"));
        }
        
    }
}