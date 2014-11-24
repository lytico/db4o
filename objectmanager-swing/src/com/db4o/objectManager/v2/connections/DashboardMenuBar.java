package com.db4o.objectManager.v2.connections;

import com.jgoodies.looks.Options;
import com.db4o.objectManager.v2.util.Log;
import com.db4o.objectManager.v2.custom.ProgressDialog;
import com.db4o.objectManager.v2.uiHelper.*;
import com.db4o.objectManager.v2.BaseMenuBar;
import com.db4o.objectManager.v2.Dashboard;
import com.db4o.objectManager.v2.Settings;
import com.db4o.objectManager.v2.demo.DemoDbTask;
import demo.objectmanager.model.DemoPopulator;

import javax.swing.*;
import java.awt.event.ActionListener;
import java.awt.event.ActionEvent;
import java.awt.Cursor;

/**
 * User: treeder
 * Date: Aug 30, 2006
 * Time: 12:27:13 PM
 */
public class DashboardMenuBar extends BaseMenuBar {
	private Dashboard dashboard;

	public DashboardMenuBar(Dashboard dashboard2, Settings settings, ActionListener helpActionListener, ActionListener aboutActionListener) {
		super(settings, helpActionListener, aboutActionListener);
		this.dashboard = dashboard2;
		JMenu menu;
		JMenuItem item;
		this.putClientProperty(Options.HEADER_STYLE_KEY, Boolean.TRUE);

		menu = new JMenu("File");

		item = createMenuItem("Create Demo Db");
		item.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent e) {
				makeDemoDb();
			}
		});
		menu.add(item);

		menu.addSeparator();
		item = createMenuItem("Exit", 'E');
		item.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent e) {
				System.exit(0);
			}
		});
		menu.add(item);

		this.add(menu);

		menu = buildHelpMenu(helpActionListener, aboutActionListener);
		menu.addSeparator();
		item = new JMenuItem("Exception Log");
		item.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent e) {
				JFrame logFrame = new JFrame("Exception Log");
				JTextArea ta = new JTextArea(20, 80);
				JScrollPane scrollPane = new JScrollPane(ta);
				logFrame.add(scrollPane);
				//logFrame.setSize(400, 300);
				logFrame.pack();
				logFrame.setVisible(true);
				ta.setText(Log.dump());
			}
		});
		menu.add(item);
		this.add(menu);

	}

	private void makeDemoDb() {
		final ProgressDialog pd = new ProgressDialog(dashboard.getFrame(), "Creating Demo Db", true);
		pd.setIndeterminate(true);
		pd.setStringPainted(true);
		pd.setString("Creating demo database...");
		// kick off defrag thread
		final com.db4o.objectManager.v2.uiHelper.SwingWorker worker = new com.db4o.objectManager.v2.uiHelper.SwingWorker() {
			boolean successful = false;
			String filename;

			public Object construct() {
				dashboard.getFrame().setCursor(Cursor.getPredefinedCursor(Cursor.WAIT_CURSOR));
				try {
					DemoDbTask task = new DemoDbTask();
					task.run();
					System.out.println("done making demo db.");
					DemoPopulator populator = task.getPopulator();
					filename = populator.getFileName();
					successful = true;
				} catch(Exception e1) {
					OptionPaneHelper.showErrorMessage(dashboard.getFrame(), "Error creating model database!" + "\n\n" + e1.getMessage(), "Error During Defragment");
					Log.addException(e1);
					e1.printStackTrace();

				}
				return null;
			}

			public void finished() {
				super.finished();
				pd.dispose();
				dashboard.getFrame().setCursor(null);
				if(successful) {
					dashboard.connectToFile(filename);
				}
			}
		};
		worker.start();
		pd.setVisible(true);

	}
}
