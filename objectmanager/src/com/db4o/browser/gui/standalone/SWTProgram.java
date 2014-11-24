/*
 * Copyright (C) 2005 db4objects Inc.  http://www.db4o.com
 */
package com.db4o.browser.gui.standalone;

import java.util.Iterator;
import java.util.LinkedList;

import org.eclipse.swt.widgets.Display;
import org.eclipse.swt.widgets.Shell;

import com.swtworkbench.community.xswt.metalogger.Logger;


/**
 * A formalization of the standard SWT program.
 * 
 * @author djo
 */
public abstract class SWTProgram {
    
    private static LinkedList shells = new LinkedList();
    
    private static boolean aShellIsOpen() {
        for (Iterator i = shells.iterator(); i.hasNext();) {
			Shell shell = (Shell) i.next();
			if (shell.isDisposed()) {
                i.remove();
            }
		}
        return !shells.isEmpty();
    }
    
    private static ICloseListener closeListener = null;
    
    public static void registerCloseListener(ICloseListener closeListener) {
        SWTProgram.closeListener = closeListener;
    }
    
    private static void fireCloseListener() {
        if (closeListener != null) {
            closeListener.closing();
        }
    }
    
    /**
     * Construct a new top-level Shell.
     * 
     * @param display The display on which to create the Shell
     * @return the new Shell
     */
    public static Shell newShell(Display display) {
        Shell shell = new Shell(display);
        shells.add(shell);
        return shell;
    }

    /**
     * Method run.
     * 
     * Run a minimal SWTProgram.  This version aborts if an uncaught exception
     * occurs.
     * 
     * @param controlFactory An IControlFactory that will define the initial 
     * Shell's UI.
     */
    public static void run(IControlFactory controlFactory) {
        Display display = new Display();
        Shell shell = newShell(display);
        
        controlFactory.createContents(shell);
        
        shell.open();
        
        while (aShellIsOpen()) {
        	if (!display.readAndDispatch()) display.sleep();
        }
        display.dispose();
        
        fireCloseListener();
    }
    
    /**
     * Method runWithLog.
     * 
     * Run a minimal SWTProgram with exception logging.  This version logs
     * any uncaught exceptions via the metalogging framework.
     * 
     * @param controlFactory An IControlFactory that will define the initial 
     * Shell's UI.
     */
    public static void runWithLog(IControlFactory controlFactory) {
        Display display = new Display();
        Shell shell = newShell(display);
        
        controlFactory.createContents(shell);
        
        shell.open();
        
        while (aShellIsOpen()) {
            try {
            	if (!display.readAndDispatch()) display.sleep();
            } catch (Throwable t) {
                Logger.log().error(t, "Uncaught exception in SWT event loop");
            }
        }
        display.dispose();
        
        fireCloseListener();
    }
}


