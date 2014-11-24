package com.db4o.internal.activation;

import com.db4o.internal.*;

/**
 * Activates an object graph to a specific depth respecting any
 * activation configuration settings that might be in effect.
 */
public class LegacyActivationDepth extends ActivationDepthImpl {

	private final int _depth;
	
	public LegacyActivationDepth(int depth) {
		this(depth, ActivationMode.ACTIVATE);
	}

	public LegacyActivationDepth(int depth, ActivationMode mode) {
		super(mode);
		_depth = depth;
	}

	public ActivationDepth descend(ClassMetadata metadata) {
		if (null == metadata) {
			return new LegacyActivationDepth(_depth -1 , _mode);
		}
		return new LegacyActivationDepth(descendDepth(metadata), _mode);
	}

	private int descendDepth(ClassMetadata metadata) {
		int depth = configuredActivationDepth(metadata) - 1;
		if (metadata.isStruct()) {
			// 	We also have to instantiate structs completely every time.
			return Math.max(1, depth);
		}
		return depth;
	}

	private int configuredActivationDepth(ClassMetadata metadata) {
		Config4Class config = metadata.configOrAncestorConfig();
		if (config != null && _mode.isActivate()) {
			return config.adjustActivationDepth(_depth);
		}
		return _depth;
	}

	public boolean requiresActivation() {
		return _depth > 0;
	}

}
