/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o;

import com.db4o.ext.*;

/**
 * the db4o server interface.
 * <br><br>- db4o servers can be opened with {@link Db4o#openServer(String, int)}.<br>
 * - Direct in-memory connections to servers can be made with {@link #openClient()} <br>
 * - TCP connections are available through {@link Db4o#openClient(String, int, String, String)}.
 * <br><br>Before connecting clients over TCP, you have to 
 * {@link #grantAccess(String, String)} to the username and password combination
 * that you want to use. 
 * @see Db4o#openServer(java.lang.String, int) Db4o.openServer
 * @see ExtObjectServer ExtObjectServer for extended functionality
 */
public interface ObjectServer {
    /**
    * closes the <code>ObjectServer</code> and writes all cached data.
    * <br><br>
    * @return true - denotes that the last instance connected to the
    * used database file was closed.
    */
    public boolean close();

    /**
     * returns an ObjectServer with extended functionality.
     * <br><br>Use this method as a convient accessor to extended methods. 
     * Every ObjectServer can be casted to an ExtObjectServer.
     * <br><br>The functionality is split to two interfaces to allow newcomers to
     * focus on the essential methods.
     */
    public ExtObjectServer ext();

    /**
     * grants client access to the specified user with the specified password.
     * <br><br>If the user already exists, the password is changed to 
     * the specified password.<br><br>
     * @param userName the name of the user
     * @param password the password to be used
     */
    public void grantAccess(String userName, String password);

    /**
     * opens a client against this server.
     * <br><br>A client opened with this method operates within the same VM
     * as the server. Since an embedded client can use direct communication, without
     * an in-between socket connection, performance will be better than a client
     * opened with 
     * {@link Db4o#openClient(java.lang.String, int, java.lang.String, java.lang.String)}
     * 
     * <br><br>Every client has it's own transaction and uses it's own cache
     * for it's own version of all peristent objects.  
     * 
     */
    public ObjectContainer openClient();

}
