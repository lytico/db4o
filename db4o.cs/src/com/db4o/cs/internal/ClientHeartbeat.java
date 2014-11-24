/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.cs.internal;

import com.db4o.cs.internal.messages.*;
import com.db4o.foundation.*;
import com.db4o.internal.*;

/**
 * @exclude
 */
public class ClientHeartbeat implements Runnable {
    
    private SimpleTimer _timer; 
    
    private final ClientObjectContainer _container;

    public ClientHeartbeat(ClientObjectContainer container) {
        _container = container;
        _timer = new SimpleTimer(this, frequency(container.configImpl()));
    }
    
    private int frequency(Config4Impl config){
        return Math.min(config.timeoutClientSocket(), config.timeoutServerSocket()) / 4;
    }

    public void run() {
        _container.writeMessageToSocket(Msg.PING);
    }
    
    public void start(){
    	_container.threadPool().start("db4o client heartbeat", _timer);
    }

    public void stop() {
        if (_timer == null){
            return;
        }
        _timer.stop();
        _timer = null;
    }

}
