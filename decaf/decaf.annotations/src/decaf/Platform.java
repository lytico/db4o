/* Copyright (C) 2011  Versant Inc.   http://www.db4o.com */
package decaf;

interface NegatablePlatform {
	void fillCompatibility(boolean[] compatibility, boolean compatible);
}

public enum Platform implements NegatablePlatform {

	JMX,
	ANNOTATION,
	OVERRIDE_FOR_INTERFACE,
	
	JDK11,
	JDK12(JDK11),
	JDK15(JDK12, JMX, ANNOTATION),
	JDK16(JDK15, OVERRIDE_FOR_INTERFACE),
	
	ANDROID(JDK15, JMX.not()),
	SHARPEN(JDK15),
	
	ALL;
	
	private final NegatablePlatform[] parents;
	private boolean[] compatibility = null;

	Platform(NegatablePlatform... parent) {
		this.parents = parent;
	}

	public boolean compatibleWith(Platform other) {
		return compatibility()[other.ordinal()];
	}

	public boolean[] compatibility() {
		if (compatibility != null) {
			return compatibility;
		}
		compatibility = new boolean[Platform.values().length];

		fillCompatibility(compatibility, true);

		return compatibility;
	}
	
	public void fillCompatibility(boolean[] compatibility, boolean compatible) {
		compatibility[ordinal()] = compatible;
		for (NegatablePlatform parent : parents) {
			parent.fillCompatibility(compatibility, compatible);
		}
	}

	NegatablePlatform not() {
		return new Negate(this);
	}

	public static class Negate implements NegatablePlatform {
		private final Platform platform;

		Negate(Platform platform) {
			this.platform = platform;
		}

		public void fillCompatibility(boolean[] compatibility, boolean value) {
			platform.fillCompatibility(compatibility, false);
		}
	}

}
