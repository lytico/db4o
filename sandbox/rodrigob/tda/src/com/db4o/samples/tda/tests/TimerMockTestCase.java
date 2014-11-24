package com.db4o.samples.tda.tests;

import com.db4o.samples.tda.*;
import com.db4o.samples.tda.mocks.*;

import db4ounit.*;

public class TimerMockTestCase implements TestCase {

	private final class RunnableMock implements Runnable {
	    public boolean ran;

	    public void run() {
	    	ran = true;
	    }
    }

	final TimerMock timer = new TimerMock();
	final RunnableMock runnable = new RunnableMock();
	
	public void testAdvancingTimeTriggersAlarms() {
		timer.setAlarm(1000, runnable);
		
		timer.advanceTime(999);
		Assert.isFalse(runnable.ran);
		timer.advanceTime(1);
		Assert.isTrue(runnable.ran);
	}
	
	public void testAlarmsCanBeCancelled() {
		
		final Alarm alarm = timer.setAlarm(42, runnable);
		alarm.cancel();
		
		timer.advanceTime(42);
		Assert.isFalse(runnable.ran);
	}
	
	public void testSetAlarmAfterAdvancingTime() {
		timer.advanceTime(1);
		timer.setAlarm(2, runnable);
		timer.advanceTime(1);
		Assert.isFalse(runnable.ran);
		timer.advanceTime(1);
		Assert.isTrue(runnable.ran);
	}

}
