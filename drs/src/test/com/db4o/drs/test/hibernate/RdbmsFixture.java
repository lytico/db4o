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

import org.hibernate.cfg.*;
import org.hibernate.tool.hbm2ddl.*;

import com.db4o.drs.hibernate.impl.*;
import com.db4o.drs.inside.*;
import com.db4o.drs.test.*;
import com.db4o.drs.test.data.*;

public abstract class RdbmsFixture implements DrsProviderFixture {
	
	protected String _name;
	
	protected TestableReplicationProviderInside _provider;
	
	protected Configuration config;
	
	protected String dbUrl;
	
	public static Configuration addAllMappings(Configuration cfg) {
	    for(Class clazz : DrsTestCase.mappings) {
			if (isSupportedForRdbms(clazz)) {
			    cfg.addClass(clazz);
			}
		}
		return cfg;
	}

    protected static boolean isSupportedForRdbms(Class clazz) {
        return clazz.getAnnotation(OptOutRdbms.class) == null;
    }
	
	public RdbmsFixture(String name) {
		_name = name;
	}
	
	public void clean() {
		if (config==null) 
			return;
		
		new SchemaExport(config).drop(false, true);
	}
	
	public void close() {
		if (_provider==null)
			throw new RuntimeException(
					"Fixture is not yet openned or has already been closed. " +
					"It maybe caused by a replicationSession.close() call in test. " +
					"You should never call replicationSession.close()");
		
		_provider.destroy();
		_provider=null;
	}

	public TestableReplicationProviderInside provider() {
		return _provider;
	}
	
	protected Configuration createConfig() {
		Configuration tmp = new Configuration();
		addAllMappings(tmp);
		return ReplicationConfiguration.decorate(tmp);
	}
	
	public void destroy(){
		
	}
}
