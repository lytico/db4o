/* Copyright (C) 2004 - 2006  db4objects Inc.  http://www.db4o.com */

package com.db4o.config;

/**
 * Implement this interface when implementing special custom Aliases
 * for classes, packages or namespaces.
 * <br><br>Aliases can be used to persist classes in the running
 * application to different persistent classes in a database file
 * or on a db4o server.
 * <br><br>Two simple Alias implementations are supplied along with 
 * db4o:<br>
 * - {@link TypeAlias} provides an #equals() resolver to match
 * names directly.<br>
 * - {@link WildcardAlias} allows simple pattern matching
 * with one single '*' wildcard character.<br>
 * <br>
 * It is possible to create
 * own complex {@link Alias} constructs by creating own resolvers
 * that implement the {@link Alias} interface.
 * <br><br>
 * Four examples of concrete usecases:
 * <br><br>
 * <code>
 * <b>// Creating an Alias for a single class</b><br> 
 * Db4o.configure().addAlias(<br>
 * &#160;&#160;new TypeAlias("com.f1.Pilot", "com.f1.Driver"));<br>
 * <br><br>
 * <b>// Accessing a .NET assembly from a Java package</b><br> 
 * Db4o.configure().addAlias(<br>
 * &#160;&#160;new WildcardAlias(<br>
 * &#160;&#160;&#160;&#160;"com.f1.*, F1RaceAssembly",<br>
 * &#160;&#160;&#160;&#160;"com.f1.*"));<br>
 * <br><br>
 * <b>// Using a different local .NET assembly</b><br> 
 * Db4o.configure().addAlias(<br>
 * &#160;&#160;new WildcardAlias(<br>
 * &#160;&#160;&#160;&#160;"com.f1.*, F1RaceAssembly",<br>
 * &#160;&#160;&#160;&#160;"com.f1.*, RaceClient"));<br>
 * <br><br>
 * <b>// Mapping a Java package onto another</b><br> 
 * Db4o.configure().addAlias(<br>
 * &#160;&#160;new WildcardAlias(<br>
 * &#160;&#160;&#160;&#160;"com.f1.*",<br>
 * &#160;&#160;&#160;&#160;"com.f1.client*"));<br></code>
 * <br><br>Aliases that translate the persistent name of a class to 
 * a name that already exists as a persistent name in the database 
 * (or on the server) are not permitted and will throw an exception
 * when the database file is opened.
 * <br><br>Aliases should be configured before opening a database file
 * or connecting to a server.
 */
public interface Alias {
    
    /**
     * return the translated name or null if not handled. 
     */
	public String resolve(String runtimeType);

}
