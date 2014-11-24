package com.db4o.browser.rcp;

import java.io.PrintStream;
import java.util.Date;

import org.eclipse.core.runtime.IPlatformRunnable;
import org.eclipse.swt.widgets.Display;
import org.eclipse.ui.PlatformUI;

import com.db4o.Db4o;
import com.db4o.browser.gui.standalone.PrintStreamLogger;
import com.db4o.browser.gui.standalone.StandaloneBrowser;
import com.swtworkbench.community.xswt.metalogger.FileLogger;
import com.swtworkbench.community.xswt.metalogger.Logger;
import com.swtworkbench.community.xswt.metalogger.StdLogger;
import com.swtworkbench.community.xswt.metalogger.TeeLogger;

/**
 * This class controls all aspects of the application's execution
 */
public class Application implements IPlatformRunnable {
	public static String[] args = null;

	/* (non-Javadoc)
	 * @see org.eclipse.core.runtime.IPlatformRunnable#run(java.lang.Object)
	 */
	public Object run(Object args) throws Exception {
        PrintStreamLogger db4ologger = new PrintStreamLogger();
        Logger.setLogger(new TeeLogger(new StdLogger(), new FileLogger(StandaloneBrowser.getLogPath(StandaloneBrowser.LOGFILE), StandaloneBrowser.getLogPath(StandaloneBrowser.LOGCONFIG))));
        Db4o.configure().setOut(new PrintStream(db4ologger, true));
        Logger.log().setDebug(getClass(), true);
        String startupDate = new Date().toString();
        Logger.log().debug(getClass(), startupDate + ": " + StandaloneBrowser.APPNAME + " " + StandaloneBrowser.VERSION + " startup");
        Logger.log().debug(getClass(), StandaloneBrowser.APPNAME + ": Initializing " + Db4o.version() + " library");
        
        if (args instanceof String[]) {
        	Application.args = (String[]) args;
        }
        
        Display display = PlatformUI.createDisplay();
		try {
			int returnCode = PlatformUI.createAndRunWorkbench(display, new ApplicationWorkbenchAdvisor());
			if (returnCode == PlatformUI.RETURN_RESTART) {
		        db4ologger.close();
		        Logger.log().debug(getClass(), new Date().toString() + ": Application shutdown");
				return IPlatformRunnable.EXIT_RESTART;
			}
	        db4ologger.close();
	        Logger.log().debug(getClass(), new Date().toString() + ": Application shutdown");
			return IPlatformRunnable.EXIT_OK;
		} finally {
			display.dispose();
		}
	}
}
