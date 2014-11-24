package com.db4o.drs.versant.ipc;

import java.util.*;

import com.db4o.rmi.*;

public interface EventProcessor {
	
	long beginReplicationGenerateTimestamp();
	
	void replaceCommitTimestamp(long commitTimestamp, long syncedTimeStamp);
	
	long[] commitReplicationGetConcurrentTimestamps(long timestamp);
	
	long generateTimestamp();

	long lastTimestamp();

	void syncTimestamp(long timestamp);

	void stop();
	
	void addListener(@Proxy @Async EventProcessorListener listener);
	
	void removeListener(@Proxy EventProcessorListener listener);
	
	public interface EventProcessorListener {
		
		void ready();

		void committed(String transactionId);
		
		void onEvent(long loid, long version);

	}

	Map<String, Long> ensureMonitoringEventsOn(String className);
	
	long defaultSignatureLoid();


}
