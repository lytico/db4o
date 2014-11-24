/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.polepos.test.continuous;

import static org.easymock.EasyMock.createMock;
import static org.easymock.EasyMock.expect;
import static org.easymock.EasyMock.replay;
import static org.easymock.EasyMock.verify;

import java.util.*;

import org.polepos.framework.*;
import org.polepos.reporters.*;

import com.db4o.polepos.continuous.*;

import db4ounit.*;

public class PerformanceMonitoringReporterTestCase implements TestCase {

	private static final String OTHER_TEAM_NAME = "other";

	private static final String CURRENT_TEAM_NAME = "current";
	
	public void testSingleLapNoAlert() {
		assertCircuitPerformance(turnReadings(1000, 1000), turnReadings(1000, 1000), turnExpectations(true, true));
	}

	public void testSingleLapTimeAlert() {
		assertCircuitPerformance(turnReadings(2000, 1000), turnReadings(1000, 1000), turnExpectations(false, true));
	}

	public void testSingleLapMemoryAlert() {
		assertCircuitPerformance(turnReadings(1000, 2000), turnReadings(1000, 1000), turnExpectations(true, false));
	}

	public void testSingleLapTimeAndMemoryAlert() {
		assertCircuitPerformance(turnReadings(2000, 2000), turnReadings(1000, 1000), turnExpectations(false, false));
	}

	public void testTwoLapsNoAlert() {
		assertCircuitPerformance(turnReadings(1000,1000,500,500), turnReadings(1000,2000,500,1000), turnExpectations(true,true,true,true));
	}

	public void testTwoLapsSingleTimeAlert() {
		assertCircuitPerformance(turnReadings(1000,1000,500,500), turnReadings(1000,1000,100,500), turnExpectations(true,true,false,true));
	}

	public void testTwoLapsTwoMemoryAlerts() {
		assertCircuitPerformance(turnReadings(1000,1000,500,500), turnReadings(1000,500,500,100), turnExpectations(true,false,true,false));
	}

	public void testTwoTurnsSingleLapNoAlert() {
		assertCircuitPerformance(turnReadings(lapReadings(500,500),lapReadings(1000,1000)), turnReadings(lapReadings(500,500),lapReadings(1000,1000)), turnExpectations(lapExpectations(true,true),lapExpectations(true,true)));
	}

	public void testTwoTurnsSingleLapTimeAlert() {
		assertCircuitPerformance(turnReadings(lapReadings(1000,500),lapReadings(1000,1000)), turnReadings(lapReadings(500,500),lapReadings(1000,1000)), turnExpectations(lapExpectations(false,true),lapExpectations(true,true)));
	}

	public void testTwoTurnsTwoLapsNoAlert() {
		assertCircuitPerformance(turnReadings(lapReadings(100,100,200,200),lapReadings(200,200,400,400)), turnReadings(lapReadings(100,100,200,200),lapReadings(200,200,400,400)), turnExpectations(lapExpectations(true,true,true,true),lapExpectations(true,true,true,true)));
	}

	public void testTwoTurnsTwoLapsSingleTimeAlert() {
		assertCircuitPerformance(turnReadings(lapReadings(100,100,500,200),lapReadings(200,200,400,400)), turnReadings(lapReadings(100,100,200,200),lapReadings(200,200,400,400)), turnExpectations(lapExpectations(true,true,false,true),lapExpectations(true,true,true,true)));
	}

	public void testTwoTurnsTwoLapsOneTimeOneMemoryAlert() {
		assertCircuitPerformance(turnReadings(lapReadings(100,200,200,200),lapReadings(400,200,400,400)), turnReadings(lapReadings(100,100,200,200),lapReadings(200,200,400,400)), turnExpectations(lapExpectations(true,false,true,true),lapExpectations(false,true,true,true)));
	}

	public void testTwoTurnsTwoLapsLotsOfAlerts() {
		assertCircuitPerformance(turnReadings(lapReadings(200,200,200,500),lapReadings(500,200,800,800)), turnReadings(lapReadings(100,100,200,200),lapReadings(200,200,400,400)), turnExpectations(lapExpectations(false,false,true,false),lapExpectations(false,true,false,false)));
	}

	private void assertCircuitPerformance(
			Reading[][] current,
			Reading[][] other,
			ReadingExpectation[][] expectations) {
		Assert.areEqual(current.length, other.length);
		Assert.areEqual(current.length, expectations.length);
		final int numTurns = current.length;
		final int numLaps = current[0].length;
		for (MeasurementType measurementType : MeasurementType.values()) {
			PerformanceComparisonStrategy strategy = initializeStrategyMock(current, other, expectations, measurementType);
			PerformanceMonitoringReporter reporter = new PerformanceMonitoringReporter(CURRENT_TEAM_NAME, measurementType, strategy);
			reporter.startSeason();
			CircuitBase circuit = new Uberlandia();
			TurnSetup[] setups = createTurnSetups(numTurns);
			Lap[] laps = createLaps(numLaps);
			reporter.sendToCircuit(circuit);
			reportResults(reporter, circuit, setups, laps, CURRENT_TEAM_NAME, current);
			reportResults(reporter, circuit, setups, laps, OTHER_TEAM_NAME, other);
			reporter.endSeason();
			PerformanceReport report = reporter.performanceReport();
			verify(strategy);
			assertPerformanceReport(current, other, expectations, report, measurementType);
		}
	}

	private void reportResults(Reporter reporter, CircuitBase circuit, TurnSetup[] setups, Lap[] laps, String teamName, Reading[][] readings) {
		Team team = new MockTeam(teamName);
		Car car = new MockCar(team, carName(teamName));
		TurnResult[] turnResults = new TurnResult[setups.length];
		for(int turnIdx = 0; turnIdx < readings.length; turnIdx++) {
			turnResults[turnIdx] = new TurnResult();
			for(int lapIdx = 0; lapIdx < readings[turnIdx].length; lapIdx++) {
				turnResults[turnIdx].report(new TimedLapsResult(circuit, team, laps[lapIdx], setups[turnIdx], 0, readings[turnIdx][lapIdx].time, readings[turnIdx][lapIdx].memory, 0, 0));
			}
		}
		reporter.report(team, car, setups, turnResults);
	}

	private void assertPerformanceReport(Reading[][] current, Reading[][] other, ReadingExpectation[][] expectations, PerformanceReport report, MeasurementType measurementType) {
		List<PerformanceFailure> failures = report.failures();
		int expectedFailureCount = countFailures(expectations, measurementType);
		Assert.areEqual(expectedFailureCount == 0, report.performanceOk());
		Assert.areEqual(expectedFailureCount, failures.size());
		MeasurementTypeExtractor extractor = EXTRACTORS.get(measurementType);
		for (PerformanceFailure failure : failures) {
			int lapIdx = lapIdx(failure.lapName);
			int turnIdx = failure.setupIdx;
			ReadingExpectation curExp = expectations[turnIdx][lapIdx];
			Reading currentReading = current[turnIdx][lapIdx];
			Reading otherReading = other[turnIdx][lapIdx];
			if(failure.measurementType == measurementType) {
				Assert.isFalse(extractor.expectedValue(curExp));
				assertFailureValues(failure, extractor.actualValue(currentReading), extractor.actualValue(otherReading));
				break;
			}
		}
	}

	private void assertFailureValues(PerformanceFailure failure,
			long currentValue, long otherValue) {
		Assert.areEqual(currentValue, failure.currentValue);
		Assert.areEqual(otherValue, failure.otherValue);
	}

	private Lap[] createLaps(int count) {
		Lap[] laps = new Lap[count];
		for(int idx = 0; idx < count; idx++) {
			laps[idx] = new Lap(lapName(idx));
		}
		return laps;
	}

	private String lapName(int idx) {
		return String.valueOf(idx);
	}

	private int lapIdx(String name) {
		return Integer.parseInt(name);
	}
	
	private TurnSetup[] createTurnSetups(int count) {
		TurnSetup[] setups = new TurnSetup[count];
		for(int idx = 0; idx < count; idx++) {
			setups[idx] = new TurnSetup(new SetupProperty(TurnSetupConfig.OBJECTCOUNT, idx));
		}
		return setups;
	}

	private PerformanceComparisonStrategy initializeStrategyMock(Reading[][] current, Reading[][] other, ReadingExpectation[][] expectations, MeasurementType measurementType) {
		PerformanceComparisonStrategy strategy = createMock(PerformanceComparisonStrategy.class);
		MeasurementTypeExtractor extractor = EXTRACTORS.get(measurementType);
		//for(int turnIdx = 0; turnIdx < current.length; turnIdx++) {
			int turnIdx = current.length - 1;
			for(int lapIdx = 0; lapIdx < current[0].length; lapIdx++) {
				long actualCurrent = extractor.actualValue(current[turnIdx][lapIdx]);
				long actualOther = extractor.actualValue(other[turnIdx][lapIdx]);
				boolean expReturn = extractor.expectedValue(expectations[turnIdx][lapIdx]);
				expect(strategy.acceptableDiff(actualCurrent, actualOther)).andReturn(expReturn);
			}
		//}
		replay(strategy);
		return strategy;
	}

	private String carName(String teamName) {
		return teamName + "'s car";
	}

	private static Reading[][] turnReadings(Reading[]... values) {
		return values;
	}

	private static Reading[][] turnReadings(long... values) {
		return turnReadings(lapReadings(values));
	}

	private static ReadingExpectation[][] turnExpectations(ReadingExpectation[]... values) {
		return values;
	}

	private static ReadingExpectation[][] turnExpectations(boolean... values) {
		return turnExpectations(lapExpectations(values));
	}

	private static Reading[] lapReadings(long... values) {
		Assert.isTrue((values.length % 2) == 0);
		Reading[] readings = new Reading[values.length / 2];
		for(int idx = 0; idx < readings.length; idx++) {
			readings[idx] = new Reading(values[2 * idx], values[2 * idx + 1]);
		}
		return readings;
	}

	private static ReadingExpectation[] lapExpectations(boolean... values) {
		Assert.isTrue((values.length % 2) == 0);
		ReadingExpectation[] expectations = new ReadingExpectation[values.length / 2];
		for(int idx = 0; idx < expectations.length; idx++) {
			expectations[idx] = new ReadingExpectation(values[2 * idx], values[2 * idx + 1]);
		}
		return expectations;
	}
	
	private static int countFailures(ReadingExpectation[][] expectations, MeasurementType measurementType) {
		int numFailures = 0;
		MeasurementTypeExtractor extractor = EXTRACTORS.get(measurementType);
		//for (int turnIdx = 0; turnIdx < expectations.length; turnIdx++) {
		int turnIdx = expectations.length - 1;
			for (int lapIdx = 0; lapIdx < expectations[0].length; lapIdx++) {
				ReadingExpectation curExp = expectations[turnIdx][lapIdx];
				if(!extractor.expectedValue(curExp)) {
					numFailures++;
				}
			}
		//}
		return numFailures;
	}

	private static class Reading {
		public final long time;
		public final long memory;

		public Reading(long time, long memory) {
			this.time = time;
			this.memory = memory;
		}
	}

	private static class ReadingExpectation {
		public final boolean timeOk;
		public final boolean memoryOk;

		public ReadingExpectation(boolean timeOk, boolean memoryOk) {
			this.timeOk = timeOk;
			this.memoryOk = memoryOk;
		}
	}

	private static class MockTeam extends Team {

		private String _name;
		
		public MockTeam(String name) {
			_name = name;
		}
		
		@Override
		public Car[] cars() {
			return null;
		}

		@Override
		public String databaseFile() {
			return null;
		}

		@Override
		public String description() {
			return null;
		}

		@Override
		public DriverBase[] drivers() {
			return null;
		}

		@Override
		public String name() {
			return _name;
		}

		@Override
		public String website() {
			return null;
		}

		@Override
		public void setUp() {
			
		}
		
	}
	
	private static class MockCircuit extends TimedLapsCircuitBase {
		@Override
		protected void addLaps() {
		}

		@Override
		public String description() {
			return null;
		}

		@Override
		public Class<? extends DriverBase> requiredDriver() {
			return null;
		}
	}

	private static class Uberlandia extends MockCircuit {
	}

	private static class MockCar extends Car {

		private String _name;
		
		public MockCar(Team team, String name) {
			super(team, "555555");
			_name = name;
		}
		
		@Override
		public String name() {
			return _name;
		}
	}
	
	private static interface MeasurementTypeExtractor {
		boolean expectedValue(ReadingExpectation exp);
		long actualValue(Reading reading);
	}
	
	private static Map<MeasurementType, MeasurementTypeExtractor> EXTRACTORS;
	
	static {
		EXTRACTORS = new HashMap<MeasurementType, MeasurementTypeExtractor>();
		EXTRACTORS.put(MeasurementType.TIME, new MeasurementTypeExtractor()  {
			public boolean expectedValue(ReadingExpectation exp) {
				return exp.timeOk;
			}
			
			public long actualValue(Reading reading) {
				return reading.time;
			}
		});
		EXTRACTORS.put(MeasurementType.MEMORY, new MeasurementTypeExtractor()  {
			public boolean expectedValue(ReadingExpectation exp) {
				return exp.memoryOk;
			}
			
			public long actualValue(Reading reading) {
				return reading.memory;
			}
		});
	}
}
