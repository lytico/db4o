/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.config.annotations.reflect;


/**
 * @exclude
 * @sharpen.ignore
 */
@decaf.Ignore(unlessCompatible=decaf.Platform.JDK15)
public class UpdatedDepthConfigurator extends Db4oConfigurator {
	private String _className;

	private int _updateDepth;

	public UpdatedDepthConfigurator(String className, int updateDepthDefault) {
		this._className = className;
		this._updateDepth = updateDepthDefault;
	}

	@Override
	protected void configure() {
		objectClass(_className).updateDepth(_updateDepth);

	}

}
