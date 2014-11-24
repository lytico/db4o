package com.db4o.objectManager.v2;

import com.db4o.objectmanager.api.prefs.Preferences;

import javax.swing.*;
import java.awt.event.ComponentListener;
import java.awt.event.ComponentEvent;
import java.awt.event.ActionEvent;

/**
 * This is here to save window preferences because there are some issues with using regular listeners, for instance:
 * - MouseListener does not fire mouse events when on Window bar.
 * - ComponentListener.componentMoved fires repeatedly while dragging the window generating tons of events
 * <p/>
 * User: treeder
 * Date: Dec 20, 2006
 * Time: 5:43:24 AM
 */
public class WindowPrefsListener implements ComponentListener {
	private Timer timer;
	private JFrame frame;
	private int counter;
	private static final int DELAY = 2000;

	public WindowPrefsListener(JFrame frame) {
		this.frame = frame;
		timer = new Timer(DELAY, new AbstractAction() {
			public void actionPerformed(ActionEvent e) {
				//System.out.println("timer action " + counter++ + "!");
				savePrefs();
			}
		});
		timer.setRepeats(false);
	}

	private void savePrefs() {
		Preferences.getDefault().setPreference(Preferences.FRAME_SIZE, frame.getSize());
		Preferences.getDefault().setPreference(Preferences.FRAME_LOCATION, frame.getLocation());
	}

	/**
	 * compoment resized actually works normally with a single event after the mouse is released,
	 * but supposedly it might behave differently on different platforms.
	 *
	 * @param e
	 */
	public void componentResized(ComponentEvent e) {
		timer.start();

	}

	public void componentMoved(ComponentEvent e) {
		timer.start();
	}

	public void componentShown(ComponentEvent e) {

	}

	public void componentHidden(ComponentEvent e) {

	}
}
