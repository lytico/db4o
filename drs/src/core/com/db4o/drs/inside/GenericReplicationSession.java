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

import java.util.*;

import com.db4o.drs.*;
import com.db4o.drs.inside.traversal.*;
import com.db4o.foundation.*;
import com.db4o.internal.*;
import com.db4o.reflect.*;

public final class GenericReplicationSession implements ReplicationSession {
	
	private static final int SIZE = 10000;

	private final ReplicationReflector _reflector;

	private final CollectionHandler _collectionHandler;

	private ReplicationProviderInside _providerA;

	private ReplicationProviderInside _providerB;

	private ReplicationProvider _directionTo; //null means bidirectional replication.

	private final ReplicationEventListener _listener;

	private final Traverser _traverser;

	private HashSet4 _processedUuids = new HashSet4(SIZE);

	private boolean _isReplicatingOnlyDeletions;


	public GenericReplicationSession(ReplicationProviderInside _peerA, ReplicationProviderInside _peerB) {
		this(_peerA, _peerB, new DefaultReplicationEventListener());
	}

	public GenericReplicationSession(ReplicationProvider providerA, ReplicationProvider providerB, ReplicationEventListener listener) {
		this(providerA, providerB, listener, null);
	}

	public GenericReplicationSession(ReplicationProvider providerA, ReplicationProvider providerB, ReplicationEventListener listener, Reflector reflector) {
		_reflector = new ReplicationReflector(providerA, providerB, reflector);
		_collectionHandler = new CollectionHandlerImpl(_reflector);
		_traverser = new GenericTraverser(_reflector, _collectionHandler);

		_providerA = (ReplicationProviderInside) providerA;
		_providerB = (ReplicationProviderInside) providerB;
		_listener = listener;
		
		_providerA.startReplicationTransaction(_providerB.getSignature());
		_providerB.startReplicationTransaction(_providerA.getSignature());
		long syncedTimeStamp = 
			Math.max(_providerA.timeStamps().commit(), _providerB.timeStamps().commit());
		_providerA.syncCommitTimestamp(syncedTimeStamp);
		_providerB.syncCommitTimestamp(syncedTimeStamp);
		
	}

	public final void close() {
		_providerA.destroy();
		_providerB.destroy();

		_providerA = null;
		_providerB = null;
		_processedUuids = null;
	}
	
	public final void commit() {
		_providerA.commitReplicationTransaction();
		_providerB.commitReplicationTransaction();
	}

	public final ReplicationProvider providerA() {
		return _providerA;
	}

	public final ReplicationProvider providerB() {
		return _providerB;
	}

	public final void replicate(Object root) {
		try {
			prepareGraphToBeReplicated(root);
			copyStateAcross(_providerA, _providerB);
			copyStateAcross(_providerB, _providerA);

			storeChangedObjectsIn(_providerA);
			storeChangedObjectsIn(_providerB);
		} finally {
			_providerA.clearAllReferences();
			_providerB.clearAllReferences();
		}
	}
	
	public void replicateDeletions(Class extent) {
		replicateDeletions(extent, _providerA);
		replicateDeletions(extent, _providerB);
	}

	
	private void replicateDeletions(Class extent, ReplicationProviderInside provider) {
		_isReplicatingOnlyDeletions = true;
		try {
			Iterator instances = provider.getStoredObjects(extent).iterator();
			while (instances.hasNext()){
				replicate(instances.next());
			}
		} finally {
			_isReplicatingOnlyDeletions = false;
		}
	}
	
	
	public final void rollback() {
		// TODO: Write tests for rollback.
		_providerA.rollbackReplication();
		_providerB.rollbackReplication();
	}

	public final void setDirection(ReplicationProvider replicateFrom, ReplicationProvider replicateTo) {
		if (replicateFrom == _providerA && replicateTo == _providerB)
			_directionTo = _providerB;
		if (replicateFrom == _providerB && replicateTo == _providerA)
			_directionTo = _providerA;
	}

	private void prepareGraphToBeReplicated(Object root) {
		_traverser.traverseGraph(root, new InstanceReplicationPreparer(_providerA, _providerB, _directionTo, _listener, _isReplicatingOnlyDeletions, _processedUuids, _traverser, _reflector, _collectionHandler));
	}

	private Object arrayClone(Object original, ReflectClass claxx, ReplicationProviderInside sourceProvider, final ReplicationProviderInside targetProvider) {
		ReflectClass componentType = _reflector.getComponentType(claxx);
		int[] dimensions = _reflector.arrayDimensions(original);
		Object result = _reflector.newArrayInstance(componentType, dimensions);
		Object[] flatContents = _reflector.arrayContents(original); //TODO Optimize: Copy the structure without flattening. Do this in ReflectArray.
		if (!(_reflector.isValueType(claxx)||_reflector.isValueType(componentType)))
			replaceWithCounterparts(flatContents, sourceProvider, targetProvider);
		_reflector.arrayShape(flatContents, 0, result, dimensions, 0);
		return result;
	}


	private void copyFieldValuesAcross(Object src, Object dest, ReflectClass claxx, ReplicationProviderInside sourceProvider, ReplicationProviderInside targetProvider) {
		if (dest == null) {
			throw new IllegalStateException("Dest cannot be null: src="+src+", class="+claxx+", source="+sourceProvider.getName()+ ", target="+targetProvider.getName());
		}
		final Iterator4 fields = FieldIterators.persistentFields(claxx);
		while (fields.moveNext()) {
			ReflectField field = (ReflectField) fields.current();
			Object value = field.get(src);
			field.set(dest, findCounterpart(value, sourceProvider, targetProvider));
		}

		ReflectClass superclass = claxx.getSuperclass();
		if (superclass == null) return;
		copyFieldValuesAcross(src, dest, superclass, sourceProvider, targetProvider);
	}

	private void copyStateAcross(final ReplicationProviderInside sourceProvider, final ReplicationProviderInside targetProvider) {
		if (_directionTo == sourceProvider) return;
		sourceProvider.visitCachedReferences(new Visitor4() {
			public void visit(Object obj) {
				copyStateAcross((ReplicationReference) obj, sourceProvider, targetProvider);
			}
		});
	}

	private void copyStateAcross(ReplicationReference sourceRef, ReplicationProviderInside sourceProvider, final ReplicationProviderInside targetProvider) {
		if (!sourceRef.isMarkedForReplicating()) {
			return;
		}
		if(sourceRef.isMarkedForDeleting()){
			return;
		}
		Object source = sourceRef.object();
		Object target = sourceRef.counterpart();
		if(source == null){
			throw new IllegalStateException("source may not be null");
		}
		if(target == null){
			throw new IllegalStateException("target may not be null");
		}
		copyStateAcross(source, target, sourceProvider, targetProvider);
	}

	private void copyStateAcross(Object source, Object dest, final ReplicationProviderInside sourceProvider, final ReplicationProviderInside targetProvider) {
		ReflectClass claxx = _reflector.forObject(source);
		
		copyFieldValuesAcross(source, dest, claxx, sourceProvider, targetProvider);
	}

	private void deleteInDestination(ReplicationReference reference, ReplicationProviderInside destination) {
		if (!reference.isMarkedForDeleting()) return;
		destination.replicateDeletion(reference.uuid());
	}

	private Object findCounterpart(Object value, ReplicationProviderInside sourceProvider, ReplicationProviderInside targetProvider) {
		if (value == null) return null;
		
		value = sourceProvider.replaceIfSpecific(value);
		
		// TODO: need to clone and findCounterpart of each reference object in the
		// struct
		if (ReplicationPlatform.isValueType(value)) return value;
		
		ReflectClass claxx = _reflector.forObject(value);
		if (claxx.isArray()) return arrayClone(value, claxx, sourceProvider, targetProvider);
		if (Platform4.isTransient(claxx)) return null; // TODO: make it a warning
		if (_reflector.isValueType(claxx)) return value;
		
		if (_collectionHandler.canHandle(value)){
			return collectionClone(value, claxx, sourceProvider, targetProvider);
		}
		
		if(Platform4.isEnum(claxx.reflector(), claxx)){
		    return value;
		}
		
		//if value is a Collection, result should be found by passing in just the value
		ReplicationReference ref = sourceProvider.produceReference(value, null, null);
		if (ref == null){
			throw new IllegalStateException("unable to find the ref of " + value + " of class " + value.getClass());
		}
		Object result = ref.counterpart();
		if (result != null){
			return result;
		}
		if(ref.isMarkedForDeleting()){
			return null;
		}
		ReplicationReference targetRef = targetProvider.produceReferenceByUUID(ref.uuid(), value.getClass());
		if(targetRef == null){
			throw new IllegalStateException("unable to find the counterpart of " + value + " of class " + value.getClass() + " uuid " + ref.uuid());
		}
		return targetRef.object();
	}

	private  Object collectionClone(Object original, ReflectClass claxx, final ReplicationProviderInside sourceProvider, final ReplicationProviderInside targetProvider) {
		return _collectionHandler.cloneWithCounterparts(sourceProvider, original, claxx, new CounterpartFinder() {
			public Object findCounterpart(Object original) {
				return GenericReplicationSession.this.findCounterpart(original, sourceProvider, targetProvider);
			}
		});
	}

	private ReplicationProviderInside other(ReplicationProviderInside peer) {
		return peer == _providerA ? _providerB : _providerA;
	}

	private void replaceWithCounterparts(Object[] objects, ReplicationProviderInside sourceProvider, final ReplicationProviderInside targetProvider) {
		for (int i = 0; i < objects.length; i++) {
			Object object = objects[i];
			if (object == null) continue;

			objects[i] = findCounterpart(object, sourceProvider, targetProvider);
		}
	}

	private void storeChangedCounterpartInDestination(ReplicationReference reference, ReplicationProviderInside destination) {
		//System.out.println("reference = " + reference);
		boolean markedForReplicating = reference.isMarkedForReplicating();
		//System.out.println("markedForReplicating = " + markedForReplicating);
		if (!markedForReplicating) return;
		destination.storeReplica(reference.counterpart());
	}

	private void storeChangedObjectsIn(final ReplicationProviderInside destination) {
		final ReplicationProviderInside source = other(destination);
		if (_directionTo == source) return;

		destination.visitCachedReferences(new Visitor4() {
			public void visit(Object obj) {
				deleteInDestination((ReplicationReference) obj, destination);
			}
		});

		source.visitCachedReferences(new Visitor4() {
			public void visit(Object obj) {
				storeChangedCounterpartInDestination((ReplicationReference) obj, destination);
			}
		});
	}

}
