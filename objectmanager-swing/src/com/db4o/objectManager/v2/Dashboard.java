package com.db4o.objectManager.v2;

import java.awt.*;
import java.awt.event.*;
import java.io.*;
import java.net.*;
import java.util.*;

import javax.swing.*;
import javax.swing.border.*;

import com.db4o.*;
import com.db4o.ext.*;
import com.db4o.objectManager.v2.connections.*;
import com.db4o.objectManager.v2.custom.*;
import com.db4o.objectManager.v2.maint.*;
import com.db4o.objectManager.v2.resources.*;
import com.db4o.objectManager.v2.uiHelper.*;
import com.db4o.objectManager.v2.uiHelper.SwingWorker;
import com.db4o.objectManager.v2.util.*;
import com.db4o.objectmanager.api.prefs.*;
import com.db4o.objectmanager.model.*;
import com.jgoodies.looks.*;

/**
 * User: treeder
 * Date: Aug 8, 2006
 * Time: 11:21:49 AM
 */
public class Dashboard {

	public static final String COPYRIGHT =
			"\u00a9 2007 db4objects Inc. All Rights Reserved.";
	public static final String VERSION = getVersion();
	public static final String TITLE = "ObjectManager " + VERSION;

	private JFrame frame;

	private Settings settings;
	private ConnectionForm connectionForm;
	private static Map openMap = new HashMap();
	private static Dashboard instance;


	/**
	 * Configures the UI, then builds and opens the UI.
	 */
	public static void main(String[] args) {
		// try to open preferences file right away so we can show message and exit
		try {
			Preferences pref = Preferences.getDefault();
		} catch (DatabaseFileLockedException e) {
			OptionPaneHelper.showErrorMessage(null, "Another instance of ObjectManager is currently open. Only one instance can be open at a time. " +
					"You can connect to multiple databases through the same instance.", "Error Opening Database");
			return;

		} catch (Exception e) {
			OptionPaneHelper.showErrorMessage(null, "An error occurred while opening ObjectManager settings: " + e.getMessage(), "Error Opening Database");
			return;
		}
		instance = new Dashboard();
		instance.configureUI();
		instance.buildInterface();
	}
	
	private static String getVersion() {
        URL url = Dashboard.class.getResource("Version");
        if (url != null) {
            try {
                Properties props = new Properties();
                props.load(url.openStream());
                return (String) props.get("version");
            } catch (IOException e) {
                System.out.println("Could not get version of ObjectManager.");
            }
        }
        return null;
    }
	
	/**
	 * Configures the UI; tries to set the system look on Mac,
	 * <code>WindowsLookAndFeel</code> on general Windows, and
	 * <code>Plastic3DLookAndFeel</code> on Windows XP and all other OS.<p>
	 * <p/>
	 * The JGoodies Swing Suite's <code>ApplicationStarter</code>,
	 * <code>ExtUIManager</code>, and <code>LookChoiceStrategies</code>
	 * classes provide a much more fine grained algorithm to choose and
	 * restore a look and theme.
	 */
	private void configureUI() {
		settings = MainFrame.createDefaultSettings();

		UIManager.put(Options.USE_SYSTEM_FONTS_APP_KEY, Boolean.TRUE);
		Options.setDefaultIconSize(new Dimension(18, 18));

		String lafName =
				//LookUtils.IS_OS_WINDOWS_XP
				//  ? Options.getCrossPlatformLookAndFeelClassName() :
				Options.getSystemLookAndFeelClassName();

		try {
			UIManager.setLookAndFeel(lafName);
		} catch (Exception e) {
			System.err.println("Can't set look & feel:" + e);
		}
		/* Font controlFont = Fonts.WINDOWS_XP_96DPI_NORMAL;
				FontSet fontSet = FontSets.createDefaultFontSet(controlFont);
				FontPolicy fontPolicy = FontPolicies.createFixedPolicy(fontSet);
				WindowsLookAndFeel.setFontPolicy(fontPolicy);*/
	}

	/**
	 * Creates and configures a frame, builds the menu bar, builds the
	 * content, locates the frame on the screen, and finally shows the frame.
	 */
	private void buildInterface() {
		frame = new JFrame();
		frame.setIconImage(ResourceManager.createImageIcon(ResourceManager.ICONS_PLAIN_16X16 + "data.png", "database").getImage());
		frame.setJMenuBar(buildMenuBar());
		frame.setContentPane(buildContentPane());
		frame.setSize(600, 462);
		frame.setResizable(false);
		locateOnScreen(frame);
		frame.setTitle(TITLE);
		frame.addWindowListener(new DashboardWindowListener(this));
		frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
		frame.setVisible(true);
	}
	
	/**
	 * Locates the frame on the screen center.
	 */
	private void locateOnScreen(Frame frame) {
		Dimension paneSize = frame.getSize();
		Dimension screenSize = frame.getToolkit().getScreenSize();
		frame.setLocation(
				(screenSize.width - paneSize.width) / 2,
				(screenSize.height - paneSize.height) / 2);
	}

	/**
	 * Builds and returns the menu bar.
	 */
	private JMenuBar buildMenuBar() {
		DashboardMenuBar dashboardMenuBar = new DashboardMenuBar(this,
				settings,
				createHelpActionListener(frame),
				createAboutActionListener(frame));
		return dashboardMenuBar;
	}

	/**
	 * Builds and returns the content pane.
	 *
	 * @return the main content pane component
	 */
	private JComponent buildContentPane() {
		JPanel panel = new JPanel(new BorderLayout());
		//panel.add(buildToolBar(), BorderLayout.NORTH);
		panel.add(buildConnectionsPanel(), BorderLayout.CENTER);
		return panel;
	}

	private Component buildConnectionsPanel() {
		JPanel panel = new JPanel(new BorderLayout());

		// create db4o icon
		String logoPath = "db4ologo.png";
		ImageIcon icon = ResourceManager.createImageIcon(logoPath, "db4o");
		JLabel label1 = new JLabel(icon, JLabel.LEFT);
		panel.add(label1, BorderLayout.NORTH);

		// gradient background
		BackgroundPanel backgroundPanel = new BackgroundPanel();


		connectionForm = new ConnectionForm(this);
		backgroundPanel.add(connectionForm.getPanel());

		// add to main panel
		panel.add(backgroundPanel, BorderLayout.CENTER);

		return panel;
	}

	// Helper Code ********************************************************

	/**
	 * Creates and answers a <code>JScrollpane</code> that has no border.
	 */
	private JScrollPane createStrippedScrollPane(Component c) {
		JScrollPane scrollPane = new JScrollPane(c);
		scrollPane.setBorder(null);
		return scrollPane;
	}

	/**
	 * Creates and answers a <code>JLabel</code> that has the text
	 * centered and that is wrapped with an empty border.
	 */
	private Component createCenteredLabel(String text) {
		JLabel label = new JLabel(text);
		label.setHorizontalAlignment(SwingConstants.CENTER);
		label.setBorder(new EmptyBorder(3, 3, 3, 3));
		return label;
	}

	public void showError(String msg) {
		JOptionPane.showMessageDialog(frame, "msg", "Error", JOptionPane.ERROR_MESSAGE);
	}

	/**
	 * Creates and answers an ActionListener that opens the help viewer.
	 *
	 * @param frame
	 */
	public static ActionListener createHelpActionListener(Component frame) {
		return new HelpActionListener(frame);
	}

	public static ActionListener createAboutActionListener(Component frame) {
		return new AboutActionListener(frame);
	}

	public JFrame getFrame() {
		return frame;
	}

	public void connectToFile(String dataFile) {
		connectionForm.connectToFile(dataFile);
	}

	public static void close() {
		Iterator keys = openMap.keySet().iterator();
		while (keys.hasNext()) {
			Object key = keys.next();
			ObjectContainer oc = (ObjectContainer) openMap.get(key);
			oc.close();
		}
		openMap.clear(); // just in case we don't actually shutdown at this point
		Preferences.close();
	}

	public static void exit() {
		close();
		System.exit(0);
	}

	/**
	 * This will put all the open connections in a map so they can be handled from here for closing and such.
	 *
	 * @param connectionSpec
	 * @param oc
	 */
	public void addOpenConnection(Db4oConnectionSpec connectionSpec, ObjectContainer oc) {
		openMap.put(connectionSpec.getFullPath(), oc);
	}

	public static void open(Db4oConnectionSpec connectionSpec) {
		instance.connectionForm.connectAndOpenFrame(connectionSpec);
	}

	/**
	 * Will run defragment on a database.
	 * todo: get more feedback from the defrag process so can show progress to user.
	 *
	 * @param connectionSpec
	 */
	public static void defragment(final Db4oConnectionSpec connectionSpec) {
		try {
			final ProgressDialog pd = new ProgressDialog(instance.frame, "Defragment", true);
			pd.setIndeterminate(true);
			pd.setStringPainted(true);
			pd.setString("Defragging Database...");
			// kick off defrag thread
			final SwingWorker worker = new SwingWorker() {
				boolean successful = true;

				public Object construct() {
					try {
						return new DefragTask(connectionSpec);
					} catch (Exception e1) {
						successful = false;
						OptionPaneHelper.showErrorMessage(instance.frame, "Error occurred during defragment: " + "\n\n" + e1.getMessage(), "Error During Defragment");
						e1.printStackTrace();
						Log.addException(e1);
					}
					return null;
				}

				public void finished() {
					super.finished();
					pd.dispose();
					if (successful) {
						OptionPaneHelper.showSuccessDialog(instance.frame, "Defragment was successful!", "Defragment Complete");
					}
					// reopen frame
					Dashboard.open(connectionSpec);
				}
			};
			worker.start();
			pd.setVisible(true);

		} catch (Exception e1) {
			OptionPaneHelper.showErrorMessage(instance.frame, e1.toString() + "\n\n" + e1.getMessage(), "Error During Defragment");
			e1.printStackTrace();
		}
	}


	private static final class AboutActionListener implements ActionListener {
		private Component frame;

		public AboutActionListener(Component frame) {

			this.frame = frame;
		}

		public void actionPerformed(ActionEvent e) {
			JOptionPane.showMessageDialog(
					frame,
					"db4o Object Manager\n" +
							"Version: " + VERSION + "\n" +
							COPYRIGHT + "\n\n");
		}

	}

	private static final class HelpActionListener implements ActionListener {
		private Component frame;

		public HelpActionListener(Component frame) {

			this.frame = frame;
		}

		public void actionPerformed(ActionEvent e) {
			JOptionPane.showMessageDialog(
					frame,
					// todo: make this linkable, need to figure out how to launch a browser
					"Please visit our online help at: \n" +
							"http://developer.db4o.com/DocsWiki/view.aspx/Reference/Object_Manager_For_db4o/Installation");
		}
	}


}
