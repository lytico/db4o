/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.polepos.idsys;

import org.polepos.framework.*;

public class PlainCommitCircuit extends TimedLapsCircuitBase {

	@Override
	protected void addLaps() {
		add(new Lap("lapAllocate", false, false));
		add(new Lap("fullCommit"));
		add(new Lap("multipleCommits"));
		add(new Lap("fragmentedCommits"));
	}

	@Override
	public String description() {
		return "map IDs sequentially";
	}

	@Override
	public Class<? extends DriverBase> requiredDriver() {
		return PlainCommitDriver.class;
	}

}
