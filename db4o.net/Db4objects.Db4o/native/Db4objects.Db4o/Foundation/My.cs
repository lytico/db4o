/* Copyright (C) 2009  Versant Inc.  http://www.db4o.com */

namespace Db4objects.Db4o.Foundation
{
	public class My<TService>
	{
		public static TService Instance
		{
			get { return (TService)Environments.My(typeof(TService));  }
		}
	}
}
