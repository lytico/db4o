/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Tests.Common.Exceptions;

namespace Db4objects.Db4o.Tests.Common.Exceptions
{
	public class Db4oIOExceptionTestCase : Db4oIOExceptionTestCaseBase
	{
		public static void Main(string[] args)
		{
			new Db4oIOExceptionTestCase().RunSolo();
		}

		protected override void Configure(IConfiguration config)
		{
			base.Configure(config);
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestActivate()
		{
			Store(new Db4oIOExceptionTestCase.Item(3));
			Fixture().Config().ActivationDepth(1);
			Fixture().Reopen(this);
			Db4oIOExceptionTestCase.Item item = (Db4oIOExceptionTestCase.Item)((Db4oIOExceptionTestCase.Item
				)RetrieveOnlyInstance(typeof(Db4oIOExceptionTestCase.Item)));
			Assert.Expect(typeof(Db4oIOException), new _ICodeBlock_25(this, item));
		}

		private sealed class _ICodeBlock_25 : ICodeBlock
		{
			public _ICodeBlock_25(Db4oIOExceptionTestCase _enclosing, Db4oIOExceptionTestCase.Item
				 item)
			{
				this._enclosing = _enclosing;
				this.item = item;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				this._enclosing.TriggerException(true);
				this._enclosing.Db().Activate(item, 3);
			}

			private readonly Db4oIOExceptionTestCase _enclosing;

			private readonly Db4oIOExceptionTestCase.Item item;
		}

		public virtual void TestClose()
		{
			Assert.Expect(typeof(Db4oIOException), new _ICodeBlock_34(this));
		}

		private sealed class _ICodeBlock_34 : ICodeBlock
		{
			public _ICodeBlock_34(Db4oIOExceptionTestCase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				this._enclosing.TriggerException(true);
				this._enclosing.Db().Close();
			}

			private readonly Db4oIOExceptionTestCase _enclosing;
		}

		public virtual void TestCommit()
		{
			Store(new Db4oIOExceptionTestCase.Item(0));
			Assert.Expect(typeof(Db4oIOException), new _ICodeBlock_44(this));
		}

		private sealed class _ICodeBlock_44 : ICodeBlock
		{
			public _ICodeBlock_44(Db4oIOExceptionTestCase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				this._enclosing.TriggerException(true);
				this._enclosing.Db().Commit();
			}

			private readonly Db4oIOExceptionTestCase _enclosing;
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestDelete()
		{
			Store(new Db4oIOExceptionTestCase.Item(3));
			Db4oIOExceptionTestCase.Item item = (Db4oIOExceptionTestCase.Item)((Db4oIOExceptionTestCase.Item
				)RetrieveOnlyInstance(typeof(Db4oIOExceptionTestCase.Item)));
			Assert.Expect(typeof(Db4oIOException), new _ICodeBlock_55(this, item));
		}

		private sealed class _ICodeBlock_55 : ICodeBlock
		{
			public _ICodeBlock_55(Db4oIOExceptionTestCase _enclosing, Db4oIOExceptionTestCase.Item
				 item)
			{
				this._enclosing = _enclosing;
				this.item = item;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				this._enclosing.TriggerException(true);
				this._enclosing.Db().Delete(item);
			}

			private readonly Db4oIOExceptionTestCase _enclosing;

			private readonly Db4oIOExceptionTestCase.Item item;
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestGet()
		{
			Store(new Db4oIOExceptionTestCase.Item(3));
			Assert.Expect(typeof(Db4oIOException), new _ICodeBlock_65(this));
		}

		private sealed class _ICodeBlock_65 : ICodeBlock
		{
			public _ICodeBlock_65(Db4oIOExceptionTestCase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				this._enclosing.TriggerException(true);
				this._enclosing.Db().QueryByExample(typeof(Db4oIOExceptionTestCase.Item));
			}

			private readonly Db4oIOExceptionTestCase _enclosing;
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestGetAll()
		{
			Store(new Db4oIOExceptionTestCase.Item(3));
			Assert.Expect(typeof(Db4oIOException), new _ICodeBlock_75(this));
		}

		private sealed class _ICodeBlock_75 : ICodeBlock
		{
			public _ICodeBlock_75(Db4oIOExceptionTestCase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				this._enclosing.TriggerException(true);
				IObjectSet os = this._enclosing.Db().QueryByExample(null);
				while (os.HasNext())
				{
					os.Next();
				}
			}

			private readonly Db4oIOExceptionTestCase _enclosing;
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestQuery()
		{
			Store(new Db4oIOExceptionTestCase.Item(3));
			Assert.Expect(typeof(Db4oIOException), new _ICodeBlock_88(this));
		}

		private sealed class _ICodeBlock_88 : ICodeBlock
		{
			public _ICodeBlock_88(Db4oIOExceptionTestCase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				this._enclosing.TriggerException(true);
				this._enclosing.Db().Query(typeof(Db4oIOExceptionTestCase.Item));
			}

			private readonly Db4oIOExceptionTestCase _enclosing;
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestRollback()
		{
			Store(new Db4oIOExceptionTestCase.Item(3));
			Assert.Expect(typeof(Db4oIOException), new _ICodeBlock_98(this));
		}

		private sealed class _ICodeBlock_98 : ICodeBlock
		{
			public _ICodeBlock_98(Db4oIOExceptionTestCase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				this._enclosing.TriggerException(true);
				this._enclosing.Db().Rollback();
			}

			private readonly Db4oIOExceptionTestCase _enclosing;
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestSet()
		{
			Assert.Expect(typeof(Db4oIOException), new _ICodeBlock_107(this));
		}

		private sealed class _ICodeBlock_107 : ICodeBlock
		{
			public _ICodeBlock_107(Db4oIOExceptionTestCase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				this._enclosing.TriggerException(true);
				this._enclosing.Db().Store(new Db4oIOExceptionTestCase.Item(3));
			}

			private readonly Db4oIOExceptionTestCase _enclosing;
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestGetByUUID()
		{
			Fixture().Config().GenerateUUIDs(ConfigScope.Globally);
			Fixture().Reopen(this);
			Db4oIOExceptionTestCase.Item item = new Db4oIOExceptionTestCase.Item(1);
			Store(item);
			Db4oUUID uuid = Db().GetObjectInfo(item).GetUUID();
			Fixture().Reopen(this);
			Assert.Expect(typeof(Db4oIOException), new _ICodeBlock_122(this, uuid));
		}

		private sealed class _ICodeBlock_122 : ICodeBlock
		{
			public _ICodeBlock_122(Db4oIOExceptionTestCase _enclosing, Db4oUUID uuid)
			{
				this._enclosing = _enclosing;
				this.uuid = uuid;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				this._enclosing.TriggerException(true);
				this._enclosing.Db().GetByUUID(uuid);
			}

			private readonly Db4oIOExceptionTestCase _enclosing;

			private readonly Db4oUUID uuid;
		}

		public class Item
		{
			public Item(int depth)
			{
				member = new Db4oIOExceptionTestCase.DeepMemeber(depth);
			}

			public Db4oIOExceptionTestCase.DeepMemeber member;
		}

		public class DeepMemeber
		{
			public DeepMemeber(int depth)
			{
				if (depth > 0)
				{
					member = new Db4oIOExceptionTestCase.DeepMemeber(--depth);
				}
			}

			public Db4oIOExceptionTestCase.DeepMemeber member;
		}
	}
}
