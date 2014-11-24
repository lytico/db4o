/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.polepos.idsys;

import org.polepos.framework.*;
import org.polepos.reporters.*;
import org.polepos.runner.*;

public class IdSystemRunner extends AbstractRunner {

	private static final String SETTINGS_FILE = "settings/IdSystem.properties";
	private static final int NUM_RUNS = 10;

	@Override
	protected Circuit[] circuits() {
		return new Circuit[] {
				new PlainAllocateCircuit(),
				new PlainCommitCircuit(),
				new PlainLookupCircuit(),
		};
	}

	@Override
	protected Reporter[] reporters() {
		return DefaultReporterFactory.defaultReporters();
	}

	@Override
	protected Team[] teams() {
		return new Team[] {
			new IdSystemTeam(),
		};
	}

	public static void main(String[] args) {
    	System.setProperty(TimedLapsCircuitBase.NUM_RUNS_PROPERTY_ID, String.valueOf(NUM_RUNS));
    	System.setProperty(TimedLapsCircuitBase.MEMORY_USAGE_PROPERTY_ID, TimedLapsCircuitBase.NullMemoryUsage.class.getName());
		new IdSystemRunner().run(SETTINGS_FILE);
	}
}
