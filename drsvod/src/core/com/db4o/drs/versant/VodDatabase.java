/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.drs.versant;

import java.io.*;
import java.net.*;
import java.util.*;

import javax.jdo.*;

import com.db4o.drs.inside.*;
import com.db4o.drs.versant.eventprocessor.*;
import com.db4o.drs.versant.eventprocessor.EventProcessorApplication.*;
import com.db4o.drs.versant.ipc.*;
import com.db4o.drs.versant.ipc.tcp.*;
import com.db4o.drs.versant.metadata.*;
import com.db4o.foundation.*;
import com.db4o.internal.*;
import com.db4o.util.IOServices.ProcessRunner;
import com.db4o.util.*;
import com.versant.odbms.*;
import com.versant.util.*;

/**
 * Helper class for VOD database. 
 */
public class VodDatabase {
	
	private static final int EVENT_PROCESSOR_PORT = 4088;

	private static final long STARTUP_TIMEOUT = DrsDebug.timeout(10000);

	public static final String USER_NAME_KEY = "javax.jdo.option.ConnectionUserName";

	public static final String PASSWORD_KEY = "javax.jdo.option.ConnectionPassword";

	public static final String CONNECTION_URL_KEY = "javax.jdo.option.ConnectionURL";
	
	private static final String JDO_METADATA_KEY = "versant.metadata."; 

	public static final String VED_DRIVER = "veddriver";
	
	private static final int DEFAULT_PORT = 5019;

	private final Properties _properties;
	
	private PersistenceManagerFactory _persistenceManagerFactory;
	
	private DatastoreManagerFactory _datastoreManagerFactory;
	
	private static int nextPort = 4120;
	
	private static VodEventDriver _eventDriver;
	
	private EventConfiguration _eventConfiguration;
	
	private Stoppable _eventProcessor;

	public VodDatabase(EventConfiguration eventConfiguration, Properties properties) {
		_eventConfiguration = eventConfiguration;
		_properties = properties;
		addDefaultProperties();
	}
	
	public VodDatabase(String name, Properties properties){
		this(new EventConfiguration(
				name, 
				properties.getProperty(USER_NAME_KEY), 
				properties.getProperty(PASSWORD_KEY), 
				name + "event.log", 
				"localhost", 
				nextPort(), 
				"localhost",
				new IncrementingEventClientPortSelectionStrategy(nextPort()), 
				"localhost", EVENT_PROCESSOR_PORT, DrsDebug.verbose),
				properties);
	}
	
	public VodDatabase(String name, String userName, String password){
		this(name, createProperties(userName, password));
	}
	
	private static Properties createProperties(String userName, String password){
		Properties properties = new Properties();
		properties.put(VodDatabase.USER_NAME_KEY, userName);
		properties.put(VodDatabase.PASSWORD_KEY, password);
		return properties;
	}
	
    public VodDatabase (PersistenceManagerFactory pmf){
    	this(extractName(pmf), pmf.getProperties());
    }
    
    public VodDatabase(EventConfiguration eventConfiguration) {
    	this(eventConfiguration, createProperties(eventConfiguration.userName, eventConfiguration.password));
	}

	public void configureEventProcessor(String host, int port){
    	eventConfiguration().eventProcessorHost = host;
    	eventConfiguration().eventProcessorPort = port;
    }
    
    private static String extractName(PersistenceManagerFactory pmf){
    	Properties properties = pmf.getProperties();
    	String connectionURL = properties.getProperty("javax.jdo.option.ConnectionURL");
    	if(isEmpty(connectionURL) || notVersantDBConnection(connectionURL)){
    		throw new IllegalArgumentException("Requires a valid database connection URL for VOD");
    	}
    	return extractName(connectionURL);
    }

    private static String extractName(String connectionURL) {
        return connectionURL.substring("versant:".length()).split("@")[0];
    }

    private static boolean notVersantDBConnection(String connectionURL) {
        return !connectionURL.startsWith("versant");
    }

    private static boolean isEmpty(String connectionURL) {
        return null==connectionURL || 0==connectionURL.trim().length();
    }


	private void addDefaultProperties(){
		
		addPropertyIfNotExists("versant.l2CacheEnabled", "false");
		
		// addPropertyIfNotExists("versant.retainConnectionInOptTx", "true");
		
		/**
		 * Setting the following property produces fully qualified classnames.
		 * VOD also uses fully qualified classnames, if they are fully qualified
		 * in the .jdo file.
		 */
		// addPropertyIfNotExists("versant.vdsNamingPolicy", "none");
		
		addPropertyIfNotExists(CONNECTION_URL_KEY, "versant:" + name() + "@localhost");
		addPropertyIfNotExists("javax.jdo.PersistenceManagerFactoryClass","com.versant.core.jdo.BootstrapPMF");
		addJdoMetaDataFiles();
	}
	
	public String name() {
		return eventConfiguration().databaseName;
	}
	
	public void addPropertyIfNotExists(String key, String value) {
		if(_properties.containsKey(key)){
			return;
		}
		_properties.setProperty(key, value);
	}

	public boolean dbExists(){
		DBListInfo[] dbList = DBUtility.dbList();
		for (DBListInfo dbListInfo : dbList) {
			String name = dbListInfo.getDBName();
			
			int indexOfHostName = name.indexOf("@");
			if(indexOfHostName >= 0){
				name = name.substring(0, indexOfHostName);
			}
			
			if(name().equals(name)){
				return true;
			}
		}
		return false;
	}
	
	public void removeDb() {
		if(! dbExists()){
			return;
		}
		Properties props = new Properties();
		props.put ("-f","");
		try{
			DBUtility.stopDB(name(),props);
		} catch(Exception ex){
			ex.printStackTrace();
		}
		props.put("-rmdir","");
		try{
			DBUtility.removeDB(name(), props);
		} catch (Exception ex){
			ex.printStackTrace();
		}
	}
	
	public void produceDb(){
		if(dbExists()){
			return;
		}
		Properties p=new Properties();
		DBUtility.makeDB(name(),p);
		DBUtility.createDB(name());
	}

	public PersistenceManagerFactory persistenceManagerFactory(){
		if(_persistenceManagerFactory == null){
			_persistenceManagerFactory = JDOHelper.getPersistenceManagerFactory(_properties);
			
		}
		return _persistenceManagerFactory;
	}
	
	public void enhance() {
		String tempFileName = Path4.getTempFileName();
		File tempFile = new File(tempFileName);
		try{
			FileOutputStream out = new FileOutputStream(tempFile);
			if(DrsDebug.verbose){
				_properties.store(System.err, null);
			}
			_properties.store(out, null);
			out.close();
			
			String[] args = new String[]{tempFile.getAbsolutePath()};
			
			ProcessResult processResult = JavaServices.java("com.db4o.drs.versant.VodEnhancer", args);
			if(DrsDebug.verbose){
				System.out.println(processResult);
			}
		} catch(IOException ioe){
			throw new RuntimeException(ioe);
		} catch (InterruptedException e) {
			throw new RuntimeException(e);
		} finally {
			tempFile.delete();
		}
	}

	public DatastoreManager createDatastoreManager() {
		return datastoreManagerFactory().getDatastoreManager();
	}
	
	public synchronized DatastoreManagerFactory datastoreManagerFactory(){
		if(_datastoreManagerFactory == null){
			DatabaseImp db = new DatabaseImp(name(), host(), port());
			ConnectionInfo con = new ConnectionInfo(db, userName(), passWord());
			ConnectionProperties connectionProperties = new ConnectionProperties();
			_datastoreManagerFactory = new DatastoreManagerFactory(con, connectionProperties);
		}
		return _datastoreManagerFactory;
	}
	
	public String userName(){
        return _properties.getProperty(USER_NAME_KEY);
	}
	
	public String passWord(){
		return _properties.getProperty(PASSWORD_KEY);
	}
	
	private int port(){
		int port = connectionUrl().getPort();
		if(port != -1){
			return port;
		}
		return DEFAULT_PORT;
	}
	
	private String host(){
		return connectionUrl().getHost();
	}

	private String connectionUrlAsString() {
		String connectionURL = _properties.getProperty(CONNECTION_URL_KEY);
		if(connectionURL == null){
			addDefaultProperties();
		}
		connectionURL = _properties.getProperty(CONNECTION_URL_KEY);
		return connectionURL.replaceAll("versant:", "http:");
	}
	
	private URL connectionUrl(){
		try {
			return new URL(connectionUrlAsString());
		} catch (MalformedURLException e) {
			throw new RuntimeException(e);
		}
	}
	
	private void addJdoMetaDataFiles() {
		addJdoMetaDataFile(CommitTimestamp.class.getPackage());
	}

	private String jdoDefinitionsForPackage(Package p) {
		return p.getName().replace('.', '/')+"/package.jdo";
	}

	public void addJdoMetaDataFile(Package p) {
		addJdoMetaDataFile(jdoDefinitionsForPackage(p));
	}
	
	public void addJdoMetaDataFile(String fileName) {
		
		final int maxMetadataEntries = 1000;
		final int quitSearchingAfterGap = 5;
		final int notSet = -1;
		
		int freeEntry = notSet;
		int lastOccupied = notSet;
		
		for (int i = 0; i < maxMetadataEntries; i++) {
			String property = _properties.getProperty(JDO_METADATA_KEY + i);
			if(property == null){
				if(freeEntry == notSet){
					freeEntry = i;
				}
				if(i - lastOccupied > quitSearchingAfterGap){
					break;
				}
			} else {
				lastOccupied = i;
				if(fileName.equals(property)){
					return;
				}
			}
		}
		_properties.setProperty(JDO_METADATA_KEY + freeEntry, fileName);
	}
	
	public void startEventDriver() {
		if(_eventDriver != null){
			return;
		}
//		int serverPort = nextPort();
//		EventClientPortSelectionStrategy clientPortStrategy = new IncrementingEventClientPortSelectionStrategy(nextPort());
//		_eventConfiguration = new EventConfiguration(_name, _name + "event.log",  "localhost", serverPort, "localhost", clientPortStrategy, _eventProcessorHost, _eventProcessorPort, DrsDebug.verbose);
		_eventDriver = new VodEventDriver(_eventConfiguration);
		boolean started = _eventDriver.start();
		if(! started ){
			_eventDriver.printStartupFailure();
			_eventDriver = null;
			throw new IllegalStateException();
		}
	}

	private static int nextPort() {
		return nextPort++;
	}
	
	public void createEventSchema() {
		VodJvi jvi = new VodJvi(this);
		jvi.createEventSchema();
	}
	
	@Override
	public String toString() {
		return "VOD " + name();
	}

	public void stopEventDriver() {
		if(_eventDriver == null){
			return;
		}
		_eventDriver.stop();
		_eventDriver = null;
	}
	
	public void startEventProcessor(){
		startEventProcessor(DrsDebug.runEventListenerEmbedded);
	}
	
	public void startEventProcessor(boolean embedded){
		if(_eventProcessor != null){
			throw new IllegalStateException();
		}
		
		if (embedded) {
			startEventProcessorInSameProcess();
			return;
		}
		
		try {
			startEventProcessorInSeparateProcess();
		} catch (IOException e) {
			throw new RuntimeException(e);
		}
	}

	private void startEventProcessorInSameProcess() {
		_eventProcessor = new EventProcessorEmbedded(_eventConfiguration);
	}
	
	public void stopEventProcessor(){
		if(_eventProcessor == null){
			return;
		}
		_eventProcessor.stop();
		_eventProcessor = null;
	}
	
	private void startEventProcessorInSeparateProcess() throws IOException {
		
		if (_eventProcessor != null) {
			throw new IllegalStateException();
		}
		
		List<String> arguments = new ArrayList<String>();
		
		addArgument(arguments, Arguments.DATABASE, name());
		addArgument(arguments, Arguments.LOGFILE, _eventConfiguration.logFileName);
		addArgument(arguments, Arguments.SERVER_PORT, _eventConfiguration.serverPort);
		addArgument(arguments, Arguments.CLIENT_PORT, _eventConfiguration.clientPort());
		addArgument(arguments, Arguments.EVENTPROCESSOR_PORT, _eventConfiguration.eventProcessorPort);
		if (DrsDebug.verbose) {
			addArgument(arguments, Arguments.VERBOSE);
		}
		
		String[] argumentsAsString = new String[arguments.size()];
		argumentsAsString = arguments.toArray(argumentsAsString);
		
		final ProcessRunner eventListenerProcess = JavaServices.startJava(EventProcessorApplication.class.getName(), argumentsAsString);
//		final ProcessRunner eventListenerProcess = JavaServices.startJavaInDebug(9898, EventProcessorApplication.class.getName(), argumentsAsString);
//		eventListenerProcess.waitFor(_name, 10000);
		
		final ClientChannelControl control = TcpCommunicationNetwork.newClient(this);
		final BlockingQueue<String> barrier = new BlockingQueue<String>();
		AbstractEventProcessorListener listener = new AbstractEventProcessorListener() {
			@Override
			public void ready() {
				barrier.add("ready!!!");
			}
			
		};
		control.sync().addListener(listener);
		
		if (barrier.next(STARTUP_TIMEOUT) == null) {;
			throw new IllegalStateException(ReflectPlatform.simpleName(EventProcessorImpl.class) + " still not running after "+STARTUP_TIMEOUT+"ms");
		}
		
		control.async().removeListener(listener);
		
		_eventProcessor = new Stoppable() {
			
			public void stop() {
				
				control.async().stop();
				control.stop();
				
				try {
					eventListenerProcess.waitFor();
				} catch (InterruptedException e) {
					throw new RuntimeException(e);
				}
			}
		};
	}
	
	private static void addArgument(List<String> arguments, String argumentName, int argumentValue) {
		addArgument(arguments, argumentName, String.valueOf(argumentValue));
	}

	private static void addArgument(List<String> arguments, String argumentName, String argumentValue) {
		addArgument(arguments, argumentName);
		arguments.add(argumentValue);
	}
	
	private static void addArgument(List<String> arguments, String argumentName) {
		arguments.add("-" + argumentName);
	}
	
	public EventConfiguration eventConfiguration(){
		return _eventConfiguration;
	}
	
	public String eventProcessorHost() {
		return eventConfiguration().eventProcessorHost;
	}
	
	public int eventProcessorPort() {
		return eventConfiguration().eventProcessorPort;
	}

	public void addUser() {
		DBUtility.addDBUser(name(), userName(), passWord(), "rw");
	}
	
}
