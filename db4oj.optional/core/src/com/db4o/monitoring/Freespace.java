/* Copyright (C) 2009  Versant Corp.  http://www.db4o.com */

package com.db4o.monitoring;

import javax.management.*;

import com.db4o.*;
import com.db4o.internal.freespace.*;
import com.db4o.monitoring.internal.*;

/**
 * @exclude
 */
@decaf.Ignore(unlessCompatible=decaf.Platform.JDK15)
public class Freespace extends MBeanRegistrationSupport implements FreespaceMBean, FreespaceListener{
	
	private final TimedReading _reusedSlots = TimedReading.newPerSecond();
	
	private int _slotCount;
	
	private int _totalFreespace;
	
	public Freespace(ObjectContainer db, Class<?> type) throws JMException {
		super(db, type);
	}

	public double getAverageSlotSize() {
		
		// Preventing division by zero concurrency by using local var
		double slotCount = _slotCount;  
		if(slotCount == 0){
			return 0;
		}
		
		return _totalFreespace / slotCount;
	}

	public double getReusedSlotsPerSecond() {
		return _reusedSlots.read();
	}

	public int getSlotCount() {
		return _slotCount;
	}

	public int getTotalFreespace() {
		return _totalFreespace;
	}

	public void slotAdded(int size) {
		_slotCount++;
		_totalFreespace+=size;
	}

	public void slotRemoved(int size) {
		_reusedSlots.increment();
		_slotCount--;
		_totalFreespace-=size;
	}

}
