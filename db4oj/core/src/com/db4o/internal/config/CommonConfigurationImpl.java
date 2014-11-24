/* Copyright (C) 2008  Versant Inc.   http://www.db4o.com */

package com.db4o.internal.config;

import java.io.*;

import com.db4o.config.*;
import com.db4o.config.encoding.*;
import com.db4o.diagnostic.*;
import com.db4o.internal.*;
import com.db4o.reflect.*;
import com.db4o.typehandlers.*;

public class CommonConfigurationImpl implements CommonConfiguration {

	private final Config4Impl _config;

	public CommonConfigurationImpl(Config4Impl config) {
		_config = config;
	}

	public void activationDepth(int depth) {
		_config.activationDepth(depth);
	}

	public int activationDepth() {
		return _config.activationDepth();
	}

	public void add(ConfigurationItem configurationItem) {
		_config.add(configurationItem);
	}
	
	public void addAlias(Alias alias) {
		_config.addAlias(alias);
	}

	public void removeAlias(Alias alias) {
		_config.removeAlias(alias);
	}

	public void allowVersionUpdates(boolean flag) {
		_config.allowVersionUpdates(flag);
	}

	public void automaticShutDown(boolean flag) {
		_config.automaticShutDown(flag);
	}

	public void bTreeNodeSize(int size) {
		_config.bTreeNodeSize(size);
	}

	public void callbacks(boolean flag) {
		_config.callbacks(flag);
	}

	public void callbackMode(CallBackMode mode) {
		_config.callbackMode(mode);
	}

	public void callConstructors(boolean flag) {
		_config.callConstructors(flag);
	}

	public void detectSchemaChanges(boolean flag) {
		_config.detectSchemaChanges(flag);
	}

	public DiagnosticConfiguration diagnostic() {
		return _config.diagnostic();
	}

	public void exceptionsOnNotStorable(boolean flag) {
		_config.exceptionsOnNotStorable(flag);
	}

	public void internStrings(boolean flag) {
		_config.internStrings(flag);
	}

	public void markTransient(String attributeName) {
		_config.markTransient(attributeName);
	}

	public void messageLevel(int level) {
		_config.messageLevel(level);
	}

	public ObjectClass objectClass(Object clazz) {
		return _config.objectClass(clazz);
	}

	public void optimizeNativeQueries(boolean optimizeNQ) {
		_config.optimizeNativeQueries(optimizeNQ);
	}

	public boolean optimizeNativeQueries() {
		return _config.optimizeNativeQueries();
	}

	public QueryConfiguration queries() {
		return _config.queries();
	}

	public void reflectWith(Reflector reflector) {
		_config.reflectWith(reflector);
	}
	
	@SuppressWarnings("deprecation")
	public void outStream(PrintStream outStream) {
		_config.setOut(outStream);
	}

	public void stringEncoding(StringEncoding encoding) {
		_config.stringEncoding(encoding);
	}

	public void testConstructors(boolean flag) {
		_config.testConstructors(flag);
	}

	public void updateDepth(int depth) {
		_config.updateDepth(depth);
	}

	public void weakReferences(boolean flag) {
		_config.weakReferences(flag);
	}

	public void weakReferenceCollectionInterval(int milliseconds) {
		_config.weakReferenceCollectionInterval(milliseconds);
	}

	public void registerTypeHandler(TypeHandlerPredicate predicate, TypeHandler4 typeHandler) {
		_config.registerTypeHandler(predicate, typeHandler);
	}

	public EnvironmentConfiguration environment() {
		return new EnvironmentConfiguration() {
			public void add(Object service) {
				_config.environmentContributions().add(service);
			}
		};
	}

	public void nameProvider(NameProvider provider) {
		_config.nameProvider(provider);
	}

	public void maxStackDepth(int maxStackDepth) {
		_config.maxStackDepth(maxStackDepth);
	}

	public int maxStackDepth() {
		return _config.maxStackDepth();
	}

}
