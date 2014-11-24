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
package simple;

import java.io.*;

import org.hibernate.cfg.Configuration;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.drs.*;
import com.db4o.drs.db4o.*;
import com.db4o.drs.hibernate.impl.*;

public class Db4oWithHibernateExample {
	
	public static void main(String[] args) {
		
		new File("handheld.db4o").delete();
		
		Pilot pilot1 = new Pilot("Scott Felton", 200);
		Pilot pilot2 = new Pilot("Frank Green", 120);

		ObjectContainer handheld = Db4oEmbedded.openFile(newEmbeddedConfiguration(), "handheld.db4o");

		handheld.store(pilot1);
		handheld.store(pilot2);
		
		Db4oEmbeddedReplicationProvider providerA = new Db4oEmbeddedReplicationProvider(handheld);

		Configuration cfg = new Configuration().configure("simple/hibernate.cfg.xml");
		HibernateReplicationProvider providerB = new HibernateReplicationProvider(cfg);
		
		ReplicationSession session = Replication.begin(providerA, providerB);

		ObjectSet changedInA = session.providerA().objectsChangedSinceLastReplication();
		while (changedInA.hasNext())
			session.replicate(changedInA.next());

		ObjectSet changedInB = session.providerB().objectsChangedSinceLastReplication();
		while (changedInB.hasNext())
			session.replicate(changedInB.next());

		session.commit();
		session.close();

		handheld.close();

		new File("handheld.db4o").delete();
	}
	
	private static EmbeddedConfiguration newEmbeddedConfiguration() {
		EmbeddedConfiguration config = Db4oEmbedded.newConfiguration();
		config.file().generateUUIDs(ConfigScope.GLOBALLY);
		config.file().generateCommitTimestamps(true);
		return config;
	}

}
