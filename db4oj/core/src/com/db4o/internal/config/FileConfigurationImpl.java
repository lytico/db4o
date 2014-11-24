/* Copyright (C) 2008  Versant Inc.   http://www.db4o.com */

package com.db4o.internal.config;

import java.io.*;

import com.db4o.config.*;
import com.db4o.ext.*;
import com.db4o.foundation.*;
import com.db4o.internal.*;
import com.db4o.io.*;

class FileConfigurationImpl implements FileConfiguration {

	private final Config4Impl _config;

	public FileConfigurationImpl(Config4Impl config) {
		_config = config;
	}

	public void blockSize(int bytes) {
		_config.blockSize(bytes);
	}

	public void databaseGrowthSize(int bytes) {
		_config.databaseGrowthSize(bytes);
	}

	public void disableCommitRecovery() {
		_config.disableCommitRecovery();
	}

	public FreespaceConfiguration freespace() {
		return _config.freespace();
	}

	public void generateUUIDs(ConfigScope setting) {
		_config.generateUUIDs(setting);
	}

	@Deprecated
	public void generateVersionNumbers(ConfigScope setting) {
		_config.generateVersionNumbers(setting);
	}

	public void generateCommitTimestamps(boolean setting) {
		_config.generateCommitTimestamps(setting);
	}

	public void storage(Storage factory) throws GlobalOnlyConfigException {
		_config.storage(factory);
	}

	public Storage storage() {
		return _config.storage();
	}

	public void lockDatabaseFile(boolean flag) {
		_config.lockDatabaseFile(flag);
	}

	public void reserveStorageSpace(long byteCount) throws DatabaseReadOnlyException, NotSupportedException {
		_config.reserveStorageSpace(byteCount);
	}

	public void blobPath(String path) throws IOException {
		_config.setBlobPath(path);
	}
	
	public void readOnly(boolean flag) {
		_config.readOnly(flag);
	}

	public void recoveryMode(boolean flag) {
		_config.recoveryMode(flag);
	}

	public void asynchronousSync(boolean flag) {
		_config.asynchronousSync(flag);
		
	}

	@Override
	public void maximumDatabaseFileSize(long bytes) {
		_config.maximumDatabaseFileSize(bytes);
	}
}
