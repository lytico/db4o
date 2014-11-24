/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Config;

namespace Db4oUnit.Extensions
{
	public interface IRuntimeConfigureAction
	{
		void Apply(IConfiguration config);
	}
}
