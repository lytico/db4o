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

import com.db4o.ObjectSet;
import com.db4o.drs.Replication;
import com.db4o.drs.ReplicationEvent;
import com.db4o.drs.ReplicationEventListener;
import com.db4o.drs.ReplicationSession;
import com.db4o.drs.hibernate.impl.HibernateReplicationProvider;
import com.db4o.drs.test.DrsTestCase;
import com.db4o.drs.test.data.*;

public class RoundRobinWithManyProviders extends DrsTestCase {
	
	HibernateReplicationProvider providerC;

	public void test() {
		initProviderC();

		replicateAB();
		replicateBC();
		replicateCA();
	}

	private void initProviderC() {
		Configuration configC = HibernateUtil.createNewDbConfig();
		configC.addClass(Pilot.class);
		providerC = new HibernateReplicationProvider(configC);
	}

	private void replicateAB() {
		Pilot pilot1 = new Pilot("Scott Felton", 200);
		a().provider().storeNew(pilot1);
		a().provider().commit();

		replicateAll(a().provider(), b().provider());
	}

	private void replicateBC() {
		ReplicationSession sess = Replication.begin(b().provider(), providerC);

		ObjectSet changed = sess.providerA()
				.objectsChangedSinceLastReplication();
		while (changed.hasNext())
			sess.replicate(changed.next());

		sess.commit();
	}

	private void replicateCA() {
		ReplicationEventListener resolver = new ReplicationEventListener() {
			public void onReplicate(ReplicationEvent e) {
				if (e.isConflict()) {
					// System.out.println("Conflict because C and A are never
					// replicated.So they don't have a replication record. This
					// makes both providers think the Pilot object was modified.
					// ");
					e.overrideWith(e.stateInProviderA());
				}
			}

		};
		ReplicationSession sess = Replication
				.begin(providerC, a().provider(), resolver);

		ObjectSet changed2 = sess.providerA()
				.objectsChangedSinceLastReplication();
		while (changed2.hasNext())
			sess.replicate(changed2.next());

		sess.commit();
	}
}
