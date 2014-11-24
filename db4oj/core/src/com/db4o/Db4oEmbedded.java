package com.db4o;

import com.db4o.config.*;
import com.db4o.ext.*;
import com.db4o.foundation.*;
import com.db4o.internal.*;
import com.db4o.internal.config.*;

/**
 * Factory class to open db4o instances in embedded mode.
 * 
 * <br><br>
 * For client server scenarious please use the com.db4o.cs.Db4oClientServer factory from the db4o-cs-java.jar
 * for methods to open db4o servers and db4o clients.
 * @since 7.5
 * 
 */
public class Db4oEmbedded {

	/**
	 * Creates a fresh {@link EmbeddedConfiguration} instance.
	 * 
	 * @return a fresh, independent configuration with all options set to their default values
	 */
	@SuppressWarnings("deprecation")
	public static EmbeddedConfiguration newConfiguration() {
		return new EmbeddedConfigurationImpl(Db4o.newConfiguration());
	}

	/**
	 * Opens an {@link ObjectContainer ObjectContainer}
	 * on the specified database file for local use.
	 * <br><br>
     * Database files can only be accessed for access from one process
	 * at one time. Subsequent attempts to open the same file will result in
	 * a {@link DatabaseFileLockedException}. <br/>
     * For multiple object containers against the same database
     * use the {@link com.db4o.ObjectContainer}.{@link ObjectContainer#ext() ext()}.{@link com.db4o.ext.ExtObjectContainer#openSession() openSession()}() method.
     * Or use the client server mode from the db4o-cs-java.jar
     *
     *
     * <br/><br/>
	 * @param config a {@link EmbeddedConfiguration} instance to be obtained via {@link #newConfiguration}
	 * @param databaseFileName an absolute or relative path to the database file
	 * @return an open {@link ObjectContainer ObjectContainer}
	 * @throws Db4oIOException I/O operation failed or was unexpectedly interrupted.
	 * @throws DatabaseFileLockedException the required database file is locked by 
	 * another process.
	 * @throws IncompatibleFileFormatException runtime 
	 * {@link com.db4o.config.EmbeddedConfiguration configuration} is not compatible
	 * with the configuration of the database file. 
	 * @throws OldFormatException open operation failed because the database file
	 * is in old format and {@link com.db4o.config.CommonConfiguration#allowVersionUpdates(boolean)}
	 * is set to false.
	 * @throws DatabaseReadOnlyException database was configured as read-only.
	 */
	public static final EmbeddedObjectContainer openFile(EmbeddedConfiguration config,
			String databaseFileName) throws Db4oIOException,
			DatabaseFileLockedException, IncompatibleFileFormatException,
			OldFormatException, DatabaseReadOnlyException {
		if (null == config) {
			throw new ArgumentNullException();
		}
		return ObjectContainerFactory.openObjectContainer(config, databaseFileName);
	}
	
	/**
	 * Same (from java) as calling {@link #openFile(EmbeddedConfiguration, String)} with a fresh configuration ({@link #newConfiguration()}).
	 * @param databaseFileName an absolute or relative path to the database file
	 * @see #openFile(EmbeddedConfiguration, String)
	 */
	public static final EmbeddedObjectContainer openFile(String databaseFileName)
		throws Db4oIOException, DatabaseFileLockedException, IncompatibleFileFormatException,
			OldFormatException, DatabaseReadOnlyException {
		return openFile(newConfiguration(), databaseFileName);
	}

}
