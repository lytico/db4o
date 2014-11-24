/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Tests.Common.Api;
using Db4objects.Db4o.Tests.Common.Refactor;

namespace Db4objects.Db4o.Tests.Common.Refactor
{
	public abstract class AccessFieldTestCaseBase : Db4oTestWithTempFile
	{
		/// <exception cref="System.Exception"></exception>
		public override void SetUp()
		{
			WithDatabase(new _IDatabaseAction_14(this));
		}

		private sealed class _IDatabaseAction_14 : AccessFieldTestCaseBase.IDatabaseAction
		{
			public _IDatabaseAction_14(AccessFieldTestCaseBase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public void RunWith(IObjectContainer db)
			{
				db.Store(this._enclosing.NewOriginalData());
			}

			private readonly AccessFieldTestCaseBase _enclosing;
		}

		protected virtual void RenameClass(Type origClazz, string targetName)
		{
			IEmbeddedConfiguration config = NewConfiguration();
			config.Common.ObjectClass(origClazz).Rename(targetName);
			WithDatabase(config, new _IDatabaseAction_24());
		}

		private sealed class _IDatabaseAction_24 : AccessFieldTestCaseBase.IDatabaseAction
		{
			public _IDatabaseAction_24()
			{
			}

			public void RunWith(IObjectContainer db)
			{
			}
		}

		// do nothing
		protected abstract object NewOriginalData();

		protected virtual void AssertField(Type targetClazz, string fieldName, Type fieldType
			, object fieldValue)
		{
			WithDatabase(new _IDatabaseAction_35(targetClazz, fieldName, fieldType, fieldValue
				));
		}

		private sealed class _IDatabaseAction_35 : AccessFieldTestCaseBase.IDatabaseAction
		{
			public _IDatabaseAction_35(Type targetClazz, string fieldName, Type fieldType, object
				 fieldValue)
			{
				this.targetClazz = targetClazz;
				this.fieldName = fieldName;
				this.fieldType = fieldType;
				this.fieldValue = fieldValue;
			}

			public void RunWith(IObjectContainer db)
			{
				IStoredClass storedClass = db.Ext().StoredClass(targetClazz);
				IStoredField storedField = storedClass.StoredField(fieldName, fieldType);
				IObjectSet result = db.Query(targetClazz);
				Assert.AreEqual(1, result.Count);
				object obj = result.Next();
				object value = (object)storedField.Get(obj);
				Assert.AreEqual(fieldValue, value);
			}

			private readonly Type targetClazz;

			private readonly string fieldName;

			private readonly Type fieldType;

			private readonly object fieldValue;
		}

		private interface IDatabaseAction
		{
			void RunWith(IObjectContainer db);
		}

		private void WithDatabase(AccessFieldTestCaseBase.IDatabaseAction action)
		{
			WithDatabase(NewConfiguration(), action);
		}

		private void WithDatabase(IEmbeddedConfiguration config, AccessFieldTestCaseBase.IDatabaseAction
			 action)
		{
			IObjectContainer db = Db4oEmbedded.OpenFile(config, TempFile());
			try
			{
				action.RunWith(db);
			}
			finally
			{
				db.Close();
			}
		}
	}
}
