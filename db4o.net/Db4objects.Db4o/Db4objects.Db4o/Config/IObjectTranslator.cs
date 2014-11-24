/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o;

namespace Db4objects.Db4o.Config
{
	/// <summary>translator interface to translate objects on storage and activation.</summary>
	/// <remarks>
	/// translator interface to translate objects on storage and activation.
	/// <br/><br/>
	/// By writing classes that implement this interface, it is possible to
	/// define how application classes are to be converted to be stored more efficiently.
	/// <br/><br/>
	/// Before starting a db4o session, translator classes need to be registered. An example:<br/>
	/// <code>
	/// IObjectClass oc = config.ObjectClass("Namespace.ClassName");<br/>
	/// oc.Translate(new FooTranslator());
	/// </code><br/><br/>
	/// </remarks>
	public interface IObjectTranslator
	{
		/// <summary>db4o calls this method during storage and query evaluation.</summary>
		/// <remarks>db4o calls this method during storage and query evaluation.</remarks>
		/// <param name="container">the ObjectContainer used</param>
		/// <param name="applicationObject">the Object to be translated</param>
		/// <returns>
		/// return the object to store.<br />It needs to be of the class
		/// <see cref="StoredClass()">StoredClass()</see>
		/// .
		/// </returns>
		object OnStore(IObjectContainer container, object applicationObject);

		/// <summary>db4o calls this method during activation.</summary>
		/// <remarks>db4o calls this method during activation.</remarks>
		/// <param name="container">the ObjectContainer used</param>
		/// <param name="applicationObject">the object to set the members on</param>
		/// <param name="storedObject">the object that was stored</param>
		void OnActivate(IObjectContainer container, object applicationObject, object storedObject
			);

		/// <summary>return the Class you are converting to.</summary>
		/// <remarks>return the Class you are converting to.</remarks>
		/// <returns>
		/// the Class of the object you are returning with the method
		/// <see cref="OnStore(Db4objects.Db4o.IObjectContainer, object)">OnStore(Db4objects.Db4o.IObjectContainer, object)
		/// 	</see>
		/// </returns>
		Type StoredClass();
	}
}
