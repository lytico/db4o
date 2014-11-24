/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Tests.Common.Handlers;

namespace Db4objects.Db4o.Tests.Common.Handlers
{
	public class ObjectArrayUpdateTestCase : HandlerUpdateTestCaseBase
	{
		private static ObjectArrayUpdateTestCase.ParentItem[] childData = new ObjectArrayUpdateTestCase.ParentItem
			[] { new ObjectArrayUpdateTestCase.ChildItem("one"), new ObjectArrayUpdateTestCase.ChildItem
			("two"), null };

		private static ObjectArrayUpdateTestCase.ParentItem[] mixedData = new ObjectArrayUpdateTestCase.ParentItem
			[] { new ObjectArrayUpdateTestCase.ParentItem("one"), new ObjectArrayUpdateTestCase.ChildItem
			("two"), new ObjectArrayUpdateTestCase.ChildItem("three"), null };

		public class ItemArrays
		{
			public ObjectArrayUpdateTestCase.ChildItem[] _typedChildren;

			public ObjectArrayUpdateTestCase.ParentItem[] _typedChildrenInParentArray;

			public object[] _untypedChildren;

			public object[] _untypedChildrenInParentArray;

			public object _untypedChildrenInObject;

			public object _untypedChildrenInParentArrayInObject;

			public ObjectArrayUpdateTestCase.ParentItem[] _typedMixed;

			public object[] _untypedMixed;

			public object _untypedMixedInObject;
		}

		public class ParentItem
		{
			public string _name;

			public ParentItem(string name)
			{
				_name = name;
			}

			public override bool Equals(object obj)
			{
				if (!(obj is ObjectArrayUpdateTestCase.ParentItem))
				{
					return false;
				}
				if (obj is ObjectArrayUpdateTestCase.ChildItem)
				{
					return false;
				}
				return HasSameNameAs((ObjectArrayUpdateTestCase.ParentItem)obj);
			}

			protected virtual bool HasSameNameAs(ObjectArrayUpdateTestCase.ParentItem other)
			{
				if (_name == null)
				{
					return other._name == null;
				}
				return _name.Equals(other._name);
			}
		}

		public class ChildItem : ObjectArrayUpdateTestCase.ParentItem
		{
			public ChildItem(string name) : base(name)
			{
			}

			public override bool Equals(object obj)
			{
				if (!(obj is ObjectArrayUpdateTestCase.ChildItem))
				{
					return false;
				}
				return HasSameNameAs((ObjectArrayUpdateTestCase.ParentItem)obj);
			}
		}

		protected override object CreateArrays()
		{
			ObjectArrayUpdateTestCase.ItemArrays item = new ObjectArrayUpdateTestCase.ItemArrays
				();
			item._typedChildren = CastToChildItemArray(childData);
			item._typedChildrenInParentArray = childData;
			item._untypedChildren = CastToChildItemArray(childData);
			item._untypedChildrenInParentArray = childData;
			item._untypedChildrenInObject = CastToChildItemArray(childData);
			item._untypedChildrenInParentArrayInObject = childData;
			item._typedMixed = mixedData;
			item._untypedMixed = mixedData;
			item._untypedMixedInObject = mixedData;
			return item;
		}

		protected override void AssertArrays(IExtObjectContainer objectContainer, object 
			obj)
		{
			ObjectArrayUpdateTestCase.ItemArrays item = (ObjectArrayUpdateTestCase.ItemArrays
				)obj;
			ArrayAssert.AreEqual(CastToChildItemArray(childData), item._typedChildren);
			ArrayAssert.AreEqual(childData, item._typedChildrenInParentArray);
			ArrayAssert.AreEqual(CastToChildItemArray(childData), item._untypedChildren);
			ArrayAssert.AreEqual(childData, item._untypedChildrenInParentArray);
			ArrayAssert.AreEqual(CastToChildItemArray(childData), (object[])item._untypedChildrenInObject
				);
			ArrayAssert.AreEqual(childData, (object[])item._untypedChildrenInParentArrayInObject
				);
			ArrayAssert.AreEqual(mixedData, item._typedMixed);
			ArrayAssert.AreEqual(mixedData, item._untypedMixed);
			ArrayAssert.AreEqual(mixedData, (object[])item._untypedMixedInObject);
		}

		private ObjectArrayUpdateTestCase.ChildItem[] CastToChildItemArray(ObjectArrayUpdateTestCase.ParentItem
			[] array)
		{
			ObjectArrayUpdateTestCase.ChildItem[] res = new ObjectArrayUpdateTestCase.ChildItem
				[array.Length];
			for (int i = 0; i < res.Length; i++)
			{
				res[i] = (ObjectArrayUpdateTestCase.ChildItem)array[i];
			}
			return res;
		}

		protected override object[] CreateValues()
		{
			// not used
			return null;
		}

		protected override void AssertValues(IExtObjectContainer objectContainer, object[]
			 values)
		{
		}

		// not used
		protected override string TypeName()
		{
			return "object-array";
		}
	}
}
