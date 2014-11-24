/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

#if !SILVERLIGHT
using System;
using System.Collections;
using Db4oUnit;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Qlin;
using Db4objects.Db4o.Tests.Common.Qlin;

namespace Db4objects.Db4o.Tests.Common.Qlin
{
	public class PrototypesTestCase : ITestLifeCycle
	{
		private Prototypes _prototypes;

		public class Item
		{
			public PrototypesTestCase.Item _child;

			public string _name;

			public int myInt;

			public virtual string Name()
			{
				return _name;
			}

			public virtual PrototypesTestCase.Item Child()
			{
				return _child;
			}

			public override string ToString()
			{
				string str = "Item " + _name;
				if (_child != null)
				{
					str += "\n  " + _child.ToString();
				}
				return str;
			}
		}

		public virtual void TestStringField()
		{
			PrototypesTestCase.Item item = ((PrototypesTestCase.Item)Prototype(typeof(PrototypesTestCase.Item
				)));
			AssertPath(item, item._name, new string[] { "_name" });
		}

		public virtual void TestStringMethod()
		{
			PrototypesTestCase.Item item = ((PrototypesTestCase.Item)Prototype(typeof(PrototypesTestCase.Item
				)));
			AssertPath(item, item.Name(), new string[] { "_name" });
		}

		public virtual void TestInstanceField()
		{
			PrototypesTestCase.Item item = ((PrototypesTestCase.Item)Prototype(typeof(PrototypesTestCase.Item
				)));
			AssertPath(item, item._child, new string[] { "_child" });
		}

		public virtual void TestInstanceMethod()
		{
			PrototypesTestCase.Item item = ((PrototypesTestCase.Item)Prototype(typeof(PrototypesTestCase.Item
				)));
			AssertPath(item, item.Child(), new string[] { "_child" });
		}

		public virtual void TestLevel2()
		{
			PrototypesTestCase.Item item = ((PrototypesTestCase.Item)Prototype(typeof(PrototypesTestCase.Item
				)));
			AssertPath(item, item.Child().Name(), new string[] { "_child", "_name" });
		}

		public virtual void TestCallingOwnFramework()
		{
			PrototypesTestCase testCase = ((PrototypesTestCase)Prototype(typeof(PrototypesTestCase
				)));
			AssertPath(testCase, testCase._prototypes, new string[] { "_prototypes" });
		}

		public virtual void TestWildToString()
		{
			PrototypesTestCase testCase = ((PrototypesTestCase)Prototype(typeof(PrototypesTestCase
				)));
			AssertIsNull(testCase, testCase._prototypes.ToString());
		}

		// keep this method, it's helpful for new tests
		private void Print(object t, object expression)
		{
			IEnumerator path = _prototypes.BackingFieldPath(((object)t).GetType(), expression
				);
			if (path == null)
			{
				Print("null");
				return;
			}
			Print(Iterators.Join(path, "[", "]", ", "));
		}

		private void Print(string @string)
		{
			Sharpen.Runtime.Out.WriteLine(@string);
		}

		private void AssertIsNull(object t, object expression)
		{
			Assert.IsNull(_prototypes.BackingFieldPath(((object)t).GetType(), expression));
		}

		private void AssertPath(object t, object expression, string[] expected)
		{
			IEnumerator path = _prototypes.BackingFieldPath(((object)t).GetType(), expression
				);
			// print(Iterators.join(path, "[", "]", ", "));
			path.Reset();
			Iterator4Assert.AreEqual(expected, path);
		}

		private object Prototype(Type clazz)
		{
			return _prototypes.PrototypeForClass(clazz);
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void SetUp()
		{
			_prototypes = new Prototypes(Prototypes.DefaultReflector(), RecursionDepth, IgnoreTransientFields
				);
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TearDown()
		{
		}

		private const bool IgnoreTransientFields = true;

		private const int RecursionDepth = 10;
	}
}
#endif // !SILVERLIGHT
