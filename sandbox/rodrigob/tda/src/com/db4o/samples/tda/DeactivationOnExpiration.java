package com.db4o.samples.tda;

import com.db4o.internal.*;
import com.db4o.internal.activation.*;
import com.db4o.samples.tda.internal.*;
import com.db4o.ta.*;

/**
 * Transparently deactivates TA aware objects after they have been unused for
 * 1 minute.
 */
public class DeactivationOnExpiration extends TransparentActivationSupport {

	private final Timer _timer;

	public DeactivationOnExpiration(Timer timer) {
		_timer = timer;
	}
	
	@Override
	protected TransparentActivationDepthProvider newActivationDepthProvider(InternalObjectContainer container) {
	 	return new TransparentDeactivationProvider(container, _timer);
	}

}
