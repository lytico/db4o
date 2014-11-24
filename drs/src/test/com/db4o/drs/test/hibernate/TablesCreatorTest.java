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

import com.db4o.drs.hibernate.impl.ReplicationConfiguration;
import com.db4o.drs.hibernate.impl.TablesCreatorImpl;

import db4ounit.Assert;
import db4ounit.TestCase;

public class TablesCreatorTest implements TestCase{
	public TablesCreatorTest() {
	}

	protected Configuration createCfg() {
		return HibernateUtil.createNewDbConfig();
	}

	protected Configuration validateCfg() {
		Configuration configuration = HibernateUtil.createNewDbConfig();
		return configuration.setProperty("hibernate.hbm2ddl.auto", "validate");
	}

	public void test() {
		tstValidate();
		tstCreate();
	}

	public void tstCreate() {
		Configuration cfg = createCfg();
		final TablesCreatorImpl creator = new TablesCreatorImpl(ReplicationConfiguration.decorate(cfg));

		creator.validateOrCreate();
	}

	public void tstValidate() {
		Configuration cfg = validateCfg();

		final TablesCreatorImpl creator = new TablesCreatorImpl(ReplicationConfiguration.decorate(cfg));

		boolean exception = false;
		try {
			creator.validateOrCreate();
		} catch (RuntimeException e) {
			exception = true;
		}

		Assert.isTrue(exception);
	}
}
