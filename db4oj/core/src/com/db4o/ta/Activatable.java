/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
package com.db4o.ta;

import com.db4o.activation.*;

/**
 * The Activatable interface must be implemented by classes in order to support transparent
 * activation/persistence.<br>
 * <br>
 * 
 * The Activatable interface may be added to persistent classes by hand or by
 * using the db4o enhancer. For further information on the enhancer see the
 * chapter "Enhancement" in the db4o reference documentation.<br>
 * <br>
 * 
 * The basic idea for Transparent Activation is:<br>
 * Objects have an activation depth of 0, so they are not activated
 * at all. Whenever a method is called on such an object, the first thing to do
 * before actually executing the method body is to activate the object to
 * populating its direct members.<br>
 * <br>
 * 
 * To illustrate this approach, we will use the following simple class.<br>
 * <pre class="prettyprint"><code>
 * public class Item {
 *      private Item next;
 *   
 *      public Item(Item next) {
 *          this.next = next;
 *      }
 *   
 *      public Item next() {
 *          return next;
 *      }
 *      public void setNext(Item itemToBeSet) {
 *          this.next = itemToBeSet;
 *      }
 *      // ...
 * }</code></pre>
 * 
 * The basic sequence of actions to get the above scheme to work is the
 * following:<br>
 * <br>
 * 
 * Whenever an object is instantiated from db4o, the database registers an
 * activator for this object. To enable this, the object has to implement the
 * Activatable interface and provide the according bind(Activator) method. The
 * default implementation of the bind method will simply store the given
 * activator reference for later use.<br>
 * <pre class="prettyprint"><code>
 * public class Item implements Activatable {
 *     private Item next;
 *     private transient Activator activator;
 * 
 *     public void bind(Activator activator) {
 *         if (this.activator == activator) {
 *              return;
 *         }
 *         if (activator != null && null != this.activator) {
 *              throw new IllegalStateException("Object can only be bound to one activator");
 *         }
 *         this.activator = activator;
 *     }
 *     public void activate(ActivationPurpose activationPurpose) {
 *         if(null!=activator){
 *              activator.activate(activationPurpose);
 *         }
 *     }
 *     // ...
 * }</code></pre>
 * 
 * The first action in every method body of an activatable object should be a
 * call to the corresponding Activator's activate() method with the given purpose.
 * If the method is reading the purpose should be {@link ActivationPurpose#READ},
 * for writing {@link ActivationPurpose#WRITE}. <br>
 * <pre class="prettyprint"><code>
 * public class Item implements Activatable {
 *     private Item next;
 *     private transient Activator activator;
 *
 *     public void bind(Activator activator) {
 *         if (this.activator == activator) {
 *              return;
 *         }
 *         if (activator != null && null != this.activator) {
 *              throw new IllegalStateException("Object can only be bound to one activator");
 *         }
 *         this.activator = activator;
 *     }
 *     public void activate(ActivationPurpose activationPurpose) {
 *         if(null!=activator){
 *              activator.activate(activationPurpose);
 *         }
 *     }
 *     public Item next() {
 *         activate(ActivationPurpose.READ);
 *         return next;
 *     }
 *     public void setNext(Item itemToBeSet) {
 *         activate(ActivationPurpose.Write);
 *         this.next = itemToBeSet;
 *     }
 *     // ...
 * }</code></pre>
 *
 * You always need to call activate() before any data access in the object. Otherwise transparent activation / persistence will not work.
 * Since this process is error prone we recommend to use the enhancer tools shipped with db4o.
 *
 * The activate() method will check whether the object is already activated.
 * If this is not the case, it will request the container to activate the object
 * to level 1 and set the activated flag accordingly.<br>
 * <br>
 *
 * To instruct db4o to actually use these hooks (i.e. to register the database
 * when instantiating an object), {@link TransparentActivationSupport} or {@link TransparentPersistenceSupport}  has to be
 * registered with the db4o configuration.<br>
 * <br>
 * <pre class="prettyprint"><code>
 * EmbeddedConfiguration config = ... // your configuration
 * configuration.common().add(new TransparentActivationSupport());
 * </code></pre>
 * 
 * If you implement this interface manually and intend to pass this class through
 * the db4o bytecode instrumentation process, make sure you also implement the
 * {@link ActivatableInstrumented} marker interface.
 */
public interface Activatable {

	/**
	 * Called by db4o after the object instantiation.
     * This method is called to bind the object to the current activator<br/>
	 * <br/>
	 * The recommended implementation of this method is to store the passed
	 * {@link Activator} in a transient field of the object.
	 * 
	 * @param activator the Activator instance to bind
	 */
	void bind(Activator activator);

	/**
	 * Should be called by every reading field access of an object. <br/>
	 * <br/>
	 * The recommended implementation of this method is to call
	 * {@link Activator#activate(ActivationPurpose)} on the {@link Activator}
	 * that was previously passed to {@link #bind(Activator)}.
	 * 
	 * @param purpose Whereever this object is accessed to read or write. See {@link ActivationPurpose}
	 */
	void activate(ActivationPurpose purpose);
}