/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.db4ounit.common.filelock;

import java.io.*;
import java.nio.channels.*;

import com.db4o.internal.*;

import db4ounit.*;

@decaf.Remove(unlessCompatible=decaf.Platform.JDK15)
public class AcquireNativeLock {

	public static void main(String[] args) throws IOException {
		RandomAccessFile raf = null;
		try{
			raf = new RandomAccessFile(args[0], "rw");
			FileChannel channel = raf.getChannel();
			try {
				channel.tryLock();
			}catch(ReflectException rex){
				Assert.fail("File shouldn't be locked already.");
				rex.printStackTrace();
			}
			System.out.println("ready");
			new BufferedReader(new InputStreamReader(System.in)).readLine();
		}finally{
			raf.close();
		}
	}
	
}
