/* Copyright (C) 2009 Versant Inc.   http://www.db4o.com */
using Db4oUnit;
using Db4objects.Db4o.Collections;

namespace Db4objects.Db4o.Tests.CLI2.Collections
{
    public class ArrayDictionary4TestCase : ITestLifeCycle
    {
        private ArrayDictionary4<string, int> dict;

        public void SetUp()
        {
            dict = new ArrayDictionary4<string, int>();
            ArrayDictionary4Asserter.PutData(dict);
        }

        public void TearDown()
        {
            dict.Clear();
        }

        public void TestItemGet()
        {
            ArrayDictionary4Asserter.AssertItemGet(dict);
        }

        public void TestItemSet()
        {
            ArrayDictionary4Asserter.AssertItemSet(dict);
        }

        public void TestKeys()
        {
            ArrayDictionary4Asserter.AssertKeys(dict);
        }

        public void TestValues()
        {
            ArrayDictionary4Asserter.AssertValues(dict);
        }

        public void TestAdd()
        {
            ArrayDictionary4Asserter.AssertAdd(dict);
        }

        public void TestContainsKey()
        {
            ArrayDictionary4Asserter.TestContainsKey(dict);
        }

        public void TestRemove()
        {
            ArrayDictionary4Asserter.AssertRemove(dict);
        }

        public void TestTryGetValue()
        {
            ArrayDictionary4Asserter.AssertTryGetValue(dict);
        }

        public void TestCount()
        {
            ArrayDictionary4Asserter.AssertCount(dict);
        }

        public void TestIsReadOnly()
        {
            ArrayDictionary4Asserter.AssertIsReadOnly(dict);
        }

        public void TestAddKeyValuePair()
        {
            ArrayDictionary4Asserter.AssertAddKeyValuePair(dict);
        }

        public void TestContains()
        {
            ArrayDictionary4Asserter.AssertContains(dict);
        }

        public void TestCopyTo()
        {
            ArrayDictionary4Asserter.AssertCopyTo(dict);
        }

        public void TestRemoveKeyValuePair()
        {
            ArrayDictionary4Asserter.AssertRemoveKeyValuePair(dict);
        }

        public void TestClear()
        {
            ArrayDictionary4Asserter.AssertClear(dict);
        }

        public void TestGetEnumerator()
        {
            ArrayDictionary4Asserter.AssertGetEnumerator(dict);
        }
    }
}
