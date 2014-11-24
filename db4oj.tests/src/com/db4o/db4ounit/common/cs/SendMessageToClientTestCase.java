/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.cs;

import com.db4o.cs.internal.messages.*;

import db4ounit.*;


public class SendMessageToClientTestCase extends ClientServerTestCaseBase {

	public static void main(String[] args) {
		new SendMessageToClientTestCase().runNetworking();
	}
	
	public void test(){
	    if(isEmbedded()){
	        // No sending messages back and forth on MTOC.
	        return;
	    }
		serverDispatcher().write(Msg.OK);
		Msg msg = client().getResponse();
		Assert.areEqual(Msg.OK, msg);
	}

}
