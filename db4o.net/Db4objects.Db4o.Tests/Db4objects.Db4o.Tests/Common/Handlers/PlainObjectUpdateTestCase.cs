/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Tests.Common.Handlers;

namespace Db4objects.Db4o.Tests.Common.Handlers
{
	public class PlainObjectUpdateTestCase : HandlerUpdateTestCaseBase
	{
		public sealed class Item
		{
			public override int GetHashCode()
			{
				int prime = 31;
				int result = 1;
				result = prime * result + ((_typed == null) ? 0 : _typed.GetHashCode());
				result = prime * result + ((_untyped == null) ? 0 : _untyped.GetHashCode());
				return result;
			}

			public override bool Equals(object obj)
			{
				if (this == obj)
				{
					return true;
				}
				if (obj == null)
				{
					return false;
				}
				if (GetType() != obj.GetType())
				{
					return false;
				}
				PlainObjectUpdateTestCase.Item other = (PlainObjectUpdateTestCase.Item)obj;
				return Check.ObjectsAreEqual(_typed, other._typed) && Check.ObjectsAreEqual(_untyped
					, other._untyped);
			}

			public object _typed;

			public object _untyped;

			public Item(object @object)
			{
				_typed = @object;
				_untyped = @object;
			}
		}

		protected override bool IsApplicableForDb4oVersion()
		{
			return Db4oMajorVersion() >= 7 && Db4oMinorVersion() >= 2;
		}

		protected override void AssertArrays(IExtObjectContainer objectContainer, object 
			obj)
		{
			object[] array = (object[])obj;
			Assert.AreEqual(2, array.Length);
			Assert.AreSame(array[0], array[1]);
		}

		protected override void AssertValues(IExtObjectContainer objectContainer, object[]
			 values)
		{
			Assert.AreEqual(1, values.Length);
			PlainObjectUpdateTestCase.Item item = (PlainObjectUpdateTestCase.Item)values[0];
			Assert.IsNotNull(item);
			Assert.IsNotNull(item._typed);
			Assert.AreSame(item._typed, item._untyped);
		}

		protected override object CreateArrays()
		{
			object @object = new object();
			return new object[] { @object, @object };
		}

		protected override object[] CreateValues()
		{
			return new object[] { new PlainObjectUpdateTestCase.Item(new object()) };
		}

		protected override string TypeName()
		{
			return typeof(object).FullName;
		}
	}
}
