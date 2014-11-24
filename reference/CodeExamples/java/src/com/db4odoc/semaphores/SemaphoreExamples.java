package com.db4odoc.semaphores;

import com.db4o.ObjectContainer;
import com.db4o.ObjectServer;
import com.db4o.cs.Db4oClientServer;


public class SemaphoreExamples {
    private static final int PORT = 1337;
    private static final String USER_AND_PASSWORD = "sa";

    public static void main(String[] args) {
        ObjectServer server = Db4oClientServer.openServer("database.db4o", PORT);
        try {
            server.grantAccess(USER_AND_PASSWORD, USER_AND_PASSWORD);
            grabSemaphore();
            tryGrabSemaphore();

        } finally {
            server.close();
        }
    }

    private static void tryGrabSemaphore() {
        ObjectContainer container = openClient();
        try{
            boolean hasLock = container.ext().setSemaphore("LockName",1000);
            if (hasLock) {
                System.out.println("Could get lock");
            } else{
                System.out.println("Couldn't get lock");
            }
            grabSemaphore();
        }finally {
            container.close();
        }
    }

    private static void grabSemaphore() {
        ObjectContainer container = openClient();

        // #example: Grab a semaphore
        // Grab the lock. Specify the name and a timeout in milliseconds
        boolean hasLock = container.ext().setSemaphore("LockName", 1000);
        try {
            // you need to check the lock
            if (hasLock) {
                System.out.println("Could get lock");
            } else{
                System.out.println("Couldn't get lock");
            }
        } finally {
            // release the lock
            container.ext().releaseSemaphore("LockName");
        }
        // #end example
    }

    private static ObjectContainer openClient() {
        return Db4oClientServer.openClient("localhost", PORT, USER_AND_PASSWORD, USER_AND_PASSWORD);
    }
}
