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
import java.awt.GridLayout;
import java.lang.reflect.Method;
import java.util.HashSet;
import java.util.Iterator;
import java.util.List;
import java.util.Set;
import java.util.Vector;

import javax.swing.DefaultCellEditor;
import javax.swing.JComboBox;
import javax.swing.JPanel;
import javax.swing.JScrollPane;
import javax.swing.JTree;
import javax.swing.tree.DefaultMutableTreeNode;
import javax.swing.tree.DefaultTreeCellEditor;
import javax.swing.tree.DefaultTreeCellRenderer;
import javax.swing.tree.DefaultTreeModel;
import javax.swing.tree.TreeModel;
import javax.swing.tree.TreePath;

import com.db4o.ext.StoredClass;
import com.db4o.ext.StoredField;
import com.db4o.reflect.ReflectClass;
import com.db4o.sync4o.SyncClassConfig;

/**
 * Tree component that collects SyncClassConfig data for a Db4oSyncSource.
 */
public class SyncClassConfigTree extends JPanel {

  final class SyncFieldConfigInfo {

    private SyncClassConfigInfo _parent;

    private Method _setterMethod;

    private Method _getterMethod;

    private Vector _fields;

    public SyncFieldConfigInfo(SyncClassConfigInfo parent, Method setterMethod,
        Method getterMethod, Class compatibleClass) {

      if ((parent == null) || (setterMethod == null) || (getterMethod == null)){
      
        throw new IllegalArgumentException();
      
      }
      
      _parent = parent;
      _setterMethod = setterMethod;
      _getterMethod = getterMethod;
      _fields = new Vector();

      StoredField[] storedFields = parent.getStoredClass().getStoredFields(); 
      for (int i = 0; i < storedFields.length; ++i) {
       
        StoredField f = (StoredField) storedFields[i];
        
        if (compatibleClass != null) {
        
          ReflectClass storedType = f.getStoredType();
          ReflectClass compatibleType = storedType.reflector().forClass(compatibleClass);
          
          if (compatibleType.isAssignableFrom(storedType)) {
          
            _fields.add(f.getName());
          
          }
          
        }
        else {
        
          _fields.add(f.getName());

        }
    
      }
    
    }

    public String getField() {

      String s;

      try {
        
        s = (String) _getterMethod.invoke(_parent.getConfig(), new Object[0]);
      
      }
      catch (Exception e) {
      
        s = "error";
      
      }

      return s;
    
    }

    public void setField(String field) {

      if ((field != null) && !_fields.contains(field)) {
      
        throw new IllegalArgumentException(
            "Attempt to set a field value that is not in the list of fields.");
      
      }

      try {
      
        _setterMethod.invoke(_parent.getConfig(), new Object[] { field });
      
      }
      catch (Exception e) {
      
        throw new IllegalArgumentException("Bad argument to setField.");
      
      }

    }

    public Vector getFields() {

      return _fields;
      
    }

    public void setFields(Vector fields) {

      _fields = fields;
      if (!_fields.contains(getField())) {
        
        setField(null);
        
      }
      
    }

    public String toString() {

      String s = getField();

      if (s == null) {
    
        s = "-unconfigured-";
      
      }

      return s;
      
    }
    
  }

  final class SyncClassConfigInfo {

    private SyncClassConfig _config;

    private StoredClass _storedClass;

    private SyncFieldConfigInfo _uniqueInfo;

    public SyncClassConfig getConfig() {

      return _config;

    }

    public StoredClass getStoredClass() {

      return _storedClass;
    
    }

    public SyncClassConfigInfo(SyncClassConfig config, StoredClass storedClass)
        throws NoSuchMethodException {

      Class cls = SyncClassConfig.class;
      Class[] stringParam = new Class[] { String.class };
      _config = config;
      _storedClass = storedClass;

      _uniqueInfo = new SyncFieldConfigInfo(this, cls.getMethod(
          "setUniqueField", stringParam), cls.getMethod("getUniqueField",
          (Class[]) null), null);

    }

    public SyncFieldConfigInfo getUniqueInfo() {

      return _uniqueInfo;
    
    }

    public String toString() {

      String s;

      if (_config.getUniqueField() == null) {
    
        s = _config.getClassName() + " -not configured for synchronization-";
      
      }
      else {
      
        s = _config.getClassName() + "-configured for syncronization-";
      
      }

      return s;
    }
    
  }

  private static final long serialVersionUID = -722664398181924425L;

  private DefaultMutableTreeNode _root;

  private JTree _tree;

  private List _validClasses;

  private Set _configs;

  /**
   * Constructs a new tree for SyncClassConfig configuration.
   *  
   * @param classes The classes to be configured.
   */
  public SyncClassConfigTree(List classes) {

    super(new GridLayout(1, 0));

    _root = new DefaultMutableTreeNode("root");

    TreeModel model = new DefaultTreeModel(_root){

      private static final long serialVersionUID = 331862003127838793L;

      public void valueForPathChanged(TreePath path, Object newValue) {

        DefaultMutableTreeNode node = (DefaultMutableTreeNode) path
            .getLastPathComponent();
        SyncFieldConfigInfo info = (SyncFieldConfigInfo) node.getUserObject();
        info.setField((String) newValue);
        
      }
      
    };

    _tree = new JTree(model){

      private static final long serialVersionUID = 5740875612425605626L;

      public boolean isPathEditable(TreePath path) {

        boolean editable = false;
        if (isEditable()) {
      
          editable = getModel().isLeaf(path.getLastPathComponent());
        
        }
        
        return editable;
      }
      
    };

    _validClasses = classes;
    _configs = new HashSet();

    _tree.setEditable(true);

    final JComboBox combo = new JComboBox();
    DefaultCellEditor comboEditor = new DefaultCellEditor(combo);
    DefaultTreeCellEditor editor = new DefaultTreeCellEditor(_tree,
        new DefaultTreeCellRenderer(), comboEditor){

      public Component getTreeCellEditorComponent(JTree tree, Object value,
          boolean isSelected, boolean expanded, boolean leaf, int row) {

        if ((value instanceof DefaultMutableTreeNode) && leaf) {
          
          combo.removeAllItems();
          SyncFieldConfigInfo info = (SyncFieldConfigInfo) ((DefaultMutableTreeNode) value)
              .getUserObject();
          
          for (Iterator i = info.getFields().iterator(); i.hasNext();) {
            
            String s = (String) i.next();
            combo.addItem(s);
            
          }
          
        }

        return super.getTreeCellEditorComponent(tree, value, isSelected,
            expanded, leaf, row);
        
      }
      
    };

    _tree.setCellEditor(editor);
    _tree.setVisible(true);

    add(new JScrollPane(_tree));
  }

  public void setConfigs(Set configs) {

    _configs = new HashSet(configs);

    for (Iterator i = configs.iterator(); i.hasNext();) {
      
      SyncClassConfig config = (SyncClassConfig) i.next();
      
      String name = config.getClassName();
      DefaultMutableTreeNode node = findConfigNode(name);

      if (node != null) {
      
        try {
        
          updateConfigNode(node, new SyncClassConfigInfo(config,
              ((SyncClassConfigInfo) node.getUserObject()).getStoredClass()));
        
        }
        catch (NoSuchMethodException e) {
        
          throw new IllegalArgumentException(e);
        
        }
        
      }
      else {
        
        // we throw away SyncClassConfig's for which we cannot
        // find a corresponding class
        _configs.remove(config);
      
      }
    
    }

    invalidate();
  }

  public Set getConfigs() {

    Set configs = new HashSet(_configs);

    if (!validateConfigs()) {
      
      throw new IllegalArgumentException(
          "Cannot obtain configs if tree is not valid.");
    
    }

    return configs;
  
  }

  public void setValidClasses(List classes) {

    _validClasses = classes;
    _root.removeAllChildren();

    for (Iterator i = classes.iterator(); i.hasNext();) {
      
      StoredClass c = (StoredClass) i.next();
      
      String name = c.getName();
      if (findConfigNode(name) == null) {
      
        try {
        
          SyncClassConfig config = new SyncClassConfig();
          config.setClassName(name);
          _configs.add(config);
          addNewConfigNode(new SyncClassConfigInfo(config, c));
        
        }
        catch (NoSuchMethodException e) {
        
          throw new IllegalArgumentException(e);
        
        }
      
      }
    
    }

    invalidate();
  }

  public boolean validateConfigs() {

    boolean valid = true;

    for (int i = 0; (valid && (i < _root.getChildCount())); ++i) {
    
      DefaultMutableTreeNode node = (DefaultMutableTreeNode) _root
          .getChildAt(i);
      valid &= validateConfig((SyncClassConfigInfo) node.getUserObject());
    
    }

    return valid;
  }

  private boolean validateConfig(SyncClassConfigInfo info) {

    SyncClassConfig c = info.getConfig();
    boolean valid = (isValidClass(c.getClassName()) &&
        isValidField(c.getClassName(), c.getUniqueField()));

    return valid;
  
  }

  private DefaultMutableTreeNode findConfigNode(String name) {

    DefaultMutableTreeNode found = null;

    for (int i = 0; (found == null) && (i < _root.getChildCount()); ++i) {
    
      DefaultMutableTreeNode node = (DefaultMutableTreeNode) _root
          .getChildAt(i);
      SyncClassConfigInfo info = (SyncClassConfigInfo) node.getUserObject();
      if (info.getConfig().getClassName().equals(name)) {
      
        found = node;
      
      }
    
    }

    return found;
  
  }

  private void addNewConfigNode(SyncClassConfigInfo info) {


    DefaultMutableTreeNode node = new DefaultMutableTreeNode(info);

    node.removeAllChildren();
    DefaultMutableTreeNode child;

    child = new DefaultMutableTreeNode(info.getUniqueInfo());
    node.add(child);

    _root.add(node);

  }

  private void updateConfigNode(DefaultMutableTreeNode node,
      SyncClassConfigInfo info) {

    node.setUserObject(info);
    DefaultMutableTreeNode child;

    child = (DefaultMutableTreeNode) node.getChildAt(0);
    child.setUserObject(info.getUniqueInfo());

  }

  private boolean isValidField(String className, String fieldName) {

    boolean exists = false;

    StoredClass c = getClassByName(className);
    if (c != null) {
  
      StoredField[] storedFields = c.getStoredFields(); 
      for (int i = 0; i < storedFields.length; ++i) {
      
        StoredField f = (StoredField) storedFields[i];
        
        if (f.getName().equals(fieldName)) {
        
          exists = true;
          break;
        
        }
      
      }
    
    }

    return exists;
  
  }

  private boolean isValidClass(String classname) {

    return (getClassByName(classname) != null);
  
  }

  private StoredClass getClassByName(String name) {

    StoredClass found = null;

    for (Iterator i = _validClasses.iterator(); i.hasNext();) {
      
      StoredClass c = (StoredClass) i.next();
      
      if (c.getName().equals(name)) {
      
        found = c;
        break;
    
      }

    }

    return found;
  }

}
