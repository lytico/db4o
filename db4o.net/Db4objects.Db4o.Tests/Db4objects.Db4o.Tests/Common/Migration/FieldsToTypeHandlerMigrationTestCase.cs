/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Delete;
using Db4objects.Db4o.Internal.Handlers;
using Db4objects.Db4o.Internal.Marshall;
using Db4objects.Db4o.Marshall;
using Db4objects.Db4o.Tests.Common.Api;
using Db4objects.Db4o.Tests.Common.Migration;
using Db4objects.Db4o.Typehandlers;

namespace Db4objects.Db4o.Tests.Common.Migration
{
	public class FieldsToTypeHandlerMigrationTestCase : Db4oTestWithTempFile
	{
		public static void Main(string[] args)
		{
			new ConsoleTestRunner(typeof(FieldsToTypeHandlerMigrationTestCase)).Run();
		}

		public class Item
		{
			public Item(int id)
			{
				_id = id;
			}

			public int _id;
		}

		internal FieldsToTypeHandlerMigrationTestCase.ItemTypeHandler _typeHandler;

		public class ItemTypeHandler : IReferenceTypeHandler, ICascadingTypeHandler, IVariableLengthTypeHandler
		{
			private int _writeCalls;

			private int _readCalls;

			public virtual void Defragment(IDefragmentContext context)
			{
				throw new NotImplementedException();
			}

			/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
			public virtual void Delete(IDeleteContext context)
			{
				throw new NotImplementedException();
			}

			public virtual void Activate(IReferenceActivationContext context)
			{
				_readCalls++;
				FieldsToTypeHandlerMigrationTestCase.Item item = (FieldsToTypeHandlerMigrationTestCase.Item
					)((UnmarshallingContext)context).PersistentObject();
				item._id = context.ReadInt() - 42;
			}

			public virtual void Write(IWriteContext context, object obj)
			{
				_writeCalls++;
				FieldsToTypeHandlerMigrationTestCase.Item item = (FieldsToTypeHandlerMigrationTestCase.Item
					)obj;
				context.WriteInt(item._id + 42);
			}

			public virtual IPreparedComparison PrepareComparison(IContext context, object obj
				)
			{
				throw new NotImplementedException();
			}

			public virtual void CascadeActivation(IActivationContext context)
			{
				throw new NotImplementedException();
			}

			public virtual void CollectIDs(QueryingReadContext context)
			{
				throw new NotImplementedException();
			}

			public virtual ITypeHandler4 ReadCandidateHandler(QueryingReadContext context)
			{
				throw new NotImplementedException();
			}

			public virtual int WriteCalls()
			{
				return _writeCalls;
			}

			public virtual int ReadCalls()
			{
				return _readCalls;
			}

			public virtual void Reset()
			{
				_writeCalls = 0;
				_readCalls = 0;
			}
		}

		public virtual void TestMigration()
		{
			_typeHandler = null;
			Store(new FieldsToTypeHandlerMigrationTestCase.Item(42));
			FieldsToTypeHandlerMigrationTestCase.Item item = RetrieveOnlyItemInstance();
			Assert.AreEqual(42, item._id);
			AssertItemStoredField(42);
			_typeHandler = new FieldsToTypeHandlerMigrationTestCase.ItemTypeHandler();
			item = RetrieveOnlyItemInstance();
			Assert.AreEqual(42, item._id);
			AssertTypeHandlerCalls(0, 0);
			AssertItemStoredField(42);
			UpdateItem();
			AssertTypeHandlerCalls(1, 0);
			AssertItemStoredField(null);
			item = RetrieveOnlyItemInstance();
			Assert.AreEqual(42, item._id);
			AssertTypeHandlerCalls(0, 1);
			AssertItemStoredField(null);
		}

		public virtual void TestTypeHandler()
		{
			_typeHandler = new FieldsToTypeHandlerMigrationTestCase.ItemTypeHandler();
			Store(new FieldsToTypeHandlerMigrationTestCase.Item(42));
			AssertTypeHandlerCalls(1, 0);
			FieldsToTypeHandlerMigrationTestCase.Item item = RetrieveOnlyItemInstance();
			Assert.AreEqual(42, item._id);
			AssertTypeHandlerCalls(0, 1);
			UpdateItem();
			AssertTypeHandlerCalls(1, 1);
		}

		private void AssertItemStoredField(object expectedValue)
		{
			IObjectContainer db = OpenContainer();
			try
			{
				IObjectSet objectSet = db.Query(typeof(FieldsToTypeHandlerMigrationTestCase.Item)
					);
				Assert.AreEqual(1, objectSet.Count);
				FieldsToTypeHandlerMigrationTestCase.Item item = (FieldsToTypeHandlerMigrationTestCase.Item
					)objectSet.Next();
				IStoredField storedField = db.Ext().StoredClass(typeof(FieldsToTypeHandlerMigrationTestCase.Item
					)).StoredField("_id", null);
				object actualValue = storedField.Get(item);
				Assert.AreEqual(expectedValue, actualValue);
			}
			finally
			{
				db.Close();
			}
		}

		private void AssertTypeHandlerCalls(int writeCalls, int readCalls)
		{
			Assert.AreEqual(writeCalls, _typeHandler.WriteCalls());
			Assert.AreEqual(readCalls, _typeHandler.ReadCalls());
		}

		private FieldsToTypeHandlerMigrationTestCase.Item RetrieveOnlyItemInstance()
		{
			IObjectContainer db = OpenContainer();
			try
			{
				IObjectSet objectSet = db.Query(typeof(FieldsToTypeHandlerMigrationTestCase.Item)
					);
				Assert.AreEqual(1, objectSet.Count);
				FieldsToTypeHandlerMigrationTestCase.Item item = (FieldsToTypeHandlerMigrationTestCase.Item
					)objectSet.Next();
				return item;
			}
			finally
			{
				db.Close();
			}
		}

		private void Store(FieldsToTypeHandlerMigrationTestCase.Item item)
		{
			IObjectContainer db = OpenContainer();
			try
			{
				db.Store(item);
			}
			finally
			{
				db.Close();
			}
		}

		private void UpdateItem()
		{
			IObjectContainer db = OpenContainer();
			try
			{
				IObjectSet objectSet = db.Query(typeof(FieldsToTypeHandlerMigrationTestCase.Item)
					);
				db.Store(objectSet.Next());
			}
			finally
			{
				db.Close();
			}
		}

		private IObjectContainer OpenContainer()
		{
			if (_typeHandler != null)
			{
				_typeHandler.Reset();
			}
			IEmbeddedConfiguration configuration = NewConfiguration();
			if (_typeHandler != null)
			{
				configuration.Common.RegisterTypeHandler(new SingleClassTypeHandlerPredicate(typeof(
					FieldsToTypeHandlerMigrationTestCase.Item)), _typeHandler);
			}
			return Db4oEmbedded.OpenFile(configuration, TempFile());
		}

		public virtual void Defragment(IDefragmentContext context)
		{
		}

		// TODO Auto-generated method stub
		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public virtual void Delete(IDeleteContext context)
		{
		}

		// TODO Auto-generated method stub
		public virtual object Read(IReadContext context)
		{
			// TODO Auto-generated method stub
			return null;
		}

		public virtual void Write(IWriteContext context, object obj)
		{
		}

		// TODO Auto-generated method stub
		public virtual IPreparedComparison PrepareComparison(IContext context, object obj
			)
		{
			// TODO Auto-generated method stub
			return null;
		}
	}
}
