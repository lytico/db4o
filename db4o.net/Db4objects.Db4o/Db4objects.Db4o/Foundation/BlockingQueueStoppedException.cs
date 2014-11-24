/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o;

namespace Db4objects.Db4o.Foundation
{
	/// <exclude></exclude>
	[System.Serializable]
	public class BlockingQueueStoppedException : Exception
	{
		public BlockingQueueStoppedException() : base()
		{
			if (DTrace.enabled)
			{
				DTrace.BlockingQueueStoppedException.Log();
			}
		}
	}
}
