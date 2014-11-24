/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
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
	public class TranslatorToTypehandlerMigrationTestCase : Db4oTestWithTempFile
	{
		public class Item
		{
			public Item(int id)
			{
				_id = id;
			}

			public int _id;
		}

		internal TranslatorToTypehandlerMigrationTestCase.ItemTranslator _translator;

		internal TranslatorToTypehandlerMigrationTestCase.ItemTypeHandler _typeHandler;

		public class ItemTranslator : IObjectTranslator
		{
			public virtual void OnActivate(IObjectContainer container, object applicationObject
				, object storedObject)
			{
				_activateCalls++;
				string str = (string)storedObject;
				TranslatorToTypehandlerMigrationTestCase.Item item = (TranslatorToTypehandlerMigrationTestCase.Item
					)applicationObject;
				item._id = int.Parse(str);
			}

			public virtual object OnStore(IObjectContainer container, object applicationObject
				)
			{
				_storeCalls++;
				TranslatorToTypehandlerMigrationTestCase.Item item = (TranslatorToTypehandlerMigrationTestCase.Item
					)applicationObject;
				return item._id.ToString();
			}

			public virtual Type StoredClass()
			{
				return typeof(string);
			}

			private int _activateCalls;

			private int _storeCalls;

			public virtual void Reset()
			{
				_activateCalls = 0;
				_storeCalls = 0;
			}

			public virtual int ActivateCalls()
			{
				return _activateCalls;
			}

			public virtual int StoreCalls()
			{
				return _storeCalls;
			}
		}

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
				((TranslatorToTypehandlerMigrationTestCase.Item)context.PersistentObject())._id =
					 context.ReadInt() - 42;
			}

			public virtual void Write(IWriteContext context, object obj)
			{
				_writeCalls++;
				TranslatorToTypehandlerMigrationTestCase.Item item = (TranslatorToTypehandlerMigrationTestCase.Item
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

		/// <exception cref="System.Exception"></exception>
		public override void SetUp()
		{
			_translator = new TranslatorToTypehandlerMigrationTestCase.ItemTranslator();
		}

		public virtual void TestMigration()
		{
			_typeHandler = null;
			_translator = new TranslatorToTypehandlerMigrationTestCase.ItemTranslator();
			Store(new TranslatorToTypehandlerMigrationTestCase.Item(42));
			AssertTranslatorCalls(1, 0);
			TranslatorToTypehandlerMigrationTestCase.Item item = RetrieveOnlyItemInstance();
			Assert.AreEqual(42, item._id);
			AssertTranslatorCalls(0, 1);
			_typeHandler = new TranslatorToTypehandlerMigrationTestCase.ItemTypeHandler();
			item = RetrieveOnlyItemInstance();
			Assert.AreEqual(42, item._id);
			AssertTranslatorCalls(0, 1);
			AssertTypeHandlerCalls(0, 0);
			UpdateItem();
			AssertTranslatorCalls(0, 1);
			AssertTypeHandlerCalls(1, 0);
			item = RetrieveOnlyItemInstance();
			Assert.AreEqual(42, item._id);
			AssertTranslatorCalls(0, 0);
			AssertTypeHandlerCalls(0, 1);
		}

		public virtual void TestTranslator()
		{
			_typeHandler = null;
			_translator = new TranslatorToTypehandlerMigrationTestCase.ItemTranslator();
			Store(new TranslatorToTypehandlerMigrationTestCase.Item(42));
			AssertTranslatorCalls(1, 0);
			TranslatorToTypehandlerMigrationTestCase.Item item = RetrieveOnlyItemInstance();
			Assert.AreEqual(42, item._id);
			AssertTranslatorCalls(0, 1);
			UpdateItem();
			AssertTranslatorCalls(1, 1);
		}

		public virtual void TestTypeHandler()
		{
			_translator = null;
			_typeHandler = new TranslatorToTypehandlerMigrationTestCase.ItemTypeHandler();
			Store(new TranslatorToTypehandlerMigrationTestCase.Item(42));
			AssertTypeHandlerCalls(1, 0);
			TranslatorToTypehandlerMigrationTestCase.Item item = RetrieveOnlyItemInstance();
			Assert.AreEqual(42, item._id);
			AssertTypeHandlerCalls(0, 1);
			UpdateItem();
			AssertTypeHandlerCalls(1, 1);
		}

		private void AssertTranslatorCalls(int storeCalls, int activateCalls)
		{
			Assert.AreEqual(storeCalls, _translator.StoreCalls());
			Assert.AreEqual(activateCalls, _translator.ActivateCalls());
		}

		private void AssertTypeHandlerCalls(int writeCalls, int readCalls)
		{
			Assert.AreEqual(writeCalls, _typeHandler.WriteCalls());
			Assert.AreEqual(readCalls, _typeHandler.ReadCalls());
		}

		private TranslatorToTypehandlerMigrationTestCase.Item RetrieveOnlyItemInstance()
		{
			IObjectContainer db = OpenContainer();
			try
			{
				IObjectSet objectSet = db.Query(typeof(TranslatorToTypehandlerMigrationTestCase.Item
					));
				Assert.AreEqual(1, objectSet.Count);
				TranslatorToTypehandlerMigrationTestCase.Item item = (TranslatorToTypehandlerMigrationTestCase.Item
					)objectSet.Next();
				return item;
			}
			finally
			{
				db.Close();
			}
		}

		private void Store(TranslatorToTypehandlerMigrationTestCase.Item item)
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
				IObjectSet objectSet = db.Query(typeof(TranslatorToTypehandlerMigrationTestCase.Item
					));
				db.Store(objectSet.Next());
			}
			finally
			{
				db.Close();
			}
		}

		private IObjectContainer OpenContainer()
		{
			if (_translator != null)
			{
				_translator.Reset();
			}
			if (_typeHandler != null)
			{
				_typeHandler.Reset();
			}
			IEmbeddedConfiguration configuration = NewConfiguration();
			if (_translator != null)
			{
				configuration.Common.ObjectClass(typeof(TranslatorToTypehandlerMigrationTestCase.Item
					)).Translate(_translator);
			}
			if (_typeHandler != null)
			{
				configuration.Common.RegisterTypeHandler(new SingleClassTypeHandlerPredicate(typeof(
					TranslatorToTypehandlerMigrationTestCase.Item)), _typeHandler);
			}
			IObjectContainer db = Db4oEmbedded.OpenFile(configuration, TempFile());
			return db;
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
