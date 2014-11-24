/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o;
using Db4objects.Db4o.Ext;

namespace Db4oUnit.Extensions.Dbmock
{
	public partial class MockServer : IObjectServer
	{
		public virtual bool Close()
		{
			throw new NotImplementedException();
		}

		public virtual IExtObjectServer Ext()
		{
			throw new NotImplementedException();
		}

		public virtual void GrantAccess(string userName, string password)
		{
			throw new NotImplementedException();
		}

		public virtual IObjectContainer OpenClient()
		{
			throw new NotImplementedException();
		}
	}
}
