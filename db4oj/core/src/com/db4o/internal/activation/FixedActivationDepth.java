package com.db4o.internal.activation;

import com.db4o.internal.*;

/**
 * Activates a fixed depth of the object graph regardless of
 * any existing activation depth configuration settings.
 */
public class FixedActivationDepth extends ActivationDepthImpl {

	private final int _depth;

	public FixedActivationDepth(int depth, ActivationMode mode) {
		super(mode);
		_depth = depth;
	}
	
	public FixedActivationDepth(int depth) {
		this(depth, ActivationMode.ACTIVATE);
	}
	
	public boolean requiresActivation() {
		return _depth > 0;
	}
	
	public ActivationDepth descend(ClassMetadata metadata) {
		if (_depth < 1) {
			return this;
		}
		return new FixedActivationDepth(_depth-1, _mode);
	}

	// TODO code duplication in fixed activation/update depth
	public FixedActivationDepth adjustDepthToBorders() {
		return new FixedActivationDepth(DepthUtil.adjustDepthToBorders(_depth));
	}

}
