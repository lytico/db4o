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


import java.io.*;
import java.util.*;

import org.polepos.circuits.arraylists.*;
import org.polepos.circuits.commits.*;
import org.polepos.circuits.complex.*;
import org.polepos.circuits.flatobject.*;
import org.polepos.circuits.fragmentation.*;
import org.polepos.circuits.inheritancehierarchy.*;
import org.polepos.circuits.listoperations.*;
import org.polepos.circuits.multithreadedqueries.*;
import org.polepos.circuits.nativeids.*;
import org.polepos.circuits.nestedlists.*;
import org.polepos.circuits.queries.*;
import org.polepos.circuits.querycaching.*;
import org.polepos.circuits.sepangmulti.*;
import org.polepos.circuits.sortedquery.*;
import org.polepos.circuits.strings.*;
import org.polepos.circuits.trees.*;
import org.polepos.framework.*;
import org.polepos.reporters.*;
import org.polepos.runner.db4o.*;
import org.polepos.teams.db4o.*;

import com.db4o.polepos.continuous.*;
import com.db4o.polepos.continuous.filealgebra.*;

/**
 * Please read the README file in the home directory first.
 */
public class PerformanceMonitoringRunner extends AbstractDb4oVersionsRaceRunner{
	
	private static final int PERFORMANCE_PERCENTAGE_THRESHOLD = 5;

	private static final String SETTINGS_FILE = "settings/PerfCircuits.properties";

	private static final int NUM_RUNS = 11;

	private final Db4oJarCollection _jarCollection;
	private final PerformanceMonitoringReporter[] _reporters;
	private final File[] _libPaths;
	
    public static void main(String[] args) {
    	System.setProperty(TimedLapsCircuitBase.NUM_RUNS_PROPERTY_ID, String.valueOf(NUM_RUNS));
    	System.setProperty(TimedLapsCircuitBase.MEMORY_USAGE_PROPERTY_ID, TimedLapsCircuitBase.NullMemoryUsage.class.getName());
    	int[] selectedIndices = null;
    	try {
    		selectedIndices = parseSelectedIndices(args[0]);
    	}
    	catch(NumberFormatException exc) {
    		System.err.println("Usage: PerformanceMonitoringRunner <selected indices, comma separated> <fixed unconditional jar folder> <jar folder paths, space separated>");
    		throw exc;
    	}
    	File[] folders = toFiles(extractFileArgs(args));
    	File[] libPaths = new File[folders.length - 1];
    	System.arraycopy(folders, 1, libPaths, 0, libPaths.length);
		System.exit(new PerformanceMonitoringRunner(selectedIndices, folders[0], libPaths).runMonitored());
    }

	private static String[] extractFileArgs(String[] args) {
		String[] files = new String[args.length - 1];
    	System.arraycopy(args, 1, files, 0, files.length);
		return files;
	}

    private static int[] parseSelectedIndices(String selectedIdxStr) {
    	String[] selectedIdxStrArr = selectedIdxStr.split(",");
    	int[] selectedIndices = new int[selectedIdxStrArr.length];
    	for (int selIdxIdx = 0; selIdxIdx < selectedIndices.length; selIdxIdx++) {
			selectedIndices[selIdxIdx] = Integer.parseInt(selectedIdxStrArr[selIdxIdx]);
		}
		return selectedIndices;
	}

	private static File[] toFiles(String[] paths) {
    	File[] files = new File[paths.length];
    	for (int pathIdx = 0; pathIdx < paths.length; pathIdx++) {
			files[pathIdx] = new File(paths[pathIdx]).getAbsoluteFile();
			if(!files[pathIdx].exists() || !files[pathIdx].isDirectory()) {
				throw new IllegalArgumentException("Not a directory: " + files[pathIdx]);
			}
		}
    	return files;
    }
    
    public int runMonitored() {
    	run(SETTINGS_FILE);
    	boolean performanceOk = true;
    	for (PerformanceMonitoringReporter reporter : _reporters) {
    		PerformanceReport report = reporter.performanceReport();
    		report.print(new OutputStreamWriter(System.err));
    		performanceOk &= report.performanceOk();
		}
    	return performanceOk ? 0 : -99;
    }

    public PerformanceMonitoringRunner(int[] selectedIndices, File fixedFolder, File[] libPaths) {
    	List<FileSource> sources = new ArrayList<FileSource>();
    	
    	_libPaths = libPaths;
    	for (File libPath : libPaths) {
			sources.add(new FolderFileSource(libPath));
		}
    	FileSource compositeSource = new CompositeFileSource(sources);
    	FileSource filteredSource = new Db4oJarSortedFileSource(compositeSource);
    	File recentJar = new TakeFirstSingleFileSource(filteredSource).file();
    	FileSource flexibleJarSource = new LenientIndexSelectingFileSource(filteredSource, selectedIndices);
		List<File> otherJars = new CompositeFileSource(flexibleJarSource, new Db4oJarSortedFileSource(new FolderFileSource(fixedFolder))).files();
    	_jarCollection = new Db4oJarCollection(recentJar, otherJars);
    	_reporters = new PerformanceMonitoringReporter[] {
    			new PerformanceMonitoringReporter(_jarCollection.currentJar().getName(), MeasurementType.TIME, new SpeedTicketPerformanceStrategy(PERFORMANCE_PERCENTAGE_THRESHOLD)),
    			//new PerformanceMonitoringReporter(_jarCollection.currentJar().getName(), MeasurementType.MEMORY, new SpeedTicketPerformanceStrategy(PERFORMANCE_PERCENTAGE_THRESHOLD)),
    	};
    }
    
    @Override
    protected Reporter[] reporters() {
    	Reporter[] defaultReporters = DefaultReporterFactory.defaultReporters();
    	Reporter[] allReporters = new Reporter[defaultReporters.length + _reporters.length + 1];
    	System.arraycopy(_reporters, 0, allReporters, 0, _reporters.length);
    	System.arraycopy(defaultReporters, 0, allReporters, _reporters.length, defaultReporters.length);
    	allReporters[allReporters.length - 1] = new TimeThresholdLoggingReporter(_jarCollection.currentJar().getName()); // new StdErrLoggingReporter();
    	return allReporters;
    }
    
    public Team[] teams() {
    	Set<Team> teams = new HashSet<Team>();
    	teams.add(db4oTeam(_jarCollection.currentJar().getName()));
    	for (File otherJar : _jarCollection.otherJars()) {
			teams.add(db4oTeam(otherJar.getName()));
		}
    	return teams.toArray(new Team[teams.size()]);
	}

	public Circuit[] circuits() {
		return new Circuit[] {
			new ReflectiveCircuitBase(Complex.class),
			new ReflectiveCircuitBase(InheritanceHierarchy.class),
			new ReflectiveCircuitBase(NestedLists.class),
			new ReflectiveCircuitBase(FlatObject.class),

			 new TreesMulti(), // ok
			 new Trees(), // ok
			 new NativeIds(), // ok
			 new Commits(),
			 new Strings(),
			 new ArrayLists(),
			 new QueriesFast(),
			 new QueriesMedium(),
			 new QueriesSlow(),
			 new ListOperations(), // ok
			 new Fragmentation(),
			 new SortedQuery(),
			 new MultithreadedQueries(),
			 new QueryCaching(),
		};
	}

	public DriverBase[] drivers() {
		return new DriverBase [] {
			new TreesMultiDb4o(),
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
		};
	}
    
	@Override
	protected File[] libPaths() {
		return _libPaths;
	}
}
