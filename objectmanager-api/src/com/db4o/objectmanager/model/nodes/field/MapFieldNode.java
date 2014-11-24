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

import java.io.*;
import java.util.*;
import java.util.logging.*;

import com.db4o.objectmanager.model.*;
import com.db4o.objectmanager.model.nodes.*;
import com.db4o.objectmanager.model.nodes.partition.*;
import com.db4o.reflect.*;

/**
 * Class MapFieldNode.
 * 
 * @author djo
 */
public class MapFieldNode extends FieldNode {

    private static Logger logger = Logger.getLogger(MapFieldNode.class.getName());

    private static class GetStrategy{
		
		GetStrategy(String keySetMethod, String getMethod) {
			this.keySetMethod = keySetMethod;
			this.getMethod = getMethod;
		}
		
		final String keySetMethod;
		final String getMethod;
	}
	
	private static final GetStrategy[] STRATEGIES = new GetStrategy[] {
		new GetStrategy("keySet", "get"),
		// FIXME: find out for .NET   new GetStrategy("keySet", "get"),
	};
	
	public static IModelNode tryToCreate(String fieldName, Object instance, IDatabase database) {
		for (int i = 0; i < STRATEGIES.length; i++) {
			IModelNode node = tryToCreate(STRATEGIES[i], fieldName, instance, database);
			if(node != null) {
				return node;
			}
		}
		return null;
	}

    public static IModelNode tryToCreate(GetStrategy strategy, String fieldName, Object fieldContents, IDatabase database) {
        MapFieldNode result;
        
        // See if we can get ReflectMethods corresponding to keySet() and get()
        ReflectClass fieldType = database.reflector().forObject(fieldContents);
        ReflectMethod keySet = null;
		ReflectMethod get = null;
        ReflectClass object = database.reflector().forName("java.lang.Object");
        keySet = fieldType.getMethod(strategy.keySetMethod, new ReflectClass[] {});
        get = fieldType.getMethod(strategy.getMethod, new ReflectClass[] {object});
        
        if (keySet == null || get == null) {
            return null;
        }
        
        try {
            result = new MapFieldNode(fieldName, fieldType, fieldContents, keySet, get, database);
            result.iterator();
        } catch (IllegalStateException e) {
            logger.log(Level.SEVERE, "Unable to invoke 'iterator()'", e);
            return null;
        }
        return result;
    }

	private ReflectMethod _keySetMethod;
	private ReflectMethod _getMethod;

	private Iterator iterator() {
        Set set;
        try {
            set = (Set) _keySetMethod.invoke(value, new Object[] {});
        } catch (Exception e) {
            logger.log(Level.SEVERE, "Unable to invoke 'keySet'", e);
            throw new IllegalStateException();
        }
        return set.iterator();
    }
	
	private Object get(Object key) {
		Object result;
		try {
			result = _getMethod.invoke(value, new Object[] {key});
		} catch (Exception e) {
			logger.log(Level.SEVERE, "Unable to invoke 'get'", e);
			throw new IllegalStateException();
		}
		return result;
	}

    public MapFieldNode(String fieldName, ReflectClass fieldType,Object instance, ReflectMethod keySetMethod, ReflectMethod getMethod, IDatabase database) {
        super(fieldName, fieldType, instance, database);
        
        _keySetMethod = keySetMethod;
		_getMethod = getMethod;
	}
    
	public boolean hasChildren() {
		return iterator().hasNext();
	}
	
	public IModelNode[] children() {
        LinkedList results = new LinkedList();
        Iterator i = iterator();
        while (i.hasNext()) {
			Object key = i.next();
            results.addLast(FieldNodeFactory.construct(key.toString(), _database.reflector().forName("java.lang.Object"),get(key), _database));
        }
        IModelNode[] finalResults = new IModelNode[results.size()];
        int elementNum=0;
        for (i = results.iterator(); i.hasNext();) {
            IModelNode element = (IModelNode) i.next();
            finalResults[elementNum] = element;
            ++elementNum;
        }
        return PartitionFieldNodeFactory.create(finalResults,0,finalResults.length,_database);
	}

	public String getText() {
        final String className = _database.reflector().forObject(value).getName() + " (id=" + _database.getId(value) + ")";
        return _fieldName.equals("") ? className : _fieldName + ": " + className;
	}

    public boolean isEditable() {
        return false;
    }

	public void printXmlValueNode(PrintStream out) {
		out.print("<" + getNodeName() + " id=\"" + getId() + "\">");
		out.print("</" + getNodeName() + ">");
	}

	public boolean shouldIndent() {
		return true;
	}
}
