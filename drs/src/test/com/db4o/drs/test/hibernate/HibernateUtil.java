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

import org.hibernate.cfg.Configuration;
import org.hibernate.cfg.Environment;

public class HibernateUtil {
	static final String HSQL_CFG_XML = "com/db4o/drs/test/hibernate/Hsql.cfg.xml";

	protected static final String JDBC_URL_HEAD = "jdbc:hsqldb:mem:unique_abc_";

	protected static int jdbcUrlCounter = 1000;

	/**
	 * Create a unique Configuration with the underlying database guaranteed to be
	 * empty.
	 *
	 * @return configuration
	 */
	public static Configuration createNewDbConfig() {
		Configuration configuration = new Configuration().configure(HSQL_CFG_XML);
		String url = JDBC_URL_HEAD + jdbcUrlCounter++;
		return configuration.setProperty(Environment.URL, url);
	}
}
