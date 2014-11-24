package db4ounit;

public class JaggedArrayAssert {

	public static void areEqual(Object[][] expected, Object[][] actual) {
		if (expected == actual) return;
		if (expected == null || actual == null) Assert.areSame(expected, actual);
		Assert.areEqual(expected.length, actual.length);
		Assert.areSame(expected.getClass(), actual.getClass());
	    for (int i = 0; i < expected.length; i++) {
	        ArrayAssert.areEqual(expected[i], actual[i]);
	    }
	}

	public static void areEqual(int[][] expected, int[][] actual) {
		if (expected == actual) return;
		if (expected == null || actual == null) Assert.areSame(expected, actual);
		Assert.areEqual(expected.length, actual.length);
		Assert.areSame(expected.getClass(), actual.getClass());
	    for (int i = 0; i < expected.length; i++) {
	        ArrayAssert.areEqual(expected[i], actual[i]);
	    }
	}

}
