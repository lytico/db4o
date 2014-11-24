package com.db4o.internal;

import com.db4o.foundation.*;
import com.db4o.reflect.*;

public class ReflectorConfigurationImpl implements ReflectorConfiguration {

	private Config4Impl _config;
	
	public ReflectorConfigurationImpl(Config4Impl config) {
		_config = config;
	}
	
	public boolean testConstructors() {
		return _config.testConstructors();
	}
	
	public boolean callConstructor(ReflectClass clazz) {
        TernaryBool specialized = callConstructorSpecialized(clazz);
		if(!specialized.isUnspecified()){
		    return specialized.definiteYes();
		}
		return _config.callConstructors().definiteYes();
    }
    
    private final TernaryBool callConstructorSpecialized(ReflectClass clazz){
    	Config4Class clazzConfig = _config.configClass(clazz.getName());
        if(clazzConfig!= null){
            TernaryBool res = clazzConfig.callConstructor();
            if(!res.isUnspecified()){
                return res;
            }
        }
        if(Platform4.isEnum(_config.reflector(), clazz)){
            return TernaryBool.NO;
        }
        ReflectClass ancestor = clazz.getSuperclass();
		if(ancestor != null){
            return callConstructorSpecialized(ancestor);
        }
        return TernaryBool.UNSPECIFIED;
    }

}
