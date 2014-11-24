package com.db4o.drs.versant.ipc;

public interface ServerChannelControl {
	void stop();
	void join() throws InterruptedException;
}