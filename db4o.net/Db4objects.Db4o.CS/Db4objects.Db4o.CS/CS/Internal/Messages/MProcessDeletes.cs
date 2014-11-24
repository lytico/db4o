/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.CS.Internal.Messages;
using Db4objects.Db4o.Ext;
using Sharpen.Lang;

namespace Db4objects.Db4o.CS.Internal.Messages
{
	public class MProcessDeletes : Msg, IServerSideMessage
	{
		public void ProcessAtServer()
		{
			Container().WithTransaction(Transaction(), new _IRunnable_12(this));
		}

		private sealed class _IRunnable_12 : IRunnable
		{
			public _IRunnable_12(MProcessDeletes _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public void Run()
			{
				try
				{
					this._enclosing.Transaction().ProcessDeletes();
				}
				catch (Db4oException e)
				{
				}
			}

			private readonly MProcessDeletes _enclosing;
		}
		// Don't send the exception to the user because delete is asynchronous
	}
}
