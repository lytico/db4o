package com.db4o.reflect.core;

import com.db4o.*;
import com.db4o.foundation.*;
import com.db4o.internal.*;
import com.db4o.reflect.*;

public class ConstructorSupport {
	
	
    public static ReflectConstructorSpec createConstructor(final ConstructorAwareReflectClass claxx, Class clazz, ReflectorConfiguration config, ReflectConstructor[] constructors){
        
        if (claxx == null) {
			return ReflectConstructorSpec.INVALID_CONSTRUCTOR; 
        }
        
        if (claxx.isAbstract() || claxx.isInterface()) {
        	return ReflectConstructorSpec.INVALID_CONSTRUCTOR;
        }

        if(! Platform4.callConstructor()){
    		boolean skipConstructor = !config.callConstructor(claxx);
            if(!claxx.isCollection()) {
            	ReflectConstructor serializableConstructor = skipConstructor(claxx, skipConstructor, config.testConstructors());
            	if(serializableConstructor != null){
            		return new ReflectConstructorSpec(serializableConstructor, null);
            	}
            }
        }

        if (!config.testConstructors()) {
        	return new ReflectConstructorSpec(new PlatformReflectConstructor(clazz), null);
        }

        if(ReflectPlatform.createInstance(clazz) != null) {
			return new ReflectConstructorSpec(new PlatformReflectConstructor(clazz), null);
		}

		Tree sortedConstructors = sortConstructorsByParamsCount(constructors);
		return findConstructor(claxx, sortedConstructors);
	}

	private static ReflectConstructorSpec findConstructor(final ReflectClass claxx,
			Tree sortedConstructors) {
		if (sortedConstructors == null) {
			return ReflectConstructorSpec.INVALID_CONSTRUCTOR;
		}
		
		Iterator4 iter = new TreeNodeIterator(sortedConstructors);
		while (iter.moveNext()) {
			Object current = iter.current();
			ReflectConstructor constructor = (ReflectConstructor) ((TreeIntObject) current)._object;
			Object[] args = nullArgumentsFor(constructor);
			Object res = constructor.newInstance(args);
			if (res != null) {
				return new ReflectConstructorSpec(constructor, args);
			}
		}
		return ReflectConstructorSpec.INVALID_CONSTRUCTOR;
	}

	private static Object[] nullArgumentsFor(ReflectConstructor constructor) {
	    ReflectClass[] paramTypes = constructor.getParameterTypes();
	    Object[] params = new Object[paramTypes.length];
	    for (int j = 0; j < params.length; j++) {
	    	params[j] = paramTypes[j].nullValue();
	    }
	    return params;
    }
	
	private static Tree sortConstructorsByParamsCount(final ReflectConstructor[] constructors) {
		Tree sortedConstructors = null;

		// sort constructors by parameter count
		for (int i = 0; i < constructors.length; i++) {
			int parameterCount = constructors[i].getParameterTypes().length;
			sortedConstructors = Tree.add(sortedConstructors,
					new TreeIntObject(i + constructors.length * parameterCount,
							constructors[i]));
		}
		return sortedConstructors;
	}
	
    public static ReflectConstructor skipConstructor(ConstructorAwareReflectClass claxx, boolean skipConstructor, boolean testConstructor) {
		if (!skipConstructor) {
			return null;
		}
		ReflectConstructor serializableConstructor = claxx.getSerializableConstructor();
		if (serializableConstructor == null) {
			return null;
		}
		if(!testConstructor || Deploy.csharp) {
			return serializableConstructor;
		}
		Object obj = serializableConstructor.newInstance((Object[]) null);
		if (obj != null) {
			return serializableConstructor;
		}
		return null;
	}


}
