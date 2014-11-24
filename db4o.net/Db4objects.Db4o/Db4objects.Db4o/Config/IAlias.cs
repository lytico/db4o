/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

namespace Db4objects.Db4o.Config
{
	/// <summary>
	/// Implement this interface when implementing special custom Aliases
	/// for classes, packages or namespaces.
	/// 
	/// </summary>
	/// <remarks>
	/// Implement this interface when implementing special custom Aliases
	/// for classes, packages or namespaces.
	/// <br/><br/>Aliases can be used to persist classes in the running
	/// application to different persistent classes in a database file
	/// or on a db4o server.
	/// <br/><br/>Two simple Alias implementations are supplied along with
	/// db4o:<br/>
	/// -
	/// <see cref="TypeAlias">TypeAlias</see>
	/// provides an #equals() resolver to match
	/// names directly.<br/>
	/// -
	/// <see cref="WildcardAlias">WildcardAlias</see>
	/// allows simple pattern matching
	/// with one single '*' wildcard character.<br/>
	/// <br/>
	/// It is possible to create
	/// own complex
	/// <see cref="IAlias">IAlias</see>
	/// constructs by creating own resolvers
	/// that implement the
	/// <see cref="IAlias">IAlias</see>
	/// interface.
	/// <br/><br/>
	/// Examples of concrete usecases:
	/// <br/><br/>
	/// <code>
	/// <b>// Creating an Alias for a single class</b><br/>
	/// ICommonConfiguration.AddAlias(<br/>
	///   new TypeAlias("Tutorial.Pilot", "Tutorial.Driver"));<br/>
	/// <br/><br/>
	/// <b>// Accessing a Java package from a .NET assembly </b><br/>
	/// ICommonConfiguration.AddAlias(<br/>
	///   new WildcardAlias(<br/>
	///     "com.f1.*",<br/>
	///     "Tutorial.F1.*, Tutorial"));<br/>
	/// <br/><br/>
	/// <b>// Using a different local .NET assembly</b><br/>
	/// ICommonConfiguration.AddAlias(<br/>
	///   new WildcardAlias(<br/>
	///     "Tutorial.F1.*, F1Race",<br/>
	///     "Tutorial.F1.*, Tutorial"));<br/>
	/// <br/><br/>
	/// </code>
	/// <br/><br/>Aliases that translate the persistent name of a class to
	/// a name that already exists as a persistent name in the database
	/// (or on the server) are not permitted and will throw an exception
	/// when the database file is opened.
	/// <br/><br/>Aliases should be configured before opening a database file
	/// or connecting to a server.
	/// 
	/// </remarks>
	public interface IAlias
	{
		/// <summary>return the stored name for a runtime name or null if not handled.</summary>
		/// <remarks>return the stored name for a runtime name or null if not handled.</remarks>
		string ResolveRuntimeName(string runtimeTypeName);

		/// <summary>return the runtime name for a stored name or null if not handled.</summary>
		/// <remarks>return the runtime name for a stored name or null if not handled.</remarks>
		string ResolveStoredName(string storedTypeName);
	}
}
