package db4ounit.tests.fixtures;

import db4ounit.*;
import db4ounit.fixtures.*;

public class Set4TestUnit implements TestLifeCycle {
	
	private final Set4 subject = (Set4)SubjectFixtureProvider.value();
	private final Object[] data = MultiValueFixtureProvider.value();

	public void setUp() {
		for (int i=0; i<data.length; ++i) {
			Object element = data[i];
			subject.add(element);
		}
	}
	
	public void testSize() {
		Assert.areEqual(data.length, subject.size());
	}
	
	public void testContains() {
		for (int i=0; i<data.length; ++i) {
			Object element = data[i];
			Assert.isTrue(subject.contains(element));
		}
	}

	public void tearDown() throws Exception {
	}
}