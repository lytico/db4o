package com.db4o.cs;

import com.db4o.*;
import com.db4o.cs.config.*;
import com.db4o.cs.internal.config.*;
import com.db4o.internal.*;

/**
 * Factory class to open db4o servers and to connect db4o clients
 * to them.
 * <br><br>
 * <b>Note:<br>
 * This class is made available in db4o-X.x-cs-java.jar / Db4objects.Db4o.CS.dll</b>
 * @since 7.5
 */
public class Db4oClientServer {

	public final static int ARBITRARY_PORT = -1;
	
	/**
	 * creates a new {@link ServerConfiguration}
	 */
	public static ServerConfiguration newServerConfiguration() {
		return new ServerConfigurationImpl(newLegacyConfig());
	}

	/**
     * opens an {@link ObjectServer ObjectServer}
	 * on the specified database file and port.
     * <br><br>
	 * @param config a custom {@link ServerConfiguration} instance to be obtained via {@link #newServerConfiguration()}
     * @param databaseFileName an absolute or relative path to the database file
     * @param port the port to be used or 0 if the server should not open a port, specify a value < 0 if an arbitrary free port should be chosen - see {@link com.db4o.ext.ExtObjectServer#port()}.
	 * @return an {@link ObjectServer ObjectServer} listening
	 * on the specified port.
     * @throws com.db4o.ext.Db4oIOException I/O operation failed or was unexpectedly interrupted.
     * @throws com.db4o.ext.DatabaseFileLockedException the required database file is locked by
     * another process.
     * @throws com.db4o.ext.IncompatibleFileFormatException runtime
     * @throws com.db4o.ext.OldFormatException open operation failed because the database file
     * is in old format and {@link com.db4o.config.Configuration#allowVersionUpdates(boolean)} 
     * is set to false.
     * @throws com.db4o.ext.DatabaseReadOnlyException database was configured as read-only.
	 */
	public static ObjectServer openServer(ServerConfiguration config,
			String databaseFileName, int port) {
		return config.networking().clientServerFactory().openServer(config, databaseFileName, port);
	}

	/**
	 * opens a db4o server with a fresh server configuration.
	 * 
	 * @see #openServer(ServerConfiguration, String, int)
	 * @see #newServerConfiguration()
	 */
	public static ObjectServer openServer(String databaseFileName, int port) {
		return openServer(newServerConfiguration(), databaseFileName, port);
	}

	/**
	 * opens a db4o client instance with the specified configuration.
	 * @param config the configuration to be used
	 * @param host the host name of the server that is to be connected to
	 * @param port the server port to connect to
	 * @param user the username for authentication
	 * @param password the password for authentication
	 * @see #openServer(ServerConfiguration, String, int)
	 * @see ObjectServer#grantAccess(String, String)
	 * @throws IllegalArgumentException if the configuration passed in has already been used.
	 */
	public static ObjectContainer openClient(ClientConfiguration config,
			String host, int port, String user, String password) {
		return config.networking().clientServerFactory().openClient(config, host, port, user, password);
	}
	
	/**
	 * opens a db4o client instance with a fresh client configuration.
	 * 
	 * @see #openClient(ClientConfiguration, String, int, String, String)
	 * @see #newClientConfiguration()
	 */
	public static ObjectContainer openClient(String host, int port, String user, String password) {
		return openClient(newClientConfiguration(), host, port, user, password);
	}
	
	/**
	 * creates a new {@link ClientConfiguration} 
	 */
	public static ClientConfiguration newClientConfiguration() {
		final Config4Impl legacy = newLegacyConfig();
		return new ClientConfigurationImpl((Config4Impl) legacy);
	}
	
    @SuppressWarnings("deprecation")
    private static Config4Impl newLegacyConfig() {
		return (Config4Impl) Db4o.newConfiguration();
	}
}
