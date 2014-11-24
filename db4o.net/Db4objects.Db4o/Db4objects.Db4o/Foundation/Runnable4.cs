/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Sharpen.Lang;

namespace Db4objects.Db4o.Foundation
{
	/// <exclude></exclude>
	public class Runnable4
	{
		private sealed class _IRunnable_10 : IRunnable
		{
			public _IRunnable_10()
			{
			}

			public void Run()
			{
			}
		}

		public static readonly IRunnable DoNothing = new _IRunnable_10();
	}
}
