/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Tests.Common.Staging;

namespace Db4objects.Db4o.Tests.Common.Staging
{
	public class OldVersionReflectFieldAfterRefactorTestCase : AbstractDb4oTestCase
	{
		private const int IdValue = 42;

		public class ItemBefore
		{
			public int _id;

			public ItemBefore(int id)
			{
				_id = id;
			}
		}

		public class ItemAfter
		{
			public string _id;
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestReflectField()
		{
			Store(new OldVersionReflectFieldAfterRefactorTestCase.ItemBefore(IdValue));
			Reopen();
			FileSession().StoredClass(typeof(OldVersionReflectFieldAfterRefactorTestCase.ItemBefore
				)).Rename(typeof(OldVersionReflectFieldAfterRefactorTestCase.ItemAfter).FullName
				);
			Reopen();
			ClassMetadata classMetadata = Container().ClassMetadataForName(typeof(OldVersionReflectFieldAfterRefactorTestCase.ItemAfter
				).FullName);
			ByRef originalField = new ByRef();
			classMetadata.TraverseDeclaredFields(new _IProcedure4_37(originalField));
			Assert.AreEqual(typeof(int).FullName, ((FieldMetadata)originalField.value).GetStoredType
				().GetName());
		}

		private sealed class _IProcedure4_37 : IProcedure4
		{
			public _IProcedure4_37(ByRef originalField)
			{
				this.originalField = originalField;
			}

			public void Apply(object field)
			{
				if (((FieldMetadata)originalField.value) == null && ((FieldMetadata)field).GetName
					().Equals("_id") && ((FieldMetadata)field).FieldType().GetName().Equals(typeof(int
					).FullName))
				{
					originalField.value = ((FieldMetadata)field);
				}
			}

			private readonly ByRef originalField;
		}
	}
}
