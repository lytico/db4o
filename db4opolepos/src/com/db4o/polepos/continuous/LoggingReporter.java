/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.polepos.continuous;

import org.polepos.framework.*;
import org.polepos.reporters.*;

public class LoggingReporter implements Reporter {

	public void endSeason() {
		log("endSeason()");
	}

	public void startSeason() {
		log("startSeason()");
	}

    public void sendToCircuit(Circuit circuit) {
		log("sendToCircuit(): " + circuit);
    }
    
    public void noDriver(Team team, Circuit circuit) {
		log("noDriver(): " + team + ", " + circuit);
    }
    
    public void report( Team team, Car car, TurnSetup[] setups, TurnResult[] results ){
		log("report(): " + team + ", " + car + ", " + setups + ", " + results);
    }
	
	private void log(String msg) {
		System.err.println(msg);
	}
	
	@Override
	public String toString() {
		return "debug logging reporter";
	}
}
