using System;
using System.Collections.Generic;
using System.IO;
using Db4objects.Db4o.Collections;
using Db4oUnit;
using Db4oUnit.Extensions;

namespace Db4objects.Db4o.Tests.CLI2.Assorted
{
	class SerializationConstructorTestCase : AbstractDb4oTestCase
	{
		protected override void Configure(Db4objects.Db4o.Config.IConfiguration config)
		{
			config.ExceptionsOnNotStorable(true);
			config.CallConstructors(false);
		}

		protected override void Store()
		{
			DummyTestRootObject testObject = new DummyTestRootObject();
			testObject.embeddedObject.boList = new TestEmbeddedObjectList<DummyTestEmbeddedObject>(testObject.embeddedObject);
			Store(testObject);
		}

		public void Test()
		{
			DummyTestRootObject root = RetrieveOnlyInstance<DummyTestRootObject>();
			Assert.IsNotNull(root.embeddedObject.boList);
		}
	}

	class DummyTestRootObject : TestRootObject
	{
		public DummyTestEmbeddedObject embeddedObject = null;
		public DummyTestRootObject()
		{
			embeddedObject = new DummyTestEmbeddedObject(this);
		}
	}

	public class DummyTestEmbeddedObject : TestEmbeddedObject
	{
		public TestEmbeddedObjectList<DummyTestEmbeddedObject> boList;
		public DummyTestEmbeddedObject(TestObject parent)
			: base(parent)
		{
		}
	}

	abstract public class TestEmbeddedObject : TestRootObject
	{
		TestObject parent;

		public TestEmbeddedObject(TestObject parent)
		{
			if (parent == null)
			{
				throw new Exception("The parameter parent can not be null");
			}
			this.parent = parent;
		}
	}

	abstract public class TestRootObject : TestObject
	{
		public TestRootObject()
			: base()
		{

		}
	}
	public abstract class TestObject
	{ }

	public class TestEmbeddedObjectList<T> : List<T> where T : TestObject
	{
		private readonly TestObject parent;

		public TestEmbeddedObjectList(TestObject parent)
		{
			if (parent == null)
			{
				throw new Exception("The parameter parent can not be null");
			}
			this.parent = parent;
		}
	}
}
