package com.db4o.objectManager.v2.connections;

import java.awt.*;
import java.io.*;

import com.db4o.*;
import com.db4o.ext.*;
import com.db4o.objectManager.v2.uiHelper.*;
import com.db4o.objectmanager.model.*;

/**
 * User: treeder
 * Date: Sep 17, 2006
 * Time: 10:42:31 PM
 */
public class ConnectionHelper {
	public static ObjectContainer connect(Component frame, Db4oConnectionSpec connectionSpec) throws Exception {
		if (connectionSpec instanceof Db4oFileConnectionSpec) {
			return connectToFile(frame, connectionSpec);
		} else if (connectionSpec instanceof Db4oSocketConnectionSpec) {
			return connectToServer((Db4oSocketConnectionSpec) connectionSpec);
		}
		throw new IllegalArgumentException("connectionSpec");
	}

	private static ObjectContainer connectToServer(Db4oSocketConnectionSpec spec) {
		return Db4o.openClient(spec.newConfiguration(), spec.getHost(), spec.getPort(), spec.getUser(), spec.getPassword());
	}

	private static ObjectContainer connectToFile(Component frame,
			Db4oConnectionSpec connectionSpec) throws Exception {
		try {
			assertFileExists(connectionSpec.getFullPath());
			return Db4o.openFile(connectionSpec.newConfiguration(), connectionSpec.getFullPath());
		} catch (DatabaseFileLockedException e) {
			OptionPaneHelper.showErrorMessage(frame, "Database file is locked. Another process must be using it.", "Database File Locked");
			throw e;
		} catch (Db4oException e) {
			// todo: finish this up after http://tracker.db4o.com/jira/browse/COR-234 is fixed
			if (e.getMessage().contains("Old database file format detected")) { // this is bad, would be nice to have a more concrete exception
				OptionPaneHelper.showConfirmWarning(frame, "Old database file format detected. Would you like to upgrade?\n" +
						"WARNING: This operation is irreversible and your application may not operate unless you update your db4o jar file to the latest version.", "Upgrade Database?");
			} else {
				throw e;
			}
		} catch (Exception e) {
			OptionPaneHelper.showErrorMessage(frame, "Could not open database! " + e.getMessage(), "Error Opening Database");
			throw e;
		}
		return null;
	}

	private static void assertFileExists(String fullPath)
			throws FileNotFoundException {
		File f = new File(fullPath);
		if (!f.exists() || f.isDirectory()) {
			throw new FileNotFoundException("File not found: " + f.getAbsolutePath());
		}
	}

	
}
