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

package org.polepos;


import org.polepos.circuits.arraylists.*;
import org.polepos.circuits.commits.*;
import org.polepos.circuits.complex.*;
import org.polepos.circuits.flatobject.*;
import org.polepos.circuits.fragmentation.*;
import org.polepos.circuits.inheritancehierarchy.*;
import org.polepos.circuits.listoperations.*;
import org.polepos.circuits.nativeids.*;
import org.polepos.circuits.nestedlists.*;
import org.polepos.circuits.queries.*;
import org.polepos.circuits.strings.*;
import org.polepos.circuits.trees.*;
import org.polepos.framework.*;
import org.polepos.runner.db4o.*;
import org.polepos.teams.db4o.*;

/**
 * Please read the README file in the home directory first.
 */
public class AllSettingsRunner extends AbstractDb4oVersionsRaceRunner{
    
    public static void main(String[] arguments) {
        new AllSettingsRunner().run();
    }
    
    public Team[] teams() {
		return new Team[] {
                db4oTeam(),
                db4oTeam(new int[] {Db4oOptions.BTREE_FREESPACE}),
                db4oTeam(new int[] {Db4oOptions.MEMORY_IO}),
                db4oTeam(new int[] {Db4oOptions.SNAPSHOT_QUERIES}),
                db4oTeam(new int[] {Db4oOptions.LAZY_QUERIES}),
		};
	}

	public Circuit[] circuits() {
		return new Circuit[] {
				
				new ReflectiveCircuitBase(Complex.class),
				new ReflectiveCircuitBase(InheritanceHierarchy.class),
				new ReflectiveCircuitBase(NestedLists.class),
				new ReflectiveCircuitBase(FlatObject.class),
				new Trees(),
				new NativeIds(),
				 new Commits(),
				 new Strings(),
				 new ArrayLists(),
				 new QueriesFast(),
				 new ListOperations(),
                 new Fragmentation(),
		};
	}

	public DriverBase[] drivers() {
		return new DriverBase [] {
		    	new ComplexDb4o(),
		    	new InheritanceHierarchyDb4o(),
		    	new NestedListsDb4o(),
		    	new FlatObjectDb4o(),
		        new TreesDb4o(),
		        new NativeIdsDb4o(),
		        new CommitsDb4o(),
		        new StringsDb4o(),
		        new ArrayListsDb4o(),
				new ListOperationsDb4o(),
				new QueriesDb4o(),
                new FragmentationDb4o(),
		};
	}
    
}
