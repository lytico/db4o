package com.db4o.objectManager.v2.importExport;

import java.util.*;

import com.db4o.objectmanager.api.util.*;
import com.db4o.reflect.*;
import com.db4o.reflect.generic.*;
import com.thoughtworks.xstream.converters.*;
import com.thoughtworks.xstream.io.*;

/**
 * User: treeder
 * Date: Mar 19, 2007
 * Time: 1:05:58 AM
 */
public class GenericObjectConverter implements Converter {

	/**
	 * This map holds all the GenericClass's we've seen so far
	 */
	private Map classMap = new HashMap();

	public GenericObjectConverter() {
	}

	public boolean canConvert(Class clazz) {
		return GenericObject.class.isAssignableFrom(clazz);
	}

	public void marshal(Object value, HierarchicalStreamWriter writer,
						MarshallingContext context) {
		GenericObject ob = (GenericObject) value;
		// todo: check in GenericObject change:
        /*GenericClass genericClass = ob.getGenericClass();
		ReflectField[] fields = genericClass.getDeclaredFields();
		for(int i = 0; i < fields.length; i++) {
			ReflectField field = fields[i];
			writer.startNode(field.getName());
			writer.addAttribute("type", field.getFieldType().getName());
			context.convertAnother(field.get(ob));
//			writer.setValue(String.valueOf(field.get(ob)));
			writer.endNode();
		}
		*/
	}

	public Object unmarshal(HierarchicalStreamReader reader,
							UnmarshallingContext context) {
		// todo: NEED CLASS NAME
		String className = "XYZ";
		GenericClass gc = (GenericClass) classMap.get(className);
		if(gc == null) {
			gc = GenericObjectUtil.makeGenericClass(className);
			classMap.put(className, gc);
		}
		GenericObject ob = new GenericObject(gc);

		while(reader.hasMoreChildren()) {
			reader.moveDown();
			String nodeName = reader.getNodeName();

			ReflectField rf = getOrCreateField(gc, nodeName);
			// todo: forname below returns nul
			System.out.println("getting node: " + gc.reflector().forName(className) + ", name:" + nodeName);
			rf.set(ob, context.convertAnother(ob, gc.reflector().forName(className).getClass()));

			reader.moveUp();
		}
		return ob;
	}

	private ReflectField getOrCreateField(GenericClass gc, String nodeName) {
		ReflectField rf = gc.getDeclaredField(nodeName);
		if(rf == null) {
			GenericField gf = new GenericField(nodeName, gc, false); // todo: set the booleans properly
			GenericField[] fieldsPrevious = (GenericField[]) gc.getDeclaredFields();
			GenericField[] fieldsNew = new GenericField[fieldsPrevious.length + 1];
			System.arraycopy(fieldsPrevious, 0, fieldsNew, 0, fieldsNew.length - 1);
			fieldsNew[fieldsNew.length-1] = gf;
			gc.initFields(fieldsNew);
			rf = gf;
		}
		return rf;
	}

}
