package com.db4o.objectManager.v2.connections;

import java.awt.event.*;

import com.db4o.objectManager.v2.*;

/**
 * User: treeder
 * Date: Nov 25, 2006
 * Time: 12:11:03 PM
 */
public class DashboardWindowListener implements WindowListener {
	private Dashboard dashboard;

	public DashboardWindowListener(Dashboard dashboard) {
		this.dashboard = dashboard;
	}

	public void windowOpened(WindowEvent e) {

	}

	public void windowClosing(WindowEvent e) {
		Dashboard.close();
	}

	public void windowClosed(WindowEvent e) {

	}

	public void windowIconified(WindowEvent e) {

	}

	public void windowDeiconified(WindowEvent e) {

	}

	public void windowActivated(WindowEvent e) {

	}

	public void windowDeactivated(WindowEvent e) {

	}
}
