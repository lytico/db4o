/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.drs.versant.eventprocessor;


import static com.db4o.drs.versant.eventprocessor.EventProcessorApplication.Arguments.*;

import java.io.*;

import org.apache.commons.cli.*;

import com.db4o.drs.versant.*;


public class EventProcessorApplication {
	
	private static final String LOCALHOST = "localhost";

	public static class Arguments{

		public static final String DATABASE = "database";
		
		public static final String USER_NAME = "username";
		
		public static final String PASSWORD = "password";
		
		public static final String VERBOSE = "verbose";
		
		public static final String LOGFILE = "logfile";
		
		public static final String SERVER = "server";
		
		public static final String SERVER_PORT = "serverport";
		
		public static final String CLIENT = "client";
		
		public static final String CLIENT_PORT = "clientport";
		
		public static final String EVENTPROCESSOR_HOST = "eventProcessorHost";
		
		public static final String EVENTPROCESSOR_PORT = "eventProcessorPort";
	}
	
	public static void main(String[] args) throws IOException {
		new EventProcessorApplication(args);
	}
	
	private EventConfiguration _eventConfiguration;

	public EventProcessorApplication(String[] args) throws IOException {
		if(! parseArguments(args)){
			return;
		}
		EventProcessorFactory.newInstance(_eventConfiguration).run();
	}
	
	private boolean parseArguments(String[] args) {
		
		Options options = new Options();
		
		options.addOption(OptionBuilder
				.withArgName( "name" )
	        	.hasArg()
	        	.isRequired()
	        	.withDescription(  "to generate replication events for" )
	        	.create(DATABASE ));
		
		options.addOption(OptionBuilder
				.withArgName( "username" )
	        	.hasArg()
	        	.isRequired()
	        	.withDescription(  "database user name" )
	        	.create(USER_NAME ));
		
		options.addOption(OptionBuilder
				.withArgName( "password" )
	        	.hasArg()
	        	.isRequired()
	        	.withDescription(  "database user password" )
	        	.create(PASSWORD));
		
		options.addOption(OptionBuilder
				.withArgName( "filename" )
	        	.hasArg()
	        	.isRequired()
	        	.withDescription(  "where events are written" )
	        	.create(LOGFILE ));
		
		options.addOption(OptionBuilder
				.withArgName( "hostname" )
	        	.hasArg()
	        	.withDescription(  "server hostname where veddriver is running" )
	        	.create(SERVER));
		
		options.addOption(OptionBuilder
				.withArgName( "port" )
	        	.hasArg()
	        	.isRequired()
	        	.withDescription(  "service port for veddriver" )
	        	.withType(Integer.class)
	        	.create(SERVER_PORT));

		options.addOption(OptionBuilder
				.withArgName( "hostname" )
	        	.hasArg()
	        	.withDescription(  "client hostname" )
	        	.create(CLIENT));

		options.addOption(OptionBuilder
			.withArgName( "port" )
        	.hasArg()
        	.isRequired()
        	.withDescription(  "client port to connect to veddriver" )
        	.withType(Integer.class)
        	.create(CLIENT_PORT));
	
		options.addOption(OptionBuilder
			.withArgName( "hostname" )
        	.hasArg()
        	.withDescription(  "host for the replication provider to connect" )
        	.create(EVENTPROCESSOR_HOST));

		options.addOption(OptionBuilder
			.withArgName( "port" )
        	.hasArg()
        	.isRequired()
        	.withDescription(  "port for the replication provider to connect" )
        	.withType(Integer.class)
        	.create(EVENTPROCESSOR_PORT));
	
		options.addOption(OptionBuilder
				.withDescription(  "prints events to the console" )
				.create(VERBOSE ));
		
		
		CommandLineParser parser = new PosixParser();
		
		CommandLine line = null;
		
		try{
			line = parser.parse( options, args);
			
			String server = optionValue(line, SERVER, LOCALHOST);
			String client = optionValue(line, CLIENT, LOCALHOST);
			
			_eventConfiguration = new EventConfiguration(
					line.getOptionValue(DATABASE),
					line.getOptionValue(USER_NAME),
					line.getOptionValue(PASSWORD),
					line.getOptionValue(LOGFILE),
					server,
					optionAsInt(line, SERVER_PORT),
					client,
					new FixedEventClientPortSelectionStrategy(optionAsInt(line, CLIENT_PORT)),
					line.getOptionValue(EVENTPROCESSOR_HOST), optionAsInt(line, EVENTPROCESSOR_PORT), line.hasOption(VERBOSE));
			
		} catch(MissingOptionException moex){
			System.out.println(moex.getMessage());
			HelpFormatter formatter = new HelpFormatter();
			formatter.printHelp( EventProcessorApplication.class.getName(), options );
			return false;
		} catch(ParseException pex){
			pex.printStackTrace();
			return false;
		}
		return true;
	}

	private int optionAsInt(CommandLine line, String optionName) {
		return Integer.parseInt(line.getOptionValue(optionName));
	}

	private String optionValue(CommandLine line, String optionName, String defaultValue) {
		return line.hasOption(optionName) ? line.getOptionValue(optionName) : defaultValue;
	}

}
