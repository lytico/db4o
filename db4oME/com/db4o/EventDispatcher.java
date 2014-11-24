/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o;

import com.db4o.reflect.*;

final class EventDispatcher
{
	private static final String[] events = {
		"objectCanDelete",
		"objectOnDelete", 
		"objectOnActivate", 
		"objectOnDeactivate",
		"objectOnNew",
		"objectOnUpdate",
		"objectCanActivate",
		"objectCanDeactivate",
		"objectCanNew",
		"objectCanUpdate"
	};
	
	static final int CAN_DELETE = 0;
	static final int DELETE = 1;
	static final int SERVER_COUNT = 2;
	static final int ACTIVATE = 2;
	static final int DEACTIVATE = 3;
	static final int NEW = 4;
	static final int UPDATE = 5;
	static final int CAN_ACTIVATE = 6;
	static final int CAN_DEACTIVATE = 7;
	static final int CAN_NEW = 8;
	static final int CAN_UPDATE = 9;
	static final int COUNT = 10;
	
	private final ReflectMethod[] methods;
	
	private EventDispatcher(ReflectMethod[] methods){
		this.methods = methods;
	}
	
	boolean dispatch(YapStream stream, Object obj, int eventID){
		if(methods[eventID] != null){
			Object[] parameters = new Object[]{stream};
			try{
				Object res = methods[eventID].invoke(obj,parameters);
				if(res != null && res instanceof Boolean){
				    return ((Boolean)res).booleanValue();
				}
			}catch(Throwable t){
			}
		}
		return true;
	}
	
	static EventDispatcher forClass(YapStream a_stream, ReflectClass classReflector){
        
        if(a_stream == null || classReflector == null){
            return null;
        }
        
		EventDispatcher dispatcher = null;
	    int count = 0;
	    if(a_stream.i_config.callbacks()){
	        count = COUNT;
	    }else if(a_stream.i_config.isServer()){
	        count = SERVER_COUNT;
	    }
	    if(count > 0){
			ReflectClass[] parameterClasses = {a_stream.i_handlers.ICLASS_OBJECTCONTAINER};
			ReflectMethod[] methods = new ReflectMethod[COUNT];
			for (int i = COUNT -1; i >=0; i--){
				try{
					ReflectMethod method = classReflector.getMethod(events[i], parameterClasses);
					if (null == method) {
						method = classReflector.getMethod(toPascalCase(events[i]), parameterClasses);
					}
					methods[i] = method;
					if(dispatcher == null){
						dispatcher = new EventDispatcher(methods);
					}
				}catch(Throwable t){}
			}
	    }
        
		return dispatcher;
	}

	private static String toPascalCase(String name) {
		return name.substring(0, 1).toUpperCase() + name.substring(1);
	}
}
