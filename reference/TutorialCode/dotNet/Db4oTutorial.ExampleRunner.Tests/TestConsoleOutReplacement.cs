using System;
using NUnit.Framework;

namespace Db4oTutorial.ExampleRunner.Tests
{
    public class TestConsoleOutReplacement
    {
        [Test]
        public void RunWithContext()
        {
            ConsoleOutReplacement.RunInContext(
                () =>
                    {
                        ConsoleOutReplacement.Out.WriteLine("Hi");
                        Assert.IsTrue(ConsoleOutReplacement.GetText().Contains("Hi"));
                    });
        }

        [Test]
        public void ReturnsText()
        {
            var result = ConsoleOutReplacement.RunInContext(
                () => ConsoleOutReplacement.Out.WriteLine("Hi"));

            Assert.IsTrue(result.Contains("Hi"));
        }


        [Test]
        public void ThrowsIfOutOfContext()
        {
            Assert.Throws<InvalidOperationException>(
                () => { var textWriter = ConsoleOutReplacement.Out; });
        }

        [Test]
        public void CleansUpContext()
        {

            ConsoleOutReplacement.RunInContext(
                () =>
                {
                    ConsoleOutReplacement.Out.WriteLine("Hi");
                });

            ConsoleOutReplacement.RunInContext(
                () =>
                {
                    ConsoleOutReplacement.Out.WriteLine("42");
                    Assert.IsTrue(ConsoleOutReplacement.GetText().Contains("42"));
                    Assert.IsFalse(ConsoleOutReplacement.GetText().Contains("Hi"));
                });
        }
    }
}