/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package  com.db4o.config;
/**
 * configuration interface for fields of classes.
 * <br/><br/>
 * Use 
 * {@link com.db4o.config.ObjectClass#objectField(String)} to access this setting.<br/><br/>
 */
public interface ObjectField {
	
	/**
	 * sets cascaded activation behaviour.
	 * <br/><br/>
	 * Setting cascadeOnActivate to true will result in the activation
	 * of the object attribute stored in this field if the parent object
	 * is activated.
	 * <br/><br/>
	 * The default setting is <b>false</b>.<br/><br/>
	 * In client-server environment this setting should be used on both 
     * client and server. <br/><br/>
     * This setting can be applied to an open object container. <br/><br/>
	 * @param flag whether activation is to be cascaded to the member object.
	 * @see Configuration#activationDepth Why activation?
	 * @see ObjectClass#cascadeOnActivate
	 * @see com.db4o.ObjectContainer#activate
	 * @see com.db4o.ext.ObjectCallbacks Using callbacks
	 */
	public void cascadeOnActivate(boolean flag);
	
	
	/**
	 * sets cascaded delete behaviour.
	 * <br/><br/>
	 * Setting cascadeOnDelete to true will result in the deletion of
	 * the object attribute stored in this field on the parent object
	 * if the parent object is passed to 
	 * {@link com.db4o.ObjectContainer#delete}.
	 * <br/><br/>
	 * <b>Caution !</b><br/>
	 * This setting will also trigger deletion of the old member object, on
	 * calls to {@link com.db4o.ObjectContainer#store(Object) }.
	 * An example of the behaviour can be found in 
	 * {@link ObjectClass#cascadeOnDelete}
	 * <br/><br/>
	 * The default setting is <b>false</b>.<br/><br/>
	 * In client-server environment this setting should be used on both 
     * client and server. <br/><br/>
     * This setting can be applied to an open object container. <br/><br/>
	 * @param flag whether deletes are to be cascaded to the member object.
	 * @see ObjectClass#cascadeOnDelete
	 * @see com.db4o.ObjectContainer#delete
	 * @see com.db4o.ext.ObjectCallbacks Using callbacks
	 */
	public void cascadeOnDelete(boolean flag);
	
	
	/**
	 * sets cascaded update behaviour.
	 * <br/><br/>
	 * Setting cascadeOnUpdate to true will result in the update
	 * of the object attribute stored in this field if the parent object
	 * is passed to
	 * {@link com.db4o.ObjectContainer#store(Object)}.
	 * <br/><br/>
	 * The default setting is <b>false</b>.<br/><br/>
	 * In client-server environment this setting should be used on both 
     * client and server. <br/><br/>
     * This setting can be applied to an open object container. <br/><br/>
	 * @param flag whether updates are to be cascaded to the member object.
	 * @see com.db4o.ObjectContainer#store(Object)
	 * @see ObjectClass#cascadeOnUpdate
	 * @see ObjectClass#updateDepth
	 * @see com.db4o.ext.ObjectCallbacks Using callbacks
	 */
	public void cascadeOnUpdate(boolean flag);
	
	
	/**
	 * turns indexing on or off.
	 * <br/><br/>Field indices dramatically improve query performance but they may
	 * considerably reduce storage and update performance.<br/>The best benchmark whether
	 * or not an index on a field achieves the desired result is the completed application
	 * - with a data load that is typical for it's use.<br/><br/>This configuration setting
	 * is only checked when the {@link com.db4o.ObjectContainer} is opened. If the
	 * setting is set to true and an index does not exist, the index will be
	 * created. If the setting is set to false and an index does exist the
	 * index will be dropped.<br/><br/>
	 * In client-server environment this setting should be used on both 
     * client and server. <br/><br/>
     * If this setting is applied to an open ObjectContainer it will take an effect on the next
     * time ObjectContainer is opened.<br/><br/>
	 * @param flag specify true or false to turn indexing on for
	 * this field
	 */
	public void indexed(boolean flag);
	

    /**
	 * renames a field of a stored class.
	 * <br/><br/>Use this method to refactor classes.
     * <br/><br/>
     * In client-server environment this setting should be used on both 
     * client and server. <br/><br/>
     * This setting can NOT be applied to an open object container. <br/><br/>
     * @param newName the new field name.
     */
    public void rename (String newName);

}
