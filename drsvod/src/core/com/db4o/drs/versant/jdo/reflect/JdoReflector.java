package com.db4o.drs.versant.jdo.reflect;

import javax.jdo.spi.*;

import com.db4o.reflect.jdk.*;


public class JdoReflector extends JdkReflector {

	public JdoReflector(ClassLoader classLoader) {
		super(classLoader);
	}

	public JdoReflector(JdkLoader classLoader) {
		super(classLoader);
	}
	
	@Override
	protected JdkClass createClass(Class clazz) {
		
		if (!PersistenceCapable.class.isAssignableFrom(clazz)) {
			return super.createClass(clazz);
		}
		
		return new JdoClass(parent(), this, (Class<PersistenceCapable>) clazz);
		
	}

}
