package com.db4o.drs.versant.jdo.reflect;

import java.lang.reflect.*;
import java.util.*;
import java.util.concurrent.*;

import javax.jdo.spi.*;

import com.db4o.internal.*;

public class JdoMirror {

	public interface FieldVisitor {
		void visit(String name, Class<?> type);
	}

	private static ConcurrentMap<Class<PersistenceCapable>, JdoMirror> mirrors = new ConcurrentHashMap<Class<PersistenceCapable>, JdoMirror>();
	private static Set<String> jdoInjectedFields = new HashSet<String>();
	
	static {
		for (Field f : Image.class.getDeclaredFields()) {
			jdoInjectedFields.add(f.getName());
		}
	}

	public static JdoMirror mirrorFor(Class<PersistenceCapable> clazz) {
		JdoMirror m = mirrors.get(clazz);
		if (m != null) {
			return m;
		}

		m = new JdoMirror(clazz);
		JdoMirror older = mirrors.putIfAbsent(clazz, m);

		return older == null ? m : older;
	}

	public static class Image {
		public String jdoFieldNames[];
		public Class jdoFieldTypes[];
		
		/*
		 * we are using transient here to mark fields we want to know are
		 * injected by JDO, but are not required by our JDO reflection
		 * functionality.
		 */
		public transient byte jdoFieldFlags[];
		public transient int jdoInheritedFieldCount;
		public transient Class jdoPersistenceCapableSuperclass;
		public transient Object jdoDetachedState[];
		public transient byte jdoFlags;
		public transient StateManager jdoStateManager;
		public transient int jdoVersantLoaded0;
		public transient int jdoVersantDirty0;
		public transient Object jdoVersantVersion;
		public transient Object jdoVersantOID;
	}

	private final Class<PersistenceCapable> clazz;
	private Image image;
	private Map<String, Integer> fieldNameToIndex;

	public JdoMirror(Class<PersistenceCapable> clazz) {
		this.clazz = clazz;
	}

	public void accept(FieldVisitor fieldVisitor) {

		for (int i = 0; i < image().jdoFieldNames.length; i++) {
			fieldVisitor.visit(image().jdoFieldNames[i], image().jdoFieldTypes[i]);
		}

	}

	private void resolveFields() {

		image = new Image();
		Field[] fs = Image.class.getDeclaredFields();
		for (Field f : fs) {
			try {
				if (Modifier.isTransient(f.getModifiers())) {
					continue;
				}
				Field src = clazz.getDeclaredField(f.getName());
				src.setAccessible(true);
				f.set(image(), src.get(null));
			} catch (NoSuchFieldException e) {
				if (!Modifier.isTransient(f.getModifiers())) {
					throw new ReflectException(e);
				}
			} catch (Exception e) {
				throw new ReflectException(e);
			}
		}
		
		fieldNameToIndex = new HashMap<String, Integer>();
		
		for(int i = 0;i<image().jdoFieldNames.length;i++) {
			fieldNameToIndex.put(image().jdoFieldNames[i], i);
		}
	}

	public Object getFieldValue(Object object, String name) throws SecurityException, NoSuchMethodException, IllegalArgumentException, IllegalAccessException, InvocationTargetException {
		
		Method m = clazz.getDeclaredMethod("jdoGet"+name, clazz);
		m.setAccessible(true);
		return m.invoke(null, object);
		
		
	}
	
	public void setFieldValue(Object object, String name, Object value) throws SecurityException, NoSuchMethodException, IllegalArgumentException, IllegalAccessException, InvocationTargetException {
		
		Method m = clazz.getDeclaredMethod("jdoSet"+name, clazz, image().jdoFieldTypes[fieldNameToIndex().get(name)]);
		m.setAccessible(true);
		m.invoke(null, object, value);
	}

	public boolean isJdoField(String name) {
		return fieldNameToIndex().containsKey(name);
	}

	private Image image() {
		if (image == null) {
			resolveFields();
		}
		return image;
	}

	private Map<String, Integer> fieldNameToIndex() {
		if (fieldNameToIndex == null) {
			resolveFields();
		}
		return fieldNameToIndex;
	}

	public static boolean isJdoInjectedField(String name) {
		return jdoInjectedFields.contains(name);
	}

}
