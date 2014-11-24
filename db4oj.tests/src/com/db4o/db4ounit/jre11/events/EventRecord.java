/* Copyright (C) 2006   Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.jre11.events;

import com.db4o.events.*;

public class EventRecord {
	public Event4 e;

	public Object args;

	public EventRecord(Event4 e_, Object args_) {
		e = e_;
		args = args_;
	}
	
	public String toString() {
		return "EventRecord(" + e + ", " + args + ")";
	}
}