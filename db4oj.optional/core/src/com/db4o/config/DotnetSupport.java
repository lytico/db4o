/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
package com.db4o.config;

import com.db4o.*;
import com.db4o.ext.*;
import com.db4o.internal.*;
import com.db4o.internal.handlers.*;

/**
 * Adds the basic configuration settings required to access a
 * .net generated database from java.
 * 
 * The configuration only makes sure that database files can be
 * successfully open and things like UUIDs can be successfully
 * retrieved.
 * @deprecated Since 8.0
 * @sharpen.ignore
 */
public class DotnetSupport implements ConfigurationItem {

	private final boolean _addCSSupport;
	
	/**
	 * @deprecated Use the constructor with the boolean parameter to specify if 
	 * client/server support is desired.
	 */
	public DotnetSupport() {
		_addCSSupport = false;	
	}
	
	/**
	 * @param addCSSupport true if mappings required for Client/Server 
	 *                     support should be included also.
	 *                     
	 * @deprecated Since 8.0
	 */
	public DotnetSupport(boolean addCSSupport) {
		_addCSSupport = addCSSupport;
	}

	public void prepare(Configuration config) {
		config.addAlias(new WildcardAlias("Db4objects.Db4o.Ext.*, Db4objects.Db4o", "com.db4o.ext.*"));		
		config.addAlias(new TypeAlias("Db4objects.Db4o.StaticField, Db4objects.Db4o", StaticField.class.getName()));
		config.addAlias(new TypeAlias("Db4objects.Db4o.StaticClass, Db4objects.Db4o", StaticClass.class.getName()));
		
		if (_addCSSupport) {
			ConfigurationItem dotNetCS;
			try {
				dotNetCS = (ConfigurationItem) Class.forName("com.db4o.cs.internal.config.DotNetSupportClientServer").newInstance();
			} catch (Exception e) {
				throw new Db4oException(e);
			} 
			dotNetCS.prepare(config);
		}
	}
	
	public void apply(InternalObjectContainer container) {
		NetTypeHandler[] handlers = Platform4.jdk().netTypes(container.reflector());
		for (int netTypeIdx = 0; netTypeIdx < handlers.length; netTypeIdx++) {
			NetTypeHandler handler = handlers[netTypeIdx];
			container.handlers().registerNetTypeHandler(handler);
		}
	}
}
