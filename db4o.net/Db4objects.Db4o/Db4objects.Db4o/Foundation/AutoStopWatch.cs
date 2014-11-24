/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Foundation;

namespace Db4objects.Db4o.Foundation
{
	public class AutoStopWatch : StopWatch
	{
		public AutoStopWatch()
		{
			Start();
		}
	}
}
