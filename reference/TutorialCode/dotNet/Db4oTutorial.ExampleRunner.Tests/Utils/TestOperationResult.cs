using System;
using Db4oTutorial.ExampleRunner.Utils;
using NUnit.Framework;

namespace Db4oTutorial.ExampleRunner.Tests.Utils
{
    [TestFixture]
    public class TestOperationResult
    {
        [Test]
        public void ReturnResult()
        {
            var success = OperationResult.Success("Hi");
            Assert.AreEqual("Hi",success.Result);
        }


        [Test]
        public void ThrowIfTryingToAccees()
        {
            var success = OperationResult.Failure<string>(new Exception());
            Assert.Throws<InvalidOperationException>(() => { var r = success.Result; });
        }


        [Test]
        public void ExceptionIsNullOnValue()
        {
            var success = OperationResult.Success("Hi");
            Assert.AreEqual(null, success.Exception);
        }

        [Test]
        public void HasExceptionOnFailure()
        {
            var ex = new Exception();
            var success = OperationResult.Failure<string>(ex);
            Assert.AreEqual(ex, success.Exception);
        }
        
    }
}