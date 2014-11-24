/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.polepos.continuous;

import java.io.*;
import java.util.*;

public class PerformanceReport {

	private List<PerformanceFailure> _failures = new ArrayList<PerformanceFailure>();
	
	public boolean performanceOk() {
		return _failures.isEmpty();
	}
	
	public void add(
			String circuitName, 
			int setupIdx,
			String lapName,
			String currentTeamName, 
			String otherTeamName,
			MeasurementType measurementType, 
			long currentValue, 
			long otherValue) {
		_failures.add(new PerformanceFailure(
				circuitName,
				setupIdx,
				lapName,
				currentTeamName,
				otherTeamName,
				measurementType,
				currentValue,
				otherValue
		));
	}
	
	public List<PerformanceFailure> failures() {
		return Collections.unmodifiableList(_failures);
	}
	
	public void print(Writer out) {
		PrintWriter prOut = new PrintWriter(out);
    	prOut.println(failures().size() + " failures");
    	int failureCount = 1;
    	List<PerformanceFailure> sortedFailures = new ArrayList<PerformanceFailure>(_failures);
    	Collections.sort(sortedFailures, new Comparator<PerformanceFailure>() {
			public int compare(PerformanceFailure pf1, PerformanceFailure pf2) {
				int circuitCmp = pf1.circuitName.compareTo(pf2.circuitName);
				if(circuitCmp != 0) {
					return circuitCmp;
				}
				int turnCmp = pf1.setupIdx - pf2.setupIdx;
				if(turnCmp != 0) {
					return turnCmp;
				}
				int lapCmp = pf1.lapName.compareTo(pf2.lapName);
				if(lapCmp != 0) {
					return lapCmp;
				}
				return pf1.otherTeamName.compareTo(pf2.otherTeamName);
			}
    	});
    	for (PerformanceFailure failure : sortedFailures) {
    		prOut.println(failureCount);
    		prOut.println(failure);
    		failureCount++;
		}
    	prOut.flush();
	}
}
