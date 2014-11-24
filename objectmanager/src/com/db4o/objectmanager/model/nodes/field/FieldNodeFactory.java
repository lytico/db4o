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

import java.util.Date;

import com.db4o.objectmanager.model.IDatabase;
import com.db4o.objectmanager.model.nodes.IModelNode;
import com.db4o.reflect.ReflectClass;


/**
 * Class FieldNodeFactory.  Construct FieldNodes from the object instances.
 * 
 * @author djo
 */
public class FieldNodeFactory {
    
    private static Class[] boxedPrimitiveTypes = {
    		Integer.class,
            Float.class,
            Double.class,
            Long.class,
            Boolean.class,
            Character.class,
            String.class,
            Date.class
    };

    /**
     * Test to see if clazz is in classArray
     * 
     * @param clazz The class to test
     * @param classArray A bunch of classes
     * @return if clazz is in classArray
     */
    private static boolean typeIn(Class clazz, Class[] classArray) {
        for (int i = 0; i < classArray.length; i++) {
            if (classArray[i].isAssignableFrom(clazz))
                return true;
        }
        return false;
    }
    
	/**
     * Construct a FieldNode.  FIXME: Eventually, each class should register
     * itself with the factory rather than hard-coding all of these tests.
     * 
	 * @param fieldName
	 * @param instance
	 * @param database 
	 * @return
	 */
	public static IModelNode construct(String fieldName, ReflectClass fieldType,Object instance, IDatabase database) {
        /*
         * There are 4 use-cases here:
         * 
         * 0) The field is a primitive type: no children
         * 1) A field is a List: can use iterator()
         * 2) A field is a Map: need to use keySet().iterator()
         * 3) A field is an object, in which case it may have fields
         */
        IModelNode result;
        
		ReflectClass valueType = database.reflector().forObject(instance);
		
		if (instance == null) {
			return new FieldNode(fieldName, fieldType,instance, database);
		}
		
        if (valueType.isSecondClass()) {
            return new PrimitiveFieldNode(fieldName, fieldType,instance, database);
        }
        
        if (valueType.isArray()) {
            return new ArrayFieldNode(fieldName, fieldType,instance, database);
        }
        
        // If keySet() and get() are present, use them
        result = MapFieldNode.tryToCreate(fieldName, instance, database);
        if (result != null) return result;
        
        // Otherwise treat all collection-like things as lists
		if (valueType.isCollection()) {
			return new CollectionFieldNode(fieldName, fieldType,instance, database);
		}

        // Otherwise it must be a plain old object
		return new FieldNode(fieldName, fieldType,instance, database);
	}
    /*
     * Reflector.Object
     * Reflector.Integer
     * Reflector.Float
     * Reflector.IntTYPE
     * ...
     */

}


