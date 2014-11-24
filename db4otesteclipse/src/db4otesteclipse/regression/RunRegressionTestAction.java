package db4otesteclipse.regression;

import db4otesteclipse.*;

public abstract class RunRegressionTestAction extends RunTestAction {
	
	protected final TestTypeSpec spec() {
		return new RegressionTestTypeSpec(mode());
	}
	
	protected abstract String mode();
}
