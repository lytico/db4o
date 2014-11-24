/* Copyright (C) 2009  Versant Inc.  http://www.db4o.com */
using Sharpen.Lang;

namespace Db4objects.Db4o.Foundation
{
	public delegate void Action4();

	public class RunnableAction : IRunnable
	{
		private readonly Action4 _action;

		public RunnableAction(Action4 action)
		{
			_action = action;
		}

		public void Run()
		{
			_action();
		}
	}
}
