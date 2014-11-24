/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.drs.versant;

import java.io.*;

import com.db4o.internal.*;
import com.db4o.util.DrsRuntime4;

public class EventConfiguration {
	
	private static final String VED_LIB_BASENAME = "vedse";

	public final String databaseName;
	
	public final String userName;
	
	public final String logFileName;
	
	public final String password;
	
	public final String serverHost;
	
	public final int serverPort;
	
	public final String clientHost;
	
	private final int clientPort;
	
	public String eventProcessorHost;
	
	public int eventProcessorPort;
	
	public final boolean verbose;


	
	public EventConfiguration(String databaseName, String userName, String password, String logFileName, String serverHost, int serverPort, String clientHost, EventClientPortSelectionStrategy clientPortStrategy, String eventProcessorHost, int eventProcessorPort, boolean verbose) {
		this.databaseName = databaseName;
		this.userName = userName;
		this.password = password;
		this.logFileName = logFileName;
		this.serverHost = serverHost;
		this.serverPort = serverPort;
		this.clientHost = clientHost;
		this.eventProcessorHost = eventProcessorHost;
		this.eventProcessorPort = eventProcessorPort;
		this.clientPort = clientPortStrategy.clientPort();
		this.verbose = verbose;
	}

	public void writeConfigFile(File file) throws IOException {
		FileWriter fileWriter = new FileWriter(file);
		PrintWriter writer = new PrintWriter(fileWriter);
		writer.println("ChannelServicePort " + serverPort);
		writer.println("Log " + logFileName);
		writer.println("LogLevel 2 ");
		
		writer.println("<EngineLibs>");
		writer.println(DrsRuntime4.runningOnWindows() ? VED_LIB_BASENAME + ".dll" : "lib" + VED_LIB_BASENAME + ".so");
		writer.println("</EngineLibs>");
		
		writer.flush();
		writer.close();
	}
	
	public int clientPort() {
		return clientPort;
	}
	
	@Override
	public String toString() {
		return Reflection4.dump(this);
	}

}
