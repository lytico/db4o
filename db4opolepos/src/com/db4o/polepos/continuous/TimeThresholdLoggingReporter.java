/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.polepos.continuous;

import org.polepos.framework.*;
import org.polepos.reporters.*;

public class TimeThresholdLoggingReporter implements Reporter {

	private static final long MIN_TIME = 10000;
	private static final long MAX_TIME = 30000;
	private final String _currentTeamName;
	private String _circuitName;

	public TimeThresholdLoggingReporter(String currentTeamName) {
		_currentTeamName = currentTeamName;
	}
	
	public void report(Team team, Car car, TurnSetup[] setups, TurnResult[] results) {
		System.err.println("Finished - Circuit: " + _circuitName + ", Team: " + team.name());
		if(!_currentTeamName.equals(team.name())) {
			return;
		}
		for(Result result : results[results.length - 1]) {
			long time = result.getTime();
//			if(time < MIN_TIME || time > MAX_TIME) {
				System.err.println(result.getCircuit().name() + "/" + result.getLap().name() + ";" +time);
//			}
		}
	}

	public void sendToCircuit(Circuit circuit) {
		_circuitName = circuit.name();
	}

	public void startSeason() {
	}

	public void endSeason() {
	}

	public void noDriver(Team team, Circuit circuit) {
	}

}
