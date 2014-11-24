/* Copyright (C) 2004 - 2006  db4objects Inc.  http://www.db4o.com

This file is part of the open source sync4o connector written to enable
the Funambol data synchronization client and server to support the
db4o object database.

sync4o is free software; you can redistribute it and/or modify it under
the terms of version 2 of the GNU General Public License as published
by the Free Software Foundation and as clarified by db4objects' GPL 
interpretation policy, available at
http://www.db4o.com/about/company/legalpolicies/gplinterpretation/
Alternatively you can write to db4objects, Inc., 1900 S Norfolk Street,
Suite 350, San Mateo, CA 94403, USA.

sync4o is distributed in the hope that it will be useful, but WITHOUT ANY
WARRANTY; without even the implied warranty of MERCHANTABILITY or
FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License
for more details.

You should have received a copy of the GNU General Public License along
with this program; if not, write to the Free Software Foundation, Inc.,
59 Temple Place - Suite 330, Boston, MA  02111-1307, USA. */
package com.db4o.sync4o.ui;

import java.awt.Component;
import java.awt.Dimension;
import java.awt.GridBagConstraints;
import java.awt.GridBagLayout;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.awt.event.WindowAdapter;
import java.awt.event.WindowEvent;
import java.beans.ExceptionListener;
import java.beans.XMLEncoder;
import java.io.File;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.PrintWriter;
import java.io.Serializable;
import java.io.StringWriter;
import java.util.ArrayList;
import java.util.List;

import javax.swing.BoxLayout;
import javax.swing.JButton;
import javax.swing.JFileChooser;
import javax.swing.JFrame;
import javax.swing.JLabel;
import javax.swing.JPanel;
import javax.swing.JTextField;
import javax.swing.SwingConstants;
import javax.swing.border.TitledBorder;

import org.apache.commons.lang.StringUtils;

import com.db4o.Db4o;
import com.db4o.ObjectContainer;
import com.db4o.ext.ExtObjectContainer;
import com.db4o.ext.StoredClass;
import com.db4o.sync4o.Db4oSyncSource;
import com.funambol.admin.AdminException;
import com.funambol.admin.mo.SyncSourceManagementObject;
import com.funambol.admin.ui.SourceManagementPanel;
import com.funambol.framework.engine.source.ContentType;
import com.funambol.framework.engine.source.SyncSourceInfo;

/**
 * This class implements the configuration panel for a Db4oSyncSource.
 */
public class Db4oSyncSourceConfigPanel extends SourceManagementPanel implements
    Serializable {

  private static final long serialVersionUID = 6928167032547595512L;

  private static final String NAME_ALLOWED_CHARS = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890-_.";

  private JPanel _namePanel = new JPanel();

  private JPanel _fieldsPanel = new JPanel(new GridBagLayout());

  private JPanel _buttonsPanel = new JPanel();

  private JTextField _nameValue = new JTextField();

  private JTextField _sourceUriValue = new JTextField();

  private JTextField _dbFileValue = new JTextField();

  private SyncClassConfigTree _classConfigsTree = new SyncClassConfigTree(new ArrayList());

  private JButton _dbFileLocateButton = new JButton();

  private JButton _confirmButton = new JButton();

  private Db4oSyncSource _syncSource = null;

  /**
   * Default constructor is required by Funambol Admin UI
   */
  public Db4oSyncSourceConfigPanel() {

    setupControls();

    // now we install our event handlers and we are ready to go...

    // handler to update the SyncClassConfigTree upon a change 
    // in database file
    _dbFileLocateButton.addActionListener(new ActionListener(){

      public void actionPerformed(ActionEvent e) {

        try {
         
          final JFileChooser fc = new JFileChooser();
          int rc = fc.showOpenDialog(Db4oSyncSourceConfigPanel.this);
          if (JFileChooser.APPROVE_OPTION == rc) {
        
            File f = fc.getSelectedFile();
            _dbFileValue.setText(f.getPath());
            Db4oSyncSourceConfigPanel.this.refreshClassConfigsFromFile();
      
          }
        
        }
        catch (Exception ex) {
      
          StringWriter s = new StringWriter();
          PrintWriter w = new PrintWriter(s);
          
          ex.printStackTrace(w);
          notifyError(new AdminException(s.toString()));
        
        }
      
      }
      
    });

    // handler for the "Add" or "Update" button
    _confirmButton.addActionListener(new ActionListener(){

      public void actionPerformed(ActionEvent event) {

        try {
          validateValues();
          updateSyncSource();

          if (getState() == STATE_INSERT) {
            Db4oSyncSourceConfigPanel.this.actionPerformed(new ActionEvent(
                Db4oSyncSourceConfigPanel.this, ACTION_EVENT_INSERT, event
                    .getActionCommand()));
          }
          else {
            Db4oSyncSourceConfigPanel.this.actionPerformed(new ActionEvent(
                Db4oSyncSourceConfigPanel.this, ACTION_EVENT_UPDATE, event
                    .getActionCommand()));
          }
        }
        catch (Exception e) {
          notifyError(new AdminException(e.getMessage()));
        }
      }
    });

  }

  private void setupControls() {

    // Layout and setup UI components...

    setLayout(new BoxLayout(this, BoxLayout.PAGE_AXIS));
    add(_namePanel);
    _namePanel.setAlignmentX(Component.LEFT_ALIGNMENT);
    add(_fieldsPanel);
    _fieldsPanel.setAlignmentX(Component.LEFT_ALIGNMENT);
    add(_classConfigsTree);
    _classConfigsTree.setAlignmentX(Component.LEFT_ALIGNMENT);
    _classConfigsTree.setPreferredSize(new Dimension(300, 300));
    add(_buttonsPanel);
    _buttonsPanel.setAlignmentX(Component.LEFT_ALIGNMENT);

    JLabel l;

    // Admin UI Management Panels use a title
    // (in a particular "title font") to identify themselves
    l = new JLabel("Edit Db4oSyncSource Configuration", SwingConstants.CENTER);
    l.setBorder(new TitledBorder(""));
    l.setFont(titlePanelFont);
    _namePanel.add(l);

    GridBagConstraints labelConstraints = new GridBagConstraints();
    labelConstraints.gridwidth = 1;
    labelConstraints.fill = GridBagConstraints.NONE;
    labelConstraints.weightx = 0.0;
    labelConstraints.gridx = 0;
    labelConstraints.gridy = 0;
    labelConstraints.anchor = GridBagConstraints.EAST;

    GridBagConstraints fieldConstraints = new GridBagConstraints();
    fieldConstraints.gridwidth = 2;
    fieldConstraints.fill = GridBagConstraints.HORIZONTAL;
    fieldConstraints.weightx = 1.0;
    fieldConstraints.gridx = 1;
    fieldConstraints.gridy = 0;

    _fieldsPanel.add(new JLabel("Source URI: "), labelConstraints);
    _fieldsPanel.add(_sourceUriValue, fieldConstraints);

    labelConstraints.gridy = GridBagConstraints.RELATIVE;
    fieldConstraints.gridy = GridBagConstraints.RELATIVE;

    _fieldsPanel.add(new JLabel("Name: "), labelConstraints);
    _fieldsPanel.add(_nameValue, fieldConstraints);

    fieldConstraints.gridwidth = 1;

    _fieldsPanel.add(new JLabel("db4o File: "), labelConstraints);
    _fieldsPanel.add(_dbFileValue, fieldConstraints);

    _dbFileValue.setEditable(false);

    fieldConstraints.gridwidth = 2;

    GridBagConstraints buttonConstraints = new GridBagConstraints();
    buttonConstraints.gridwidth = 1;
    buttonConstraints.fill = GridBagConstraints.NONE;
    buttonConstraints.gridx = 2;
    buttonConstraints.gridy = 3;
    _dbFileLocateButton.setText("...");
    _fieldsPanel.add(_dbFileLocateButton, buttonConstraints);

    buttonConstraints.gridwidth = 3;
    buttonConstraints.fill = GridBagConstraints.NONE;
    buttonConstraints.gridx = 0;
    buttonConstraints.gridy = GridBagConstraints.RELATIVE;
    buttonConstraints.anchor = GridBagConstraints.CENTER;

    // Ensure all the controls use the Admin UI standard font
    Component[] components = _fieldsPanel.getComponents();
    for (int i = 0; i < components.length; i++){
      
      Component c = components[i];
      c.setFont(defaultFont);
      
    }

    _confirmButton.setText("Add");
    _buttonsPanel.add(_confirmButton);

  }

  /**
   * Loads the provided SyncSource for editing.
   * 
   * @param _syncSource
   *          the SyncSource instance to be edited.
   */
  public void updateForm() {

    // Ensure the SyncSource that has been provided is valid
    if (!(getSyncSource() instanceof Db4oSyncSource)) {
  
      notifyError(new AdminException(
          "This is not a Db4oSyncSource! Unable to process."));
      return;
    
    }

    _syncSource = (Db4oSyncSource) getSyncSource();

    if (getState() == STATE_INSERT) {
    
      _confirmButton.setText("Add");
    
    }
    else if (getState() == STATE_UPDATE) {
    
      _confirmButton.setText("Save");
    
    }

    _sourceUriValue.setText(_syncSource.getSourceURI());
    _nameValue.setText(_syncSource.getName());
    _sourceUriValue.setEditable(_syncSource.getSourceURI() == null);

    String filename = _syncSource.getDbFilename();
    _dbFileValue.setText((filename != null) ? filename : "");
    Db4oSyncSourceConfigPanel.this.refreshClassConfigsFromFile();

    _classConfigsTree.setConfigs(_syncSource.getClassConfigs());
  
  }

  /**
   * Set SyncSource properties with the values provided by the user.
   */
  private void updateSyncSource() {

    _syncSource.setSourceURI(_sourceUriValue.getText().trim());
    _syncSource.setName(_nameValue.getText().trim());
    _syncSource.setDbFilename(_dbFileValue.getText());
    _syncSource.setClassConfigs(_classConfigsTree.getConfigs());

    ContentType[] contentTypes = new ContentType[] { new ContentType(
        "application/octet-stream", "1.0") };

    _syncSource.setInfo(new SyncSourceInfo(contentTypes, 0));
  
  }

  /**
   * Checks if the values provided by the user are all valid. Whenever an
   * invalid value is found, an IllegalArgumentException is thrown (this
   * appears to be the pattern expected by the calling Funambol infrastructure).
   */
  private void validateValues() {

    String value = null;

    value = _nameValue.getText();
    if (StringUtils.isBlank(value)) {
      notifyError(new AdminException(
          "Field 'Name' cannot be empty. Please provide a SyncSource name."));
      return;
    }

    if (!StringUtils.containsOnly(value, NAME_ALLOWED_CHARS.toCharArray())) {
      notifyError(new AdminException(
          "Only the following characters are allowed for field 'Name': \n"
              + NAME_ALLOWED_CHARS));
      return;
    }

    value = _sourceUriValue.getText();
    if (StringUtils.isBlank(value)) {
      notifyError(new AdminException(
          "Field 'Source URI' cannot be empty. Please provide a SyncSource URI."));
      return;
    }

    value = _dbFileValue.getText();
    if (StringUtils.isBlank(value)) {
      notifyError(new AdminException(
          "Field 'db4o File' cannot be empty. Please provide a path to a db4o file."));
      return;
    }

    File f = new File(value);
    if (!isValidDatabase(f)) {
      notifyError(new AdminException(
          "Field 'db4o File' must refer to an existing db4o file."));
      return;
    }

    if (!_classConfigsTree.validateConfigs()) {
      notifyError(new AdminException(
          "Configuration of classes to be synchronized is in error."));
      return;
    }
  }

  private void refreshClassConfigsFromFile() {

    String s = _dbFileValue.getText();
    if (!StringUtils.isBlank(s)) {
      
      List classes = getStoredClasses(new File(s));
      _classConfigsTree.setValidClasses(classes);
    
    }
  
  }

  /**
   * Tests whether the supplied file is a valid db4o database file.
   * @param f The file to be tested.
   * @return true if the file is a db4o database, else false
   */
  private boolean isValidDatabase(File f) {
	ObjectContainer oc = null;
	try{
		oc = Db4o.openFile(f.getAbsolutePath());
	}
	catch(com.db4o.ext.DatabaseFileLockedException fle){
		return true;
	}
	if(oc != null){
		oc.close();
		return true;
	}
    return false;
  }

  /**
   * Returns all the "non-system" classes stored within a db4o database. 
   * @param f The db4o database to be queried for stored classes.
   * @return An array of StoredClass objects describing the classes in the database.
   */
  private static List getStoredClasses(File f) {

    List classes = new ArrayList();
    if (f != null) {
      
      ObjectContainer container = Db4o.openFile(f.getAbsolutePath());

      if (container != null) {
      
        ExtObjectContainer db = container.ext();
        StoredClass[] storedClasses = db.storedClasses(); 
        for (int i = 0; i < storedClasses.length; i++){

          StoredClass c = storedClasses[i];
          
          if (!c.getName().startsWith("com.db4o")) {
          
            classes.add(c);
          
          }

        }
        container.close();
    
      }
    
    }

    return classes;
  }

  // only used for testing
  private static void showTestUI() {

    // Make sure we have nice window decorations.
    JFrame.setDefaultLookAndFeelDecorated(true);

    // Create and set up the window.
    JFrame frame = new JFrame("Db4oSyncSourceConfigPanel Test Harness");
    frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);

    // Create and set up the content pane.
    final Db4oSyncSource source = new Db4oSyncSource();
    Db4oSyncSourceConfigPanel p = new Db4oSyncSourceConfigPanel();
    p.setManagementObject(new SyncSourceManagementObject(source, null, null,
        null, null));
    p.updateForm();
    p.setOpaque(true); // content panes must be opaque
    frame.setContentPane(p);

    frame.addWindowListener(new WindowAdapter(){
      
      public void windowClosing(WindowEvent ev){
        
        XMLEncoder encoder = null;
        try{
          FileOutputStream s = new FileOutputStream("test.xml");
          encoder = new XMLEncoder(s);
          encoder.setExceptionListener(new ExceptionListener() {
            public void exceptionThrown(Exception exception) {
                exception.printStackTrace();
            }
          });          
        }
        catch (FileNotFoundException e){
          e.printStackTrace();
          System.exit(-1);
        }
        encoder.writeObject((Object)source);
        encoder.flush();
        encoder.close();
        
      }
      
    });
    
    // Display the window.
    frame.pack();
    frame.setVisible(true);
    
  }

  // only used for testing
  public static void main(String[] args) {

    // Schedule a job for the event-dispatching thread:
    // creating and showing this application's GUI.
    javax.swing.SwingUtilities.invokeLater(new Runnable(){

      public void run() {

        showTestUI();
      }
    });
  }
}
