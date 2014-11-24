/*
 * Copyright (C) 2005 db4objects Inc.  http://www.db4o.com
 */
package com.db4o.browser.gui.controllers.detail.generator;

import java.io.ByteArrayInputStream;
import java.io.InputStream;
import java.io.UnsupportedEncodingException;

/**
 * StringInputStreamFactory.  A class that converts a java.lang.String into a 
 * java.io.InputStream.
 *
 * @author djo
 */
public class StringInputStreamFactory {
	public static InputStream construct(final String rawMaterial) {
		try {
			return new ByteArrayInputStream(rawMaterial.getBytes("utf-8"));
		} catch (UnsupportedEncodingException e) {
			throw new RuntimeException("utf-8 should always be available",e);
		}
	}

}
