package com.db4o.samples.tda.mocks;

import com.db4o.samples.tda.*;

public class AlarmMock implements Alarm {

	private final int _timeout;
	private Runnable _runnable;

	public AlarmMock(int timeout, Runnable runnable) {
		_timeout = timeout;
		_runnable = runnable;
    }

	void reactToTimeChange(int newTime) {
		if (!isActive())
			return;
		
		if (newTime < _timeout)
			return;
		
		_runnable.run();
		cancel();
    }

	boolean isActive() {
		return _runnable != null;
    }

	public void cancel() {
	    _runnable = null;
    }

}
