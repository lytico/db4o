/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */
package com.db4o.omplus.ui.dialog.login.debug;

import org.eclipse.swt.*;
import org.eclipse.swt.layout.*;
import org.eclipse.swt.widgets.*;

import com.db4o.config.*;
import com.db4o.omplus.*;
import com.db4o.omplus.ui.dialog.login.model.*;
import com.db4o.omplus.ui.dialog.login.presentation.*;
import com.db4o.omplus.ui.dialog.login.presentation.CustomConfigPane.*;

public class ShowCustomConfigDialogMain {
	public static void main(String[] args) {
		Display display = new Display();
		final Shell shell = new Shell(display);
		shell.setLayout (new GridLayout());
		ErrorMessageSink errSink = new ErrorMessageSink() {
			public void showError(String msg) {
				System.err.println(msg);
			}
			
			public void showExc(String msg, Throwable exc) {
				System.err.println("ERR: " + msg);
				exc.printStackTrace();
			}

			public void logWarning(String msg, Throwable exc) {
				System.err.println("WARN: " + msg);
				exc.printStackTrace();
			}
		};
		CustomConfigSink sink = new CustomConfigSink() {
			public void customConfig(String[] jarPaths, String[] configClassNames) {
				System.out.println(java.util.Arrays.toString(jarPaths));
				System.out.println(java.util.Arrays.toString(configClassNames));
			}
		};
		JarPathSource jarPathSource = new JarPathSource() {
			public String jarPath() {
				FileDialog fileChooser = new FileDialog(shell, SWT.OPEN);
				fileChooser.setFilterExtensions(new String[] { "*.jar" });
				fileChooser.setFilterNames(new String[] { "Jar Files (*.jar)" });
				return fileChooser.open();
			}
		};
		new CustomConfigPane(shell, shell, new CustomConfigModelImpl(new String[0], new String[0], sink, new SPIConfiguratorExtractor(EmbeddedConfigurationItem.class), new ErrorMessageHandler(errSink)), jarPathSource);
		shell.pack();
		shell.open ();
		while (!shell.isDisposed ()) {
			if (!display.readAndDispatch ()) display.sleep ();
		}
		display.dispose ();
	}
	

}
