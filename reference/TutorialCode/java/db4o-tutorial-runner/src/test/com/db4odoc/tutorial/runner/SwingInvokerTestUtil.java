package com.db4odoc.tutorial.runner;

/**
 * @author roman.stoffel@gamlor.info
 * @since 20.04.11
 */

import com.db4odoc.tutorial.utils.Modifiable;
import com.db4odoc.tutorial.utils.NoArgAction;
import org.testng.Assert;

import javax.swing.*;

/**
 * Provides a small utitily to run test in the swing-dispatcher.
 * <br/>
 * tests itself
 *
 * @author <a href="mailto:roman.stoffel@gamlor.info">Roman Stoffel</a>
 */
public final class SwingInvokerTestUtil {
    private SwingInvokerTestUtil() {
    }

    public static void runTestInSwingInvoker(final NoArgAction toRun) {
        if (SwingUtilities.isEventDispatchThread()) {
            toRun.invoke();
        } else {
            runOnSwingDispatcherThread(toRun);
        }
    }

    private static void runOnSwingDispatcherThread(final NoArgAction toRun) {
        final Modifiable<Throwable> exceptions = new Modifiable<Throwable>(null);
        final Object waitingObject = new Object();
        final Modifiable<Boolean> ran = Modifiable.create(false);
        SwingUtilities.invokeLater(new Runnable() {
            @Override
            public void run() {
                try {
                    toRun.invoke();
                } catch (AssertionError e) {
                    exceptions.setValue(e);
                } catch (Exception e) {
                    exceptions.setValue(e);
                } finally {
                    synchronized (waitingObject) {
                        ran.setValue(true);
                        waitingObject.notify();
                    }
                }
            }
        });
        synchronized (waitingObject) {
            while (!ran.getValue()) {
                try {
                    waitingObject.wait();
                } catch (InterruptedException e) {
                    Assert.fail("Unexpected interrupt", e);
                }
            }
        }
        if (null != exceptions.getValue()) {
            Assert.fail("In Swing-UI, see cause", exceptions.getValue());
        }
    }
}
