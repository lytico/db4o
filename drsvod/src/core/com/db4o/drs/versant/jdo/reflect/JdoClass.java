package com.db4o.drs.versant.jdo.reflect;

import java.lang.reflect.*;
import java.util.*;

import javax.jdo.spi.*;

import com.db4o.reflect.*;
import com.db4o.reflect.jdk.*;

public class JdoClass extends JdkClass {

	private JdoMirror mirror;

	public JdoClass(Reflector reflector, JdoReflector jdoReflector, Class<PersistenceCapable> clazz) {
		super(reflector, jdoReflector, clazz);
	}

	JdoMirror mirror() {
		if (mirror != null) {
			return mirror;
		}
		return mirror = JdoMirror.mirrorFor((Class<PersistenceCapable>) getJavaClass());
	}

	@Override
	protected JdkField createField(Field field) {
		return mirror().isJdoField(field.getName()) ? new JdoField(_reflector, this, field) : super.createField(field);
	}

	@Override
	public ReflectField[] getDeclaredFields() {
		ReflectField[] original = super.getDeclaredFields();
		List<ReflectField> filtered = new ArrayList<ReflectField>();
		for (ReflectField rf : original) {
			if (!JdoMirror.isJdoInjectedField(rf.getName())) {
				filtered.add(rf);
			}
		}

		return filtered.toArray(new ReflectField[filtered.size()]);
	}

	@Override
	public ReflectField getDeclaredField(String name) {
		return JdoMirror.isJdoInjectedField(name) ? null : super.getDeclaredField(name);
	}
}
