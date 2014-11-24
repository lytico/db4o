/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Fixtures;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Consistency;
using Db4objects.Db4o.Events;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Tests.Common.Events;

namespace Db4objects.Db4o.Tests.Common.Events
{
	/// <exclude></exclude>
	public class ExceptionInUpdatingCallbackCorruptionTestCase : AbstractDb4oTestCase
		, IOptOutMultiSession
	{
		public static void Main(string[] args)
		{
			new ExceptionInUpdatingCallbackCorruptionTestCase().RunSolo();
		}

		public class Holder
		{
			public IList list = new ArrayList();

			public ExceptionInUpdatingCallbackCorruptionTestCase.Item item;
		}

		public class Item
		{
		}

		private bool doThrow;

		/// <exception cref="System.Exception"></exception>
		protected override void Configure(IConfiguration config)
		{
			config.ObjectClass(typeof(ExceptionInUpdatingCallbackCorruptionTestCase.Holder)).
				CascadeOnUpdate(true);
			config.ObjectClass(typeof(ExceptionInUpdatingCallbackCorruptionTestCase.Holder)).
				CascadeOnDelete(true);
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			Store(new ExceptionInUpdatingCallbackCorruptionTestCase.Holder());
			Store(new ExceptionInUpdatingCallbackCorruptionTestCase.Item());
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Db4oSetupAfterStore()
		{
			EventRegistryFactory.ForObjectContainer(Db()).Updated += new System.EventHandler<Db4objects.Db4o.Events.ObjectInfoEventArgs>
				(new _IEventListener4_48(this).OnEvent);
		}

		private sealed class _IEventListener4_48
		{
			public _IEventListener4_48(ExceptionInUpdatingCallbackCorruptionTestCase _enclosing
				)
			{
				this._enclosing = _enclosing;
			}

			public void OnEvent(object sender, Db4objects.Db4o.Events.ObjectInfoEventArgs args
				)
			{
				if (this._enclosing.doThrow)
				{
					if (((ObjectInfoEventArgs)args).Info.GetObject().GetType().Equals(typeof(ExceptionInUpdatingCallbackCorruptionTestCase.Item
						)))
					{
						throw new Exception();
					}
				}
			}

			private readonly ExceptionInUpdatingCallbackCorruptionTestCase _enclosing;
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestExceptionDuringItemUpdate()
		{
			ExceptionInUpdatingCallbackCorruptionTestCase.Holder holder = ((ExceptionInUpdatingCallbackCorruptionTestCase.Holder
				)RetrieveOnlyInstance(typeof(ExceptionInUpdatingCallbackCorruptionTestCase.Holder
				)));
			ExceptionInUpdatingCallbackCorruptionTestCase.Item item = ((ExceptionInUpdatingCallbackCorruptionTestCase.Item
				)RetrieveOnlyInstance(typeof(ExceptionInUpdatingCallbackCorruptionTestCase.Item)
				));
			holder.item = item;
			WithException(new _IBlock4_63(this, holder));
			CheckConsistencyFull();
		}

		private sealed class _IBlock4_63 : IBlock4
		{
			public _IBlock4_63(ExceptionInUpdatingCallbackCorruptionTestCase _enclosing, ExceptionInUpdatingCallbackCorruptionTestCase.Holder
				 holder)
			{
				this._enclosing = _enclosing;
				this.holder = holder;
			}

			public void Run()
			{
				this._enclosing.Db().Store(holder, int.MaxValue);
			}

			private readonly ExceptionInUpdatingCallbackCorruptionTestCase _enclosing;

			private readonly ExceptionInUpdatingCallbackCorruptionTestCase.Holder holder;
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestExceptionDuringExistingListUpdate()
		{
			ExceptionInUpdatingCallbackCorruptionTestCase.Holder holder = ((ExceptionInUpdatingCallbackCorruptionTestCase.Holder
				)RetrieveOnlyInstance(typeof(ExceptionInUpdatingCallbackCorruptionTestCase.Holder
				)));
			ExceptionInUpdatingCallbackCorruptionTestCase.Item item = ((ExceptionInUpdatingCallbackCorruptionTestCase.Item
				)RetrieveOnlyInstance(typeof(ExceptionInUpdatingCallbackCorruptionTestCase.Item)
				));
			holder.list.Add(item);
			WithException(new _IBlock4_75(this, holder));
			CheckConsistencyFull();
		}

		private sealed class _IBlock4_75 : IBlock4
		{
			public _IBlock4_75(ExceptionInUpdatingCallbackCorruptionTestCase _enclosing, ExceptionInUpdatingCallbackCorruptionTestCase.Holder
				 holder)
			{
				this._enclosing = _enclosing;
				this.holder = holder;
			}

			public void Run()
			{
				this._enclosing.Db().Store(holder, int.MaxValue);
			}

			private readonly ExceptionInUpdatingCallbackCorruptionTestCase _enclosing;

			private readonly ExceptionInUpdatingCallbackCorruptionTestCase.Holder holder;
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestExceptionDuringNewListUpdate()
		{
			ExceptionInUpdatingCallbackCorruptionTestCase.Holder holder = ((ExceptionInUpdatingCallbackCorruptionTestCase.Holder
				)RetrieveOnlyInstance(typeof(ExceptionInUpdatingCallbackCorruptionTestCase.Holder
				)));
			ExceptionInUpdatingCallbackCorruptionTestCase.Item item = ((ExceptionInUpdatingCallbackCorruptionTestCase.Item
				)RetrieveOnlyInstance(typeof(ExceptionInUpdatingCallbackCorruptionTestCase.Item)
				));
			holder.list = new ArrayList();
			holder.list.Add(item);
			WithException(new _IBlock4_88(this, holder));
			CheckConsistencyFull();
		}

		private sealed class _IBlock4_88 : IBlock4
		{
			public _IBlock4_88(ExceptionInUpdatingCallbackCorruptionTestCase _enclosing, ExceptionInUpdatingCallbackCorruptionTestCase.Holder
				 holder)
			{
				this._enclosing = _enclosing;
				this.holder = holder;
			}

			public void Run()
			{
				this._enclosing.Db().Store(holder, int.MaxValue);
			}

			private readonly ExceptionInUpdatingCallbackCorruptionTestCase _enclosing;

			private readonly ExceptionInUpdatingCallbackCorruptionTestCase.Holder holder;
		}

		private void WithException(IBlock4 block)
		{
			doThrow = true;
			try
			{
				block.Run();
			}
			catch (Exception)
			{
			}
			finally
			{
				doThrow = false;
			}
		}

		/// <exception cref="System.Exception"></exception>
		private void CheckConsistencyFull()
		{
			CheckConsistency();
			Commit();
			CheckConsistency();
			Reopen();
			CheckConsistency();
		}

		private void CheckConsistency()
		{
			ConsistencyReport report = new ConsistencyChecker((LocalObjectContainer)Container
				()).CheckSlotConsistency();
			if (!report.Consistent())
			{
				Assert.Fail(report.ToString());
			}
		}
	}
}
