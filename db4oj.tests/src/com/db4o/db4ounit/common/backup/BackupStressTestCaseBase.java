/* Copyright (C) 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.backup;

import java.io.*;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.consistency.*;
import com.db4o.db4ounit.common.api.*;
import com.db4o.foundation.io.*;
import com.db4o.internal.*;
import com.db4o.query.*;

import db4ounit.*;

public abstract class BackupStressTestCaseBase extends Db4oTestWithTempFile  {
    
    private static boolean verbose = false;
    
    private static boolean runOnOldJDK = false;
    
    private static final int ITERATIONS = 5;
    
    private static final int OBJECTS = 50;
    
    private static final int COMMITS = 10;
    
    private ObjectContainer _objectContainer;
    
    private volatile boolean _inBackup;
    
    private volatile boolean _noMoreBackups;
    
    private int _backups;
    
    private int _commitCounter;
        
    public void test() throws Exception {
    	openDatabase();
    	try {        
    		runTestIterations();
    	} finally {
    		closeDatabase();
    	}
        checkBackups();
    }

	private void runTestIterations() throws Exception {
		if(! runOnOldJDK && isOldJDK()) {
            System.out.println("BackupStressTest is too slow for regression testing on Java JDKs < 1.4");
            return;
        }
        
        BackupStressIteration iteration = new BackupStressIteration();
        _objectContainer.store(iteration);
        _objectContainer.commit();
        Thread backupThread = startBackupThread();
        for (int i = 1; i <= ITERATIONS; i++) {
        	Query exQuery = _objectContainer.query();
        	exQuery.constrain(BackupStressItem.class);
        	exQuery.descend("_iteration").constrain(i - 1).smaller();
        	ObjectSet<BackupStressItem> exRes = exQuery.execute();
			for(BackupStressItem item :exRes) {
        		//item._name = item._name + "u";
        		//_objectContainer.store(item);
        		_objectContainer.delete(item);
                doCommitIfRequired();
        	}
            _objectContainer.commit();
            for (int obj = 0; obj < OBJECTS; obj++) {
                _objectContainer.store(new BackupStressItem("i" + obj, i));
                doCommitIfRequired();
            }
            iteration.setCount(i);
            _objectContainer.store(iteration);
            _objectContainer.commit();
        }
        _noMoreBackups = true;
        backupThread.join();
	}

	private void doCommitIfRequired() {
		_commitCounter ++;
		if(_commitCounter >= COMMITS){
		    _objectContainer.commit();
		    _commitCounter = 0;
		}
	}

	private Thread startBackupThread() {
		Thread thread = new Thread(new Runnable() {
					public void run() {
				        while(!_noMoreBackups){
				            _backups ++;
				            String fileName = backupFile(_backups);
				            deleteFile(fileName);
							_inBackup = true;
							backup(_objectContainer, fileName);
							_inBackup = false;		            
				        }
				    }
				}, "BackupStressTestCase.startBackupThread");
		thread.start();
		return thread;
	}
   
	protected abstract void backup(ObjectContainer db, String fileName);
	
	private void openDatabase(){
        _objectContainer = Db4oEmbedded.openFile(config(), tempFile());
    }
    
    private void closeDatabase() throws InterruptedException{
        while(_inBackup){
            Thread.sleep(1000);
        }
        _objectContainer.close();
    }
    
	private void checkBackups() throws IOException{
        stdout("BackupStressTest");
        stdout("Backups created: " + _backups);
        
        for (int i = 1; i <= _backups; i++) {
            stdout("Backup " + i);
            ObjectContainer container = Db4oEmbedded.openFile(config(), backupFile(i));
            try {
	            stdout("Open successful");
	            Query q = container.query();
	            q.constrain(BackupStressIteration.class);
	            BackupStressIteration iteration = (BackupStressIteration) q.execute().next();
	            
	            int iterations = iteration.getCount();
	            
	            stdout("Iterations in backup: " + iterations);
	            
	            if(iterations > 0){
	                q = container.query();
	                q.constrain(BackupStressItem.class);
	                q.descend("_iteration").constrain(new Integer(iteration.getCount()));
	                ObjectSet items = q.execute();
	                Assert.areEqual(OBJECTS, items.size());
	                while(items.hasNext()){
	                    BackupStressItem item = (BackupStressItem) items.next();
	                    Assert.areEqual(iterations, item._iteration);
	                }
	            }
	            Assert.isTrue(new ConsistencyChecker(container).checkSlotConsistency().consistent());
            } finally {            
            	container.close();
            }
            stdout("Backup OK");
        }
        stdout("BackupStressTest " + _backups + " files OK.");
        for (int i = 1; i <= _backups; i++) {
            deleteFile(backupFile(i));
        }
    }

	private void deleteFile(String fname) {
		File4.delete(fname);
	}
    
    private boolean isOldJDK(){
        ObjectContainerBase stream = (ObjectContainerBase)_objectContainer;
        return stream.needsLockFileThread();
    }
    
    private String backupFile(int count){
        return tempFile() + count;
    }

    private void stdout(String string) {
        if(verbose){
            System.out.println(string);
        }
    }

	private EmbeddedConfiguration config() {
		EmbeddedConfiguration config = newConfiguration();
        config.common().objectClass(BackupStressItem.class).objectField("_iteration").indexed(true);
        config.common().reflectWith(Platform4.reflectorForType(BackupStressItem.class));
        return config;
	}

}
