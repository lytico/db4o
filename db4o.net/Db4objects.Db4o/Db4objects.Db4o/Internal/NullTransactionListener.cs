/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o;

namespace Db4objects.Db4o.Internal
{
	public class NullTransactionListener : ITransactionListener
	{
		public static readonly ITransactionListener Instance = new Db4objects.Db4o.Internal.NullTransactionListener
			();

		private NullTransactionListener()
		{
		}

		public virtual void PostRollback()
		{
		}

		public virtual void PreCommit()
		{
		}
	}
}
