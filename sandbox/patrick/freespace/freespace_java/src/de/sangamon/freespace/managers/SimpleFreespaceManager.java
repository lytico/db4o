package de.sangamon.freespace.managers;

import java.util.*;

import de.sangamon.freespace.core.*;

public class SimpleFreespaceManager implements FreespaceManager {

	private List<Freespace> _space = new ArrayList<Freespace>();
	private int _acquired = 0;
	private int _freed = 0;
	
	@Override
	public Freespace acquire() {
		int idx = 0;
		while(idx < _space.size()) {
			if(_space.get(idx) == null) {
				break;
			}
			idx++;
		}
		FreespaceUtil.sleep(FreespaceUtil.ACQUIRE_TICKS);
		Freespace freespace = new Freespace(idx);
		if(idx == _space.size()) {
			_space.add(freespace);
		}
		else {
			_space.set(idx, freespace);
		}
		log(freespace, "acq");
		_acquired++;
		return freespace;
	}

	@Override
	public void free(Freespace freed) {
		FreespaceUtil.sleep(FreespaceUtil.FREE_TICKS);
		assert _space.get(freed.idx()) == freed : freed + " !=" + _space.get(freed.idx());
		_space.set(freed.idx(), null);
		log(freed, "fre");
		_freed++;
	}

	@Override
	public void shutdown() {
		//System.err.println("SCORE: " + score() + "(" + _acquired +" - " + _freed + ")");
	}
	
	public int score() {
		return _acquired - _freed;
	}

	private void log(Freespace freespace, String msg) {
		// System.out.println(msg + " " + freespace.idx());
	}

}
