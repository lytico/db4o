/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o;

final class Session
{
	final String			i_fileName;
	YapStream				i_stream;
	private int				i_openCount;
	
	Session(String a_fileName){
		i_fileName = a_fileName;
	}
	
	/**
	 * returns true, if session is to be closed completely
	 */
	boolean closeInstance(){
		i_openCount --;
		return i_openCount < 0;
	}
	
	public boolean equals(Object a_object){
		return i_fileName.equals(((Session)a_object).i_fileName);
	}
	
	String fileName(){
		return i_fileName;
	}
	
	YapStream subSequentOpen(){
		if( i_stream.isClosed()){
			return null;
		}
		i_openCount ++;
		return i_stream;
	}
	

	
}
