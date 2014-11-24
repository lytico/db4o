/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.polepos.idsys;

import org.polepos.framework.*;

public class PlainLookupCircuit extends TimedLapsCircuitBase {

	@Override
	protected void addLaps() {
		add(new Lap("lapAllocate", false, false));
		add(new Lap("fullLookup"));
		add(new Lap("multipleLookups"));
		add(new Lap("fragmentedLookups"));
	}

	@Override
	public String description() {
		return "lookup IDs sequentially";
	}

	@Override
	public Class<? extends DriverBase> requiredDriver() {
		return PlainLookupDriver.class;
	}

}
