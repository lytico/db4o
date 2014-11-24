package com.db4o.drs.versant.enhancer;

import java.lang.instrument.*;
import java.util.*;

/**
 * @sharpen.ignore
 */
public class JdoEnhancerAgent {

	public static void premain(String agentArguments, Instrumentation instrumentation) {
		
		Map<String, byte[]> cache = new EnhancerStarter(agentArguments).enhance();

		instrumentation.addTransformer(new EnhancerTransformer(cache));
	}

}
