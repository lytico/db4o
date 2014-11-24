/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.IO;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Config.Encoding;
using Db4objects.Db4o.Diagnostic;
using Db4objects.Db4o.Reflect;
using Db4objects.Db4o.Typehandlers;

namespace Db4objects.Db4o.Config
{
	/// <summary>
	/// Common configuration methods, applicable for
	/// embedded, client and server use of db4o.<br /><br />
	/// In Client/Server use it is good practice to configure the
	/// client and the server in exactly the same way.
	/// </summary>
	/// <remarks>
	/// Common configuration methods, applicable for
	/// embedded, client and server use of db4o.<br /><br />
	/// In Client/Server use it is good practice to configure the
	/// client and the server in exactly the same way.
	/// </remarks>
	/// <since>7.5</since>
	public interface ICommonConfiguration
	{
		/// <summary>adds a new Alias for a class, namespace or package.</summary>
		/// <remarks>
		/// adds a new Alias for a class, namespace or package.
		/// <br /><br />Aliases can be used to persist classes in the running
		/// application to different persistent classes in a database file
		/// or on a db4o server.
		/// <br /><br />Two simple Alias implementations are supplied along with
		/// db4o:<br />
		/// -
		/// <see cref="TypeAlias">TypeAlias</see>
		/// provides an #equals() resolver to match
		/// names directly.<br />
		/// -
		/// <see cref="WildcardAlias">WildcardAlias</see>
		/// allows simple pattern matching
		/// with one single '*' wildcard character.<br />
		/// <br />
		/// It is possible to create
		/// own complex
		/// <see cref="IAlias">IAlias</see>
		/// constructs by creating own resolvers
		/// that implement the
		/// <see cref="IAlias">IAlias</see>
		/// interface.
		/// <br /><br />
		/// Examples of concrete usecases:
		/// <br /><br />
		/// <code>
		/// <b>// Creating an Alias for a single class</b><br />
		/// EmbeddedConfiguration config = Db4oEmbedded.newConfiguration();
		/// config.common().addAlias(<br />
		/// &#160;&#160;new TypeAlias("com.f1.Pilot", "com.f1.Driver"));<br />
		/// <br /><br />
		/// <b>// Mapping a Java package onto another</b><br />
		/// config.common().addAlias(<br />
		/// &#160;&#160;new WildcardAlias(<br />
		/// &#160;&#160;&#160;&#160;"com.f1.*",<br />
		/// &#160;&#160;&#160;&#160;"com.f1.client*"));<br /></code>
		/// <br /><br />Aliases that translate the persistent name of a class to
		/// a name that already exists as a persistent name in the database
		/// (or on the server) are not permitted and will throw an exception
		/// when the database file is opened.
		/// <br /><br />Aliases should be configured before opening a database file
		/// or connecting to a server.<br /><br />
		/// In client/server environment it is good practice to configure the
		/// client and the server in exactly the same way.
		/// </remarks>
		void AddAlias(IAlias alias);

		/// <summary>
		/// Removes an alias previously added with
		/// <see cref="AddAlias(IAlias)">AddAlias(IAlias)</see>
		/// .
		/// </summary>
		/// <param name="alias">the alias to remove</param>
		void RemoveAlias(IAlias alias);

		/// <summary>sets the activation depth to the specified value.</summary>
		/// <remarks>
		/// sets the activation depth to the specified value.
		/// <br/><br/><b>Why activation?</b><br/>
		/// When objects are instantiated from the database, the instantiation of member
		/// objects needs to be limited to a certain depth. Otherwise a single object
		/// could lead to loading the complete database into memory, if all objects where
		/// reachable from a single root object.<br/><br/>
		/// db4o uses the concept "depth", the number of field-to-field hops an object
		/// is away from another object. <b>The preconfigured "activation depth" db4o uses
		/// in the default setting is 5.</b>
		/// <br/><br/>Whenever an application iterates through the
		/// <see cref="Db4objects.Db4o.IObjectSet">IObjectSet</see>
		/// of a query result, the result objects
		/// will be activated to the configured activation depth.<br/><br/>
		/// A concrete example with the preconfigured activation depth of 5:<br/>
		/// <pre>
		/// // Object foo is the result of a query, it is delivered by the ObjectSet
		/// object foo = objectSet.Next();</pre>
		/// foo.member1.member2.member3.member4.member5 will be a valid object<br/>
		/// foo, member1, member2, member3 and member4 will be activated<br/>
		/// member5 will be deactivated, all of it's members will be null<br/>
		/// member5 can be activated at any time by calling
		/// <see cref="Db4objects.Db4o.IObjectContainer.Activate">IObjectContainer.Activate(member5, depth)
		/// </see>
		/// .
		/// <br/><br/>
		/// Note that raising the global activation depth will consume more memory and
		/// have negative effects on the performance of first-time retrievals. Lowering
		/// the global activation depth needs more individual activation work but can
		/// increase performance of queries.<br/><br/>
		/// <see cref="Db4objects.Db4o.IObjectContainer.Deactivate">IObjectContainer.Deactivate(object, depth)
		/// </see>
		/// can be used to manually free memory by deactivating objects.<br/><br/>
		/// In client/server environment it is good practice to configure the
		/// client and the server in exactly the same way. <br/><br/>.
		/// </remarks>
		/// <seealso cref="Db4objects.Db4o.Config.IObjectClass.MaximumActivationDepth">configuring classes individually
		/// </seealso>
		/// <summary>gets the configured activation depth.</summary>
		/// <summary>sets the activation depth to the specified value.</summary>
		/// <remarks>
		/// sets the activation depth to the specified value.
		/// <br/><br/><b>Why activation?</b><br/>
		/// When objects are instantiated from the database, the instantiation of member
		/// objects needs to be limited to a certain depth. Otherwise a single object
		/// could lead to loading the complete database into memory, if all objects where
		/// reachable from a single root object.<br/><br/>
		/// db4o uses the concept "depth", the number of field-to-field hops an object
		/// is away from another object. <b>The preconfigured "activation depth" db4o uses
		/// in the default setting is 5.</b>
		/// <br/><br/>Whenever an application iterates through the
		/// <see cref="Db4objects.Db4o.IObjectSet">IObjectSet</see>
		/// of a query result, the result objects
		/// will be activated to the configured activation depth.<br/><br/>
		/// A concrete example with the preconfigured activation depth of 5:<br/>
		/// <pre>
		/// // Object foo is the result of a query, it is delivered by the ObjectSet
		/// object foo = objectSet.Next();</pre>
		/// foo.member1.member2.member3.member4.member5 will be a valid object<br/>
		/// foo, member1, member2, member3 and member4 will be activated<br/>
		/// member5 will be deactivated, all of it's members will be null<br/>
		/// member5 can be activated at any time by calling
		/// <see cref="Db4objects.Db4o.IObjectContainer.Activate">IObjectContainer.Activate(member5, depth)
		/// </see>
		/// .
		/// <br/><br/>
		/// Note that raising the global activation depth will consume more memory and
		/// have negative effects on the performance of first-time retrievals. Lowering
		/// the global activation depth needs more individual activation work but can
		/// increase performance of queries.<br/><br/>
		/// <see cref="Db4objects.Db4o.IObjectContainer.Deactivate">IObjectContainer.Deactivate(object, depth)
		/// </see>
		/// can be used to manually free memory by deactivating objects.<br/><br/>
		/// In client/server environment it is good practice to configure the
		/// client and the server in exactly the same way. <br/><br/>.
		/// </remarks>
		/// <seealso cref="Db4objects.Db4o.Config.IObjectClass.MaximumActivationDepth">configuring classes individually
		/// </seealso>
		/// <summary>gets the configured activation depth.</summary>
		int ActivationDepth
		{
			get;
			set;
		}

		/// <summary>
		/// adds ConfigurationItems to be applied when
		/// an ObjectContainer or ObjectServer is opened.
		/// </summary>
		/// <remarks>
		/// adds ConfigurationItems to be applied when
		/// an ObjectContainer or ObjectServer is opened.
		/// </remarks>
		/// <param name="configurationItem">the ConfigurationItem</param>
		void Add(IConfigurationItem configurationItem);

		/// <summary>turns automatic database file format version updates on.</summary>
		/// <remarks>
		/// turns automatic database file format version updates on.
		/// <br /><br />Upon db4o database file format version changes,
		/// db4o can automatically update database files to the
		/// current version. db4objects does not provide functionality
		/// to reverse this process. It is not ensured that updated
		/// database files can be read with older db4o versions.
		/// In some cases (Example: using ObjectManager) it may not be
		/// desirable to update database files automatically therefore
		/// automatic updating is turned off by default for
		/// security reasons.
		/// <br /><br />Call this method to turn automatic database file
		/// version updating on.
		/// <br /><br />If automatic updating is turned off, db4o will refuse
		/// to open database files that use an older database file format.<br /><br />
		/// In client/server environment it is good practice to configure the
		/// client and the server in exactly the same way.
		/// </remarks>
		bool AllowVersionUpdates
		{
			set;
		}

		/// <summary>turns automatic shutdown of the engine on and off.</summary>
		/// <remarks>
		/// turns automatic shutdown of the engine on and off.
		/// The default and recommended setting is <code>true</code>.<br/><br/>
		/// </remarks>
		bool AutomaticShutDown
		{
			set;
		}

		/// <summary>configures the size of BTree nodes in indexes.</summary>
		/// <remarks>
		/// configures the size of BTree nodes in indexes.
		/// <br /><br />Default setting: 100
		/// <br />Lower values will allow a lower memory footprint
		/// and more efficient reading and writing of small slots.
		/// <br />Higher values will reduce the overall number of
		/// read and write operations and allow better performance
		/// at the cost of more RAM use.<br /><br />
		/// In client/server environment it is good practice to configure the
		/// client and the server in exactly the same way.
		/// </remarks>
		/// <value>the number of elements held in one BTree node.</value>
		int BTreeNodeSize
		{
			set;
		}

		/// <summary>turns callback methods on and off.</summary>
		/// <remarks>
		/// turns callback methods on and off.
		/// <br /><br />Callbacks are turned on by default.<br /><br />
		/// A tuning hint: If callbacks are not used, you can turn this feature off, to
		/// prevent db4o from looking for callback methods in persistent classes. This will
		/// increase the performance on system startup.<br /><br />
		/// In a client/server environment it is good practice to configure the
		/// client and the server in exactly the same way.
		/// </remarks>
		/// <value>false to turn callback methods off</value>
		/// <seealso cref="Db4objects.Db4o.Ext.IObjectCallbacks">Using callbacks</seealso>
		bool Callbacks
		{
			set;
		}

		/// <summary>
		/// advises db4o to try instantiating objects with/without calling
		/// constructors.
		/// </summary>
		/// <remarks>
		/// advises db4o to try instantiating objects with/without calling
		/// constructors.
		/// <br/><br/>
		/// Not all .NET-environments support this feature. db4o will
		/// attempt, to follow the setting as good as the enviroment supports.
		/// This setting may also be overridden for individual classes in
		/// <see cref="Db4objects.Db4o.Config.IObjectClass.CallConstructor">Db4objects.Db4o.Config.IObjectClass.CallConstructor
		/// </see>
		/// .
		/// <br/><br/>The default setting depends on the features supported by your current environment.<br/><br/>
		/// In a client/server environment it is good practice to configure the
		/// client and the server in exactly the same way.
		/// <br/><br/>
		/// </remarks>
		/// <seealso cref="Db4objects.Db4o.Config.IObjectClass.CallConstructor">Db4objects.Db4o.Config.IObjectClass.CallConstructor
		/// </seealso>
		bool CallConstructors
		{
			set;
		}

		/// <summary>
		/// tuning feature: configures whether db4o checks all persistent classes upon system
		/// startup, for added or removed fields.
		/// </summary>
		/// <remarks>
		/// tuning feature: configures whether db4o checks all persistent classes upon system
		/// startup, for added or removed fields.
		/// <br /><br />If this configuration setting is set to false while a database is
		/// being created, members of classes will not be detected and stored.
		/// <br /><br />This setting can be set to false in a production environment after
		/// all persistent classes have been stored at least once and classes will not
		/// be modified any further in the future.<br /><br />
		/// In a client/server environment it is good practice to configure the
		/// client and the server in exactly the same way.
		/// <br /><br />Default value: true
		/// </remarks>
		/// <value>the desired setting</value>
		bool DetectSchemaChanges
		{
			set;
		}

		/// <summary>returns the configuration interface for diagnostics.</summary>
		/// <remarks>returns the configuration interface for diagnostics.</remarks>
		/// <returns>the configuration interface for diagnostics.</returns>
		IDiagnosticConfiguration Diagnostic
		{
			get;
		}

		/// <summary>configures whether Exceptions are to be thrown, if objects can not be stored.
		/// 	</summary>
		/// <remarks>
		/// configures whether Exceptions are to be thrown, if objects can not be stored.
		/// <br /><br />db4o requires the presence of a constructor that can be used to
		/// instantiate objects. If no default public constructor is present, all
		/// available constructors are tested, whether an instance of the class can
		/// be instantiated. Null is passed to all constructor parameters.
		/// The first constructor that is successfully tested will
		/// be used throughout the running db4o session. If an instance of the class
		/// can not be instantiated, the object will not be stored. By default,
		/// execution will be stopped with an Exception. This method can
		/// be used to configure db4o to not throw an
		/// <see cref="Db4objects.Db4o.Ext.ObjectNotStorableException">ObjectNotStorableException
		/// 	</see>
		/// if an object can not be stored.
		/// <br /><br />
		/// The default for this setting is <b>true</b>.<br /><br />
		/// In a client/server environment it is good practice to configure the
		/// client and the server in exactly the same way.<br /><br />
		/// </remarks>
		/// <value>true to throw Exceptions if objects can not be stored.</value>
		bool ExceptionsOnNotStorable
		{
			set;
		}

		/// <summary>configures db4o to call Intern() on strings upon retrieval.</summary>
		/// <remarks>
		/// configures db4o to call Intern on strings upon retrieval if set to true.
		/// In client/server environment the setting should be used on both
		/// client and server.
		/// </remarks>
		bool InternStrings
		{
			set;
		}

		// TODO: refactor to use provider?
		/// <summary>allows to mark fields as transient with custom annotations/attributes.</summary>
		/// <remarks>
		/// allows to mark fields as transient with custom annotations/attributes.
		/// <br /><br />.NET only: Call this method with the attribute name that you
		/// wish to use to mark fields as transient. Multiple transient attributes
		/// are possible by calling this method multiple times with different
		/// attribute names.<br /><br />
		/// In a client/server environment it is good practice to configure the
		/// client and the server in exactly the same way. <br /><br />
		/// </remarks>
		/// <param name="attributeName">
		/// - the fully qualified name of the attribute, including
		/// it's namespace
		/// </param>
		void MarkTransient(string attributeName);

		/// <summary>sets the detail level of db4o messages.</summary>
		/// <remarks>
		/// sets the detail level of db4o messages. Messages will be output to the
		/// configured output
		/// <see cref="System.IO.TextWriter">TextWriter</see>
		/// .
		/// <br /><br />
		/// Level 0 - no messages<br />
		/// Level 1 - open and close messages<br />
		/// Level 2 - messages for new, update and delete<br />
		/// Level 3 - messages for activate and deactivate<br /><br />
		/// When using client-server and the level is set to 0, the server will override this and set it to 1.  To get around this you can set the level to -1.  This has the effect of not returning any messages.<br /><br />
		/// In client-server environment this setting can be used on client or on server
		/// depending on which information do you want to track (server side provides more
		/// detailed information).<br /><br />
		/// </remarks>
		/// <value>integer from 0 to 3</value>
		/// <seealso cref="OutStream(System.IO.TextWriter)">TODO: replace int with enumeration
		/// 	</seealso>
		int MessageLevel
		{
			set;
		}

		// TODO: can we provide meaningful java side semantics for this one?
		// TODO: USE A CLASS!!!!!!
		/// <summary>
		/// returns an
		/// <see cref="IObjectClass">IObjectClass</see>
		/// object
		/// to configure the specified class.
		/// <br /><br />
		/// The clazz parameter can be any of the following:<br />
		/// - a fully qualified classname as a String.<br />
		/// - a Class object.<br />
		/// - any other object to be used as a template.<br /><br />
		/// </summary>
		/// <param name="clazz">class name, Class object, or example object.<br /><br /></param>
		/// <returns>
		/// an instance of an
		/// <see cref="IObjectClass">IObjectClass</see>
		/// object for configuration.
		/// </returns>
		IObjectClass ObjectClass(object clazz);

		/// <summary>
		/// If set to true, db4o will try to optimize native queries
		/// dynamically at query execution time, otherwise it will
		/// run native queries in unoptimized mode as SODA evaluations.
		/// </summary>
		/// <remarks>
		/// If set to true, db4o will try to optimize native queries
		/// dynamically at query execution time, otherwise it will
		/// run native queries in unoptimized mode as SODA evaluations.
		/// The following assemblies should be available for native query switch to take effect:
		/// Db4objects.Db4o.NativeQueries.dll, Db4objects.Db4o.Instrumentation.dll.
		/// <br/><br/>The default setting is <code>true</code>.<br/><br/>
		/// In a client/server environment it is good practice to configure the
		/// client and the server in exactly the same way. <br/><br/>
		/// </remarks>
		/// <seealso cref="Db4objects.Db4o.Config.ICommonConfiguration.OptimizeNativeQueries">Db4objects.Db4o.Config.ICommonConfiguration.OptimizeNativeQueries</seealso>
		/// <summary>
		/// If set to true, db4o will try to optimize native queries
		/// dynamically at query execution time, otherwise it will
		/// run native queries in unoptimized mode as SODA evaluations.
		/// </summary>
		/// <remarks>
		/// If set to true, db4o will try to optimize native queries
		/// dynamically at query execution time, otherwise it will
		/// run native queries in unoptimized mode as SODA evaluations.
		/// The following assemblies should be available for native query switch to take effect:
		/// Db4objects.Db4o.NativeQueries.dll, Db4objects.Db4o.Instrumentation.dll.
		/// <br/><br/>The default setting is <code>true</code>.<br/><br/>
		/// In a client/server environment it is good practice to configure the
		/// client and the server in exactly the same way. <br/><br/>
		/// </remarks>
		/// <seealso cref="Db4objects.Db4o.Config.ICommonConfiguration.OptimizeNativeQueries">Db4objects.Db4o.Config.ICommonConfiguration.OptimizeNativeQueries</seealso>
		bool OptimizeNativeQueries
		{
			get;
			set;
		}

		/// <summary>returns the Query configuration interface.</summary>
		/// <remarks>returns the Query configuration interface.</remarks>
		IQueryConfiguration Queries
		{
			get;
		}

		/// <summary>configures the use of a specially designed reflection implementation.</summary>
		/// <remarks>
		/// configures the use of a specially designed reflection implementation.
		/// <br /><br />
		/// db4o internally uses java.lang.reflect.* by default. On platforms that
		/// do not support this package, customized implementations may be written
		/// to supply all the functionality of the interfaces in the com.db4o.reflect
		/// package. This method can be used to install a custom reflection
		/// implementation.<br /><br />
		/// In client-server environment this setting should be used on both the client and
		/// the server side (reflector class must be available)<br /><br />
		/// </remarks>
		void ReflectWith(IReflector reflector);

		/// <summary>
		/// Assigns a
		/// <see cref="System.IO.TextWriter">TextWriter</see>
		/// where db4o is to print its event messages.
		/// <br /><br />Messages are useful for debugging purposes and for learning
		/// to understand, how db4o works. The message level can be raised with
		/// <see cref="IConfiguration.MessageLevel(int)">IConfiguration.MessageLevel(int)</see>
		/// to produce more detailed messages.
		/// <br /><br />Use <code>outStream(System.out)</code> to print messages to the
		/// console.<br /><br />
		/// In client-server environment this setting should be used on the same side
		/// where
		/// <see cref="IConfiguration.MessageLevel(int)">IConfiguration.MessageLevel(int)</see>
		/// is used.<br /><br />
		/// </summary>
		/// <value>the new PrintStream for messages.</value>
		/// <seealso cref="MessageLevel(int)">MessageLevel(int)</seealso>
		TextWriter OutStream
		{
			set;
		}

		/// <summary>configures the string encoding to be used.</summary>
		/// <remarks>
		/// configures the string encoding to be used.
		/// <br/><br/>The string encoding can not be changed in the lifetime of a
		/// database file. To set up the database with the correct string encoding,
		/// this configuration needs to be set correctly <b>before</b> a database
		/// file is created with the first call to
		/// <see cref="Db4objects.Db4o.Db4oFactory.OpenFile">Db4objects.Db4o.Db4oFactory.OpenFile
		/// </see>
		/// or
		/// <see cref="Db4objects.Db4o.Db4oFactory.OpenServer">Db4objects.Db4o.Db4oFactory.OpenServer
		/// </see>
		/// .
		/// <br/><br/>For subsequent open calls, db4o remembers built-in
		/// string encodings. If a custom encoding is used (an encoding that is
		/// not supplied from within the db4o library), the correct encoding
		/// needs to be configured correctly again for all subsequent calls
		/// that open database files.
		/// <br/><br/>
		/// In client-server mode, the server and all clients need to have the same string encoding.<br/><br/>
		/// Example:<br/>
		/// <code>config.StringEncoding = StringEncodings.Utf8();</code>
		/// </remarks>
		/// <seealso cref="Db4objects.Db4o.Config.Encoding.StringEncodings">Db4objects.Db4o.Config.Encoding.StringEncodings
		/// </seealso>
		IStringEncoding StringEncoding
		{
			set;
		}

		/// <summary>
		/// tuning feature: configures whether db4o should try to instantiate one instance
		/// of each persistent class on system startup.
		/// </summary>
		/// <remarks>
		/// tuning feature: configures whether db4o should try to instantiate one instance
		/// of each persistent class on system startup.
		/// <br /><br />In a production environment this setting can be set to false,
		/// if all persistent classes have public default constructors.
		/// <br /><br />
		/// In a client/server environment it is good practice to configure the
		/// client and the server in exactly the same way. <br /><br />
		/// Default value: true
		/// </remarks>
		/// <value>the desired setting</value>
		bool TestConstructors
		{
			set;
		}

		/// <summary>specifies the global updateDepth.</summary>
		/// <remarks>
		/// specifies the global updateDepth.
		/// <br /><br />see the documentation of
		/// <see cref="Db4objects.Db4o.IObjectContainer.Store(object)"></see>
		/// for further details.<br /><br />
		/// The value be may be overridden for individual classes.<br /><br />
		/// The default setting is 1: Only the object passed to
		/// <see cref="Db4objects.Db4o.IObjectContainer.Store(object)">Db4objects.Db4o.IObjectContainer.Store(object)
		/// 	</see>
		/// will be updated.<br /><br />
		/// In a client/server environment it is good practice to configure the
		/// client and the server in exactly the same way. <br /><br />
		/// </remarks>
		/// <value>the depth of the desired update.</value>
		/// <seealso cref="IObjectClass.UpdateDepth(int)">IObjectClass.UpdateDepth(int)</seealso>
		/// <seealso cref="IObjectClass.CascadeOnUpdate(bool)">IObjectClass.CascadeOnUpdate(bool)
		/// 	</seealso>
		/// <seealso cref="Db4objects.Db4o.Ext.IObjectCallbacks">Using callbacks</seealso>
		int UpdateDepth
		{
			set;
		}

		/// <summary>turns weak reference management on or off.</summary>
		/// <remarks>
		/// turns weak reference management on or off.
		/// <br /><br />
		/// This method must be called before opening a database.
		/// <br /><br />
		/// Performance may be improved by running db4o without using weak
		/// references durring memory management at the cost of higher
		/// memory consumption or by alternatively implementing a manual
		/// memory management scheme using
		/// <see cref="Db4objects.Db4o.Ext.IExtObjectContainer.Purge(object)">Db4objects.Db4o.Ext.IExtObjectContainer.Purge(object)
		/// 	</see>
		/// <br /><br />Setting the value to false causes db4o to use hard
		/// references to objects, preventing the garbage collection process
		/// from disposing of unused objects.
		/// <br /><br />The default setting is true.
		/// </remarks>
		bool WeakReferences
		{
			set;
		}

		/// <summary>configures the timer for WeakReference collection.</summary>
		/// <remarks>
		/// configures the timer for WeakReference collection.
		/// <br /><br />The default setting is 1000 milliseconds.
		/// <br /><br />Configure this setting to zero to turn WeakReference
		/// collection off.
		/// </remarks>
		/// <value>the time in milliseconds</value>
		int WeakReferenceCollectionInterval
		{
			set;
		}

		/// <summary>
		/// Allows registering special TypeHandlers for customized marshalling
		/// and customized comparisons.
		/// </summary>
		/// <remarks>
		/// Allows registering special TypeHandlers for customized marshalling
		/// and customized comparisons.  <br/>
		/// This is only for adventurous people, since type handlers work a the lowest levels:<br/>
		/// <ul>
		/// <li>There is no versioning support: If you need to change the serialisation scheme of the type your are on your own.</li>
		/// <li>The typehandler-API is mostly undocumented and not as stable as other db4o APIs.</li>
		/// <li>Elaborate type handlers need deep knowledge of  undocumented, internal implementation details of db4o. </li>
		/// <li>Mistakes in typehandlers can lead to cryptic error messages and database corruption.</li>
		/// </ul>
		/// </remarks>
		/// <param name="predicate">
		/// to specify for which classes and versions the
		/// TypeHandler is to be used.
		/// </param>
		/// <param name="typeHandler">to be used for the classes that match the predicate.</param>
		void RegisterTypeHandler(ITypeHandlerPredicate predicate, ITypeHandler4 typeHandler
			);

		/// <seealso cref="Db4objects.Db4o.Foundation.IEnvironment">Db4objects.Db4o.Foundation.IEnvironment
		/// 	</seealso>
		IEnvironmentConfiguration Environment
		{
			get;
		}

		/// <summary>
		/// Registers a
		/// <see cref="INameProvider">INameProvider</see>
		/// that assigns a custom name to the database to be used in
		/// <see cref="object.ToString()">object.ToString()</see>
		/// .
		/// </summary>
		void NameProvider(INameProvider provider);

		/// <summary><p>Sets the max stack depth that will be used for recursive storing and activating an object.
		/// 	</summary>
		/// <remarks>
		/// <p>Sets the max stack depth that will be used for recursive storing and activating an object.
		/// <p>The default value is set to
		/// <see cref="Db4objects.Db4o.Internal.Const4.DefaultMaxStackDepth">Db4objects.Db4o.Internal.Const4.DefaultMaxStackDepth
		/// 	</see>
		/// <p>On Android platform, we recommend setting this to 2.
		/// </remarks>
		/// <value>the desired max stack depth.</value>
		/// <summary>gets the configured max stack depth.</summary>
		/// <remarks>gets the configured max stack depth.</remarks>
		/// <returns>the configured max stack depth.</returns>
		int MaxStackDepth
		{
			get;
			set;
		}
	}
}
