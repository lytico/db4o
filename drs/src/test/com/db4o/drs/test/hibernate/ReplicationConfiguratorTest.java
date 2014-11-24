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
package com.db4o.drs.test.hibernate;

import org.hibernate.*;
import org.hibernate.cfg.*;

import com.db4o.*;
import com.db4o.drs.*;
import com.db4o.drs.hibernate.*;
import com.db4o.drs.hibernate.impl.*;
import com.db4o.drs.hibernate.metadata.*;
import com.db4o.drs.test.data.*;

import db4ounit.*;

public class ReplicationConfiguratorTest implements TestCase {
	protected SessionFactory sessionFactory;
	protected Configuration cfg;
	String reuseUrl;
	

	public ReplicationConfiguratorTest() {

	}

	public void tstCollectionRemove() {
		CollectionHolder ch = new CollectionHolder();

		Session session = openSession();

		Transaction tx = session.beginTransaction();

		session.save(ch);
		session.flush();

		Uuid uuid = getUuid(session, ch);
		Assert.isNotNull(uuid );

		ch.set().add("8");
		session.flush();

		ch.list().add("88");
		session.flush();

		ch.map().put("88", "88");
		session.flush();

		ch.set(null);
		session.flush();

		ch.list(null);
		session.flush();

		ch.map(null);
		session.flush();

		session.delete(ch);
		session.flush();
		ensureDeleted(session, uuid);
		tx.commit();

		session.close();
	}

	public void tstCollectionUpdate() {
		CollectionHolder ch = new CollectionHolder();

		Session session = openSession();

		Transaction tx;
		tx = session.beginTransaction();

		session.save(ch);
		session.flush();

		Uuid uuid = getUuid(session, ch);
		Assert.isNotNull(uuid);

		ch.set().add("8");
		session.flush();

		ch.list().add("88");
		session.flush();

		ch.map().put("88", "88");
		session.flush();
		tx.commit();

		tx = session.beginTransaction();
		session.delete(ch);
		session.flush();

		ensureDeleted(session, uuid);
		tx.commit();

		session.close();
	}

	public void tstReferenceType() {
		CollectionHolder ch = new CollectionHolder();

		Session session = openSession();
		Transaction tx;
		tx = session.beginTransaction();

		session.save(ch);
		session.flush();

		Uuid uuid = getUuid(session, ch);
		Assert.isNotNull(uuid);

		ch.name("changed");
		tx.commit();

		tx = session.beginTransaction();
		session.delete(ch);
		session.flush();

		tx.commit();

		ensureDeleted(session, uuid);

		session.close();
	}

	public void test() {
			oneRound();
			doDummyReplication();
			oneRound();
	}

	/**
	 * Simulate real life application. Do a round of replication.
	 */
	private void doDummyReplication() {
		
		
		HibernateReplicationProvider providerA = new HibernateReplicationProvider(cfg);
		HibernateReplicationProvider providerB = new HibernateReplicationProvider(HibernateUtil.createNewDbConfig());
		final ReplicationSession r = Replication.begin(providerA, providerB);
		
		final ObjectSet changed = r.providerA().objectsChangedSinceLastReplication();
		while (changed.hasNext())
			r.replicate(changed.next());
		
		r.commit();
		r.close();
	}

	private void oneRound() {
		if (reuseUrl==null){
			cfg = HibernateUtil.createNewDbConfig();
			reuseUrl = cfg.getProperty(Environment.URL);
		} else {
			Configuration configuration = new Configuration().configure(HibernateUtil.HSQL_CFG_XML);
			cfg = configuration.setProperty(Environment.URL, reuseUrl);
		}
		
		Util.addClass(cfg, CollectionHolder.class);
		ReplicationConfigurator.configure(cfg);

		sessionFactory = cfg.buildSessionFactory();

		Session session = sessionFactory.openSession();
		session.close();

		tstReferenceType();
		tstCollectionUpdate();
		tstCollectionRemove();
	}
	
	protected void clean() {
			Session session = openSession();
			Transaction tx = session.beginTransaction();
			session.createQuery("delete from CollectionHolder").executeUpdate();
			tx.commit();
			session.close();
			sessionFactory.close();
	}

	protected void ensureDeleted(Session session, Uuid uuid) {
		Assert.isNull(Util.getByUUID(session, uuid));
	}

	protected Uuid getUuid(Session session, Object obj) {
		return Util.getUuid(session, obj);
	}

	protected Session openSession() {
		Session session = sessionFactory.openSession();
		//session.setFlushMode(FlushMode.COMMIT);
		ReplicationConfigurator.install(session, cfg);
		return session;
	}	
}
