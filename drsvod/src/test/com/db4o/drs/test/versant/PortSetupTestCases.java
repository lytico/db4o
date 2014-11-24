package com.db4o.drs.test.versant;


import com.db4o.drs.versant.*;
import com.db4o.drs.versant.eventprocessor.EventProcessorFactory;
import com.db4o.drs.versant.eventprocessor.EventProcessorImpl;
import db4ounit.Assert;
import db4ounit.ConsoleTestRunner;

import java.io.File;
import java.io.IOException;
import java.net.ServerSocket;
import java.net.Socket;
import java.util.concurrent.*;

public class PortSetupTestCases extends VodProviderTestCaseBase {

    private static final int PORT_NUMBER = 4222;
    private ExecutorService executor;

    public static void main(String[] args) {
        new ConsoleTestRunner(PortSetupTestCases.class).run();
    }

    @Override
    public void setUp() {
        super.setUp();
        executor =  Executors.newSingleThreadExecutor(new ThreadFactory() {
			
			public Thread newThread(Runnable arg0) {
				Thread t = new Thread(arg0);
				t.setDaemon(true);
				return t;
			}
		});
    }

    @Override
    public void tearDown() {
        super.tearDown();
        executor.shutdownNow();
    }

    public void testDoNotConnectToEarly() throws Exception {
        final ServerSocket socket = new ServerSocket(4088);
        Future<Object> channel = executor.submit(new Callable<Object>() {
            public Object call() throws Exception {
                return socket.accept();
            }
        });
        new VodReplicationProvider(_vod);
        Thread.sleep(100);

        Assert.isFalse(channel.isDone());

        channel.cancel(false);
    }

    public void testOpensEventProcessorPort() throws Exception {
        File tempFile = File.createTempFile("logFile", "tmp");
        tempFile.deleteOnExit();

        EventConfiguration config = new EventConfiguration(
        		_vod.name(),
        		_vod.userName(),
        		_vod.passWord(),
                tempFile.getAbsolutePath(),
                "localhost",
                _vod.eventConfiguration().serverPort,
                "localhost",
                new FixedEventClientPortSelectionStrategy(4100),"localhost", PORT_NUMBER,true);


        final EventProcessorImpl eventProcessor = EventProcessorFactory.newInstance(config);
        executor.submit(new Runnable() {
            public void run() {
                eventProcessor.run();
            }
        });
        Thread.sleep(500);
        assertPortIsOpen(PORT_NUMBER);

        eventProcessor.stop();
    }

    private void assertPortIsOpen(int port) {
        Socket localhost = null;
        try {
            localhost = new Socket("localhost", port);
            localhost.getInputStream();
        } catch (IOException e) {
            Assert.fail("Couldn't connect to port "+port,e);
        }
        if (localhost != null) {
            try {
                localhost.close();
            } catch (IOException e) {
            }
        }
    }
}