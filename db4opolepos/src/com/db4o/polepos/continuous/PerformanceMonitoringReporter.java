/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.polepos.continuous;

import java.util.*;

import org.polepos.framework.*;
import org.polepos.reporters.*;

public class PerformanceMonitoringReporter implements Reporter {
	private final String _currentTeamName;
	private final PerformanceComparisonStrategy _strategy;
	private Map<CircuitLapKey, LapResults> _results;
	private PerformanceReport _report;
	private MeasurementType _measurementType;
	
	public PerformanceMonitoringReporter(String currentTeamName, MeasurementType measurementType, PerformanceComparisonStrategy strategy) {
		_currentTeamName = currentTeamName;
		_strategy = strategy;
		_measurementType = measurementType;
	}

	public void startSeason() {
		_report = null;
		_results = new HashMap<CircuitLapKey, LapResults>();
	}
	
	public void endSeason() {
		_report = new PerformanceReport();
		for (LapResults lapResults : _results.values()) {
			checkLapResults(lapResults);
		}
		_results = null;
	}

	private void checkLapResults(LapResults lapResults) {
		Result currentTeamResult = lapResults.currentTeamResult();
		for (Result otherTeamResult : lapResults) {
			checkMeasurement(lapResults.setupIndex(), currentTeamResult, otherTeamResult, _measurementType.value(currentTeamResult), _measurementType.value(otherTeamResult));
		}
	}

	private void checkMeasurement(
			int setupIdx,
			Result currentTeamResult,
			Result otherTeamResult, 
			long currentValue, 
			long otherValue) {
		long diff = currentValue - otherValue;
		double percentageValue = (diff * 100D) / otherValue;
		System.err.println(currentTeamResult.getCircuit().name() +"/" + currentTeamResult.getLap().name() + ": " + String.format("%.2f", percentageValue) + "% (" + otherValue + " vs " + currentValue +")");
		
		if(!_strategy.acceptableDiff(currentValue, otherValue)) {
			_report.add(
					currentTeamResult.getCircuit().name(),
					setupIdx,
					currentTeamResult.getLap().name(),
					_currentTeamName,
					otherTeamResult.getTeam().name(),
					_measurementType,
					currentValue,
					otherValue
			);
		}
	}

	public void noDriver(Team team, Circuit circuit) {
	}

	public void report(Team team, Car car, TurnSetup[] setups, TurnResult[] results) {
//		int turnIdx = 0;
//		for (TurnResult turnResult : results) {
//			for (Result result : turnResult) {
//				registerResult(result, turnIdx);
//			}
//			turnIdx++;
//		}
		for (Result result : results[results.length - 1]) {
			registerResult(result, results.length - 1);
		}
	}

	private void registerResult(Result result, int setupIdx) {
		CircuitLapKey key = new CircuitLapKey(result.getCircuit(), setupIdx, result.getLap());
		if(!_results.containsKey(key)) {
			_results.put(key, new LapResults(setupIdx));
		}
		LapResults lapResults = _results.get(key);
		lapResults.add(result);
	}

	public void sendToCircuit(Circuit circuit) {
	}

	public PerformanceReport performanceReport() {
		return _report;
	}
	
	private static class CircuitLapKey {
		private Circuit _circuit;
		private int _setupIdx;
		private Lap _lap;
		
		public CircuitLapKey(Circuit circuit, int setupIdx, Lap lap) {
			_circuit = circuit;
			_setupIdx = setupIdx;
			_lap = lap;
		}
		
		@Override
		public boolean equals(Object obj) {
			if(this == obj) {
				return true;
			}
			if(obj == null || getClass() != obj.getClass()) {
				return false;
			}
			CircuitLapKey key = (CircuitLapKey)obj;
			return _circuit.equals(key._circuit) && _lap.equals(key._lap) && _setupIdx == key._setupIdx;
		}
		
		@Override
		public int hashCode() {
			return _circuit.hashCode() * 43 + _lap.hashCode() * 23 + _setupIdx;
		}
	}

	private class LapResults implements Iterable<Result> {
		private Result _currentTeamResult;
		private List<Result> _otherTeamsResults = new ArrayList<Result>();
		private int _setupIdx;
		
		public LapResults(int setupIdx) {
			_setupIdx = setupIdx;
		}

		public void add(Result result) {
//			System.out.println(result.getTeam().name() + ", " + result.getCircuit().name() + ", "  + result.getLap().name() + ", " + result.getIndex());
			if(_currentTeamName.equals(result.getTeam().name())) {
				if(_currentTeamResult != null) {
					throw new IllegalStateException();
				}
				_currentTeamResult = result;
			}
			else {
				_otherTeamsResults.add(result);
			}
		}
		
		public int setupIndex() {
			return _setupIdx;
		}
		
		public Result currentTeamResult() {
			return _currentTeamResult;
		}
		
		public Iterator<Result> iterator() {
			return _otherTeamsResults.iterator();
		}
	}
}
