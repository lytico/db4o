/* Copyright (C) 2009   Versant Inc.   http://www.db4o.com */

package com.db4o.reflect.core;

import com.db4o.foundation.*;

/**
 * a spec holding a constructor, it's arguments
 * and information, if the constructor can instantiate
 * objects.
 */
public class ReflectConstructorSpec {
	private ReflectConstructor _constructor;
	private Object[] _args;
	private TernaryBool _canBeInstantiated;

	public static final ReflectConstructorSpec UNSPECIFIED_CONSTRUCTOR =
		new ReflectConstructorSpec(TernaryBool.UNSPECIFIED);

	public static final ReflectConstructorSpec INVALID_CONSTRUCTOR =
		new ReflectConstructorSpec(TernaryBool.NO);

	public ReflectConstructorSpec(ReflectConstructor constructor, Object[] args) {
		_constructor = constructor;
		_args = args;
		_canBeInstantiated = TernaryBool.YES; 
	}
	
	private ReflectConstructorSpec(TernaryBool canBeInstantiated) {
		_canBeInstantiated = canBeInstantiated;
		_constructor = null;
	}
	
	/**
	 * creates a new instance.
	 * @return the newly created instance.
	 */
	public Object newInstance() {
		if(_constructor == null) {
			return null;
		}
		return _constructor.newInstance(_args);
	}
	
	/**
	 * returns true if an instance can be instantiated
	 * with the constructor, otherwise false.
	 */
	public TernaryBool canBeInstantiated(){
		return _canBeInstantiated;
	}
}
