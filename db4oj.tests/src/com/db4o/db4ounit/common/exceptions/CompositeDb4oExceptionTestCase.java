/* Copyright (C) 2009  Versant Corp.  http://www.db4o.com */

package com.db4o.db4ounit.common.exceptions;

import java.io.*;

import com.db4o.ext.*;

import db4ounit.*;

/**
 * @sharpen.remove
 */
public class CompositeDb4oExceptionTestCase implements TestCase{
	
	public void test(){
		try{
			throwCompositeException();
		} catch( CompositeDb4oException ex){
			StringWriter stringWriter = new StringWriter();
			PrintWriter printWriter = new PrintWriter(stringWriter);
			ex.printStackTrace(printWriter);
			String stackTrace = stringWriter.toString();
			StringAssert.contains("throwCompositeException", stackTrace);
			StringAssert.contains("method1", stackTrace);
			StringAssert.contains("method2", stackTrace);
			ex.printStackTrace();
		}
	}

	private void throwCompositeException() {
		Exception ex1 = null;
		Exception ex2 = null;
		
		try{
			method1();
		}catch(Exception ex){
			ex1 = ex;
		}
		
		try{
			method2();
		}catch(Exception ex){
			ex2 = ex;
		}
		
		throw new CompositeDb4oException(ex1, ex2);
	}
	
	private void method1(){
		throw new RuntimeException();
	}
	
	private void method2 () {
		throw new RuntimeException();
	}

}
