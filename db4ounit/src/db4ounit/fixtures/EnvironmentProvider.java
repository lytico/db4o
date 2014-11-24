package db4ounit.fixtures;

import com.db4o.foundation.*;

public class EnvironmentProvider implements FixtureProvider {
	
	private final FixtureVariable _variable = new FixtureVariable() {
		@Override
		public void with(final Object value, final Runnable runnable) {
			super.with(value, new Runnable() { public void run() {
				Environments.runWith((Environment)value, runnable);
			}});
		}
	};

	public FixtureVariable variable() {
		return _variable;
	}

	public Iterator4 iterator() {
		return Iterators.singletonIterator(Environments.newConventionBasedEnvironment());
	}

}
