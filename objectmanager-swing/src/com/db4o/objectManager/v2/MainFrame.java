package com.db4o.objectManager.v2;

import java.awt.Component;
import java.awt.Dimension;
import java.awt.Point;
import java.awt.event.WindowListener;
import java.awt.event.WindowEvent;

import javax.swing.*;
import javax.swing.plaf.metal.DefaultMetalTheme;
import javax.swing.plaf.metal.MetalLookAndFeel;

import com.jgoodies.looks.LookUtils;
import com.jgoodies.looks.Options;
import com.jgoodies.looks.plastic.PlasticLookAndFeel;
import com.db4o.objectmanager.model.Db4oConnectionSpec;
import com.db4o.ObjectContainer;
import com.db4o.objectManager.v2.resources.ResourceManager;

/**
 * Main frame for a particular connection.
 * <p/>
 * <p/>
 * User: treeder
 * Date: Aug 8, 2006
 * Time: 11:21:49 AM
 */
public class MainFrame extends JFrame implements WindowListener {

	protected static final Dimension PREFERRED_SIZE =
			LookUtils.IS_LOW_RESOLUTION
					? new Dimension(650, 510)
					: new Dimension(730, 560);

	private static String WINDOW_TITLE = Dashboard.TITLE;


	/**
	 * Describes optional settings of the JGoodies Looks.
	 */
	private final Settings settings;
	private Db4oConnectionSpec connectionSpec;
	private MainPanel mainPanel;

	/**
	 * Constructs a <code>DemoFrame</code>, configures the UI,
	 * and builds the content.
	 */
	protected MainFrame(Settings settings, Db4oConnectionSpec connectionSpec, ObjectContainer oc) {
		super(WINDOW_TITLE + " - " + connectionSpec.getPath());
		this.settings = settings;
		this.connectionSpec = connectionSpec;
		configureUI();
		build(oc);
		setDefaultCloseOperation(WindowConstants.DISPOSE_ON_CLOSE);
		addWindowListener(this);
	}

	private static MainFrame createDefaultFrame(Settings settings, Dimension frameSize, Point frameLocation, Db4oConnectionSpec connectionSpec, ObjectContainer oc) {
		MainFrame instance = new MainFrame(settings, connectionSpec, oc);
		if (frameSize != null) instance.setSize(frameSize);
		else instance.setSize(PREFERRED_SIZE);
		if (frameLocation != null) instance.setLocation(frameLocation);
		else instance.locateOnScreen(instance);
		instance.setVisible(true);
		return instance;
	}

	public static MainFrame createDefaultFrame(Dimension frameSize, Point frameLocation, Db4oConnectionSpec connectionSpec, ObjectContainer oc) {
		return createDefaultFrame(MainFrame.createDefaultSettings(), frameSize, frameLocation, connectionSpec, oc);
	}

	static Settings createDefaultSettings() {
		Settings settings = Settings.createDefault();
		// Configure the settings here.
		return settings;
	}


	/**
	 * Configures the user interface; requests Swing settings and
	 * JGoodies Looks options from the launcher.
	 */
	private void configureUI() {
		// UIManager.put("ToolTip.hideAccelerator", Boolean.FALSE);

		Options.setDefaultIconSize(new Dimension(18, 18));

		Options.setUseNarrowButtons(settings.isUseNarrowButtons());

		// Global options
		Options.setTabIconsEnabled(settings.isTabIconsEnabled());
		UIManager.put(Options.POPUP_DROP_SHADOW_ENABLED_KEY, settings.isPopupDropShadowEnabled());

		// Swing Settings
		LookAndFeel selectedLaf = settings.getSelectedLookAndFeel();
		if (selectedLaf instanceof PlasticLookAndFeel) {
			PlasticLookAndFeel.setPlasticTheme(settings.getSelectedTheme());
			PlasticLookAndFeel.setTabStyle(settings.getPlasticTabStyle());
			PlasticLookAndFeel.setHighContrastFocusColorsEnabled(
					settings.isPlasticHighContrastFocusEnabled());
		} else if (selectedLaf.getClass() == MetalLookAndFeel.class) {
			MetalLookAndFeel.setCurrentTheme(new DefaultMetalTheme());
		}
	

		// Work around caching in MetalRadioButtonUI
		JRadioButton radio = new JRadioButton();
		radio.getUI().uninstallUI(radio);
		JCheckBox checkBox = new JCheckBox();
		checkBox.getUI().uninstallUI(checkBox);

		try {
			UIManager.setLookAndFeel(selectedLaf);
		} catch (Exception e) {
			System.out.println("Can't change L&F: " + e);
		}

	}

	/**
	 * Builds the <code>DemoFrame</code> using Options from the Launcher.
	 * @param oc
	 */
	private void build(ObjectContainer oc) {
		setContentPane(buildContentPane(oc));
		//setTitle(getWindowTitle());
		setJMenuBar(new ConnectedMenuBar(this, settings,
				Dashboard.createHelpActionListener(this),
				Dashboard.createAboutActionListener(this)));
		setIconImage(ResourceManager.createImageIcon(ResourceManager.ICONS_PLAIN_16X16 + "data.png", "database").getImage());

	}


	/**
	 * Builds and answers the content.
	 * @param oc
	 */
	private JComponent buildContentPane(ObjectContainer oc) {
		mainPanel = new MainPanel(this, settings, connectionSpec, oc);
		return mainPanel;
	}


	/**
	 * Locates the given component on the screen's center.
	 */
	protected void locateOnScreen(Component component) {
		Dimension paneSize = component.getSize();
		Dimension screenSize = component.getToolkit().getScreenSize();
		component.setLocation(
				(screenSize.width - paneSize.width) / 2,
				(screenSize.height - paneSize.height) / 2);
	}


	public void setConnectionInfo(Db4oConnectionSpec connectionInfo) {
		mainPanel.setConnectionSpec(connectionInfo);
	}

	public void windowOpened(WindowEvent e) {

	}

	public void windowClosing(WindowEvent e) {
		close();
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

	public ObjectContainer getObjectContainer() {
		return mainPanel.getObjectContainer();
	}

	public Db4oConnectionSpec getConnectionSpec() {
		return connectionSpec;
	}

	public void closeObjectContainer() {
		mainPanel.closeObjectContainer();
	}


	public void exit() {
		close();
		Dashboard.exit();
	}

	public void close() {
		mainPanel.closeObjectContainer();
		dispose();
	}

}