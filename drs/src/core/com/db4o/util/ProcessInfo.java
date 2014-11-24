/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.util;

import com.db4o.internal.*;

public class ProcessInfo {
	
	public final String name;
	
	public final long processId;
	
	public ProcessInfo(String name, long processId) {
		this.name = name;
		this.processId = processId;
	}
	
	@Override
	public String toString() {
		return Reflection4.dump(this);
	}

}
