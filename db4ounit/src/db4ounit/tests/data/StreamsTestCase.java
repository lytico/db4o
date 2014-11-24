package db4ounit.tests.data;

import com.db4o.foundation.*;

import db4ounit.*;

public class StreamsTestCase implements TestCase {
	
	public void testSeries() {
		final Collection4 calls = new Collection4();
		final Iterator4 series = Iterators.series("", new Function4() {
			public Object apply(Object value) {
				calls.add(value);
				return value + "*";
			}
		}).iterator();
		Assert.isTrue(series.moveNext());
		Assert.isTrue(series.moveNext());
		Iterator4Assert.areEqual(new Object[] { "", "*" }, calls.iterator());
	}
	
}
