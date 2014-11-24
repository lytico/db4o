/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o;
using Db4objects.Db4o.Config;

namespace Db4objects.Db4o.Config
{
	/// <summary>
	/// interface to allow instantiating objects by calling specific constructors.
	/// 
	/// </summary>
	/// <remarks>
	/// interface to allow instantiating objects by calling specific constructors.
	/// <br/><br/>
	/// By writing classes that implement this interface, it is possible to
	/// define which constructor is to be used during the instantiation of a stored object.
	/// <br/><br/>
	/// Before starting a db4o session, translator classes that implement the ObjectConstructor or
	/// <see cref="IObjectTranslator">IObjectTranslator</see>
	/// need to be registered.<br/><br/>
	/// Example:<br/>
	/// <code>
	/// IConfiguration config = Db4oFactory.Configure();<br/>
	/// IObjectClass oc = config.ObjectClass("Namespace.ClassName");<br/>
	/// oc.Translate(new FooTranslator());
	/// </code><br/><br/>
	/// </remarks>
	public interface IObjectConstructor : IObjectTranslator
	{
		/// <summary>db4o calls this method when a stored object needs to be instantiated.</summary>
		/// <remarks>
		/// db4o calls this method when a stored object needs to be instantiated.
		/// <br /><br />
		/// </remarks>
		/// <param name="container">the ObjectContainer used</param>
		/// <param name="storedObject">
		/// the object stored with
		/// <see cref="IObjectTranslator.OnStore(Db4objects.Db4o.IObjectContainer, object)">ObjectTranslator.onStore
		/// 	</see>
		/// .
		/// </param>
		/// <returns>the instantiated object.</returns>
		object OnInstantiate(IObjectContainer container, object storedObject);
	}
}
