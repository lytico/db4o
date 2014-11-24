package com.db4o.omplus.ui.dialog.login;

import org.eclipse.swt.*;
import org.eclipse.swt.graphics.*;
import org.eclipse.swt.layout.*;
import org.eclipse.swt.widgets.*;

import com.db4o.config.*;
import com.db4o.cs.config.*;
import com.db4o.omplus.*;
import com.db4o.omplus.connection.*;
import com.db4o.omplus.datalayer.*;
import com.db4o.omplus.ui.*;
import com.db4o.omplus.ui.dialog.login.model.*;
import com.db4o.omplus.ui.dialog.login.presentation.*;
import com.db4o.omplus.ui.dialog.login.presentation.CustomConfigPane.*;

public class LoginDialog {

	public static final String SHELL_ID = LoginDialog.class.getName() + "$shell";
	public static final String TAB_FOLDER_ID = LoginDialog.class.getName() + "$tabFolder";
	public static final String LOCAL_TAB_ID = LoginDialog.class.getName() + "$localTab";
	public static final String REMOTE_TAB_ID = LoginDialog.class.getName() + "$remoteTab";

	private static final String LOCAL_DIALOG_TITLE = "Connect to db4o database";
	private static final String REMOTE_DIALOG_TITLE = "Connect to db4o server";

	private Shell mainCompositeShell;

	private LoginPresentationModel model;

	public LoginDialog(Shell parentShell, RecentConnectionList recentConnections, final Connector connector, ErrorMessageHandler err) {
		mainCompositeShell = new Shell(parentShell, SWT.APPLICATION_MODAL | SWT.DIALOG_TRIM);
		OMESWTUtil.assignWidgetId(mainCompositeShell, SHELL_ID);
		mainCompositeShell.setText("Connection Info");
		model = new LoginPresentationModel(recentConnections, err, connector);

		createContents(err);
		
		try {
			mainCompositeShell.setImage(ImageUtility.getImage(OMPlusConstants.DB4O_WIND_ICON));
		}
		catch(Exception exc) {
			// FIXME
		}
		setScreenLocation(parentShell);
	}

	private void setScreenLocation(Shell parentShell) {
		Rectangle parentBounds = parentShell.getBounds();
		Point parentLocation = parentShell.getLocation();
		Rectangle dialogBounds = mainCompositeShell.getBounds();
		int xOff = (parentBounds.width - dialogBounds.width) / 2;
		int yOff = (parentBounds.height - dialogBounds.height) / 2;
		mainCompositeShell.setLocation(parentLocation.x + xOff, parentLocation.y + yOff);
	}

	public void open() {
		mainCompositeShell.open();
	}

	protected Control createContents(ErrorMessageHandler err) {
		TabFolder folder = new TabFolder(mainCompositeShell, SWT.BORDER);
		OMESWTUtil.assignWidgetId(folder, TAB_FOLDER_ID);
		LocalPresentationModel localModel = new LocalPresentationModel(model, new DialogCustomConfigSource(mainCompositeShell, err, EmbeddedConfigurationItem.class));
		RemotePresentationModel remoteModel = new RemotePresentationModel(model, new DialogCustomConfigSource(mainCompositeShell, err, ClientConfigurationItem.class));
		Composite localPane = new LoginPane<FileConnectionParams>(mainCompositeShell, folder, LOCAL_DIALOG_TITLE, new LocalLoginPaneSpec(localModel));
		Composite remotePane = new LoginPane<RemoteConnectionParams>(mainCompositeShell, folder, REMOTE_DIALOG_TITLE, new RemoteLoginPaneSpec(remoteModel));
		addTab(folder, "Local", LOCAL_DIALOG_TITLE, localPane, LOCAL_TAB_ID);
		addTab(folder, "Remote", REMOTE_DIALOG_TITLE, remotePane, REMOTE_TAB_ID);
		folder.pack(true);
		mainCompositeShell.pack(true);
		return mainCompositeShell;
	}
	
	private void addTab(TabFolder folder, String name, String toolTip, Composite content, String id) {
		TabItem item = new TabItem(folder, SWT.NULL);
		item.setToolTipText(toolTip);
		item.setText(name);
		item.setControl(content);
		OMESWTUtil.assignWidgetId(item, id);
	}

	private final static class DialogCustomConfigSource implements CustomConfigSource {		
		private final Shell shell;
		private final ErrorMessageHandler errHandler;
		private final Class<?> spi;

		public DialogCustomConfigSource(Shell shell, ErrorMessageHandler errHandler, Class<?> spi) {
			this.shell = shell;
			this.errHandler = errHandler;
			this.spi = spi;
		}
		
		public void requestCustomConfig(CustomConfigSink configSink, String[] jarPaths, String[] selectedConfigNames) {
			Shell dialog = new Shell(shell, SWT.APPLICATION_MODAL | SWT.DIALOG_TRIM);
			dialog.setLayout(new GridLayout());
			dialog.setText("Jars/Configurators");
			CustomConfigModel customModel = new CustomConfigModelImpl(jarPaths, selectedConfigNames, configSink , new SPIConfiguratorExtractor(spi), errHandler);			
			JarPathSource jarPathSource = new JarPathSource() {
				public String jarPath() {
					FileDialog fileChooser = new FileDialog(shell, SWT.OPEN);
					fileChooser.setFilterExtensions(new String[] { "*.jar" });
					fileChooser.setFilterNames(new String[] { "Jar Files (*.jar)" });
					return fileChooser.open();
				}
			};
			new CustomConfigPane(dialog, dialog, customModel, jarPathSource);
			dialog.pack(true);
			dialog.open();
		}
	}

}
