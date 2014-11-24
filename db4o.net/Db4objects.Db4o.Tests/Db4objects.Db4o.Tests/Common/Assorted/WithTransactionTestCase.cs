/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Internal;
using Sharpen.Lang;

namespace Db4objects.Db4o.Tests.Common.Assorted
{
	public class WithTransactionTestCase : AbstractDb4oTestCase
	{
		public virtual void Test()
		{
			Transaction originalTransaction = Container().Transaction;
			Transaction transaction = null;
			lock (Container().Lock())
			{
				transaction = Container().NewUserTransaction();
			}
			Transaction finalTransaction = transaction;
			Container().WithTransaction(transaction, new _IRunnable_19(this, finalTransaction
				));
			Assert.AreSame(originalTransaction, Container().Transaction);
		}

		private sealed class _IRunnable_19 : IRunnable
		{
			public _IRunnable_19(WithTransactionTestCase _enclosing, Transaction finalTransaction
				)
			{
				this._enclosing = _enclosing;
				this.finalTransaction = finalTransaction;
			}

			public void Run()
			{
				Assert.AreSame(finalTransaction, this._enclosing.Container().Transaction);
			}

			private readonly WithTransactionTestCase _enclosing;

			private readonly Transaction finalTransaction;
		}
	}
}
