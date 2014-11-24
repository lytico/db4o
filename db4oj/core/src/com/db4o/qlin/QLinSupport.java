/* Copyright (C) 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.qlin;

import com.db4o.foundation.*;
import com.db4o.reflect.*;
import com.db4o.reflect.core.*;


/**
 * static import support class for {@link QLin} queries.
 * @since 8.0
 */
public class QLinSupport {
	
	/**
	 * returns a prototype object for a specific class
	 * to be passed to the where expression of a QLin 
	 * query. 
	 * @see QLin#where(Object)
	 */
	public static <T> T prototype(Class<T> clazz){
		try{
			return _prototypes.prototypeForClass(clazz);
		} catch(PrototypesException ex){
			throw new QLinException(ex);
		}
	}

	
	/**
	 * sets the context for the next query on this thread.
	 * This method should never have to be called manually.
	 * The framework should set the context up. 
	 */
	public static void context(ReflectClass claxx){
		_context.value(claxx);
	}
	
	/**
	 * sets the context for the next query on this thread.
	 * This method should never have to be called manually.
	 * The framework should set the context up. 
	 */
	public static void context(Class clazz){
		_context.value(ReflectorUtils.reflectClassFor(_prototypes.reflector(), clazz));
	}
	
	/**
	 * shortcut for the {@link #prototype(Class)} method.
	 */
	public static <T> T p(Class<T> clazz){
		return prototype(clazz);
	}
	
	/**
	 * parameter for {@link QLin#orderBy(Object, QLinOrderByDirection)}
	 */
	public static QLinOrderByDirection ascending(){
		return QLinOrderByDirection.ASCENDING;
	}
	
	/**
	 * parameter for {@link QLin#orderBy(Object, QLinOrderByDirection)}
	 */
	public static QLinOrderByDirection descending(){
		return QLinOrderByDirection.DESCENDING;
	}
	
	/**
	 * public for implementors, do not use directly 
	 */
	public static Iterator4<String> backingFieldPath(Object expression){
		checkForNull(expression);
		if(expression instanceof ReflectField){
			return Iterators.iterate( ((ReflectField)expression).getName());
		}
		Iterator4 path = _prototypes.backingFieldPath(_context.value(), expression);
		if(path != null){
			return path;
		}
		return Iterators.iterate(fieldByFieldName(expression).getName());
	}

	
	/**
	 * converts an expression to a single field. 
	 */
	public static ReflectField field(Object expression){
		checkForNull(expression);
		if(expression instanceof ReflectField){
			return (ReflectField)expression;
		}
		Iterator4 path = _prototypes.backingFieldPath(_context.value(), expression);
		if(path != null){
			if(path.moveNext()){
				expression = path.current();
			}
			if(path.moveNext()){
				path.reset();
				throw new QLinException("expression can not be converted to a single field. It evaluates to: " + 
						Iterators.join(path, "[", "]", ", "));
			}
		}
		return fieldByFieldName(expression);
	}

	private static ReflectField fieldByFieldName(Object expression) {
		if(expression instanceof String){
			ReflectField field = ReflectorUtils.field(_context.value(), (String)expression);
			if(field != null){
				return field;
			}
		}
		throw new QLinException("expression can not be mapped to a field");
	}
	
	private static void checkForNull(Object expression) {
		if(expression == null){
			throw new QLinException("expression can not be null");
		}
	}
	
	private static final boolean IGNORE_TRANSIENT_FIELDS = true;
	
	private static final int RECURSION_DEPTH = 4;
	
	private static final Prototypes _prototypes = 
		new Prototypes(Prototypes.defaultReflector(),RECURSION_DEPTH, IGNORE_TRANSIENT_FIELDS);
	
	private static final DynamicVariable<ReflectClass> _context = DynamicVariable.newInstance();
	
}
