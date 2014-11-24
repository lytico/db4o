/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Tests.Common.References;

namespace Db4objects.Db4o.Tests.Common.References
{
	public class HardObjectReferenceTestCase : AbstractDb4oTestCase
	{
		public static void Main(string[] args)
		{
			new HardObjectReferenceTestCase().RunSolo();
		}

		public class Item
		{
			public string _name;

			public Item(string name)
			{
				_name = name;
			}

			public override bool Equals(object obj)
			{
				if (obj == null)
				{
					return false;
				}
				if (GetType() != obj.GetType())
				{
					return false;
				}
				return _name.Equals(((HardObjectReferenceTestCase.Item)obj)._name);
			}
		}

		public virtual void TestPeekPersisted()
		{
			HardObjectReferenceTestCase.Item item = new HardObjectReferenceTestCase.Item("one"
				);
			Store(item);
			int id = (int)Db().GetID(item);
			Assert.AreEqual(item, Peek(id)._object);
			Db().Delete(item);
			Db().Commit();
			Assert.IsNull(Peek(id));
		}

		private HardObjectReference Peek(int id)
		{
			return HardObjectReference.PeekPersisted(Trans(), id, 1);
		}
	}
}
