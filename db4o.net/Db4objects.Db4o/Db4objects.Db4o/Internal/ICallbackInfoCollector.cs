/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

namespace Db4objects.Db4o.Internal
{
	public interface ICallbackInfoCollector
	{
		void Added(int id);

		void Updated(int id);

		void Deleted(int id);
	}
}
