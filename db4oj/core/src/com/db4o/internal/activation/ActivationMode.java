package com.db4o.internal.activation;

public final class ActivationMode {
	
	public static final ActivationMode ACTIVATE = new ActivationMode();
	
	public static final ActivationMode DEACTIVATE = new ActivationMode();
	
	public static final ActivationMode PEEK = new ActivationMode();

	public static final ActivationMode PREFETCH = new ActivationMode();
	
	public static final ActivationMode REFRESH = new ActivationMode();
	
	private ActivationMode() {
	}
	
	public String toString() {
		if (isActivate()) {
			return "ACTIVATE";
		}
		if (isDeactivate()) {
			return "DEACTIVATE";
		}
		if (isPrefetch()) {
			return "PREFETCH";
		}
		if (isRefresh()) {
			return "REFRESH";
		}
		return "PEEK";
	}

	public boolean isDeactivate() {
		return this == DEACTIVATE;
	}

	public boolean isActivate() {
		return this == ACTIVATE;
	}

	public boolean isPeek() {
		return this == PEEK;
	}

	public boolean isPrefetch() {
		return this == PREFETCH;
	}
	
	public boolean isRefresh() {
		return this == REFRESH;
	}
}
