using System.Collections.Generic;
using Db4oUnit.Extensions;
using NUnit.Framework;

namespace Db4oDoc.Code.DB4OTests
{
    // #example: Basic test case
    public class ExampleTestCase : AbstractDb4oTestCase
    {
        public static void Main(string[] args)
        {
            new ExampleTestCase().RunSolo();
        }

        public void TestStoresElement()
        {
            Db().Store(new TestItem());
            IList<TestItem> result = Db().Query<TestItem>();
            Assert.AreEqual(1, result.Count);
        }


        private class TestItem
        {
        }
    }
    // #end example
}