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

import org.hibernate.CallbackException;
import org.hibernate.HibernateException;
import org.hibernate.Interceptor;
import org.hibernate.Session;
import org.hibernate.cfg.Configuration;
import org.hibernate.event.FlushEvent;
import org.hibernate.event.FlushEventListener;
import org.hibernate.event.PostInsertEvent;
import org.hibernate.event.PostInsertEventListener;
import org.hibernate.event.PostUpdateEvent;
import org.hibernate.event.PostUpdateEventListener;
import org.hibernate.event.PreDeleteEvent;
import org.hibernate.event.PreDeleteEventListener;

import java.io.Serializable;

/**
 * @author Albert Kwan
 */
public interface ObjectLifeCycleEventsListener extends
		PostInsertEventListener, PostUpdateEventListener,
		PreDeleteEventListener, Interceptor, FlushEventListener {
	public void onCollectionRemove(Object collection, Serializable key) throws CallbackException;

	public void onCollectionUpdate(Object collection, Serializable key) throws CallbackException;

	public void onPostUpdate(PostUpdateEvent event);

	void configure(Configuration cfg);

	void destroy();

	void install(Session session, Configuration cfg);

	void onFlush(FlushEvent event) throws HibernateException;

	void onPostInsert(PostInsertEvent event);

	boolean onPreDelete(PreDeleteEvent event);
}
