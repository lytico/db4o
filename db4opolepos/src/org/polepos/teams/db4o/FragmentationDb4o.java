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

import org.polepos.circuits.fragmentation.*;
import org.polepos.runner.db4o.*;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.query.*;


public class FragmentationDb4o extends Db4oDriver implements FragmentationDriver{
	

	@Override
	public void configure(Configuration config) {
		
	}

    public void store() {
        int count = setup().getObjectCount();
        begin();
        for ( int i = 1; i <= count; i++ ){
            store( new SilverstoneItem(i) );
            addToCheckSum(i);
        }
        commit();
    }
    
    public void deleteAndStore() {
        int updateCount = setup().getUpdateCount();
        for (int i = 0; i < updateCount; i++) {
            begin();
            Query q = db().query();
            q.constrain(SilverstoneItem.class);
            ObjectSet objectSet = q.execute();
            while(objectSet.hasNext()){
                SilverstoneItem item = (SilverstoneItem) objectSet.next();
                db().delete(item);
                addToCheckSum(item.getIndex());
            }
            commit();
            store();
        }
    }

}
