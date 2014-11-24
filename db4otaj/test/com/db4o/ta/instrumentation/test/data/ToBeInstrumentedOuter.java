package com.db4o.ta.instrumentation.test.data;

public class ToBeInstrumentedOuter {

	public void foo() {
		new Runnable() {
			public void run() {
			}
		}.run();
	}
	
}
