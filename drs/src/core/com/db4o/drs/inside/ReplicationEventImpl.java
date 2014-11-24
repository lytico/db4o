/* Copyright (C) 2004 - 2008  Versant Inc.  http://www.db4o.com

This file is part of the db4o open source object database.

db4o is free software; you can redistribute it and/or modify it under
the terms of version 2 of the GNU General Public License as published
by the Free Software Foundation and as clarified by db4objects' GPL 
interpretation policy, available at
http://www.db4o.com/about/company/legalpolicies/gplinterpretation/
Alternatively you can write to db4objects, Inc., 1900 S Norfolk Street,
Suite 350, San Mateo, CA 94403, USA.

db4o is distributed in the hope that it will be useful, but WITHOUT ANY
WARRANTY; without even the implied warranty of MERCHANTABILITY or
FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License
for more details.

You should have received a copy of the GNU General Public License along
with this program; if not, write to the Free Software Foundation, Inc.,
59 Temple Place - Suite 330, Boston, MA  02111-1307, USA. */
package com.db4o.drs.inside;

import com.db4o.drs.ObjectState;
import com.db4o.drs.ReplicationEvent;

final class ReplicationEventImpl implements ReplicationEvent {

	final ObjectStateImpl _stateInProviderA = new ObjectStateImpl();
	final ObjectStateImpl _stateInProviderB = new ObjectStateImpl();
	private boolean _isConflict;

	ObjectState _actionChosenState;
	boolean _actionWasChosen;
	boolean _actionShouldStopTraversal;
	long _creationDate;

	public ObjectState stateInProviderA() {
		return _stateInProviderA;
	}

	public ObjectState stateInProviderB() {
		return _stateInProviderB;
	}

	public long objectCreationDate() {
		return _creationDate;
	}

	public boolean isConflict() {
		return _isConflict;
	}

	public void overrideWith(ObjectState chosen) {
		if (_actionWasChosen) throw new RuntimeException(); //FIXME Use Db4o's standard exception throwing.
		_actionWasChosen = true;
		_actionChosenState = chosen;
	}

	public void stopTraversal() {
		_actionShouldStopTraversal = true;
	}

	void resetAction() {
		_actionChosenState = null;
		_actionWasChosen = false;
		_actionShouldStopTraversal = false;
		_creationDate = -1;
	}

	void conflict(boolean isConflict) {
		_isConflict = isConflict;
	}

}
