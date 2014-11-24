/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test.legacy;

import java.io.*;

import com.db4o.*;
import com.db4o.ext.*;
import com.db4o.test.*;
import com.db4o.types.*;

import db4ounit.extensions.util.*;

/**
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class ExternalBlobs {
	
	static final String BLOB_FILE_IN = AllTestsConfAll.BLOB_PATH + "/regressionBlobIn.txt"; 
	static final String BLOB_FILE_OUT = AllTestsConfAll.BLOB_PATH + "/regressionBlobOut.txt"; 
	
	public Blob blob;
	
	public void configure(){
		try{
			Db4o.configure().setBlobPath(AllTestsConfAll.BLOB_PATH);
			deleteFiles();
		}catch(Exception e){
			e.printStackTrace();
		}
	}
	
	public void storeOne(){
	}
	
	public void testOne(){
		
		if(new File(AllTestsConfAll.BLOB_PATH).exists()){
			try{
				char[] chout = new char[]{'H', 'i', ' ', 'f','o', 'l', 'k','s'};
				deleteFiles();
				FileWriter fw = new FileWriter(BLOB_FILE_IN);
				fw.write(chout);
				fw.flush();
				fw.close();
				blob.readFrom(new File(BLOB_FILE_IN));
				double status = blob.getStatus();
				while(status > Status.COMPLETED){
					Thread.sleep(50);
					status = blob.getStatus();
				}
				
				blob.writeTo(new File(BLOB_FILE_OUT));
				status = blob.getStatus();
				while(status > Status.COMPLETED){
					Thread.sleep(50);
					status = blob.getStatus();
				}
				File resultingFile = new File(BLOB_FILE_OUT);
				Test.ensure(resultingFile.exists());
				if(resultingFile.exists()){
					FileReader fr = new FileReader(resultingFile);
					char[] chin = new char[chout.length];
					fr.read(chin);
					for (int i = 0; i < chin.length; i++) {
						Test.ensure(chout[i] == chin[i]);
                    }
                    fr.close();
				}
			}catch(Exception e){
				Test.ensure(false);
				e.printStackTrace();
			}
		}
		
	}

	private void deleteFiles() throws IOException {
		IOUtil.deleteDir(AllTestsConfAll.BLOB_PATH);
	}

}
