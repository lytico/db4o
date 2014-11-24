package com.db4o.omplus.ui.dialog;

import org.eclipse.swt.*;
import org.eclipse.swt.events.*;
import org.eclipse.swt.graphics.*;
import org.eclipse.swt.layout.*;
import org.eclipse.swt.widgets.*;

import com.db4o.omplus.connection.*;
import com.db4o.omplus.datalayer.*;

public class AboutDialog 
{
	private Shell mainCompositeShell;
	
	private final int MAIN_SHELL_WIDTH = 400;
	private final int MAIN_SHELL_HEIGHT = 290;
	
	private Composite imageComposite;
	
	private Composite upperComposite;
	private Label objectMgrLabel;
	private final String OBJECT_MANAGER_STRING ="ObjectManager Enterprise (1.0.4)";
	private FontData objMgrFontData =	new FontData("TAHOMA", 8, SWT.BOLD);
	private Font objMgrFont = new Font(null,objMgrFontData);
	
	private Label db4oLabel;
	private FontData db4oFontData =	new FontData("TAHOMA", 8, SWT.BOLD);
	private Font db4oFont = new Font(null,db4oFontData);
		
	private Composite bottomComposite;
	private Label copyrightLabel;
	private final String COPYRIGHT_STRING = "(c) Copyright 2010 Versant, Inc.";
	private Button okButton;

	
	
	
	public AboutDialog(Shell parentShell) 
	{
		mainCompositeShell = new Shell(parentShell,SWT.APPLICATION_MODAL | SWT.DIALOG_TRIM);
		mainCompositeShell.setText("About ObjectManager Enterprise");
		Rectangle rectangle = parentShell.getBounds(); 
		mainCompositeShell.setBounds(rectangle.width/3, rectangle.height/3, MAIN_SHELL_WIDTH,MAIN_SHELL_HEIGHT);
		mainCompositeShell.setImage(ImageUtility.getImage(OMPlusConstants.DB4O_WIND_ICON));
		createComponents();
	}
	
	public void open()
	{
		mainCompositeShell.open();
	}
	
	private void createComponents()
	{
		imageComposite = new Composite(mainCompositeShell, SWT.NONE);
		upperComposite = new Composite(mainCompositeShell, SWT.NONE);
		bottomComposite = new Composite(mainCompositeShell, SWT.BORDER);
		
		setImageCompositeComponents();
		setUpperComposite();
		setBottomComposite();
		
		setMainShellLayout();
		mainCompositeShell.layout(true, true);		
		
		mainCompositeShell.addDisposeListener(new DisposeListener() {

			public void widgetDisposed(DisposeEvent e)
			{
				disposeFontsAndColors();
				
			}
			
		});
	}	
	
	private void setImageCompositeComponents() {
		imageComposite.setLayout(new FormLayout());
		Label img_Label = new Label(imageComposite, SWT.NONE);
		img_Label.setLayoutData(new FormData(450, 75));
		Image img = ImageUtility.getImage(OMPlusConstants.DB4O_LOGO_IMAGE);
		img_Label.setImage(img);	
	}
	
	private void setUpperComposite()
	{
		objectMgrLabel = new Label(upperComposite, SWT.NONE);
		objectMgrLabel.setText(OBJECT_MANAGER_STRING);
		objectMgrLabel.setFont(objMgrFont);
		
		db4oLabel= new Label(upperComposite, SWT.NONE);
		db4oLabel.setText(new ConnectionStatus().getVersion());
		db4oLabel.setFont(db4oFont);
		
		setLayoutForUpperComposite();		
	}
	
	private void setLayoutForUpperComposite() 
	{
		upperComposite.setLayout(new FormLayout());
		
		FormData data = new FormData();
		data.top = new FormAttachment(5,5);
		data.left = new FormAttachment(25,5);
		objectMgrLabel.setLayoutData(data);
		
		data = new FormData();
		data.top = new FormAttachment(objectMgrLabel,20);
		data.left = new FormAttachment(30,5);
		db4oLabel.setLayoutData(data);
		
		upperComposite.layout(true);
		
	}
	
	private void setBottomComposite()
	{
		copyrightLabel = new Label(bottomComposite, SWT.NONE);
		copyrightLabel.setText(COPYRIGHT_STRING);
		
		okButton = new Button(bottomComposite, SWT.PUSH);
		okButton.setText("OK");
		okButton.addSelectionListener(new SelectionListener() 
		{

			public void widgetDefaultSelected(SelectionEvent e) {
				// Auto-generated method stub
				
			}

			public void widgetSelected(SelectionEvent e) 
			{
				disposeFontsAndColors();
				mainCompositeShell.dispose();				
			}
			
		});
		
		setLayoutForBottomComposite();		
	}

	private void setLayoutForBottomComposite() 
	{
		bottomComposite.setLayout(new FormLayout());
		
		FormData data = new FormData();
		data.top = new FormAttachment(2, 0);
		data.left = new FormAttachment(2, 0);
		copyrightLabel.setLayoutData(data);
		
		data = new FormData();
		data.top = new FormAttachment(2, 0);
		data.left = new FormAttachment(copyrightLabel, 5);
		data.right = new FormAttachment(98, 0);
		okButton.setLayoutData(data);
		
		bottomComposite.layout(true);
		
	}

	private void setMainShellLayout()
	{
//		mainCompositeShell.setSize(MAIN_SHELL_WIDTH, MAIN_SHELL_HEIGHT);
		mainCompositeShell.setLayout(new FormLayout());
		
		FormData data = new FormData();
		data.top = new FormAttachment(0,0);
		data.left = new FormAttachment(0,0);
		imageComposite.setLayoutData(data);
		
		data = new FormData();
		data.top = new FormAttachment(imageComposite,4);
		data.left = new FormAttachment(0,0);
		data.width = MAIN_SHELL_WIDTH;
		data.height = 140;
		upperComposite.setLayoutData(data);	
		
		
		data = new FormData();
		data.top = new FormAttachment(upperComposite,4);
		data.left = new FormAttachment(0,0);
		data.width = MAIN_SHELL_WIDTH;
		bottomComposite.setLayoutData(data);	
		
		mainCompositeShell.layout(true);			
	}
	
	/**
	 * dispose fonts explicitly
	 */
	private void disposeFontsAndColors()
	{
		objMgrFont.dispose();
		db4oFont.dispose();
	}

}
