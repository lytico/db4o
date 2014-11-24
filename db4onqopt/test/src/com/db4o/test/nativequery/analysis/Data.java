package com.db4o.test.nativequery.analysis;

import java.util.Date;

import com.db4o.activation.*;
import com.db4o.ta.*;

class Data extends Base {
	boolean bool;
	float value;
	float otherValue;
	String name;
	Data next;
	int[] intArray;
	Data[] objArray;
	Boolean boolWrapper;
	Date date;
	
	private int secret;
	
	public boolean getBool() {
		return bool;
	}
	
	public float getValue() {
		return value;
	}
	public float getValue(int times) {
		return otherValue;
	}
	public String getName() {
		return name;
	}
	public Data getNext() {
		return next;
	}
	
	public boolean hasNext() {
		return getNext()!=null;
	}

	public Date getDate() {
		return date;
	}
	
	public void someMethod() {
		System.out.println();
	}

	public boolean sameSecret(Data other) {
		return secret == other.secret;
	}
	
	public void activate(ActivationPurpose purpose) {
	}

	public void activate() {
		activate(ActivationPurpose.READ);
	}
	
	public void activate(String str) {
	}
	
	public static void activate(Activatable act) {
		act.activate(ActivationPurpose.READ);
	}
}
