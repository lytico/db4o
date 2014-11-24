package com.db4o.drs.versant.enhancer;

import java.lang.instrument.*;
import java.security.*;
import java.util.*;

/**
 * @sharpen.ignore
 */
public final class EnhancerTransformer implements ClassFileTransformer {
	private final Map<String, byte[]> cache;

	EnhancerTransformer(Map<String, byte[]> cache) {
		this.cache = cache;
	}

	public byte[] transform(ClassLoader loader, String className, Class<?> classBeingRedefined, ProtectionDomain protectionDomain, byte[] classfileBuffer)
			throws IllegalClassFormatException {
		
		return cache.remove(className);
		
	}
}