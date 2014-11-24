package db4ounit.fixtures;

public class FixtureTestSuiteDescription extends FixtureBasedTestSuite {

	private FixtureProvider[] _providers;
	private Class[] _testUnits;
	
	public void fixtureProviders(FixtureProvider... providers) {
		_providers = providers;
	}
	
	public void testUnits(Class... testUnits) {
		_testUnits = testUnits;
	}

	@Override
	public FixtureProvider[] fixtureProviders() {
		return _providers;
	}

	@Override
	public Class[] testUnits() {
		return _testUnits;
	}
}
