/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Drs;
using Db4objects.Drs.Inside;

namespace Db4objects.Drs.Inside
{
	internal sealed class ReplicationEventImpl : IReplicationEvent
	{
		internal readonly ObjectStateImpl _stateInProviderA = new ObjectStateImpl();

		internal readonly ObjectStateImpl _stateInProviderB = new ObjectStateImpl();

		private bool _isConflict;

		internal IObjectState _actionChosenState;

		internal bool _actionWasChosen;

		internal bool _actionShouldStopTraversal;

		internal long _creationDate;

		public IObjectState StateInProviderA()
		{
			return _stateInProviderA;
		}

		public IObjectState StateInProviderB()
		{
			return _stateInProviderB;
		}

		public long ObjectCreationDate()
		{
			return _creationDate;
		}

		public bool IsConflict()
		{
			return _isConflict;
		}

		public void OverrideWith(IObjectState chosen)
		{
			if (_actionWasChosen)
			{
				throw new Exception();
			}
			//FIXME Use Db4o's standard exception throwing.
			_actionWasChosen = true;
			_actionChosenState = chosen;
		}

		public void StopTraversal()
		{
			_actionShouldStopTraversal = true;
		}

		internal void ResetAction()
		{
			_actionChosenState = null;
			_actionWasChosen = false;
			_actionShouldStopTraversal = false;
			_creationDate = -1;
		}

		internal void Conflict(bool isConflict)
		{
			_isConflict = isConflict;
		}
	}
}
