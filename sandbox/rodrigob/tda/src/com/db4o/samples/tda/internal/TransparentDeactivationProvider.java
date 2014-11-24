package com.db4o.samples.tda.internal;

import java.util.*;

import com.db4o.activation.*;
import com.db4o.ext.*;
import com.db4o.internal.*;
import com.db4o.internal.activation.*;
import com.db4o.samples.tda.*;
import com.db4o.samples.tda.Timer;

public class TransparentDeactivationProvider extends TransparentActivationDepthProvider {
	
	private final InternalObjectContainer _container;
	private final Timer _timer;
	// TODO: consider a WeakHashMap instead
	private final Map<ObjectInfo, Alarm> _activeAlarms = new HashMap<ObjectInfo, Alarm>();
	
	public TransparentDeactivationProvider(InternalObjectContainer container, Timer timer) {
		_container = container;
		_timer = timer;
    }
	
	@Override
	public void activationRequestedFor(final ObjectInfo object, ActivationPurpose purpose) {
		
		cancelExistingAlarm(object);
		setDeactivationAlarmFor(object);
		
	}

	private void setDeactivationAlarmFor(final ObjectInfo object) {
	    final Alarm alarm = _timer.setAlarm(1 * 60 * 1000, new Runnable() {
			public void run() {
				// TODO: consider exposing a deactivate(ObjectInfo) method
				// TODO: what about concurrent clients to the object being deactivated?
				_container.deactivate(object.getObject());
			}
		});
		_activeAlarms.put(object, alarm);
    }

	private void cancelExistingAlarm(final ObjectInfo object) {
	    final Alarm existing = _activeAlarms.remove(object);
		if (existing != null)
			existing.cancel();
    }

}
