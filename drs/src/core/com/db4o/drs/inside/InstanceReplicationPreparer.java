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

import static com.db4o.drs.foundation.Logger4Support.*;

import com.db4o.drs.*;
import com.db4o.drs.foundation.*;
import com.db4o.drs.inside.traversal.*;
import com.db4o.foundation.*;
import com.db4o.internal.*;
import com.db4o.reflect.*;

class InstanceReplicationPreparer implements Visitor {

	private final ReplicationProviderInside _providerA;
	private final ReplicationProviderInside _providerB;
	private final ReplicationProvider _directionTo;
	private final ReplicationEventListener _listener;
	private final boolean _isReplicatingOnlyDeletions;
	
	private final HashSet4 _uuidsProcessedInSession;
	private final Traverser _traverser;
	private final ReplicationReflector _reflector;
	private final CollectionHandler _collectionHandler;

	/**
	 * Purpose: handle circular references
	 * TODO Big Refactoring: Evolve this to handle ALL reference logic (!) and remove it from the providers. 
	 */
	private final IdentitySet4 _objectsPreparedToReplicate = new IdentitySet4(100);
	/**
	 * key = object originated from one provider
	 * value = the counterpart ReplicationReference of the original object
	 */
	private Map4 _counterpartRefsByOriginal = new IdentityHashtable4(100);
	
	private final ReplicationEventImpl _event;
	private final ObjectStateImpl _stateInA;
	private final ObjectStateImpl _stateInB;

	private Object _obj;
	private Object _referencingObject;
	private String _fieldName;	
	
	InstanceReplicationPreparer(ReplicationProviderInside providerA, ReplicationProviderInside providerB, ReplicationProvider directionTo, ReplicationEventListener listener, boolean isReplicatingOnlyDeletions, HashSet4 uuidsProcessedInSession, Traverser traverser, ReplicationReflector reflector, CollectionHandler collectionHandler) {
		_event = new ReplicationEventImpl();
		_stateInA = _event._stateInProviderA;
		_stateInB = _event._stateInProviderB;
		
		_providerA = providerA;
		_providerB = providerB;
		_directionTo = directionTo;
		_listener = listener;
		_isReplicatingOnlyDeletions = isReplicatingOnlyDeletions;
		_uuidsProcessedInSession = uuidsProcessedInSession;
		_traverser = traverser;
		_reflector = reflector;
		_collectionHandler = collectionHandler;
	}
	
	private ReplicationProviderInside directionFrom() {
		return _providerA == _directionTo ? _providerB : _providerA;
	}


	public final boolean visit(Object obj) {
		if (directionFrom().isSecondClassObject(obj)) return false;
		if (isValueType(obj)) return true;
		if (_objectsPreparedToReplicate.contains(obj)) return false;
		_objectsPreparedToReplicate.add(obj);
		return prepareObjectToBeReplicated(obj, null, null);
	}

	private boolean isValueType(Object o) {
		return ReplicationPlatform.isValueType(o);
	}
	
	private boolean prepareObjectToBeReplicated(Object obj, Object referencingObject, String fieldName) {
		//TODO Optimization: keep track of the peer we are traversing to avoid having to look in both.
		
		logIdentity(obj);

		_obj = obj;
		_referencingObject = referencingObject;
		_fieldName = fieldName;

		ReplicationReference refA = _providerA.produceReference(_obj, _referencingObject, _fieldName);
		ReplicationReference refB = _providerB.produceReference(_obj, _referencingObject, _fieldName);
		
		ReflectClass claxx = _reflector.forObject(_obj);
		Reflector reflector = claxx.reflector();

		if (refA == null && refB == null) {
			if (Platform4.isEnum(reflector, claxx)) {
				return false;
			}
			throw new RuntimeException("" + _obj.getClass() + " " + _obj + " must be stored in one of the databases being replicated.");
		}
		if (refA != null && refB != null) {
			if (Platform4.isEnum(reflector, claxx)) {
				return false;
			}
			throw new RuntimeException("" + _obj.getClass() + " " + _obj + " cannot be referenced by both databases being replicated."); //FIXME: Use db4o's standard for throwing exceptions.
		}

		ReplicationProviderInside owner = refA == null ? _providerB : _providerA;
		ReplicationReference ownerRef = refA == null ? refB : refA;

		ReplicationProviderInside other = other(owner);

		DrsUUID uuid = ownerRef.uuid();
		ReplicationReference otherRef = other.produceReferenceByUUID(uuid, _obj.getClass());

		if (refA == null){
			refA = otherRef;
		}else{
			refB = otherRef;
		}

		//TODO for circular referenced object, otherRef should not be null in the subsequent pass.
		//But db4o always return null. A bug. check!
		if (otherRef == null) { //Object is only present in one ReplicationProvider. Missing in the other. Could have been deleted or never replicated.
			if (wasProcessed(uuid)){
				ReplicationReference otherProcessedRef = other.produceReferenceByUUID(uuid, _obj.getClass());
				if(otherProcessedRef != null){
					ownerRef.setCounterpart(otherProcessedRef.object());
				}
				return false;
			}
			markAsProcessed(uuid);

			long creationTime = ownerRef.uuid().getLongPart();

			if (creationTime > owner.timeStamps().from()) { //if it was created after the last time two ReplicationProviders were replicated it has to be treated as new.
				if (_isReplicatingOnlyDeletions){
					return false;
				}
				return handleNewObject(_obj, ownerRef, owner, other, _referencingObject, _fieldName, true, false);
			} else {
				// If it was created before the last time two ReplicationProviders were replicated it has to be treated as deleted.
				// No, not always, in a three-way replication setup it can also be new.
				return handleMissingObjectInOther(_obj, ownerRef, owner, other, _referencingObject, _fieldName);
			}
		}

		if (_isReplicatingOnlyDeletions) return false;

		ownerRef.setCounterpart(otherRef.object());
		otherRef.setCounterpart(ownerRef.object());
		
		if (wasProcessed(uuid)) return false;  //Has to be done AFTER the counterpart is set because object yet to be replicated might reference the current one, replicated previously.
		markAsProcessed(uuid);

		Object objectA = refA.object();
		Object objectB = refB.object();

		boolean changedInA = _providerA.wasModifiedSinceLastReplication(refA);
		//System.out.println("changedInA = " + changedInA);
		boolean changedInB = _providerB.wasModifiedSinceLastReplication(refB);
		//System.out.println("changedInB = " + changedInB);

		if (!changedInA && !changedInB) return false;

		boolean conflict = false;
		if (changedInA && changedInB) conflict = true;
		if (changedInA && _directionTo == _providerA) conflict = true;
		if (changedInB && _directionTo == _providerB) conflict = true;

		Object prevailing = _obj;

		_providerA.activate(objectA);
		_providerB.activate(objectB);

		_event.resetAction();
		_event.conflict(conflict);

		_event._creationDate = TimeStampIdGenerator.idToMilliseconds(uuid.getLongPart());

		_stateInA.setAll(objectA, false, changedInA, TimeStampIdGenerator.idToMilliseconds(ownerRef.version()));
		_stateInB.setAll(objectB, false, changedInB, TimeStampIdGenerator.idToMilliseconds(otherRef.version()));
		_listener.onReplicate(_event);

		if (conflict) {
			if (!_event._actionWasChosen) throwReplicationConflictException();
			if (_event._actionChosenState == null) return false;
			if (_event._actionChosenState == _stateInA) prevailing = objectA;
			if (_event._actionChosenState == _stateInB) prevailing = objectB;
		} else {
			if (_event._actionWasChosen) {
				if (_event._actionChosenState == _stateInA) prevailing = objectA;
				if (_event._actionChosenState == _stateInB) prevailing = objectB;
				if (_event._actionChosenState == null) return false;
			} else {
				if (changedInA) prevailing = objectA;
				if (changedInB) prevailing = objectB;
			}
		}

		ReplicationProviderInside prevailingPeer = prevailing == objectA ? _providerA : _providerB;
		if (_directionTo == prevailingPeer) return false;

		if (!conflict)
			prevailingPeer.activate(prevailing); //Already activated if there was a conflict.

		if (prevailing != _obj) {
			otherRef.setCounterpart(_obj);
			otherRef.markForReplicating(true);
			markAsNotProcessed(uuid);
			_traverser.extendTraversalTo(prevailing); //Now we start traversing objects on the other peer! Is that cool or what? ;)
		} else {
			ownerRef.markForReplicating(true);
		}

		return !_event._actionShouldStopTraversal;
	}


	private void markAsNotProcessed(DrsUUID uuid) {
		_uuidsProcessedInSession.remove(uuid);
	}


	private void markAsProcessed(DrsUUID uuid) {
		if (_uuidsProcessedInSession.contains(uuid)){
			throw new RuntimeException("illegal state");
		}
		_uuidsProcessedInSession.add(uuid);
	}


	private boolean wasProcessed(DrsUUID uuid) {
		return _uuidsProcessedInSession.contains(uuid);
	}


	private ReplicationProviderInside other(ReplicationProviderInside peer) {
		return peer == _providerA ? _providerB : _providerA;
	}


	private boolean handleMissingObjectInOther(Object obj, ReplicationReference ownerRef,
			ReplicationProviderInside owner, ReplicationProviderInside other,
			Object referencingObject, String fieldName) {
		boolean isConflict = false;
		boolean wasModified = owner.wasModifiedSinceLastReplication(ownerRef);
		if (wasModified) isConflict = true;
		if (_directionTo == other) isConflict = true;

		Object prevailing = null; //by default, deletion prevails
		if (isConflict) owner.activate(obj);

		_event.resetAction();
		_event.conflict(isConflict);

		_event._creationDate = TimeStampIdGenerator.idToMilliseconds(ownerRef.uuid().getLongPart());
		long modificationDate = TimeStampIdGenerator.idToMilliseconds(ownerRef.version());

		if (owner == _providerA) {
			_stateInA.setAll(obj, false, wasModified, modificationDate);
			_stateInB.setAll(null, false, false, ObjectStateImpl.UNKNOWN);
		} else { //owner == _providerB
			_stateInA.setAll(null, false, false, ObjectStateImpl.UNKNOWN);
			_stateInB.setAll(obj, false, wasModified, modificationDate);
		}

		_listener.onReplicate(_event);

		if (isConflict && !_event._actionWasChosen) throwReplicationConflictException();

		if (_event._actionWasChosen) {
			if (_event._actionChosenState == null) return false;
			if (_event._actionChosenState == _stateInA) prevailing = _stateInA.getObject();
			if (_event._actionChosenState == _stateInB) prevailing = _stateInB.getObject();
		}

		if (prevailing == null) { //Deletion has prevailed.
			if (_directionTo == other) return false;
			ownerRef.markForDeleting();
			return !_event._actionShouldStopTraversal;
		}

		boolean needsToBeActivated = !isConflict; //Already activated if there was a conflict.
		return handleNewObject(obj, ownerRef, owner, other, referencingObject, fieldName, needsToBeActivated, true);
	}


	private boolean handleNewObject(Object obj, ReplicationReference ownerRef, ReplicationProviderInside owner,
			ReplicationProviderInside other, Object referencingObject, String fieldName, boolean needsToBeActivated, boolean listenerAlreadyNotified) {
		if (_directionTo == owner) return false;
	
		if (needsToBeActivated) owner.activate(obj);
	
		if (!listenerAlreadyNotified) {
			_event.resetAction();
			_event.conflict(false);
			_event._creationDate = TimeStampIdGenerator.idToMilliseconds(ownerRef.uuid().getLongPart());
	
			if (owner == _providerA) {
				_stateInA.setAll(obj, true, false, ObjectStateImpl.UNKNOWN);
				_stateInB.setAll(null, false, false, ObjectStateImpl.UNKNOWN);
			} else {
				_stateInA.setAll(null, false, false, ObjectStateImpl.UNKNOWN);
				_stateInB.setAll(obj, true, false, ObjectStateImpl.UNKNOWN);
			}
			
			if(_listener != null){
				_listener.onReplicate(_event);
		
				if (_event._actionWasChosen) {
					if (_event._actionChosenState == null) return false;
					if (_event._actionChosenState.getObject() != obj) return false;
				}
			}
		}
		
		Object counterpart = emptyClone(owner, obj);
	
		ownerRef.setCounterpart(counterpart);
		ownerRef.markForReplicating(true);
	
		ReplicationReference otherRef = other.referenceNewObject(counterpart, ownerRef, getCounterpartRef(referencingObject), fieldName);
		if (otherRef == null) {
			return false;
		}
		otherRef.setCounterpart(obj);
	
		putCounterpartRef(obj, otherRef);
	
		if (_event._actionShouldStopTraversal) return false;
	
		return true;
	}


	private void throwReplicationConflictException() {
		throw new ReplicationConflictException("A replication conflict ocurred and the ReplicationEventListener, if any, did not choose which state should override the other.");
	}


	private Object emptyClone(ReplicationProviderInside sourceProvider, Object obj) {
		if (obj == null) return null;
		ReflectClass claxx = reflectClass(obj);
		if (_reflector.isValueType(claxx)) throw new RuntimeException("IllegalState");
//		if (claxx.isArray()) return arrayClone(obj, claxx, sourceProvider); //Copy arrayClone() from GenericReplicationSession if necessary.
		if (claxx.isArray())  throw new RuntimeException("IllegalState"); //Copy arrayClone() from GenericReplicationSession if necessary.
		if (_collectionHandler.canHandleClass(claxx)) {
			return collectionClone(sourceProvider, obj, claxx);
		}
		
		Object result = claxx.newInstance();
		if (result == null)
			throw new RuntimeException("Unable to create a new instance of " + obj.getClass()); //FIXME Use db4o's standard for throwing exceptions.
		return result;
	}


	private ReflectClass reflectClass(Object obj) {
		return _reflector.forObject(obj);
	}

	
	private Object collectionClone(ReplicationProviderInside sourceProvider, Object original, ReflectClass claxx) {
		return _collectionHandler.emptyClone(sourceProvider, original, claxx);
	}
	

	private ReplicationReference getCounterpartRef(Object original) {
		return (ReplicationReference) _counterpartRefsByOriginal.get(original);
	}


	private void putCounterpartRef(Object obj, ReplicationReference otherRef) {
		if (_counterpartRefsByOriginal.get(obj) != null) throw new RuntimeException("illegal state");
		_counterpartRefsByOriginal.put(obj, otherRef);
	}

	
}
