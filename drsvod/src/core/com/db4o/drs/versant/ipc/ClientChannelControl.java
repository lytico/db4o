package com.db4o.drs.versant.ipc;

public interface ClientChannelControl {
	
	EventProcessor sync();
	
	EventProcessor async();
	
	void stop();
	
	void join() throws InterruptedException;
}