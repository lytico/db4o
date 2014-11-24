/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Activation;

namespace Db4objects.Db4o.TA
{
	/// <summary>
	/// The IActivatable interface must be implemented by classes in order to support transparent
	/// activation/persistence
	/// <br/>
	/// <br/>
	/// The IActivatable interface may be added to persistent classes by hand
	/// or by using the db4o instrumentation (Db4oTools).  For further information instrumentation see the
	/// chapter "Enhancement" in the db4o reference documentation.
	/// </summary>
	/// <remarks>
	/// The IActivatable interface must be implemented by classes in order to support transparent
	/// activation/persistence
	/// <br/>
	/// <br/>
	/// The IActivatable interface may be added to persistent classes by hand
	/// or by using the db4o instrumentation (Db4oTools).  For further information instrumentation see the
	/// chapter "Enhancement" in the db4o reference documentation.
	/// <br/>
	/// <br/>
	/// The basic idea for Transparent Activation is:
	/// <br/>
	/// Objects have an activation depth of 0, so they are not activated
	/// at all. Whenever a method is called on such an object, the first thing to do
	/// before actually executing the method body is to activate the object to
	/// populating its direct members.
	/// <br/>
	/// <br/>
	/// To illustrate this approach, we will use the following simple class.
	/// <br/>
	/// <br/>
	/// <pre><code>
	/// public class Item
	/// {
	///    private Item next;
	/// 
	///    public Item(Item next)
	///    {
	///       this.next = next;
	///    }
	/// 
	///    public Item Next
	///    {
	///       get { return next; }
	///       set { next = value; }
	///    }
	/// 
	///    // ...
	/// }
	/// }</code></pre>
	/// The basic sequence of actions to get the above scheme to work is the
	/// following:<br/>
	/// <br/>
	/// Whenever an object is instantiated from db4o, the database registers an
	/// activator for this object. To enable this, the object has to implement the
	/// Activatable interface and provide the according bind(Activator) method. The
	/// default implementation of the bind method will simply store the given
	/// activator reference for later use.<br/>
	/// <br/>
	/// <code>
	/// public class Item : IActivatable
	/// {
	///    private Item next;
	///    [NonSerialized] private IActivator activator;
	/// 
	///    public void Bind(IActivator activator)
	///    {
	///       if (this.activator == activator)
	///       {
	///          return;
	///       }
	///       if (activator != null &amp;&amp; null != this.activator)
	///       {
	///          throw new InvalidOperationException("Object can only be bound to one activator");
	///       }
	///       this.activator = activator;
	///    }
	/// 
	///    public void Activate(ActivationPurpose activationPurpose)
	///    {
	///       if (null != activator)
	///       {
	///          activator.Activate(activationPurpose);
	///       }
	///    }
	/// 
	///    // ...
	/// }
	/// </code>
	/// The first action in every method body of an activatable object should be a
	/// call to the corresponding activator's Activate() method with the given purpose.
	/// If the method is reading the purpose should be <see cref="ActivationPurpose.Read"/>,
	/// for writing <see cref="ActivationPurpose.Write"/>. <br/>
	/// <br/>
	/// <code>public class Item : IActivatable
	/// {
	///    private Item next;
	///    [NonSerialized] private IActivator activator;
	/// 
	///    public void Bind(IActivator activator)
	///    {
	///       if (this.activator == activator)
	///       {
	///          return;
	///       }
	///       if (activator != null &amp;&amp; null != this.activator)
	///       {
	///          throw new InvalidOperationException("Object can only be bound to one activator");
	///       }
	///       this.activator = activator;
	///    }
	/// 
	///    public void Activate(ActivationPurpose activationPurpose)
	///    {
	///       if (null != activator)
	///       {
	///          activator.Activate(activationPurpose);
	///       }
	///    }
	/// 
	///    public Item Next
	///    {
	///       get
	///       {
	///          Activate(ActivationPurpose.Read);
	///          return next;
	///       }
	///       set
	///       {
	///          Activate(ActivationPurpose.Write);
	///          next = value;
	///       }
	///    }
	/// 
	///    // ...
	/// }
	/// </code>
	/// You always need to call Activate() before any data access in the object. Otherwise transparent activation / persistence will not work.
	/// Since this process is error prone we recommend to use the enhancer tools shipped with db4o.
	/// 
	/// The Activate() method will check whether the object is already activated.
	/// If this is not the case, it will request the container to activate the object
	/// to level 1 and set the activated flag accordingly.<br/>
	/// <br/>
	/// 
	/// To instruct db4o to actually use these hooks (i.e. to register the database
	/// when instantiating an object), <see cref="TransparentActivationSupport"/> or <see cref="TransparentPersistenceSupport"/>  has to be
	/// registered with the db4o configuration.<br/>
	/// <br/>
	/// <code>
	/// ICommonConfiguration config = ... // your configuration
	/// config.Add(new TransparentActivationSupport());
	/// </code>
	/// </remarks>
	public interface IActivatable
	{
		/// <summary>Called by db4o after the object instantiation.</summary>
		/// <remarks>
		/// Called by db4o after the object instantiation.
		/// This method is called to bind the object to the current activator<br/>
		/// <br/>
		/// The recommended implementation of this method is to store the passed
		/// <see cref="Db4objects.Db4o.Activation.IActivator">Db4objects.Db4o.Activation.IActivator
		/// 	</see>
		/// in a transient field of the object.
		/// </remarks>
		/// <param name="activator">the Activator instance to bind</param>
		void Bind(IActivator activator);

		/// <summary>Should be called by every reading field access of an object.</summary>
		/// <remarks>
		/// Should be called by every reading field access of an object. <br/>
		/// <br/>
		/// The recommended implementation of this method is to call
		/// <see cref="Db4objects.Db4o.Activation.IActivator.Activate(Db4objects.Db4o.Activation.ActivationPurpose)
		/// 	">Db4objects.Db4o.Activation.IActivator.Activate(Db4objects.Db4o.Activation.ActivationPurpose)
		/// 	</see>
		/// on the
		/// <see cref="Db4objects.Db4o.Activation.IActivator">Db4objects.Db4o.Activation.IActivator
		/// 	</see>
		/// that was previously passed to
		/// <see cref="Bind(Db4objects.Db4o.Activation.IActivator)">Bind(Db4objects.Db4o.Activation.IActivator)
		/// 	</see>
		/// .
		/// </remarks>
		/// <param name="purpose">
		/// Whereever this object is accessed to read or write. See
		/// <see cref="Db4objects.Db4o.Activation.ActivationPurpose">Db4objects.Db4o.Activation.ActivationPurpose
		/// 	</see>
		/// </param>
		void Activate(ActivationPurpose purpose);
	}
}
