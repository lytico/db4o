/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package db4ounit.fixtures;

public class LabeledObject<T> implements Labeled {
	
	private final T _value;
	
	private final String _label;
	
	public LabeledObject  (T value, String label){
		_value = value;
		_label = label;
	}
	
	public LabeledObject  (T value){
		this(value, null);
	}


	public String label() {
		if(_label == null){
			return _value.toString();
		}
		return _label;
	}
	
	public T value(){
		return _value;
	}
	
	public static <T> LabeledObject<T>[] forObjects(T...values){
		LabeledObject<T> [] labeledObjects = new LabeledObject[values.length];
		for (int i = 0; i < values.length; i++) {
			labeledObjects[i] = new LabeledObject<T>(values[i]);
		}
		return labeledObjects;
	}

}
