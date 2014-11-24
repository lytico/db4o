package server;

import com.db4o.messaging.MessageRecipient;
import com.db4o.Db4o;
import com.db4o.ObjectServer;
import com.db4o.ObjectContainer;
import java.io.File;

public class StartServer implements ServerConfiguration, MessageRecipient, Runnable{

	/**
	 * setting the value to true denotes that the server should be closed
	 */
	private boolean stop = false;

	/**
	 * starts a db4o server using the configuration from {@link ServerConfiguration}.
	 */
	public static void main(String[] arguments) {
		new StartServer().run();
	}

	/**
	 * opens the ObjectServer, and waits forever until close() is called
	 * or a StopServer message is being received.
	 */
	public void run(){
		synchronized(this){
			ObjectServer db4oServer = Db4o.openServer(getDbDirectory() + FILE, PORT);
			db4oServer.grantAccess(USER, PASS);

			// Using the messaging functionality to redirect all
			// messages to this.processMessage
			db4oServer.ext().configure().clientServer().setMessageRecipient(this);

			// to identify the thread in a debugger
			Thread.currentThread().setName(this.getClass().getName());

			// We only need low priority since the db4o server has
			// it's own thread.
			Thread.currentThread().setPriority(Thread.MIN_PRIORITY);
			try {
					if(! stop){
						// wait forever for notify() from close()
						this.wait(Long.MAX_VALUE);
					}
			} catch (Exception e) {
				e.printStackTrace();
			}
			db4oServer.close();
		}
	}
      private static File getDbDirectory() {
        String dbfile = System.getProperty("user.home") + "/db4o/data/";
        File f = new File(dbfile);
        if (!f.exists()) {
            f.mkdirs();
        }
        return f;
    }

    /**
	 * messaging callback
	 * @see com.db4o.messaging.MessageRecipient#processMessage(ObjectContainer, Object)
	 */
	public void processMessage(ObjectContainer con, Object message) {
		if(message instanceof StopServer){
			close();
		}
	}

	/**
	 * closes this server.
	 */
	public void close(){
		synchronized(this){
			stop = true;
			this.notify();
		}
	}
}