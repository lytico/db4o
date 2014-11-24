package de.sangamon.freespace.main;

import java.util.*;

import de.sangamon.freespace.core.*;

public class FreespaceClient implements Runnable {

	private final int _id;
	private final int _numRuns;
	private final int _numAcquisitions;
	private final FreespaceManager _manager;
	private final Object _lock = new Object();
	
	public FreespaceClient(int id, int numRuns, int numAcquisitions, FreespaceManager manager) {
		_id = id;
		_numRuns = numRuns;
		_numAcquisitions = numAcquisitions;
		_manager = manager;
	}

	@Override
	public void run() {
		List<Freespace> acquired = new ArrayList<Freespace>(_numAcquisitions);
		for(int runIdx = 0; runIdx < _numRuns; runIdx++) {
			for(int acqIdx = 0; acqIdx < _numAcquisitions; acqIdx++) {
				acquired.add(_manager.acquire());
			}
			log(acquired, runIdx, "acq");
			FreespaceUtil.sleep(FreespaceUtil.CLIENT_WORK_TICKS);
			FreespaceUtil.wait(_lock, FreespaceUtil.CLIENT_IDLE_TIME);
			for(Freespace acq : acquired) {
				_manager.free(acq);
			}
			log(acquired, runIdx, "fre");
			acquired.clear();
			FreespaceUtil.sleep(FreespaceUtil.CLIENT_WORK_TICKS);
			FreespaceUtil.wait(_lock, FreespaceUtil.CLIENT_IDLE_TIME);
		}
	}

	private void log(List<Freespace> acquired, int runIdx, String msg) {
		//System.out.println(_id + " " + msg + " " + acquired.size() + " (" + runIdx + ")");
	}

}
