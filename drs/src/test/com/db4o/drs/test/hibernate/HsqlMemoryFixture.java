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

import org.hibernate.Session;
import org.hibernate.SessionFactory;
import org.hibernate.Transaction;
import org.hibernate.cfg.Environment;

import com.db4o.drs.hibernate.impl.HibernateReplicationProvider;

public class HsqlMemoryFixture extends RdbmsFixture {
	private static final String HSQL_CFG_XML = "com/db4o/drs/test/hibernate/Hsql.cfg.xml";
	private static final String JDBC_URL_HEAD = "jdbc:hsqldb:mem:unique_";
	private static int jdbcUrlCounter = 100000;
		
	public HsqlMemoryFixture(String name) {
		super(name);
	}

	public void clean() {
		if (config==null)
			return;
		
		SessionFactory sf = createConfig().configure(HSQL_CFG_XML)
			.setProperty(Environment.URL, dbUrl).buildSessionFactory();
		Session session = sf.openSession();
		Transaction tx = session.beginTransaction();

		try {
			session.connection().createStatement().execute("SHUTDOWN IMMEDIATELY");
		} catch (Exception e) {
			throw new RuntimeException(e);
		}
	
		tx.commit();
		session.close();
		sf.close();
	}

	public void open()  {
		config = createConfig().configure(HSQL_CFG_XML);
		jdbcUrlCounter =jdbcUrlCounter+1;
		dbUrl = JDBC_URL_HEAD + jdbcUrlCounter;
		_provider = new HibernateReplicationProvider(config.setProperty(Environment.URL, dbUrl), _name);
	}
}
