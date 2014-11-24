package com.db4o.objectManager.v2;

import java.awt.event.*;
import java.io.*;

import javax.swing.*;

import com.db4o.objectManager.v2.resources.*;
import com.db4o.objectManager.v2.uiHelper.*;

/**
 * User: treeder
 * Date: Aug 21, 2006
 * Time: 6:07:00 PM
 */
public class ConnectedMenuBar extends BaseMenuBar {
	private MainFrame frame;

	public ConnectedMenuBar(MainFrame mainFrame, Settings settings, ActionListener helpActionListener, ActionListener aboutActionListener) {
		super(settings, helpActionListener, aboutActionListener);
		frame = mainFrame;

		add(buildFileMenu());
		if (!frame.getConnectionSpec().isRemote()) {
			add(buildManageMenu());
		}
		add(buildHelpMenu(helpActionListener, aboutActionListener));
	}

	private JMenu buildManageMenu() {
		JMenuItem item;

		JMenu menu = createMenu("Manage", 'M');

		// Build a submenu that has the noIcons hint set.

		item = makeBackupItem();
		menu.add(item);

		/*item = makeExportItem();
		menu.add(item);*/

		menu.addSeparator();

		item = makeDefragItem();
		menu.add(item);

		return menu;
	}

	private JMenuItem makeExportItem() {
		JMenuItem item;
		// todo: find an export icon
		item = createMenuItem("Export\u2026", ResourceManager.createImageIcon("saveas_edit.gif"), 'b');
		item.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent e) {
				JFileChooser fileChooser = new JFileChooser();
				int response = fileChooser.showSaveDialog(frame);
				if (response == JFileChooser.APPROVE_OPTION) {
					File f = fileChooser.getSelectedFile();
					try {
						//todo: EXPORT! frame.getObjectContainer().ext().backup(f.getAbsolutePath()); // todo: change this to use UISession.getObjectContainer
						OptionPaneHelper.showSuccessDialog(frame, "Export completed successfully.", "Export Successful");
					} catch (Exception e1) {
						e1.printStackTrace();
						OptionPaneHelper.showErrorMessage(frame, "Error during export! " + e1.getMessage(), "Error during export");
					}
				}
			}
		});
		return item;
	}

	private JMenuItem makeDefragItem() {
		JMenuItem item;
		item = createMenuItem("Defragment", 'd', KeyStroke.getKeyStroke("ctrl shift D"));
		item.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent e) {
				int result = JOptionPane.showConfirmDialog(frame, "Are you sure you want to defragment this database?\n" +
						"This operation will shutdown the currently running database and reopen after the operation completes.\n" +
						"Please be aware of the side effects of running defragment. See db4o manual.");
				if (result == JOptionPane.YES_OPTION) {
					frame.close();
					Dashboard.defragment(frame.getConnectionSpec());
				}
			}
		});
		item.setEnabled(true);
		return item;
	}

	private JMenuItem makeBackupItem() {
		JMenuItem item;
		item = createMenuItem("Backup\u2026", ResourceManager.createImageIcon("saveas_edit.gif"), 'b');
		item.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent e) {
				JFileChooser fileChooser = new JFileChooser();
				int response = fileChooser.showSaveDialog(frame);
				if (response == JFileChooser.APPROVE_OPTION) {
					File f = fileChooser.getSelectedFile();
					try {
						frame.getObjectContainer().ext().backup(f.getAbsolutePath()); // todo: change this to use UISession.getObjectContainer
						OptionPaneHelper.showSuccessDialog(frame, "Backup completed successfully.", "Backup Successful");
					} catch (Exception e1) {
						e1.printStackTrace();
						OptionPaneHelper.showErrorMessage(frame, "Error during backup! " + e1.getMessage(), "Error during backup");
					}
				}
			}
		});
		return item;
	}

	private JMenu buildFileMenu() {
		JMenuItem item;

		JMenu menu = createMenu("File", 'F');

		item = createMenuItem("Close", 'C', KeyStroke.getKeyStroke("ctrl F4"));
		item.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent e) {
				frame.close();

			}
		});
		menu.add(item);
		menu.addSeparator();

		item = createMenuItem("Exit", 'E');
		item.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent e) {
				frame.exit();
			}
		});
		menu.add(item);

		return menu;
	}


}
