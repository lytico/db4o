/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

namespace Db4objects.Db4o.Tests.Common.Reflect.Custom
{
	public class PersistenceContext
	{
		private readonly string _url;

		private object _providerContext;

		public PersistenceContext(string url)
		{
			_url = url;
		}

		public virtual string Url()
		{
			return _url;
		}

		public virtual void SetProviderContext(object context)
		{
			_providerContext = context;
		}

		public virtual object GetProviderContext()
		{
			return _providerContext;
		}
	}
}
