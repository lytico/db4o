/*
 * This file is part of com.db4o.browser.
 *
 * com.db4o.browser is free software; you can redistribute it and/or modify
 * it under the terms of version 2 of the GNU General Public License
 * as published by the Free Software Foundation.
 *
 * com.db4o.browser is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with com.swtworkbench.ed; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 */
package com.db4o.objectmanager.model.nodes.field;

import java.io.PrintStream;
import java.util.logging.Logger;
import java.util.logging.Level;
import java.util.Date;

//import org.eclipse.ve.sweet.converter.ConverterRegistry;

import com.db4o.objectmanager.model.IDatabase;
import com.db4o.objectmanager.model.nodes.IModelNode;
import com.db4o.objectmanager.model.nodes.NullNode;
import com.db4o.reflect.ReflectClass;
import com.db4o.reflect.ReflectField;
import com.db4o.reflect.jdk.JdkReflector;


/**
 * Class FieldNode.  The patriarch of the FieldNode hierarchy.  Implements
 * common functionality.<p>
 * 
 * A FieldNode is always an object reference.  If one has a child of FieldNode
 * in the inheritence hierarchy, the child's type says what the FieldNode 
 * represents.  Otherwise, the FieldNode is just an object reference, which
 * can point to an instance (in which case delegate is an InstanceNode) or
 * that can point to null (in which case delegate is the NullNode).<p>
 * 
 * Note that we always deal with the boxed versions of primitives, not the
 * second-class versions, since those are the ones the reflection API gives us,
 * regardless of the actual underlying type.
 * 
 * @author djo
 */
public class FieldNode implements IModelNode {

    protected String _fieldName;
	protected Object value;
	protected IDatabase _database;
    protected IModelNode delegate = null;
    protected ReflectClass _fieldType;
    private boolean showType;

	public FieldNode(String fieldName, ReflectClass fieldType,Object instance, IDatabase database) {
        _fieldName = fieldName;
        value = instance;
		_database = database;
        _fieldType=fieldType;

		if(value==null) {
			delegate=new NullNode(database);
			return;
		}
        ReflectClass clazz = database.reflector().forObject(value);
		delegate = new InstanceNode(value, database);
	}
    
    public IDatabase getDatabase() {
        return _database;
    }

	/* (non-Javadoc)
	 * @see com.db4o.browser.gui.ITreeNode#mayHaveChildren()
	 */
	public boolean hasChildren() {
		return delegate.hasChildren();
	}

	/* (non-Javadoc)
	 * @see com.db4o.browser.gui.ITreeNode#children()
	 */
	public IModelNode[] children() {
		return delegate.children();
	}

	/* (non-Javadoc)
	 * @see com.db4o.browser.gui.ITreeNode#getText()
	 */
	public String getText() {
        String fieldDataType = "";
        if (showType) {
            fieldDataType = "(" + _fieldType.getName() + ") ";
        }
		return  fieldDataType // Show the data type
                + (_fieldName.equals("") ? delegate.getText()             // Show the field name
                        : _fieldName + ": " + delegate.getText());        // Show the actual value
	}
	
	/* (non-Javadoc)
	 * @see com.db4o.objectmanager.model.nodes.IModelNode#getName()
	 */
	public String getName() {
        String fieldDataType = "";
        if (showType) {
            fieldDataType = "(" + _fieldType.getName() + ") ";
        }
		return fieldDataType +_fieldName;
	}
	
	/* (non-Javadoc)
	 * @see com.db4o.objectmanager.model.nodes.IModelNode#getValueString()
	 */
	public String getValueString() {
		return (value==null ? "null" : value.toString());
	}

	public boolean equals(Object obj) {
		if(obj==this) {
			return true;
		}
		if(obj==null||getClass()!=obj.getClass()) {
			return false;
		}
		FieldNode node=(FieldNode)obj;
		return value.equals(node.value);
	}
	
	public int hashCode() {
		return value.hashCode();
	}

    public static Object field(ReflectField field, Object instance) {
        try {
            field.setAccessible();
            return field.get(instance);
        } catch (Exception e) {
            Logger.getLogger(FieldNode.class.getName()).log(Level.WARNING, "Unable to get the field contents", e);
            throw new IllegalStateException();
        }
    }

    public void setShowType(boolean showType) {
        this.showType = showType;
    }

    public boolean isEditable() {
        if (value == null) {
            return false;
        }
        //return _database.reflector().forObject(value).isPrimitive();
        /*return ConverterRegistry.canConvert(_database.reflector().forObject(value).getName(),
                _database.reflector().forClass(String.class).getName());
        */
        return canConvert(value, _fieldType);
    }

    private boolean canConvert(Object value, ReflectClass fieldType) {
        Class c = JdkReflector.toNative(fieldType);
        if(c == null){
            // JdkReflector may return null if GenericObject
            return false;
        }
        return c.isPrimitive() || String.class.isAssignableFrom(c) || Number.class.isAssignableFrom(c) || Date.class.isAssignableFrom(c);
    }

    public Object getEditValue() {
        return value;
    }

	public long getId() {
		return delegate.getId();
	}

	protected String getNodeName() {
		if (_fieldName == null || _fieldName.equals("")) {
			return value.getClass().getName();
		} else {
			return _fieldName;
		}
	}

	public void printXmlReferenceNode(PrintStream out) {
		if (delegate instanceof NullNode) {
			out.print("<" + getNodeName() + " reference=\"-1\"/>\n");
		} else {
			out.print("<" + getNodeName() + " reference=\"" + getId() + "\"/>\n");
		}
	}
	
	public void printXmlStart(PrintStream out) {
		out.println("<" + getNodeName() + " id=\"" + getId() 
				+ (_fieldName == null || _fieldName.equals("") ? "" : "\" className=\"" + value.getClass().getName()) 
				+ "\">");
	}

	public void printXmlEnd(PrintStream out) {
		out.println("</" + getNodeName() + ">");
	}

	public void printXmlValueNode(PrintStream out) {
		out.print("<" + getNodeName() + ">");
		delegate.printXmlValueNode(out);
		out.print("</" + getNodeName() + ">");
	}

	public boolean shouldIndent() {
		return true;
	}

}
