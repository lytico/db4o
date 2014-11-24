package db4otesteclipse.unit;

import db4otesteclipse.*;

public class RunUnitTestAction extends RunTestAction {
	protected TestTypeSpec spec() {
		return new UnitTestTypeSpec();
	}
}
