/* Copyright (C) 2009  Versant Corp.  http://www.db4o.com */

package com.db4o.polepos.continuous;

import org.polepos.framework.*;
import org.polepos.reporters.*;

public class StdErrLoggingReporter implements Reporter {
	
	private String circuitName;

	public void sendToCircuit(Circuit circuit) {
		circuitName = circuit.name();
		//System.err.println("Running on circuit: " + circuitName);
	}

	public void report(Team team, Car car, TurnSetup[] setups, TurnResult[] results) {
		System.err.println("Finished - Circuit: " + circuitName + ", Team: " + team.name());
	}

	public void startSeason() {
	}

	public void endSeason() {
	}

	public void noDriver(Team team, Circuit circuit) {
	}

}
