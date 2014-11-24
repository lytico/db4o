/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Fixtures;
using Db4oUnit.Fixtures;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Config;
using Db4objects.Db4o.Internal.Ids;
using Db4objects.Db4o.Tests.Common.Exceptions;
using Db4objects.Db4o.Tests.Common.Exceptions.Propagation;

namespace Db4objects.Db4o.Tests.Common.Exceptions.Propagation
{
	public class ExceptionDuringTopLevelCallTestSuite : FixtureBasedTestSuite, IDb4oTestCase
		, IOptOutNetworkingCS, IOptOutIdSystem
	{
		public class ExceptionDuringTopLevelCallTestUnit : AbstractDb4oTestCase
		{
			private ExceptionSimulatingStorage _storage;

			private ExceptionSimulatingIdSystem _idSystem;

			private IIdSystemFactory _idSystemFactory;

			private object _unactivated;

			public class Item
			{
				public string _name;
			}

			/// <exception cref="System.Exception"></exception>
			protected override void Configure(IConfiguration config)
			{
				if (Platform4.NeedsLockFileThread())
				{
					config.LockDatabaseFile(false);
				}
				IExceptionPropagationFixture propagationFixture = CurrentExceptionPropagationFixture
					();
				IExceptionFactory exceptionFactory = new _IExceptionFactory_37(propagationFixture
					);
				_storage = new ExceptionSimulatingStorage(config.Storage, exceptionFactory);
				config.Storage = _storage;
				_idSystemFactory = new _IIdSystemFactory_61(this, exceptionFactory);
				ConfigureIdSystem(config);
			}

			private sealed class _IExceptionFactory_37 : IExceptionFactory
			{
				public _IExceptionFactory_37(IExceptionPropagationFixture propagationFixture)
				{
					this.propagationFixture = propagationFixture;
					this._alreadyCalled = false;
				}

				private bool _alreadyCalled;

				public void ThrowException()
				{
					try
					{
						if (!this._alreadyCalled)
						{
							propagationFixture.ThrowInitialException();
						}
						else
						{
							propagationFixture.ThrowShutdownException();
						}
					}
					finally
					{
						this._alreadyCalled = true;
					}
				}

				public void ThrowOnClose()
				{
					propagationFixture.ThrowCloseException();
				}

				private readonly IExceptionPropagationFixture propagationFixture;
			}

			private sealed class _IIdSystemFactory_61 : IIdSystemFactory
			{
				public _IIdSystemFactory_61(ExceptionDuringTopLevelCallTestUnit _enclosing, IExceptionFactory
					 exceptionFactory)
				{
					this._enclosing = _enclosing;
					this.exceptionFactory = exceptionFactory;
				}

				public IIdSystem NewInstance(LocalObjectContainer container)
				{
					this._enclosing._idSystem = new ExceptionSimulatingIdSystem(container, exceptionFactory
						);
					return this._enclosing._idSystem;
				}

				private readonly ExceptionDuringTopLevelCallTestUnit _enclosing;

				private readonly IExceptionFactory exceptionFactory;
			}

			private void ConfigureIdSystem(IConfiguration config)
			{
				Db4oLegacyConfigurationBridge.AsIdSystemConfiguration(config).UseCustomSystem(_idSystemFactory
					);
			}

			/// <exception cref="System.Exception"></exception>
			protected override void Db4oSetupAfterStore()
			{
				Store(new ExceptionDuringTopLevelCallTestSuite.ExceptionDuringTopLevelCallTestUnit.Item
					());
			}

			public virtual void TestExceptionDuringTopLevelCall()
			{
				_unactivated = ((ExceptionDuringTopLevelCallTestSuite.ExceptionDuringTopLevelCallTestUnit.Item
					)RetrieveOnlyInstance(typeof(ExceptionDuringTopLevelCallTestSuite.ExceptionDuringTopLevelCallTestUnit.Item
					)));
				Db().Deactivate(_unactivated);
				_storage.TriggerException(true);
				_idSystem.TriggerException(true);
				DatabaseContext context = new DatabaseContext(Db(), _unactivated);
				CurrentExceptionPropagationFixture().AssertExecute(context, CurrentOperationFixture
					());
				if (context.StorageIsClosed())
				{
					AssertIsNotLocked(FileSession().FileName());
				}
			}

			private void AssertIsNotLocked(string fileName)
			{
				IEmbeddedConfiguration embeddedConfiguration = Db4oEmbedded.NewConfiguration();
				embeddedConfiguration.IdSystem.UseCustomSystem(_idSystemFactory);
				IObjectContainer oc = Db4oEmbedded.OpenFile(embeddedConfiguration, fileName);
				oc.Close();
			}

			private IExceptionPropagationFixture CurrentExceptionPropagationFixture()
			{
				return ((IExceptionPropagationFixture)ExceptionBehaviourFixture.Value);
			}

			private TopLevelOperation CurrentOperationFixture()
			{
				return ((TopLevelOperation)OperationFixture.Value);
			}

			/// <exception cref="System.Exception"></exception>
			protected override void Db4oTearDownBeforeClean()
			{
				_storage.TriggerException(false);
				_idSystem.TriggerException(false);
			}
		}

		private static FixtureVariable ExceptionBehaviourFixture = FixtureVariable.NewInstance
			("exc");

		private static FixtureVariable OperationFixture = FixtureVariable.NewInstance("op"
			);

		public override IFixtureProvider[] FixtureProviders()
		{
			return new IFixtureProvider[] { new Db4oFixtureProvider(), new SimpleFixtureProvider
				(ExceptionBehaviourFixture, new object[] { new OutOfMemoryErrorPropagationFixture
				(), new OneTimeDb4oExceptionPropagationFixture(), new OneTimeRuntimeExceptionPropagationFixture
				(), new RecurringDb4oExceptionPropagationFixture(), new RecurringRuntimeExceptionPropagationFixture
				(), new RecoverableExceptionPropagationFixture() }), new SimpleFixtureProvider(OperationFixture
				, new object[] { new _TopLevelOperation_123("commit"), new _TopLevelOperation_127
				("store"), new _TopLevelOperation_131("activate"), new _TopLevelOperation_145("peek"
				), new _TopLevelOperation_149("qbe"), new _TopLevelOperation_153("query") }) };
		}

		private sealed class _TopLevelOperation_123 : TopLevelOperation
		{
			public _TopLevelOperation_123(string baseArg1) : base(baseArg1)
			{
			}

			public override void Apply(DatabaseContext context)
			{
				context._db.Commit();
			}
		}

		private sealed class _TopLevelOperation_127 : TopLevelOperation
		{
			public _TopLevelOperation_127(string baseArg1) : base(baseArg1)
			{
			}

			public override void Apply(DatabaseContext context)
			{
				context._db.Store(new Item());
			}
		}

		private sealed class _TopLevelOperation_131 : TopLevelOperation
		{
			public _TopLevelOperation_131(string baseArg1) : base(baseArg1)
			{
			}

			public override void Apply(DatabaseContext context)
			{
				context._db.Activate(context._unactivated, int.MaxValue);
			}
		}

		private sealed class _TopLevelOperation_145 : TopLevelOperation
		{
			public _TopLevelOperation_145(string baseArg1) : base(baseArg1)
			{
			}

			// - no deactivate test, since it doesn't trigger I/O activity
			// - no getByID test, not refactored to asTopLevelCall, since it has custom, more relaxed exception handling -> InvalidSlotExceptionTestCase
			// FIXME doesn't trigger initial exception - deletes are processed in finally block
			//					new TopLevelOperation("delete") {
			//						@Override
			//						public void apply(DatabaseContext context) {
			//							context._db.delete(context._unactivated);
			//						}
			//					},
			public override void Apply(DatabaseContext context)
			{
				context._db.Ext().PeekPersisted(context._unactivated, 1, true);
			}
		}

		private sealed class _TopLevelOperation_149 : TopLevelOperation
		{
			public _TopLevelOperation_149(string baseArg1) : base(baseArg1)
			{
			}

			public override void Apply(DatabaseContext context)
			{
				context._db.QueryByExample(new Item());
			}
		}

		private sealed class _TopLevelOperation_153 : TopLevelOperation
		{
			public _TopLevelOperation_153(string baseArg1) : base(baseArg1)
			{
			}

			public override void Apply(DatabaseContext context)
			{
				IObjectSet result = context._db.Query().Execute();
				if (result.HasNext())
				{
					result.Next();
				}
			}
		}

		public override Type[] TestUnits()
		{
			return new Type[] { typeof(ExceptionDuringTopLevelCallTestSuite.ExceptionDuringTopLevelCallTestUnit
				) };
		}
	}
}
