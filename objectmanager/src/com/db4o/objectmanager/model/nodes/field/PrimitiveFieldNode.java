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
/*
import org.eclipse.ve.sweet.converter.ConverterRegistry;
import org.eclipse.ve.sweet.converter.IConverter;*/

import com.db4o.objectmanager.model.IDatabase;
import com.db4o.objectmanager.model.nodes.IModelNode;
import com.db4o.reflect.ReflectClass;



/**
 * Class PrimitiveFieldNode. Represents primitive types like Integer, int,
 * Float, float, etc.
 * 
 * @author djo
 */
public class PrimitiveFieldNode extends FieldNode {

	/**
	 * @param fieldName
	 * @param instance
	 * @param database 
	 */
	public PrimitiveFieldNode(String fieldName, ReflectClass fieldType,Object instance, IDatabase database) {
		super(fieldName, fieldType,instance, database);
	}
    
    /* (non-Javadoc)
	 * @see com.db4o.browser.gui.FieldNode#mayHaveChildren()
	 */
	public boolean hasChildren() {
		return false;
	}
	
	public IModelNode[] children() {
		return new IModelNode[0];
	}

	public void printXmlReferenceNode(PrintStream out) {
		out.println("Unexpected: Primitives should never be referenced!");
	}

	public void printXmlValueNode(PrintStream out) {
		out.print("<" + _fieldName + ">");
		out.print(XmlEntity.encode(convert(value)));
		out.print("</" + _fieldName + ">");
	}

	private String convert(Object instance) {
		//IConverter converter = converter(instance.getClass());
		//if (converter == null) {
			return instance.toString();
		//}
		//return (String) converter.convert(instance);
	}
/*
	private IConverter converter(Class type) {
		return ConverterRegistry.get(type.getName(), String.class.getName());
	}*/

	public boolean shouldIndent() {
		return false;
	}
	
	public long getId() {
		return -1;
	}
}


