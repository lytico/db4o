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
import org.polepos.circuits.complexconcurrency.*;
import org.polepos.circuits.flatobject.*;
import org.polepos.circuits.fragmentation.*;
import org.polepos.circuits.inheritancehierarchy.*;
import org.polepos.circuits.listoperations.*;
import org.polepos.circuits.multithreadedqueries.*;
import org.polepos.circuits.nativeids.*;
import org.polepos.circuits.nestedlists.*;
import org.polepos.circuits.queries.*;
import org.polepos.circuits.querycaching.*;
import org.polepos.circuits.sortedquery.*;
import org.polepos.circuits.strings.*;
import org.polepos.circuits.trees.*;
import org.polepos.framework.*;
import org.polepos.reporters.*;
import org.polepos.runner.db4o.*;
import org.polepos.teams.db4o.*;

import com.db4o.config.*;
import com.db4o.internal.*;
import com.db4o.internal.caching.*;
import com.db4o.internal.config.*;
import com.db4o.io.*;

/**
 * Please read the README file in the home directory first.
 */
public class AllRacesRunner extends AbstractDb4oVersionsRaceRunner{
    
    private static String JAR_TRUNK = "db4o-8.1.187.15546-all-java5.jar";

    private static String JAR_PRODUCTION = "db4o-8.0.184.15484-all-java5.jar";
    
    private static String JAR_74 = "db4o-7.4.136.14268-java5.jar";
    
    private static String JAR_STABLE = "db4o-7.12.145.14388-all-java5.jar";
    
    public static void main(String[] arguments) {
        new AllRacesRunner().run();
    }
    
    @Override
    protected Reporter[] reporters() {
    	//return new Reporter[] {new LoggingReporter()};
    	return DefaultReporterFactory.defaultReporters();
    }
    
    public Team[] teams() {

		return new Team[] {
				configuredDb4oTeam(JAR_74),
				configuredDb4oTeam(JAR_STABLE),
				configuredDb4oTeam(JAR_PRODUCTION),
				configuredDb4oTeam(JAR_TRUNK),
				
//				configuredDb4oTeam(JAR_DEVEL, new SingleBTreeIdSystem()),
//				configuredDb4oTeam(JAR_DEVEL, new PointerBasedIdSystem()),
				
//				db4oTeam(JAR_PRODUCTION),
//				
//				// configuredDb4oTeam(JAR_PRODUCTION, new BTreeFreespaceManager()),
//				
//				db4oTeam(JAR_STABLE),
//				
//				db4oTeam(JAR_DEVEL, new int[]{Db4oOptions.CLIENT_SERVER_TCP, Db4oOptions.CLIENT_SERVER}),
//				db4oTeam(JAR_PRODUCTION, new int[]{Db4oOptions.CLIENT_SERVER_TCP, Db4oOptions.CLIENT_SERVER}),
//				db4oTeam(JAR_STABLE, new int[]{Db4oOptions.CLIENT_SERVER_TCP, Db4oOptions.CLIENT_SERVER}),
				
		};
	}
    
    private ConfigurationSetting bTreeIdSystem(){
    	return new ConfigurationSetting() {
			
			public String name() {
				return "BTreeIdSystem";
			}
			
			public void apply(Object config) {
				Db4oLegacyConfigurationBridge.asIdSystemConfiguration((Config4Impl)config).useStackedBTreeSystem();
			}
		};
    	
    }
    
    private ConfigurationSetting fileBasedTransactionLog(){
		return new ConfigurationSetting(){
			public void apply(Object config) {
				((Config4Impl)config).fileBasedTransactionLog(true);
			}
			public String name() {
				return "CachedIoAdapter";
			}
			
		};
    }

	private ConfigurationSetting lru() {
		return new ConfigurationSetting(){
			public void apply(Object config) {
				((Configuration)config).storage(new CachingStorage(new FileStorage()));
			}
			public String name() {
				return "NewLRU";
			}
		};
	}

	private ConfigurationSetting lRU2Q() {
		return new ConfigurationSetting(){
			public void apply(Object config) {
				((Configuration)config).storage(new CachingStorage(new FileStorage()){
					@Override
					protected Cache4 newCache() {
						return CacheFactory.new2QCache(30);
					}
					
				});
			}
			public String name() {
				return "LRU2Q";
			}
		};
	}

	private ConfigurationSetting slotCache(final int slotCacheSize) {
		return new ConfigurationSetting(){
			public void apply(Object config) {
				((Configuration)config).cache().slotCacheSize(slotCacheSize);
			}
			public String name() {
				return "" + slotCacheSize + " slotCacheSize";
			}
		};
	}
    

	public Circuit[] circuits() {
		
		if(false){
			return new Circuit[]{
					// new Fragmentation(),
					new QueriesSlow(),
			};
		}
		
		if(false){
			return new Circuit[]{
					new ComplexConcurrency(),
					new QueryCentricConcurrency(),
					new InsertCentricConcurrency(),
			};
		}
		
		return new Circuit[] {
			new ReflectiveCircuitBase(Complex.class),
			new ReflectiveCircuitBase(NestedLists.class),
			new ReflectiveCircuitBase(InheritanceHierarchy.class),
			new ReflectiveCircuitBase(FlatObject.class),
//			new ComplexConcurrency(),
//			new QueryCentricConcurrency(),
//			new InsertCentricConcurrency(),
			new Trees(),
			new NativeIds(),
			 new Commits(),
			 new Strings(),
			 new ArrayLists(),
			 new QueriesFast(),
			 new QueriesMedium(),
			 new QueriesSlow(),
			 new ListOperations(),
			 new Fragmentation(),
			 new SortedQuery(),
			 new MultithreadedQueries(),
			 new QueryCaching(),
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
			new SortedQueryDb4o(),
			new MultithreadedQueriesDb4o(),
			new QueryCachingDb4o(),
	    	new ComplexConcurrencyDb4o(),
		};
	}
    
}
