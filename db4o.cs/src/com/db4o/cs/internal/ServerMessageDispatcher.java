/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.cs.internal;

import com.db4o.cs.internal.messages.*;
import com.db4o.internal.*;


/**
 * @exclude
 */
public interface ServerMessageDispatcher extends ClientConnection, MessageDispatcher, CommittedCallbackDispatcher {

	public void queryResultFinalized(int queryResultID);

	public Socket4Adapter socket();

	public int dispatcherID();

	public LazyClientObjectSetStub queryResultForID(int queryResultID);

	public void switchToMainFile();

	public void switchToFile(MSwitchToFile file);

	public void useTransaction(MUseTransaction transaction);

	public void mapQueryResultToID(LazyClientObjectSetStub stub, int queryResultId);

	public ObjectServerImpl server();

	public void login();

	public boolean close();
	
	public boolean close(ShutdownMode mode);
	
	public void closeConnection();

	public void caresAboutCommitted(boolean care);
	
	public boolean caresAboutCommitted();
	
	public boolean write(Msg msg);

	public CallbackObjectInfoCollections committedInfo();

	public ClassInfoHelper classInfoHelper();

	public boolean processMessage(Msg message);

	public void join() throws InterruptedException;

	public void setDispatcherName(String name);
	
	public Transaction transaction();

}
