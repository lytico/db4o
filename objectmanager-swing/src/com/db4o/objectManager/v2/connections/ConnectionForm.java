package com.db4o.objectManager.v2.connections;

import com.jgoodies.forms.layout.FormLayout;
import com.jgoodies.forms.builder.DefaultFormBuilder;
import com.jgoodies.looks.Fonts;
import com.db4o.objectmanager.model.Db4oConnectionSpec;
import com.db4o.objectmanager.model.Db4oFileConnectionSpec;
import com.db4o.objectmanager.model.Db4oSocketConnectionSpec;
import com.db4o.objectmanager.api.prefs.Preferences;
import com.db4o.ObjectContainer;
import com.db4o.objectManager.v2.connections.RecentConnectionList;
import com.db4o.objectManager.v2.connections.ConnectionHelper;
import com.db4o.objectManager.v2.Dashboard;
import com.db4o.objectManager.v2.MainFrame;
import com.db4o.objectManager.v2.WindowPrefsListener;

import javax.swing.*;
import java.awt.*;
import java.awt.event.*;
import java.io.File;

/**
 * User: treeder
 * Date: Nov 25, 2006
 * Time: 9:32:16 AM
 */
public class ConnectionForm {
	private JTextField hostTextField;
	private JTextField portTextField;
	private JTextField usernameTextField;
	private JTextField passwordTextField;
	private JTextField fileTextField;

	private JPanel panel;
	private Dashboard dashboard;
	private RecentConnectionList recentConnectionList;


	public ConnectionForm(Dashboard dashboard2) {
		this.dashboard = dashboard2;

		// add welcome message
		JLabel welcome = new JLabel("Recent Connections");
		welcome.setFont(Fonts.WINDOWS_VISTA_96DPI_LARGE);//new Font("Verdana", Font.PLAIN, 30));
		welcome.setForeground(Color.white);
		welcome.setVerticalAlignment(JLabel.TOP);

		recentConnectionList = new RecentConnectionList();
		recentConnectionList.getList().addMouseListener(new MouseAdapter() {
			public void mouseClicked(MouseEvent e) {
				Db4oConnectionSpec connectionSpec = recentConnectionList.getSelected();
				if (connectionSpec != null) {
					showInForm(connectionSpec);
					if (e.getClickCount() == 2) {
						connectAndOpenFrame(connectionSpec);
					}
				}
			}
		});
		recentConnectionList.getList().addKeyListener(new KeyAdapter() {
			public void keyTyped(KeyEvent e) {
				if (e.getKeyChar() == KeyEvent.VK_ENTER) {
					Db4oConnectionSpec connectionSpec = recentConnectionList.getSelected();
					if (connectionSpec != null) {
						showInForm(connectionSpec);
						connectAndOpenFrame(connectionSpec);
					}
				}
			}

		});

		JLabel welcome2 = new JLabel("New Connection");
		welcome2.setFont(Fonts.WINDOWS_VISTA_96DPI_LARGE);//new Font("Verdana", Font.PLAIN, 30));
		welcome2.setForeground(Color.white);

		FormLayout layout = new FormLayout(
				"right:max(40dlu;pref), 3dlu, 120dlu, 7dlu," // 1st major colum
						+ "right:max(40dlu;pref), 3dlu, 80dlu",		// 2nd major column
				"");										 // add rows dynamically
		DefaultFormBuilder builder = new DefaultFormBuilder(layout);
		builder.setDefaultDialogBorder();

		// builder.appendSeparator("Flange");

		builder.append(welcome);

		builder.nextLine();
		builder.append("", recentConnectionList, 3);

		builder.nextLine();
		builder.append(welcome2);
		builder.nextLine();
		//builder.append("");
		builder.appendSeparator("Local");
		fileTextField = new JTextField();
		builder.append("File:", fileTextField);
		final JFileChooser fc = new JFileChooser();
		final Button browse = new Button("Browse");
		browse.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent e) {
				//Handle open button action.
				if (e.getSource() == browse) {
					int returnVal = fc.showOpenDialog(dashboard.getFrame());
					if (returnVal == JFileChooser.APPROVE_OPTION) {
						File file = fc.getSelectedFile();
						fileTextField.setText(file.getAbsolutePath());
					} else {

					}
				}
			}
		});
		builder.append(browse);
		builder.nextLine();
		JButton openButton = new JButton("Open");
		openButton.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent e) {
				connectToFile();
			}
		});
		builder.append("", openButton);


		builder.nextLine();
		builder.appendSeparator("Remote");

		//builder.append("");
		hostTextField = new JTextField();
		builder.append("Host:", hostTextField);
		portTextField = new JTextField();
		portTextField.setColumns(5);
		// todo: validate port to ensure this is an integer - portTextField.addActionListener();
		builder.append("Port:", portTextField);
		builder.nextLine();
		usernameTextField = new JTextField();
		builder.append("Username:", usernameTextField);
		builder.nextLine();
		//builder.append("");
		passwordTextField = new JTextField();
		builder.append("Password:", passwordTextField);
		builder.nextLine();
		//builder.append("");
		JButton connectButton = new JButton("Connect");
		connectButton.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent e) {
				if (hostTextField.getText().length() > 0) {
					// then open file connection
					Db4oConnectionSpec connectionSpec = new Db4oSocketConnectionSpec(hostTextField.getText(), Integer.parseInt(portTextField.getText()), usernameTextField.getText(), passwordTextField.getText(), false);
					connectAndOpenFrame(connectionSpec);
				}
			}
		});
		builder.append("", connectButton);

		panel = builder.getPanel();
		panel.setOpaque(false);
	}

	public void connectToFile(String dataFile) {
		fileTextField.setText(dataFile);
		connectToFile();
	}

	private void connectToFile() {
		if (fileTextField.getText().length() > 0) {
			// then open file connection
			Db4oConnectionSpec connectionSpec = new Db4oFileConnectionSpec(fileTextField.getText(), false);
			connectAndOpenFrame(connectionSpec);
		}
	}

	// todo: move this method into Dashboard
	public void connectAndOpenFrame(Db4oConnectionSpec connectionSpec) {
		working();

		// connect before opening frame to make sure all is good
		ObjectContainer oc;
		try {
			oc = ConnectionHelper.connect(dashboard.getFrame(), connectionSpec);
		} catch (Exception ex) {
			// if fails to open, stop, message already shown
			stopWorking();
			return;
		}

		recentConnectionList.addNewConnectionSpec(connectionSpec);
		clearForm();
		Dimension frameSize = (Dimension) Preferences.getDefault().getPreference(Preferences.FRAME_SIZE);
		Point frameLocation = (Point) Preferences.getDefault().getPreference(Preferences.FRAME_LOCATION);

		MainFrame instance = MainFrame.createDefaultFrame(frameSize, frameLocation, connectionSpec, oc);
		instance.addComponentListener(new WindowPrefsListener(instance));
		
		dashboard.addOpenConnection(connectionSpec, oc);
		stopWorking();
	}

	private void stopWorking() {
		dashboard.getFrame().setCursor(null);
	}

	private void working() {
		dashboard.getFrame().setCursor(Cursor.getPredefinedCursor(Cursor.WAIT_CURSOR));
	}

	private void clearForm() {
		fileTextField.setText("");
		usernameTextField.setText("");
		passwordTextField.setText("");
		portTextField.setText("");
		hostTextField.setText("");
	}


	private void showInForm(Db4oConnectionSpec connectionSpec) {
		clearForm();
		if (connectionSpec instanceof Db4oFileConnectionSpec) {
			fileTextField.setText(connectionSpec.getPath());
		} else if (connectionSpec instanceof Db4oSocketConnectionSpec) {
			Db4oSocketConnectionSpec spec = (Db4oSocketConnectionSpec) connectionSpec;
			hostTextField.setText(spec.getHost());
			portTextField.setText(spec.getPort() + "");
			usernameTextField.setText(spec.getUser());
			passwordTextField.setText(spec.getPassword());
		}
	}

	public JTextField getHostTextField() {
		return hostTextField;
	}

	public void setHostTextField(JTextField hostTextField) {
		this.hostTextField = hostTextField;
	}

	public JTextField getPortTextField() {
		return portTextField;
	}

	public void setPortTextField(JTextField portTextField) {
		this.portTextField = portTextField;
	}

	public JTextField getUsernameTextField() {
		return usernameTextField;
	}

	public void setUsernameTextField(JTextField usernameTextField) {
		this.usernameTextField = usernameTextField;
	}

	public JTextField getPasswordTextField() {
		return passwordTextField;
	}

	public void setPasswordTextField(JTextField passwordTextField) {
		this.passwordTextField = passwordTextField;
	}

	public Component getPanel() {
		return panel;
	}
}
