/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package  com.db4o.config;

/**
 * configuration interface for classes.
 * <br><br><b>Examples: ../com/db4o/samples/translators/Default.java.</b><br><br>
 * Use the global Configuration object to configure db4o before opening an
 * {@link com.db4o.ObjectContainer ObjectContainer}.<br><br>
 * <b>Example:</b><br>
 * <code>
 * Configuration config = Db4o.configure();<br>
 * ObjectClass oc = config.objectClass("package.className");<br>
 * oc.updateDepth(3);<br>
 * oc.minimumActivationDepth(3);<br>
 * </code>
 */
public interface ObjectClass {
    
    /**
     * advises db4o to try instantiating objects of this class with/without
     * calling constructors.
     * <br><br>
     * Not all JDKs / .NET-environments support this feature. db4o will
     * attempt, to follow the setting as good as the enviroment supports.
     * In doing so, it may call implementation-specific features like
     * sun.reflect.ReflectionFactory#newConstructorForSerialization on the
     * Sun Java 1.4.x/5 VM (not available on other VMs) and 
     * FormatterServices.GetUninitializedObject() on
     * the .NET framework (not available on CompactFramework).<br><br>
     * This setting may also be set globally for all classes in
     * {@link Configuration#callConstructors(boolean)}.<br><br>
     * @param flag - specify true, to request calling constructors, specify
     * false to request <b>not</b> calling constructors.
	 * @see Configuration#callConstructors
     */
    public void callConstructor(boolean flag);
	
	
	/**
	 * sets cascaded activation behaviour.
	 * <br><br>
	 * Setting cascadeOnActivate to true will result in the activation
	 * of all member objects if an instance of this class is activated.
	 * <br><br>
	 * The default setting is <b>false</b>.<br><br>
	 * @param flag whether activation is to be cascaded to member objects.
	 * @see ObjectField#cascadeOnActivate
	 * @see com.db4o.ObjectContainer#activate
	 * @see com.db4o.ext.ObjectCallbacks Using callbacks
	 * @see Configuration#activationDepth Why activation?
	 */
	public void cascadeOnActivate(boolean flag);


	/**
	 * sets cascaded delete behaviour.
	 * <br><br>
	 * Setting cascadeOnDelete to true will result in the deletion of
	 * all member objects of instances of this class, if they are 
	 * passed to
     * {@link com.db4o.ObjectContainer#delete(Object)}. 
	 * <br><br>
	 * <b>Caution !</b><br>
	 * This setting will also trigger deletion of old member objects, on
	 * calls to {@link com.db4o.ObjectContainer#set(Object)}.<br><br>
	 * An example of the behaviour:<br>
	 * <code>
	 * ObjectContainer con;<br>
	 * Bar bar1 = new Bar();<br>
	 * Bar bar2 = new Bar();<br>
	 * foo.bar = bar1;<br>
	 * con.set(foo);  // bar1 is stored as a member of foo<br>
	 * foo.bar = bar2;<br>
	 * con.set(foo);  // bar2 is stored as a member of foo
	 * </code><br>The last statement will <b>also</b> delete bar1 from the
	 * ObjectContainer, no matter how many other stored objects hold references
	 * to bar1.
	 * <br><br>
	 * The default setting is <b>false</b>.<br><br>
	 * @param flag whether deletes are to be cascaded to member objects.
	 * @see ObjectField#cascadeOnDelete
	 * @see com.db4o.ObjectContainer#delete
	 * @see com.db4o.ext.ObjectCallbacks Using callbacks
	 */
	public void cascadeOnDelete(boolean flag);
	
	
	/**
	 * sets cascaded update behaviour.
	 * <br><br>
	 * Setting cascadeOnUpdate to true will result in the update
	 * of all member objects if a stored instance of this class is passed
	 * to {@link com.db4o.ObjectContainer#set(Object)}.<br><br>
	 * The default setting is <b>false</b>.<br><br>
	 * @param flag whether updates are to be cascaded to member objects.
	 * @see ObjectField#cascadeOnUpdate
	 * @see com.db4o.ObjectContainer#set
	 * @see com.db4o.ext.ObjectCallbacks Using callbacks
	 */
	public void cascadeOnUpdate(boolean flag);
	
	
	/**
	 * registers an attribute provider for special query behavior.
	 * <br><br>The query processor will compare the object returned by the
	 * attribute provider instead of the actual object, both for the constraint
	 * and the candidate persistent object.<br><br> Preinstalled attribute
	 * providers are documented
	 * in the sourcecode of 
	 * com.db4o.samples.translators.Default.java#defaultConfiguration().<br><br>
	 * @param attributeProvider the attribute provider to be used
	 */
	public void compare(ObjectAttribute attributeProvider);
	
	
    /**
     * Must be called before databases are created or opened
     * so that db4o will control versions and generate UUIDs
     * for objects of this class, which is required for using replication.
     * 
     * @param setting 
     */
    public void enableReplication(boolean setting);

	
	/**
     * generate UUIDs for stored objects of this class.
     * 
     * @param setting 
     */
    public void generateUUIDs(boolean setting);

    
    /**
     * generate version numbers for stored objects of this class.
     * 
     * @param setting
     */
    public void generateVersionNumbers(boolean setting);
    

    /**
	 * sets the maximum activation depth to the desired value.
	 * <br><br>A class specific setting overrides the
     * {@link Configuration#activationDepth(int) global setting}
     * <br><br>
     * @param depth the desired maximum activation depth
	 * @see Configuration#activationDepth Why activation?
	 * @see ObjectClass#cascadeOnActivate
     */
    public void maximumActivationDepth (int depth);



    /**
	 * sets the minimum activation depth to the desired value.
	 * <br><br>A class specific setting overrides the
     * {@link Configuration#activationDepth(int) global setting}
	 * <br><br>
     * @param depth the desired minimum activation depth
	 * @see Configuration#activationDepth Why activation?
	 * @see ObjectClass#cascadeOnActivate
     */
    public void minimumActivationDepth (int depth);


    /**
	 * returns an {@link ObjectField ObjectField} object
	 * to configure the specified field.
	 * <br><br>
     * @param fieldName the fieldname of the field to be configured.<br><br>
     * @return an instance of an {@link ObjectField ObjectField}
	 *  object for configuration.
     */
    public ObjectField objectField (String fieldName);
    
    
    /**
     * turns on storing static field values for this class.
     * <br><br>By default, static field values of classes are not stored
     * to the database file. By turning the setting on for a specific class
     * with this switch, all <b>non-simple-typed</b> static field values of this
     * class are stored the first time an object of the class is stored, and
     * restored, every time a database file is opened afterwards.
     * <br><br>The setting will be ignored for simple types.
     * <br><br>Use this setting for constant static object members.
     * <br><br>This option will slow down the process of opening database
     * files and the stored objects will occupy space in the database file.
     */
    public void persistStaticFieldValues();
    
    
    /**
     * creates a temporary mapping of a persistent class to a different class.
     * <br><br>If meta information for this ObjectClass has been stored to
     * the database file, it will be read from the database file as if it
     * was representing the class specified by the clazz parameter passed to
     * this method. 
     * The clazz parameter can be any of the following:<br>
     * - a fully qualified classname as a String.<br>
     * - a Class object.<br>
     * - any other object to be used as a template.<br><br>
     * This method will be ignored if the database file already contains meta
     * information for clazz.
     * @param clazz class name, Class object, or example object.<br><br>
     */
    public void readAs(Object clazz);


    /**
	 * renames a stored class.
	 * <br><br>Use this method to refactor classes.
     * <br><br><b>Examples: ../com/db4o/samples/rename.</b><br><br>
     * @param newName the new fully qualified classname.
     */
    public void rename (String newName);



    /**
	 * allows to specify if transient fields are to be stored.
	 * <br>The default for every class is <code>false</code>.<br><br>
     * @param flag whether or not transient fields are to be stored.
     */
    public void storeTransientFields (boolean flag);



    /**
	 * registers a translator for this class.
     * <br><br>
	 * Preinstalled translators are documented in the sourcecode of
	 * com.db4o.samples.translators.Default.java#defaultConfiguration().
	 * <br><br>Example translators can also be found in this folder.<br><br>
     * @param translator this may be an {@link ObjectTranslator ObjectTranslator}
     *  or an {@link ObjectConstructor ObjectConstructor}
	 * @see ObjectTranslator
	 * @see ObjectConstructor
     */
    public void translate (ObjectTranslator translator);



    /**
	 * specifies the updateDepth for this class.
	 * <br><br>see the documentation of
	 * {@link com.db4o.ObjectContainer#set(Object)}
	 * for further details.<br><br>
	 * The default setting is 0: Only the object passed to
	 * {@link com.db4o.ObjectContainer#set(Object)} will be updated.<br><br>
     * @param depth the depth of the desired update for this class.
	 * @see Configuration#updateDepth
	 * @see ObjectClass#cascadeOnUpdate
	 * @see ObjectField#cascadeOnUpdate
	 * @see com.db4o.ext.ObjectCallbacks Using callbacks
     */
    public void updateDepth (int depth);
}



