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
package com.db4o.drs.hibernate.impl;

import java.io.Serializable;
import java.sql.PreparedStatement;
import java.sql.SQLException;
import java.util.HashMap;
import java.util.HashSet;
import java.util.Map;
import java.util.Set;

import org.hibernate.CallbackException;
import org.hibernate.EmptyInterceptor;
import org.hibernate.HibernateException;
import org.hibernate.Session;
import org.hibernate.cfg.Configuration;
import org.hibernate.collection.PersistentCollection;
import org.hibernate.event.EventListeners;
import org.hibernate.event.FlushEvent;
import org.hibernate.event.FlushEventListener;
import org.hibernate.event.PostInsertEvent;
import org.hibernate.event.PostInsertEventListener;
import org.hibernate.event.PostUpdateEvent;
import org.hibernate.event.PostUpdateEventListener;
import org.hibernate.event.PreDeleteEvent;
import org.hibernate.event.PreDeleteEventListener;

import com.db4o.drs.hibernate.metadata.ObjectReference;
import com.db4o.drs.hibernate.metadata.Uuid;
import com.db4o.foundation.TimeStampIdGenerator;

public class ObjectLifeCycleEventsListenerImpl 
	extends EmptyInterceptor 
	implements ObjectLifeCycleEventsListener {
	
	private final static String DELETE_SQL = "delete from " + ObjectReference.Table.NAME
			+ " where " +Uuid.Table.CREATED +"= ? "
			+ " AND " +Uuid.Table.PROVIDER+ " = ?";

	private final Set<ObjectReference> _dirtyNewRefs = new HashSet<ObjectReference>();

	private final Set<HibernateObjectId> _dirtyUpdatedRefs = new HashSet<HibernateObjectId>();

	private final Set<ObjectReference> _deletedRefs = new HashSet<ObjectReference>();

	private Set<Configuration> _configs = new HashSet<Configuration>();

	private Map<Thread, Session> _threadSessionMap = new HashMap<Thread, Session>();

	private boolean _alive = true;
	
	public final void configure(Configuration cfg) {
		new TablesCreatorImpl(ReplicationConfiguration.decorate(cfg)).validateOrCreate();

		Util.initMySignature(cfg);

		_configs.add(cfg);
		setListeners(cfg);
	}

	public final void destroy() {
		_alive = false;
		for(Session session : _threadSessionMap.values()) {
			GeneratorMap.remove(session);
		}
		_threadSessionMap.clear();
		_threadSessionMap = null;

		for (Configuration cfg : _configs)
			resetListeners(cfg);

		_configs.clear();
		_configs = null;
	}

	public final void install(Session session, Configuration cfg) {
		_threadSessionMap.put(Thread.currentThread(), session);

		final long minNext = Util.getMaxReplicationRecordVersion(session);
		
		GeneratorMap.put(session, new TimeStampIdGenerator(minNext));
	}

	public final void onCollectionRemove(Object collection, Serializable key) throws CallbackException {
		collectionUpdated(collection);
	}

	public final void onCollectionUpdate(Object collection, Serializable key) throws CallbackException {
		collectionUpdated(collection);
	}

	public final void onFlush(FlushEvent event) throws HibernateException {
		final Session s = getSession();

		TimeStampIdGenerator generator = GeneratorMap.get(s);
		
		long timestamp = generator.generate();
		
		for (ObjectReference ref : _dirtyNewRefs) {
			Uuid uuid = new Uuid();
			uuid.setCreated(generator.generate());
			uuid.setProvider(Util.genMySignature(s));

			ref.setUuid(uuid);
			ref.setModified(timestamp);
			getSession().save(ref);
		}

		for (HibernateObjectId hid : _dirtyUpdatedRefs) {
			ObjectReference ref = Util.getObjectReferenceById(getSession(), hid._className, hid._hibernateId);
			if (ref != null && !_dirtyNewRefs.contains(ref) && !_deletedRefs.contains(ref)) {
				ref.setModified(timestamp);
				getSession().update(ref);
			}
		}

		_dirtyNewRefs.clear();
		_dirtyUpdatedRefs.clear();
		_deletedRefs.clear();
	}

	public void onPostInsert(PostInsertEvent event) {
		ensureAlive();

		Object entity = event.getEntity();

		if (Util.isInstanceOfInternalObject(entity)) return;
		long id = Util.castAsLong(event.getId());

		ObjectReference ref = new ObjectReference();

		ref.setClassName(entity.getClass().getName());
		ref.setTypedId(id);

		_dirtyNewRefs.add(ref);
	}

	public final void onPostUpdate(PostUpdateEvent event) {
		ensureAlive();
		Object object = event.getEntity();

		if (Util.isInstanceOfInternalObject(object)) return;

		ObjectUpdated(object, Util.castAsLong(event.getId()));
	}

	public final boolean onPreDelete(PreDeleteEvent event) {
		ensureAlive();

		boolean veto = false;

		objectDeleted(event);

		return veto;
	}

	protected void ObjectUpdated(Object obj, long id) {
		HibernateObjectId hid = new HibernateObjectId(id, obj.getClass().getName());
		_dirtyUpdatedRefs.add(hid);
	}

	private void collectionUpdated(Object collection) {
		if (!(collection instanceof PersistentCollection))
			throw new RuntimeException(collection + " should always be PersistentCollection");

		PersistentCollection persistentCollection = ((PersistentCollection) collection);
		Object owner = persistentCollection.getOwner();

		if (Util.isInstanceOfInternalObject(owner)) return;
		
		final long ownerId = getId(owner);
		ObjectUpdated(owner, ownerId);
	}
	
	private void deleteObjectRef(ObjectReference ref) {
		Session s = getSession();

		try {
			PreparedStatement ps = s.connection().prepareStatement(DELETE_SQL);
			ps.setLong(1, ref.getUuid().getCreated());
			ps.setLong(2, ref.getUuid().getProvider().getId());

			int affected = ps.executeUpdate();
			if (affected != 1)
				throw new RuntimeException("can't delete the ObjectRef " + ref);
		} catch (SQLException e) {
			throw new RuntimeException(e);
		}

		s.evict(ref);
	}

	private void ensureAlive() {if (!_alive) throw new RuntimeException("dead");}

	private long getId(Object obj) {
		Session session = getSession();
		return (Long) session.getIdentifier(obj);
	}

	private Session getSession() {
		Session session = _threadSessionMap.get(Thread.currentThread());

		if (session == null)
			throw new RuntimeException("Unable to update the version number of an object. Did you forget to call ReplicationConfigurator.install(session, cfg) after opening a session?");
		return session;
	}

	private void objectDeleted(PreDeleteEvent event) {
		ObjectReference ref = Util.getObjectReferenceById(getSession(), event.getEntity().getClass().getName(), Util.castAsLong(event.getId()));
		if (ref == null) return;

		_deletedRefs.add(ref);
		deleteObjectRef(ref);
	}

	private void resetListeners(Configuration cfg) {
		cfg.setInterceptor(EmptyInterceptor.INSTANCE);
		EventListeners el = cfg.getEventListeners();

		PostUpdateEventListener[] o2 = el.getPostUpdateEventListeners();
		PostUpdateEventListener[] r2 = (PostUpdateEventListener[])
				Util.removeElement(o2, this);
		if ((o2.length - r2.length) != 1)
			throw new RuntimeException("can't remove");
		el.setPostUpdateEventListeners(r2);

		PreDeleteEventListener[] o3 = el.getPreDeleteEventListeners();
		PreDeleteEventListener[] r3 = (PreDeleteEventListener[])
				Util.removeElement(o3, this);
		if ((o3.length - r3.length) != 1)
			throw new RuntimeException("can't remove");
		el.setPreDeleteEventListeners(r3);

		PostInsertEventListener[] o4 = el.getPostInsertEventListeners();
		PostInsertEventListener[] r4 = (PostInsertEventListener[])
				Util.removeElement(o4, this);
		if ((o4.length - r4.length) != 1)
			throw new RuntimeException("can't remove");

		FlushEventListener[] o5 = el.getFlushEventListeners();
		FlushEventListener[] r5 = (FlushEventListener[])
				Util.removeElement(o5, this);
		if ((o5.length - r5.length) != 1)
			throw new RuntimeException("can't remove");

		el.setPostInsertEventListeners(r4);
	}

	private void setListeners(Configuration cfg) {
		cfg.setInterceptor(this);

		EventListeners el = cfg.getEventListeners();

		el.setPostUpdateEventListeners((PostUpdateEventListener[])
				Util.add(el.getPostUpdateEventListeners(), this));

		el.setPostInsertEventListeners((PostInsertEventListener[])
				Util.add(el.getPostInsertEventListeners(), this));

		el.setPreDeleteEventListeners((PreDeleteEventListener[])
				Util.add(el.getPreDeleteEventListeners(), this));

		el.setFlushEventListeners((FlushEventListener[])
				Util.add(el.getFlushEventListeners(), this));
	}

}
