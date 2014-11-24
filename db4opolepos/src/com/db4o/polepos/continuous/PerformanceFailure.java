/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.polepos.continuous;

public class PerformanceFailure {

	public final String circuitName;
	public final int setupIdx;
	public final String lapName;
	public final String currentTeamName;
	public final String otherTeamName;
	public final MeasurementType measurementType;
	public final long currentValue;
	public final long otherValue;
	
	public PerformanceFailure(
			String circuitName, 
			int setupIdx,
			String lapName,
			String currentTeamName, 
			String otherTeamName,
			MeasurementType measurementType, 
			long currentValue, 
			long otherValue) {
		this.circuitName = circuitName;
		this.setupIdx = setupIdx;
		this.lapName = lapName;
		this.currentTeamName = currentTeamName;
		this.otherTeamName = otherTeamName;
		this.measurementType = measurementType;
		this.currentValue = currentValue;
		this.otherValue = otherValue;
	}

	@Override
	public String toString() {
		return 
			currentTeamName + " vs " + otherTeamName
			+ "\n"
			+ circuitName +" "
			+ setupIdx + " "
			+ lapName + ": "
			+ measurementType
			+ "\n" 
			+ currentValue + " <> " + otherValue + " = " + String.format("%.3f", percentageIncrease(currentValue, otherValue)) + "%";
	}
	
	private double percentageIncrease(long currentValue, long otherValue) {
		long diff = currentValue - otherValue;
		return diff * 100.0 / otherValue;
	}
}
