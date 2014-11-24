/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package com.db4o.internal.activation;

import com.db4o.internal.*;

public class LegacyActivationDepthProvider implements ActivationDepthProvider {
	
	public static final ActivationDepthProvider INSTANCE = new LegacyActivationDepthProvider();
	
	public ActivationDepth activationDepthFor(ClassMetadata classMetadata, ActivationMode mode) {
		if (mode.isPrefetch()) {
			return new LegacyActivationDepth(1, mode);
		}
		final int globalLegacyActivationDepth = configImpl(classMetadata).activationDepth();
		Config4Class config = classMetadata.configOrAncestorConfig();
		int defaultDepth = null == config
			? globalLegacyActivationDepth
			: config.adjustActivationDepth(globalLegacyActivationDepth);
		return new LegacyActivationDepth(defaultDepth, mode);
	}
	
	public ActivationDepth activationDepth(int depth, ActivationMode mode) {
		return new LegacyActivationDepth(depth, mode);
	}

	private Config4Impl configImpl(ClassMetadata classMetadata) {
		return classMetadata.container().configImpl();
	}
}
