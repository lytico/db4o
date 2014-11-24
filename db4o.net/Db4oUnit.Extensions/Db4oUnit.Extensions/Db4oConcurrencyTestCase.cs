/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit.Extensions;
using Db4objects.Db4o.Foundation;

namespace Db4oUnit.Extensions
{
	/// <exclude></exclude>
	public class Db4oConcurrencyTestCase : Db4oClientServerTestCase
	{
		private bool[] _done;

		/// <exception cref="System.Exception"></exception>
		protected override void Db4oSetupAfterStore()
		{
			InitTasksDoneFlag();
			base.Db4oSetupAfterStore();
		}

		private void InitTasksDoneFlag()
		{
			_done = new bool[ThreadCount()];
		}

		protected virtual void MarkTaskDone(int seq, bool done)
		{
			_done[seq] = done;
		}

		/// <exception cref="System.Exception"></exception>
		protected virtual void WaitForAllTasksDone()
		{
			while (!AreAllTasksDone())
			{
				Runtime4.Sleep(1);
			}
		}

		private bool AreAllTasksDone()
		{
			for (int i = 0; i < _done.Length; ++i)
			{
				if (!_done[i])
				{
					return false;
				}
			}
			return true;
		}
	}
}
