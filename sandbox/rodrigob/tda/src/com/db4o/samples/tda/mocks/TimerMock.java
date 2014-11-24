package com.db4o.samples.tda.mocks;

import java.util.*;

import com.db4o.samples.tda.*;
import com.db4o.samples.tda.Timer;

public class TimerMock implements Timer {

	private final List<AlarmMock> _alarms = new ArrayList<AlarmMock>();
	private int _time;

	public Alarm setAlarm(int timeoutMilliseconds, Runnable runnable) {
		final AlarmMock alarm = new AlarmMock(_time + timeoutMilliseconds, runnable);
		_alarms .add(alarm);
		return alarm;
    }

	public void advanceTime(int milliseconds) {
		
		_time += milliseconds;
		
		reactToTimeChange();
    }

	private void reactToTimeChange() {
	    final Iterator<AlarmMock> iterator = _alarms.iterator();
		while (iterator.hasNext()) {
	        final AlarmMock alarm = iterator.next();
	        alarm.reactToTimeChange(_time);
	        if (!alarm.isActive()) {
	        	iterator.remove();
	        }
        }
    }

}
