/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */
package com.db4o.internal;

import com.db4o.foundation.*;

public class FieldIndexException extends ChainedRuntimeException {

	private String _className;
	private String _fieldName;
	
	public FieldIndexException(FieldMetadata field) {
		this(null,null,field);
	}

	public FieldIndexException(String msg,FieldMetadata field) {
		this(msg,null,field);
	}

	public FieldIndexException(Throwable cause,FieldMetadata field) {
		this(null,cause,field);
	}

	public FieldIndexException(String msg, Throwable cause,FieldMetadata field) {
		this(msg,cause,field.containingClass().getName(),field.getName());
	}

	public FieldIndexException(String msg, Throwable cause,String className,String fieldName) {
		super(enhancedMessage(msg,className, fieldName), cause);
		_className=className;
		_fieldName=fieldName;
	}

	public String className() {
		return _className;
	}
	
	public String fieldName() {
		return _fieldName;
	}
	
	private static String enhancedMessage(String msg,String className,String fieldName) {
		String enhancedMessage="Field index for "+className+"#"+fieldName;
		if(msg!=null) {
			enhancedMessage+=": "+msg;
		}
		return enhancedMessage;
	}
}
