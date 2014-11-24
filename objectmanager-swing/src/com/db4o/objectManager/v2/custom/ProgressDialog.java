package com.db4o.objectManager.v2.custom;

import javax.swing.*;
import java.awt.Dimension;

/**
 * User: treeder
 * Date: Nov 25, 2006
 * Time: 12:56:47 PM
 */
public class ProgressDialog extends JDialog {
	private JProgressBar progressBar;

	public ProgressDialog(JFrame frame, String title, boolean modal) {
		super(frame, title, modal);
		progressBar = new JProgressBar();
		progressBar.setPreferredSize(new Dimension(300, 40));
		JPanel contents = (JPanel) getContentPane();
		contents.setBorder(BorderFactory.createEmptyBorder(10, 10, 10, 10));
		contents.add(progressBar);
		
		setDefaultCloseOperation(DO_NOTHING_ON_CLOSE);
		pack();
	}

	public void setString(String s) {
		progressBar.setString(s);

	}

	public void setIndeterminate(boolean b) {
		progressBar.setIndeterminate(b);
	}

	public void setStringPainted(boolean b) {
		progressBar.setStringPainted(b);
	}
}
