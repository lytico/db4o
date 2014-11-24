package com.db4o.foundation;

import java.lang.reflect.*;

/**
 * @sharpen.ignore
 */
public class Enum4 implements Comparable {
	private final String _name;
	private final int _ordinal;
	
	protected Enum4(String name, int ordinal) {
		_ordinal = ordinal;
		_name = name;
	}
	
	public final String toString() {
		return _name;
	}	

	public final int compareTo(Object rhs) {
		if (rhs.getClass() != getClass()) {
			throw new ClassCastException();
		}
		
		Enum4 other = (Enum4) rhs;
		return _ordinal - other._ordinal;
	}
	
	public final String name() {
		return _name;
	}
	
	public final int ordinal() {
		return _ordinal;
	}
	
	public Enum4 valueOf(Class enumClass, String value) {
		Enum4[] values = null;
		Throwable t = null;
		
		try {
			values = values(enumClass);
		} catch (IllegalArgumentException e) {
			t = e;
		} catch (SecurityException e) {
			t = e;
		} catch (IllegalAccessException e) {
			t = e;
		} catch (InvocationTargetException e) {
			t = e;
		} catch (NoSuchMethodException e) {
			t = e;
		}
		
		if (t != null) {
			throw new IllegalArgumentException(enumClass + ": " + t.getMessage());
		}
		
		for(int i = 0; i < values.length; i++) {
			if (values[i].name().equals(value)) return values[i];			
		}
		
		throw new IllegalArgumentException("No enum const class: " + enumClass.getName() + "." + value); 
	}
	
	private Enum4[] values(Class enumClass) throws IllegalArgumentException, IllegalAccessException, InvocationTargetException, SecurityException, NoSuchMethodException {
		if (!Enum4.class.isAssignableFrom(enumClass)) {
			throw new ClassCastException(enumClass.getName());
		}
		
		final Method valuesMethod = enumClass.getMethod("values", new Class[0]);
		return (Enum4[]) valuesMethod.invoke(null, new Object[0]);
	}
}