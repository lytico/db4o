/* Copyright (C) 2009 Versant Inc.   http://www.db4o.com */
using System;
using System.Collections.Generic;
using Db4oUnit;

namespace Db4objects.Db4o.Tests.CLI2.Collections
{
    class ArrayDictionary4Asserter
    {
        public const int DataLength = 10;

        private const string format = "Key {0}";

        public static void PutData(IDictionary<string, int> dict)
        {
            for (int i = 0; i < DataLength; i++)
            {
                dict.Add(string.Format(format, i), i);
            }
        }

        public static void AssertItemGet(IDictionary<string, int> dict)
        {
            for (int i = 0; i < DataLength; i++)
            {
                Assert.AreEqual(i, dict[string.Format(format, i)]);
            }

            try
            {
                int value = dict[null];
                Assert.Fail("Should throw ArgumentNullException.");
            }
            catch (ArgumentNullException)
            {
                //Expected
            }

            try
            {
                int value = dict["Invalid key"];
                Assert.Fail("Should throw KeyNotFoundException.");
            }
            catch (KeyNotFoundException)
            {
                //
            }
        }

        public static void AssertItemSet(IDictionary<string, int> dict)
        {
            for (int i = 0; i < DataLength; i++)
            {
                dict[string.Format(format, i)] = i * 100;
            }
            for (int i = 0; i < DataLength; i++)
            {
                Assert.AreEqual(i * 100, dict[string.Format(format, i)]);
            }

            string key = "New Key 100";
            dict[key] = 1000;
            Assert.AreEqual(1000, dict[key]);

            try
            {
                dict[null] = 100;
                Assert.Fail("Should throw ArgumentNullException.");
            }
            catch (ArgumentNullException)
            {
                //
            }
        }

        public static void AssertKeys(IDictionary<string, int> dict)
        {
            ICollection<string> keys = dict.Keys;
            
            Assert.AreEqual(DataLength, keys.Count);

            for (int i = 0; i < DataLength; i++)
            {
                Assert.IsTrue(keys.Contains(string.Format(format, i)));
            }
        }

        public static void AssertValues(IDictionary<string, int> dict)
        {
            ICollection<int> values = dict.Values;
            
            Assert.AreEqual(DataLength, values.Count);
            for (int i = 0; i < DataLength; i++)
            {
                Assert.IsTrue(values.Contains(i));
            }
        }

        public static void AssertAdd(IDictionary<string, int> dict)
        {
            try
            {
                dict.Add(null, 100);
                Assert.Fail("Should throw ArgumentNullException.");
            }
            catch (ArgumentNullException)
            {
                //
            }

            try
            {
                dict.Add(string.Format(format, 0), 100);
                Assert.Fail("Should throw ArgumentException.");
            }
            catch (ArgumentException)
            {
                //
            }

            for (int i = DataLength; i < DataLength * 2; i++)
            {
                dict.Add(string.Format(format, i), i);
            }

            for (int i = 0; i < DataLength * 2; i++)
            {
                Assert.AreEqual(i, dict[string.Format(format, i)]);
            }
        }

        public static void TestContainsKey(IDictionary<string, int> dict)
        {
            for (int i = 0; i < DataLength; i++)
            {
                Assert.IsTrue(dict.ContainsKey(string.Format(format, i)));
            }

            Assert.IsFalse(dict.ContainsKey("Invalid key"));

            try
            {
                dict.ContainsKey(null);
                Assert.Fail("Should throw ArgumentNullException.");
            }
            catch (ArgumentNullException)
            {
                //
            }
        }

        public static void AssertRemove(IDictionary<string, int> dict)
        {
            for (int i = 0; i < DataLength; i++)
            {
                Assert.IsTrue(dict.Remove(string.Format(format, i)));
            }

            Assert.IsFalse(dict.Remove("Invalid Key"));

            try
            {
                dict.Remove(null);
                Assert.Fail("Should throw ArgumentNullException.");
            }
            catch (ArgumentNullException)
            {
                //
            }
        }

        public static void AssertTryGetValue(IDictionary<string, int> dict)
        {
            for (int i = 0; i < DataLength; i++)
            {
                int value;
                Assert.IsTrue(dict.TryGetValue(string.Format(format, i), out value));
                Assert.AreEqual(i, value);
            }


            int value2;
            Assert.IsFalse(dict.TryGetValue(string.Format(format, 100), out value2));
            Assert.AreEqual(default(int), value2);

            try
            {
                int value;
                dict.TryGetValue(null, out value);
                Assert.Fail("Should throw ArgumentNullException.");
            }
            catch (ArgumentNullException)
            {
                //
            }
        }

        public static void AssertCount(IDictionary<string, int> dict)
        {
            Assert.AreEqual(DataLength, dict.Count);
            dict.Remove(string.Format(format, 0));
            Assert.AreEqual(DataLength - 1, dict.Count);
            dict.Remove(string.Format(format, 1));
            dict.Remove(string.Format(format, 2));
            Assert.AreEqual(DataLength - 3, dict.Count);
            dict["new key"] = 100;
            Assert.AreEqual(DataLength - 2, dict.Count);
        }

        public static void AssertIsReadOnly(IDictionary<string, int> dict)
        {
            Assert.IsFalse(dict.IsReadOnly);
        }

        public static void AssertAddKeyValuePair(IDictionary<string, int> dict)
        {
            for (int i = DataLength; i < DataLength * 2; i++)
            {
                KeyValuePair<string, int> keyValuePair = new KeyValuePair<string, int>(string.Format(format, i), i);
                dict.Add(keyValuePair);
            }

            for (int i = 0; i < DataLength * 2; i++)
            {
                Assert.AreEqual(i, dict[string.Format(format, i)]);
            }

            try
            {
                KeyValuePair<string, int> keyValuePair = new KeyValuePair<string, int>(string.Format(format, 0), 100);
                dict.Add(keyValuePair);
                Assert.Fail("Should throw ArgumentException.");
            }
            catch (ArgumentException)
            {
                //
            }

            try
            {
                KeyValuePair<string, int> keyValuePair = new KeyValuePair<string, int>(null, 100);
                dict.Add(keyValuePair);
                Assert.Fail("Should throw ArgumentNullException.");
            }
            catch (ArgumentNullException)
            {
            }
        }

        public static void AssertContains(IDictionary<string, int> dict)
        {
            for (int i = 0; i < DataLength; i++)
            {
                KeyValuePair<string, int> keyValuePair = new KeyValuePair<string, int>(string.Format(format, i), i);
                Assert.IsTrue(dict.Contains(keyValuePair));
            }

            {
                KeyValuePair<string, int> keyValuePair = new KeyValuePair<string, int>(string.Format(format, 0), 100);
                Assert.IsFalse(dict.Contains(keyValuePair));
            }

            try
            {
                KeyValuePair<string, int> keyValuePair = new KeyValuePair<string, int>(null, 100);
                dict.Contains(keyValuePair);
                Assert.Fail("Should throw ArgumentNullException.");
            }
            catch (ArgumentNullException)
            {
                //
            }

        }

        public static void AssertCopyTo(IDictionary<string, int> dict)
        {
            KeyValuePair<string, int>[] keyValuePairs = new KeyValuePair<string, int>[dict.Count];
            dict.CopyTo(keyValuePairs, 0);
            for (int i = 0; i < DataLength; i++)
            {
                Assert.AreEqual(string.Format(format, i), keyValuePairs[i].Key);
                Assert.AreEqual(i, keyValuePairs[i].Value);
            }

            try
            {
                dict.CopyTo(null, 0);
                Assert.Fail("Should throw ArgumentNullException.");
            }
            catch (ArgumentNullException)
            {
                //
            }

            try
            {
                dict.CopyTo(keyValuePairs, -1);
                Assert.Fail("Should throw ArgumentOutOfRangeException.");
            }
            catch (ArgumentOutOfRangeException)
            {
                //
            }

            try
            {
                dict.CopyTo(keyValuePairs, keyValuePairs.Length);
                Assert.Fail("Should throw ArgumentException.");
            }
            catch (ArgumentException)
            {
                //
            }
        }

        public static void AssertRemoveKeyValuePair(IDictionary<string, int> dict)
        {
            for (int i = 0; i < DataLength; i++)
            {
                KeyValuePair<string, int> keyValuePair = new KeyValuePair<string, int>(string.Format(format, i), i);
                Assert.IsTrue(dict.Remove(keyValuePair));
            }

            {
                KeyValuePair<string, int> keyValuePair = new KeyValuePair<string, int>(string.Format(format, 0), 100);
                Assert.IsFalse(dict.Remove(keyValuePair));
            }

            try
            {
                KeyValuePair<string, int> keyValuePair = new KeyValuePair<string, int>(null, 100);
                dict.Remove(keyValuePair);
                Assert.Fail("Should throw ArgumentNullException.");
            }
            catch (ArgumentNullException)
            {
                //
            }
        }

        public static void AssertClear(IDictionary<string, int> dict)
        {
            dict.Clear();
            Assert.AreEqual(0, dict.Count);
            Assert.IsFalse(dict.ContainsKey(string.Format(format, 0)));
        }

        public static void AssertGetEnumerator(IDictionary<string, int> dict)
        {
            IEnumerator<KeyValuePair<string, int>> enumerator = dict.GetEnumerator();
            int index = 0;
            while (enumerator.MoveNext())
            {
                Assert.AreEqual(string.Format("Key {0}", index), enumerator.Current.Key);
                Assert.AreEqual(index, enumerator.Current.Value);
                index++;
            }
			Assert.AreEqual(dict.Count, index);
        }

    }
}
