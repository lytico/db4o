/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Fixtures;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Constraints;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Constraints;

namespace Db4objects.Db4o.Tests.Common.Constraints
{
	public class UniqueFieldValueConstraintTestCase : AbstractDb4oTestCase, ICustomClientServerConfiguration
	{
		public class Item
		{
			public string _str;

			public Item()
			{
			}

			public Item(string str)
			{
				_str = str;
			}
		}

		public class IHaveNothingToDoWithItemInstances
		{
			public static int _constructorCallsCounter;

			public IHaveNothingToDoWithItemInstances()
			{
				_constructorCallsCounter++;
			}

			public IHaveNothingToDoWithItemInstances(int value)
			{
				_constructorCallsCounter = 0;
			}
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void ConfigureClient(IConfiguration config)
		{
			base.Configure(config);
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void ConfigureServer(IConfiguration config)
		{
			Configure(config);
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Configure(IConfiguration config)
		{
			base.Configure(config);
			IndexField(config, typeof(UniqueFieldValueConstraintTestCase.Item), "_str");
			config.Add(new UniqueFieldValueConstraint(typeof(UniqueFieldValueConstraintTestCase.Item
				), "_str"));
			config.ObjectClass(typeof(UniqueFieldValueConstraintTestCase.IHaveNothingToDoWithItemInstances
				)).CallConstructor(true);
			config.WeakReferences(false);
		}

		//Constructor calls is reliable only if we don't use WeakReferences.
		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			AddItem("1");
			AddItem("2");
			AddItem("3");
		}

		public virtual void TestNewViolates()
		{
			AddItem("2");
			CommitExpectingViolation();
		}

		public virtual void TestUpdateViolates()
		{
			UpdateItem("2", "3");
			CommitExpectingViolation();
		}

		public virtual void TestUpdateDoesNotViolate()
		{
			UpdateItem("2", "4");
			Db().Commit();
		}

		public virtual void TestUpdatingSameObjectDoesNotViolate()
		{
			UpdateItem("2", "2");
			Db().Commit();
		}

		public virtual void TestNewAfterDeleteDoesNotViolate()
		{
			DeleteItem("2");
			AddItem("2");
			Db().Commit();
		}

		public virtual void TestDeleteAfterNewDoesNotViolate()
		{
			UniqueFieldValueConstraintTestCase.Item existing = QueryItem("2");
			AddItem("2");
			Db().Delete(existing);
			Db().Commit();
		}

		private void DeleteItem(string value)
		{
			Db().Delete(QueryItem(value));
		}

		private void CommitExpectingViolation()
		{
			Assert.Expect(typeof(UniqueFieldValueConstraintViolationException), new _ICodeBlock_104
				(this));
			Db().Rollback();
		}

		private sealed class _ICodeBlock_104 : ICodeBlock
		{
			public _ICodeBlock_104(UniqueFieldValueConstraintTestCase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				this._enclosing.Db().Commit();
			}

			private readonly UniqueFieldValueConstraintTestCase _enclosing;
		}

		private UniqueFieldValueConstraintTestCase.Item QueryItem(string str)
		{
			IQuery q = NewQuery(typeof(UniqueFieldValueConstraintTestCase.Item));
			q.Descend("_str").Constrain(str);
			return (UniqueFieldValueConstraintTestCase.Item)q.Execute().Next();
		}

		private void AddItem(string value)
		{
			Store(new UniqueFieldValueConstraintTestCase.Item(value));
		}

		private void UpdateItem(string existing, string newValue)
		{
			UniqueFieldValueConstraintTestCase.Item item = QueryItem(existing);
			item._str = newValue;
			Store(item);
		}

		public virtual void TestObjectsAreNotReadUnnecessarily()
		{
			AddItem("5");
			Store(new UniqueFieldValueConstraintTestCase.IHaveNothingToDoWithItemInstances(unchecked(
				(int)(0xdb40))));
			Db().Commit();
			Assert.AreEqual(ExpectedConstructorsCalls(), UniqueFieldValueConstraintTestCase.IHaveNothingToDoWithItemInstances
				._constructorCallsCounter);
		}

		private int ExpectedConstructorsCalls()
		{
			if (IsNetworkClientServer())
			{
				return ExpectedConstructorCallOnCSMode();
			}
			return 1;
		}

		private int ExpectedConstructorCallOnCSMode()
		{
			return 2;
		}

		private bool IsNetworkClientServer()
		{
			return IsMultiSession() && !IsEmbedded();
		}
	}
}
