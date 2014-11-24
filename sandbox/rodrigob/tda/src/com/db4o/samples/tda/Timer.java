package com.db4o.samples.tda;

public interface Timer {

	Alarm setAlarm(int timeoutMilliseconds, Runnable runnable);

}
