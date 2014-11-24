/* Copyright (C) 2008   Versant Inc.   http://www.db4o.com */
package com.db4o.db4ounit.common.events;

import com.db4o.events.*;
import com.db4o.foundation.*;

class EventInfo {
	public EventInfo(String eventFirerName, Procedure4<EventRegistry> eventListenerSetter) {
		this(eventFirerName, true, eventListenerSetter);
	}

	public EventInfo(String eventFirerName, boolean isClientServerEvent, Procedure4<EventRegistry> eventListenerSetter) {
		_listenerSetter = eventListenerSetter;
		_eventFirerName = eventFirerName;
		_isClientServerEvent = isClientServerEvent;
	}

	public Procedure4<EventRegistry> listenerSetter() {
		return _listenerSetter;
	}

	public String eventFirerName() {
		return _eventFirerName;
	}

	public boolean isClientServerEvent()  {
		return _isClientServerEvent;
	}
	
	private final Procedure4<EventRegistry> _listenerSetter;
	private final String _eventFirerName;
	private final boolean _isClientServerEvent;
}