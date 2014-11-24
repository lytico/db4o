/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o;

import com.db4o.config.*;

/**
 * @exclude 
 */
public class Unobfuscated {
    
    static Object random;
    
	static boolean createDb4oList(Object a_stream){
	    ((YapStream)a_stream).checkClosed();
	    return ! ((YapStream)a_stream).isInstantiating();
	}
	
	public static byte[] generateSignature() {
	    // TODO: We could add part of the file name to improve 
	    //       signature security.
	    YapWriter writer = new YapWriter(null, 300);
	    if(! Deploy.csharp) {
		    try {
	            new YapStringIO().write(writer, "localhost");
	            writer.append((byte)0);
	            writer.append(new byte[]{127,0,0,1});
	        } catch (Exception e) {
	        }
	    }
	    YLong.writeLong(System.currentTimeMillis(), writer);
        YLong.writeLong(randomLong(), writer);
        YLong.writeLong(randomLong() + 1, writer);
        return writer.getWrittenBytes();
	}
	
	static void logErr (Configuration config, int code, String msg, Throwable t) {
		Messages.logErr(config, code, msg, t);
	}
	
	static void purgeUnsychronized(Object a_stream, Object a_object){
	    ((YapStream)a_stream).purge1(a_object);
	}
	
	public static long randomLong() {
	    if(Deploy.csharp) {
	        // TODO: route to .NET implementation
	        return System.currentTimeMillis();
	    }else {
	        if(random == null){
	            random = new java.util.Random();
	        }
	        return ((java.util.Random)random).nextLong();
	    }
	}
	
	static void shutDownHookCallback(Object a_stream){
		((YapStream)a_stream).failedToShutDown();
	}


}
