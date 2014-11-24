/* 
This file is part of the PolePosition database benchmark
http://www.polepos.org

This program is free software; you can redistribute it and/or
modify it under the terms of the GNU General Public License
as published by the Free Software Foundation; either version 2
of the License, or (at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public
License along with this program; if not, write to the Free
Software Foundation, Inc., 59 Temple Place - Suite 330, Boston,
MA  02111-1307, USA. */

package org.polepos.teams.db4o;

import org.polepos.circuits.querycaching.*;
import org.polepos.data.*;
import org.polepos.runner.db4o.*;

import com.db4o.config.*;
import com.db4o.query.*;


public class QueryCachingDb4o extends Db4oDriver implements QueryCachingDriver {
	

	@Override
	public void configure(Configuration config) {
		indexField(config, Pilot.class  , "mPoints");
	}

    public void store() {
        begin();
        int count = setup().getObjectCount();
        for (int i = 1; i <= count; i++) {
            storePilot(i);
        }
        commit();
    }

    public void query() {
        int queryCount = setup().getSelectCount();
        for (int i = 0; i < queryCount; i++) {
            queryForPoints(10);
            queryForPoints(11);
        }
    }

	private void queryForPoints(int points) {
		Query q = db().query();
		q.constrain(Pilot.class);
		q.descend("mPoints").constrain(new Integer(points));
		doQuery(q);
	}

    private void storePilot(int idx) {
        Pilot pilot = new Pilot("Pilot_" + idx, "Jonny_" + idx, idx, idx);
        store(pilot);
    }

}
