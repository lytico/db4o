/* Copyright (C) 2009  Versant Inc.  http://www.db4o.com */

package com.db4o.io;

import java.io.*;

import com.db4o.ext.*;
import com.db4o.internal.*;

/**
 * @sharpen.ignore
 */
public class RandomAccessFileFactory {
	
	public static RandomAccessFile newRandomAccessFile(String path, boolean readOnly, boolean lockFile){
		boolean ok = false;
		RandomAccessFile raf = null;
		try {
			raf = new RandomAccessFile(path, readOnly ? "r" : "rw");
			if (lockFile && ! readOnly) {
				Platform4.lockFile(path, raf);
			} 
			ok = true;
			return raf;
		} catch (IOException e) {
			throw new Db4oIOException(e);
		} finally{
			if(! ok && raf != null){
				try {
					raf.close();
				} catch (IOException e) {
				
				}
			}
		}
	}

}
