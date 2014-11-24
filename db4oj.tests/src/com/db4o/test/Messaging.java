/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test;

import com.db4o.messaging.*;

public class Messaging implements MessageRecipient{
	
	static final String MSG = "hibabe";
	
	public Object lastMessage;
	
	public String messageString;
	
	public void test(){
		
		if(Test.isClientServer()){
			
			Test.server().ext().configure().clientServer().setMessageRecipient(this);
            
			MessageSender sender = Test.objectContainer().configure().clientServer().getMessageSender();
			this.messageString = MSG; 
			sender.send(this);
			
			synchronized(this) {
			    try {
                    this.wait(5000);
                } catch (InterruptedException e1) {
                    e1.printStackTrace();
                }
			}
			
			Test.ensure(lastMessage instanceof Messaging);
			Messaging received = (Messaging)lastMessage;
			Test.ensure(received.messageString.equals(MSG));
		}
	}

    public void processMessage(MessageContext con, Object message) {
		synchronized(this) {
	    	lastMessage = message;
	    	this.notify();
		}
        
    }

}
