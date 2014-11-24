/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.drs.versant;

import java.io.*;

import com.db4o.drs.inside.*;
import com.db4o.util.IOServices.ProcessRunner;
import com.db4o.util.*;


public class VodEventDriver {

	
	
	private final EventConfiguration _eventConfiguration;
	
	private ProcessRunner _process;
	
	public VodEventDriver(EventConfiguration config) {
		_eventConfiguration = config;
	}

	public boolean start() {
		if(_process != null){
			throw new IllegalStateException();
		}
		File configFile = new File(Path4.getTempFileName());
		try {
			_eventConfiguration.writeConfigFile(configFile);
			return startVedProcess(configFile.getAbsolutePath());
		} catch (IOException e) {
			e.printStackTrace();
		} finally {
			configFile.delete();
		}
		return true;
	}

	private boolean startVedProcess(String configFilePath) {
		if(_process != null){
			throw new IllegalStateException();
		}
		try {		
			_process = new ProcessRunner(VodDatabase.VED_DRIVER, (new String[]{
					databaseName(), configFilePath 
			})) {
				@Override
				protected void output(String line) {
					if (DrsDebug.verbose) {
						System.out.println("[VEDDRIVER OUTPUT] " + line);
					}
				};
			};
		} catch (IOException e) {
			e.printStackTrace();
			destroyProcess();
			return false;
		}
		try{
			_process.waitFor(databaseName(), 10000);
		} catch (RuntimeException ex){
			ex.printStackTrace();
			destroyProcess();
			return false;
		}
		return true;
	}
	

	private void destroyProcess() {
		if(_process != null){
			try{
				_process.destroy();
			} catch(RuntimeException rex){
				rex.printStackTrace();
			}
			if(DrsDebug.verbose){
				System.out.println(_process.processResult());
			}
			_process = null;
		}
	}

	public void stop() {
		if(_process == null){
			throw new IllegalStateException();
		}
		destroyProcess();
	}

	private String databaseName() {
		return _eventConfiguration.databaseName;
	}
	
	public void printStartupFailure(){
		System.err.println("Failed to start " + VodDatabase.VED_DRIVER);
		System.err.println("Configuration:\n" + _eventConfiguration);
		if(_process != null){
			System.err.println(_process.processResult());
		}
	}

}
