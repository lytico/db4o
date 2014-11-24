/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Fixtures;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Config;
using Db4objects.Db4o.Internal.Ids;
using Db4objects.Db4o.Internal.Slots;
using Db4objects.Db4o.Tests.Common.Assorted;
using Db4objects.Db4o.Tests.Common.Exceptions;

namespace Db4objects.Db4o.Tests.Common.Exceptions
{
	public class InvalidSlotExceptionTestCase : AbstractDb4oTestCase, IOptOutIdSystem
	{
		private const int InvalidId = 3;

		private const int OutOfMemoryId = 4;

		public static void Main(string[] args)
		{
			new InvalidSlotExceptionTestCase().RunAll();
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Configure(IConfiguration config)
		{
			IIdSystemConfiguration idSystemConfiguration = Db4oLegacyConfigurationBridge.AsIdSystemConfiguration
				(config);
			idSystemConfiguration.UseCustomSystem(new _IIdSystemFactory_29());
		}

		private sealed class _IIdSystemFactory_29 : IIdSystemFactory
		{
			public _IIdSystemFactory_29()
			{
			}

			public IIdSystem NewInstance(LocalObjectContainer container)
			{
				return new InvalidSlotExceptionTestCase.MockIdSystem(container);
			}
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestInvalidSlotException()
		{
			Assert.Expect(typeof(Db4oRecoverableException), new _ICodeBlock_38(this));
			Assert.IsFalse(Db().IsClosed());
		}

		private sealed class _ICodeBlock_38 : ICodeBlock
		{
			public _ICodeBlock_38(InvalidSlotExceptionTestCase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				this._enclosing.Db().GetByID(InvalidSlotExceptionTestCase.InvalidId);
			}

			private readonly InvalidSlotExceptionTestCase _enclosing;
		}

		public virtual void TestDbNotClosedOnOutOfMemory()
		{
			Assert.Expect(typeof(Db4oRecoverableException), typeof(OutOfMemoryException), new 
				_ICodeBlock_47(this));
			Assert.IsFalse(Db().IsClosed());
		}

		private sealed class _ICodeBlock_47 : ICodeBlock
		{
			public _ICodeBlock_47(InvalidSlotExceptionTestCase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				this._enclosing.Db().GetByID(InvalidSlotExceptionTestCase.OutOfMemoryId);
			}

			private readonly InvalidSlotExceptionTestCase _enclosing;
		}

		public class A
		{
			internal InvalidSlotExceptionTestCase.A _a;

			public A(InvalidSlotExceptionTestCase.A a)
			{
				this._a = a;
			}
		}

		public class MockIdSystem : DelegatingIdSystem
		{
			public MockIdSystem(LocalObjectContainer container) : base(container)
			{
			}

			public override Slot CommittedSlot(int id)
			{
				if (id == OutOfMemoryId)
				{
					throw new OutOfMemoryException();
				}
				if (id == InvalidId)
				{
					throw new InvalidIDException(id);
				}
				return _delegate.CommittedSlot(id);
			}
		}
	}
}
