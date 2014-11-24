/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.IO;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Config.Encoding;
using Db4objects.Db4o.Diagnostic;
using Db4objects.Db4o.IO;
using Db4objects.Db4o.Reflect;
using Db4objects.Db4o.Typehandlers;

namespace Db4objects.Db4o.Config
{
	/// <summary>configuration interface.</summary>
	/// <remarks>
	/// configuration interface.
	/// <br /><br />This interface contains methods to configure db4o.<br /><br />
	/// The global Configuration context is available with
	/// <see cref="Db4objects.Db4o.Db4oFactory.Configure()">Db4objects.Db4o.Db4oFactory.Configure()
	/// 	</see>
	/// .
	/// When an ObjectContainer or ObjectServer is opened, the global Configuration
	/// context is cloned and copied into the ObjectContainer/ObjectServer.
	/// That means every ObjectContainer/ObjectServer gets it's own copy of
	/// configuration settings.<br /><br />
	/// <b>Most configuration settings should be set before opening an
	/// ObjectContainer/ObjectServer</b>.
	/// <br /><br />Some configuration settings can be modified on an open
	/// ObjectContainer/ObjectServer. The local Configuration context is
	/// available with
	/// <see cref="Db4objects.Db4o.Ext.IExtObjectContainer.Configure()">Db4objects.Db4o.Ext.IExtObjectContainer.Configure()
	/// 	</see>
	/// and
	/// <see cref="Db4objects.Db4o.Ext.IExtObjectServer.Configure()">Db4objects.Db4o.Ext.IExtObjectServer.Configure()
	/// 	</see>
	/// .
	/// </remarks>
	public interface IConfiguration
	{
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
		/// In client/server environment the same setting should be used on both
		/// client and server<br/><br/>.
		/// </remarks>
		/// <param name="depth">the desired global activation depth.</param>
		/// 
		/// <seealso cref="Db4objects.Db4o.Config.IObjectClass.MaximumActivationDepth">configuring classes individually
		/// </seealso>
		/// 
		/// <summary>gets the configured activation depth.</summary>
		void ActivationDepth(int depth);

		/// <summary>gets the configured activation depth.</summary>
		/// <remarks>gets the configured activation depth.</remarks>
		/// <returns>the configured activation depth.</returns>
		int ActivationDepth();

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

		/// <summary>adds a new Alias for a class, namespace or package.
		/// </summary>
		/// <remarks>
		/// adds a new Alias for a class, namespace or package.
		/// <br/>
		/// <br/>
		/// Aliases can be used to persist classes in the running application
		/// to different persistent classes in a database file or on a db4o
		/// server.
		/// <br/>
		/// <br/>
		/// Two simple Alias implementations are supplied along with db4o:
		/// <br/>
		/// -
		/// <see cref="TypeAlias">TypeAlias</see>
		/// provides an #equals() resolver to match names directly.
		/// <br/>
		/// -
		/// <see cref="WildcardAlias">WildcardAlias</see>
		/// allows simple pattern matching with one single '*' wildcard
		/// character.
		/// <br/>
		/// <br/>
		/// It is possible to create own complex
		/// <see cref="IAlias">IAlias</see>
		/// constructs by creating own resolvers that implement the
		/// <see cref="IAlias">IAlias</see>
		/// interface.
		/// <br/>
		/// <br/>
		/// Four examples of concrete usecases:
		/// <br/>
		/// <br/>
		/// <code>
		/// <b>// Creating an Alias for a single class</b>
		/// <br/>
		/// Db4oFactory.Configure().AddAlias(
		/// <br/>  new TypeAlias("Tutorial.F1.Pilot", "Tutorial.F1.Driver"));<br/>
		/// <br/><br/>
		/// <b>// Accessing a Java package from a .NET assembly</b><br/>
		/// Db4o.configure().addAlias(<br/>
		///   new WildcardAlias(<br/>
		///     "com.f1.*",<br/>
		///     "Tutorial.F1.*, Tutorial"));<br/>
		/// <br/><br/>
		/// <b>// Using a different local .NET assembly</b><br/>
		/// Db4o.configure().addAlias(<br/>
		///   new WildcardAlias(<br/>
		///     "Tutorial.F1.*, Tutorial",<br/>
		///     "Tutorial.F1.*, RaceClient"));<br/>
		/// </code>
		/// <br/><br/>Aliases that translate the persistent name of a class to
		/// a name that already exists as a persistent name in the database
		/// (or on the server) are not permitted and will throw an exception
		/// when the database file is opened.
		/// <br/><br/>Aliases should be configured before opening a database file
		/// or connecting to a server.
		/// </remarks>
		void AddAlias(IAlias alias);

		/// <summary>
		/// Removes an alias previously added with
		/// <see cref="AddAlias(IAlias)">AddAlias(IAlias)</see>
		/// .
		/// </summary>
		/// <param name="alias">the alias to remove</param>
		void RemoveAlias(IAlias alias);

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
		/// In client-server environment this setting should be used on both client
		/// and server.
		/// </remarks>
		void AllowVersionUpdates(bool flag);

		/// <summary>turns automatic shutdown of the engine on and off.</summary>
		/// <remarks>
		/// turns automatic shutdown of the engine on and off.
		/// </remarks>
		/// <param name="flag">whether db4o should shut down automatically.</param>
		void AutomaticShutDown(bool flag);

		/// <summary>sets the storage data blocksize for new ObjectContainers.</summary>
		/// <remarks>
		/// sets the storage data blocksize for new ObjectContainers.
		/// <br /><br />The standard setting is 1 allowing for a maximum
		/// database file size of 2GB. This value can be increased
		/// to allow larger database files, although some space will
		/// be lost to padding because the size of some stored objects
		/// will not be an exact multiple of the block size. A
		/// recommended setting for large database files is 8, since
		/// internal pointers have this length.<br /><br />
		/// This setting is only effective when the database is first created, in
		/// client-server environment in most cases it means that the setting
		/// should be used on the server side.
		/// </remarks>
		/// <param name="bytes">the size in bytes from 1 to 127</param>
		/// <exception cref="Db4objects.Db4o.Config.GlobalOnlyConfigException"></exception>
		void BlockSize(int bytes);

		/// <summary>configures the size of BTree nodes in indexes.</summary>
		/// <remarks>
		/// configures the size of BTree nodes in indexes.
		/// <br /><br />Default setting: 100
		/// <br />Lower values will allow a lower memory footprint
		/// and more efficient reading and writing of small slots.
		/// <br />Higher values will reduce the overall number of
		/// read and write operations and allow better performance
		/// at the cost of more RAM use.<br /><br />
		/// This setting should be used on both client and server in
		/// client-server environment.
		/// </remarks>
		/// <param name="size">the number of elements held in one BTree node.</param>
		void BTreeNodeSize(int size);

		/// <summary>configures caching of BTree nodes.</summary>
		/// <remarks>
		/// configures caching of BTree nodes.
		/// <br /><br />Clean BTree nodes will be unloaded on #commit and
		/// #rollback unless they are configured as cached here.
		/// <br /><br />Default setting: 0
		/// <br />Possible settings: 1, 2 or 3
		/// <br /><br /> The potential number of cached BTree nodes can be
		/// calculated with the following formula:<br />
		/// maxCachedNodes = bTreeNodeSize ^ bTreeCacheHeight<br /><br />
		/// This setting should be used on both client and server in
		/// client-server environment.
		/// </remarks>
		/// <param name="height">the height of the cache from the root</param>
		void BTreeCacheHeight(int height);

		/// <summary>returns the Cache configuration interface.</summary>
		/// <remarks>returns the Cache configuration interface.</remarks>
		ICacheConfiguration Cache();

		/// <summary>turns callback methods on and off.</summary>
		/// <remarks>
		/// turns callback methods on and off.
		/// <br /><br />Callbacks are turned on by default.<br /><br />
		/// A tuning hint: If callbacks are not used, you can turn this feature off, to
		/// prevent db4o from looking for callback methods in persistent classes. This will
		/// increase the performance on system startup.<br /><br />
		/// In client/server environment this setting should be used on both
		/// client and server.
		/// </remarks>
		/// <param name="flag">false to turn callback methods off</param>
		/// <seealso cref="Db4objects.Db4o.Ext.IObjectCallbacks">Using callbacks</seealso>
		void Callbacks(bool flag);

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
		/// In client/server environment this setting should be used on both
		/// client and server.
		/// <br/><br/>
		/// </remarks>
		/// <param name="flag">
		/// - specify true, to request calling constructors, specify
		/// false to request <b>not</b> calling constructors.
		/// </param>
		/// <seealso cref="Db4objects.Db4o.Config.IObjectClass.CallConstructor">Db4objects.Db4o.Config.IObjectClass.CallConstructor
		/// </seealso>
		void CallConstructors(bool flag);

		/// <summary>
		/// turns
		/// <see cref="IObjectClass.MaximumActivationDepth(int)">individual class activation depth configuration
		/// 	</see>
		/// on
		/// and off.
		/// <br /><br />This feature is turned on by default.<br /><br />
		/// In client/server environment this setting should be used on both
		/// client and server.<br /><br />
		/// </summary>
		/// <param name="flag">
		/// false to turn the possibility to individually configure class
		/// activation depths off
		/// </param>
		/// <seealso cref="ActivationDepth()">Why activation?</seealso>
		void ClassActivationDepthConfigurable(bool flag);

		/// <summary>returns client/server configuration interface.</summary>
		/// <remarks>returns client/server configuration interface.</remarks>
		IClientServerConfiguration ClientServer();

		/// <summary>
		/// configures the size database files should grow in bytes, when no
		/// free slot is found within.
		/// </summary>
		/// <remarks>
		/// configures the size database files should grow in bytes, when no
		/// free slot is found within.
		/// <br /><br />Tuning setting.
		/// <br /><br />Whenever no free slot of sufficient length can be found
		/// within the current database file, the database file's length
		/// is extended. This configuration setting configures by how much
		/// it should be extended, in bytes.<br /><br />
		/// This configuration setting is intended to reduce fragmentation.
		/// Higher values will produce bigger database files and less
		/// fragmentation.<br /><br />
		/// To extend the database file, a single byte array is created
		/// and written to the end of the file in one write operation. Be
		/// aware that a high setting will require allocating memory for
		/// this byte array.
		/// </remarks>
		/// <param name="bytes">amount of bytes</param>
		void DatabaseGrowthSize(int bytes);

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
		/// In a client/server environment this setting should be configured both on the
		/// client and and on the server.
		/// <br /><br />Default value: true
		/// </remarks>
		/// <param name="flag">the desired setting</param>
		void DetectSchemaChanges(bool flag);

		/// <summary>returns the configuration interface for diagnostics.</summary>
		/// <remarks>returns the configuration interface for diagnostics.</remarks>
		/// <returns>the configuration interface for diagnostics.</returns>
		IDiagnosticConfiguration Diagnostic();

		/// <summary>turns commit recovery off.</summary>
		/// <remarks>
		/// turns commit recovery off.
		/// <br /><br />db4o uses a two-phase commit algorithm. In a first step all intended
		/// changes are written to a free place in the database file, the "transaction commit
		/// record". In a second step the
		/// actual changes are performed. If the system breaks down during commit, the
		/// commit process is restarted when the database file is opened the next time.
		/// On very rare occasions (possibilities: hardware failure or editing the database
		/// file with an external tool) the transaction commit record may be broken. In this
		/// case, this method can be used to try to open the database file without commit
		/// recovery. The method should only be used in emergency situations after consulting
		/// db4o support.
		/// </remarks>
		void DisableCommitRecovery();

		/// <summary>configures the use of encryption.</summary>
		/// <remarks>
		/// configures the use of encryption.
		/// <br /><br />This method needs to be called <b>before</b> a database file
		/// is created with the first
		/// <see cref="Db4objects.Db4o.Db4oFactory.OpenFile(string)">Db4objects.Db4o.Db4oFactory.OpenFile(string)
		/// 	</see>
		/// .
		/// <br /><br />If encryption is set to true,
		/// you need to supply a password to seed the encryption mechanism.<br /><br />
		/// db4o database files keep their encryption format after creation.<br /><br />
		/// </remarks>
		/// <param name="flag">
		/// true for turning encryption on, false for turning encryption
		/// off.
		/// </param>
		/// <seealso cref="Password(string)">Password(string)</seealso>
		/// <exception cref="Db4objects.Db4o.Config.GlobalOnlyConfigException"></exception>
		[System.ObsoleteAttribute(@"use a custom encrypting IoAdapter instead")]
		void Encrypt(bool flag);

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
		/// execution will continue without any message or error. This method can
		/// be used to configure db4o to throw an
		/// <see cref="Db4objects.Db4o.Ext.ObjectNotStorableException">ObjectNotStorableException
		/// 	</see>
		/// if an object can not be stored.
		/// <br /><br />
		/// The default for this setting is <b>true</b>.<br /><br />
		/// In client/server environment this setting should be used on both
		/// client and server.<br /><br />
		/// </remarks>
		/// <param name="flag">false to not throw Exceptions if objects can not be stored (fail silently).
		/// 	</param>
		void ExceptionsOnNotStorable(bool flag);

		/// <summary>returns the freespace configuration interface.</summary>
		/// <remarks>returns the freespace configuration interface.</remarks>
		IFreespaceConfiguration Freespace();

		/// <summary>configures db4o to generate UUIDs for stored objects.</summary>
		/// <remarks>
		/// configures db4o to generate UUIDs for stored objects.
		/// This setting should be used when the database is first created.<br /><br />
		/// </remarks>
		/// <param name="setting">the scope for UUID generation: disabled, generate for all classes, or configure individually
		/// 	</param>
		void GenerateUUIDs(ConfigScope setting);

		/// <summary>configures db4o to generate version numbers for stored objects.</summary>
		/// <remarks>
		/// configures db4o to generate version numbers for stored objects.
		/// This setting should be used when the database is first created.
		/// </remarks>
		/// <param name="setting">the scope for version number generation: disabled, generate for all classes, or configure individually
		/// 	</param>
		[System.ObsoleteAttribute(@"As of version 8.0 please use GenerateCommitTimestamps(bool) instead."
			)]
		void GenerateVersionNumbers(ConfigScope setting);

		/// <summary>
		/// Configures db4o to generate commit timestamps for all stored objects.<br />
		/// <br />
		/// All the objects commited within a transaction will share the same commit timestamp.
		/// </summary>
		/// <remarks>
		/// Configures db4o to generate commit timestamps for all stored objects.<br />
		/// <br />
		/// All the objects commited within a transaction will share the same commit timestamp.
		/// <br />
		/// This setting should be used when the database is first created.<br />
		/// <br />
		/// Afterwards you can access the object's commit timestamp like this:<br />
		/// <br />
		/// <pre>
		/// ObjectContainer container = ...;
		/// ObjectInfo objectInfo = container.ext().getObjectInfo(obj);
		/// long commitTimestamp = objectInfo.getVersion();
		/// </pre>
		/// </remarks>
		/// <param name="flag">
		/// if true, commit timetamps will be generated for all stored
		/// objects. If you already have commit timestamps for stored
		/// objects and later set this flag to false, although you wont be
		/// able to access them, the commit timestamps will still be taking
		/// space in your file container. The only way to free that space
		/// is defragmenting the container.
		/// </param>
		/// <since>8.0</since>
		void GenerateCommitTimestamps(bool flag);

		/// <summary>configures db4o to call #intern() on strings upon retrieval.</summary>
		/// <remarks>
		/// configures db4o to call #intern() on strings upon retrieval.
		/// In client/server environment the setting should be used on both
		/// client and server.
		/// </remarks>
		/// <param name="flag">true to intern strings</param>
		void InternStrings(bool flag);

		/// <summary>returns true if strings will be interned.</summary>
		/// <remarks>returns true if strings will be interned.</remarks>
		bool InternStrings();

		/// <summary>allows to configure db4o to use a customized byte IO storage mechanism.</summary>
		/// <remarks>
		/// allows to configure db4o to use a customized byte IO storage mechanism.
		/// <br /><br />Implement the interface
		/// <see cref="Db4objects.Db4o.IO.IStorage">Db4objects.Db4o.IO.IStorage</see>
		/// to
		/// write your own. Possible usecases could be improved performance
		/// with a native library, mirrored write to two files, encryption or
		/// read-on-write fail-safety control.<br /><br />
		/// </remarks>
		/// <value>- the factory</value>
		/// <seealso cref="Db4objects.Db4o.IO.CachingStorage">Db4objects.Db4o.IO.CachingStorage
		/// 	</seealso>
		/// <seealso cref="Db4objects.Db4o.IO.MemoryStorage">Db4objects.Db4o.IO.MemoryStorage
		/// 	</seealso>
		/// <seealso cref="Db4objects.Db4o.IO.FileStorage">Db4objects.Db4o.IO.FileStorage</seealso>
		/// <seealso cref="Db4objects.Db4o.IO.StorageDecorator">Db4objects.Db4o.IO.StorageDecorator
		/// 	</seealso>
		/// <exception cref="Db4objects.Db4o.Config.GlobalOnlyConfigException"></exception>
		/// <summary>
		/// returns the configured
		/// <see cref="Db4objects.Db4o.IO.IStorage">Db4objects.Db4o.IO.IStorage</see>
		/// </summary>
		IStorage Storage
		{
			get;
			set;
		}

		/// <summary>allows to mark fields as transient with custom attributes.</summary>
		/// <remarks>
		/// allows to mark fields as transient with custom attributes.
		/// <br /><br />.NET only: Call this method with the attribute name that you
		/// wish to use to mark fields as transient. Multiple transient attributes
		/// are possible by calling this method multiple times with different
		/// attribute names.<br /><br />
		/// In client/server environment the setting should be used on both
		/// client and server.<br /><br />
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
		/// <param name="level">integer from 0 to 3</param>
		/// <seealso cref="SetOut(System.IO.TextWriter)">SetOut(System.IO.TextWriter)</seealso>
		void MessageLevel(int level);

		/// <summary>can be used to turn the database file locking thread off.</summary>
		/// <param name="flag">
		/// false to turn database file locking off.
		/// </param>
		void LockDatabaseFile(bool flag);

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
		/// <br/><br/>The default setting is true.<br/><br/>
		/// In client-server environment this setting should be used on both client and server.<br/><br/>
		/// </remarks>
		/// <param name="optimizeNQ">
		/// true, if db4o should try to optimize
		/// native queries at query execution time, false otherwise
		/// </param>
		/// 
		/// <seealso cref="Db4objects.Db4o.Config.ICommonConfiguration.OptimizeNativeQueries">Db4objects.Db4o.Config.ICommonConfiguration.OptimizeNativeQueries</seealso>
		void OptimizeNativeQueries(bool optimizeNQ);

		/// <summary>indicates whether Native Queries will be optimized dynamically.</summary>
		/// <remarks>indicates whether Native Queries will be optimized dynamically.</remarks>
		/// <returns>
		/// boolean true if Native Queries will be optimized
		/// dynamically.
		/// </returns>
		/// <seealso cref="OptimizeNativeQueries()">OptimizeNativeQueries()</seealso>
		bool OptimizeNativeQueries();

		/// <summary>protects the database file with a password.</summary>
		/// <remarks>
		/// protects the database file with a password.
		/// <br /><br />To set a password for a database file, this method needs to be
		/// called <b>before</b> a database file is created with the first
		/// <see cref="Db4objects.Db4o.Db4oFactory.OpenFile(string)">Db4objects.Db4o.Db4oFactory.OpenFile(string)
		/// 	</see>
		/// .
		/// <br /><br />All further attempts to open
		/// the file, are required to set the same password.<br /><br />The password
		/// is used to seed the encryption mechanism, which makes it impossible
		/// to read the database file without knowing the password.<br /><br />
		/// </remarks>
		/// <param name="pass">the password to be used.</param>
		/// <exception cref="Db4objects.Db4o.Config.GlobalOnlyConfigException"></exception>
		[System.ObsoleteAttribute(@"use a custom encrypting IoAdapter instead")]
		void Password(string pass);

		/// <summary>returns the Query configuration interface.</summary>
		/// <remarks>returns the Query configuration interface.</remarks>
		IQueryConfiguration Queries();

		/// <summary>turns readOnly mode on and off.</summary>
		/// <remarks>
		/// turns readOnly mode on and off.
		/// <br /><br />This method configures the mode in which subsequent calls to
		/// <see cref="Db4objects.Db4o.Db4oFactory.OpenFile(string)">Db4o.openFile()</see>
		/// will open files.
		/// <br /><br />Readonly mode allows to open an unlimited number of reading
		/// processes on one database file. It is also convenient
		/// for deploying db4o database files on CD-ROM.<br /><br />
		/// In client-server environment this setting should be used on the server side
		/// in embedded mode and ONLY on client side in networked mode.<br /><br />
		/// </remarks>
		/// <param name="flag">
		/// true for configuring readOnly mode for subsequent
		/// calls to
		/// <see cref="Db4objects.Db4o.Db4oEmbedded.OpenFile(string)">Db4o.openFile()</see>
		/// .
		/// </param>
		void ReadOnly(bool flag);

		/// <summary>
		/// turns recovery mode on and off.<br /><br />
		/// Recovery mode can be used to try to retrieve as much as possible
		/// out of an already corrupted database.
		/// </summary>
		/// <remarks>
		/// turns recovery mode on and off.<br /><br />
		/// Recovery mode can be used to try to retrieve as much as possible
		/// out of an already corrupted database. In recovery mode internal
		/// checks are more relaxed. Null or invalid objects may be returned
		/// instead of throwing exceptions.<br /><br />
		/// Use this method with care as a last resort to get data out of a
		/// corrupted database.
		/// </remarks>
		/// <param name="flag">true to turn recover mode on.</param>
		void RecoveryMode(bool flag);

		/// <summary>configures the use of a specially designed reflection implementation.</summary>
		/// <remarks>
		/// configures the use of a specially designed reflection implementation.
		/// <br/><br/>
		/// db4o internally uses System.Reflection by default. On platforms that
		/// do not support this package, customized implementations may be written
		/// to supply all the functionality of the interfaces in System.Reflection
		/// namespace. This method can be used to install a custom reflection
		/// implementation.
		/// 
		/// </remarks>
		void ReflectWith(IReflector reflector);

		/// <summary>tuning feature only: reserves a number of bytes in database files.</summary>
		/// <remarks>
		/// tuning feature only: reserves a number of bytes in database files.
		/// <br /><br />The global setting is used for the creation of new database
		/// files. Continous calls on an ObjectContainer Configuration context
		/// (see
		/// <see cref="Db4objects.Db4o.Ext.IExtObjectContainer.Configure()">Db4objects.Db4o.Ext.IExtObjectContainer.Configure()
		/// 	</see>
		/// ) will
		/// continually allocate space.
		/// <br /><br />The allocation of a fixed number of bytes at one time
		/// makes it more likely that the database will be stored in one
		/// chunk on the mass storage. Less read/write head movement can result
		/// in improved performance.<br /><br />
		/// <b>Note:</b><br /> Allocated space will be lost on abnormal termination
		/// of the database engine (hardware crash, VM crash). A Defragment run
		/// will recover the lost space. For the best possible performance, this
		/// method should be called before the Defragment run to configure the
		/// allocation of storage space to be slightly greater than the anticipated
		/// database file size.
		/// <br /><br />
		/// In client-server environment this setting should be used on the server side. <br /><br />
		/// Default configuration: 0<br /><br />
		/// </remarks>
		/// <param name="byteCount">the number of bytes to reserve</param>
		/// <exception cref="Db4objects.Db4o.Ext.DatabaseReadOnlyException"></exception>
		/// <exception cref="System.NotSupportedException"></exception>
		void ReserveStorageSpace(long byteCount);

		/// <summary>
		/// configures the path to be used to store and read
		/// Blob data.
		/// </summary>
		/// <remarks>
		/// configures the path to be used to store and read
		/// Blob data.
		/// <br /><br />
		/// In client-server environment this setting should be used on the
		/// server side. <br /><br />
		/// </remarks>
		/// <param name="path">the path to be used</param>
		/// <exception cref="System.IO.IOException"></exception>
		void SetBlobPath(string path);

		/// <summary>
		/// Assigns a
		/// <see cref="System.IO.TextWriter">TextWriter</see>
		/// where db4o is to print its event messages.
		/// <br /><br />Messages are useful for debugging purposes and for learning
		/// to understand, how db4o works. The message level can be raised with
		/// <see cref="MessageLevel(int)">MessageLevel(int)</see>
		/// to produce more detailed messages.
		/// <br /><br />Use <code>setOut(System.out)</code> to print messages to the
		/// console.<br /><br />
		/// In client-server environment this setting should be used on the same side
		/// where
		/// <see cref="MessageLevel(int)">MessageLevel(int)</see>
		/// is used.<br /><br />
		/// </summary>
		/// <param name="outStream">the new PrintStream for messages.</param>
		/// <seealso cref="MessageLevel(int)">MessageLevel(int)</seealso>
		void SetOut(TextWriter outStream);

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
		/// <br/><br/>Example:<br/>
		/// <code>config.StringEncoding(StringEncodings.Utf8());</code>
		/// </remarks>
		/// <seealso cref="Db4objects.Db4o.Config.Encoding.StringEncodings">Db4objects.Db4o.Config.Encoding.StringEncodings
		/// </seealso>
		void StringEncoding(IStringEncoding encoding);

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
		/// In client-server environment this setting should be used on both client and server
		/// side. <br /><br />
		/// Default value: true
		/// </remarks>
		/// <param name="flag">the desired setting</param>
		void TestConstructors(bool flag);

		/// <summary>specifies the global updateDepth.</summary>
		/// <remarks>
		/// specifies the global updateDepth.
		/// <br /><br />see the documentation of
		/// <see cref="com.db4o.ObjectContainer#set"></see>
		/// for further details.<br /><br />
		/// The value be may be overridden for individual classes.<br /><br />
		/// The default setting is 1: Only the object passed to
		/// <see cref="com.db4o.ObjectContainer#set">com.db4o.ObjectContainer#set</see>
		/// will be updated.<br /><br />
		/// In client-server environment this setting should be used on both client and
		/// server sides.<br /><br />
		/// </remarks>
		/// <param name="depth">the depth of the desired update.</param>
		/// <seealso cref="IObjectClass.UpdateDepth(int)">IObjectClass.UpdateDepth(int)</seealso>
		/// <seealso cref="IObjectClass.CascadeOnUpdate(bool)">IObjectClass.CascadeOnUpdate(bool)
		/// 	</seealso>
		/// <seealso cref="Db4objects.Db4o.Ext.IObjectCallbacks">Using callbacks</seealso>
		void UpdateDepth(int depth);

		/// <summary>turns weak reference management on or off.</summary>
		/// <remarks>
		/// turns weak reference management on or off.
		/// <br/><br/>
		/// This method must be called before opening a database.
		/// <br/><br/>
		/// Performance may be improved by running db4o without using weak
		/// references durring memory management at the cost of higher
		/// memory consumption or by alternatively implementing a manual
		/// memory management scheme using
		/// <see cref="IExtObjectContainer.Purge">IExtObjectContainer.Purge</see>
		/// <br/><br/>Setting the value to false causes db4o to use hard
		/// references to objects, preventing the garbage collection process
		/// from disposing of unused objects.
		/// <br/><br/>The default setting is true.
		/// </remarks>
		void WeakReferences(bool flag);

		/// <summary>configures the timer for WeakReference collection.</summary>
		/// <remarks>
		/// configures the timer for WeakReference collection.
		/// <br/><br/>The default setting is 1000 milliseconds.
		/// <br/><br/>Configure this setting to zero to turn WeakReference
		/// collection off.
		/// 
		/// </remarks>
		/// <param name="milliseconds">the time in milliseconds</param>
		void WeakReferenceCollectionInterval(int milliseconds);

		/// <summary>
		/// allows registering special TypeHandlers for customized marshalling
		/// and customized comparisons.
		/// </summary>
		/// <remarks>
		/// allows registering special TypeHandlers for customized marshalling
		/// and customized comparisons.
		/// </remarks>
		/// <param name="predicate">
		/// to specify for which classes and versions the
		/// TypeHandler is to be used.
		/// </param>
		/// <param name="typeHandler">to be used for the classes that match the predicate.</param>
		void RegisterTypeHandler(ITypeHandlerPredicate predicate, ITypeHandler4 typeHandler
			);

		/// <seealso cref="ICommonConfiguration.MaxStackDepth()">ICommonConfiguration.MaxStackDepth()
		/// 	</seealso>
		int MaxStackDepth();

		/// <seealso cref="ICommonConfiguration.MaxStackDepth(int)"></seealso>
		void MaxStackDepth(int maxStackDepth);
	}
}
