/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.internal.query;

import java.util.*;

import com.db4o.ext.*;
import com.db4o.foundation.*;
import com.db4o.internal.*;
import com.db4o.internal.handlers.*;
import com.db4o.internal.marshall.*;
import com.db4o.reflect.*;
import com.db4o.ta.*;
import com.db4o.typehandlers.*;

public class SodaQueryComparator implements Comparator<Integer>, IntComparator {
	
	public static class Ordering {
		
		@decaf.Public
		private Direction _direction;
		
		@decaf.Public
		private String[] _fieldPath;
		
		@decaf.Public
		transient List<FieldMetadata> _resolvedPath;
		
		public Ordering(Direction direction, String... fieldPath) {
			_direction = direction;
			_fieldPath = fieldPath;
		}

		public Direction direction() {
			return _direction;
		}
		
		public String[] fieldPath() {
			return _fieldPath;
		}
	}
	
	public static class Direction {
		public static final Direction ASCENDING = new Direction(0);
		public static final Direction DESCENDING = new Direction(1);
		
		@decaf.Public
		private int value;
		
		@decaf.Public
		private Direction() {
		}
		
		private Direction(int value) {
			this.value = value;
		}
		
		@Override
		public boolean equals(Object obj) {
			return ((Direction)obj).value == value;
		}

		@Override
		public String toString() {
			return this.equals(ASCENDING) ? "ASCENDING" : "DESCENDING";
		}
	}

	private final LocalObjectContainer _container;
	private final LocalTransaction _transaction;
	private final ClassMetadata _extentType;
	private final Ordering[] _orderings;
	private final Map<Integer, ByteArrayBuffer> _bufferCache = new HashMap<Integer, ByteArrayBuffer>();
	private final Map<FieldValueKey, Object> _fieldValueCache = new HashMap<FieldValueKey, Object>();

	public SodaQueryComparator(
			LocalObjectContainer container,
			Class extentType,
			Ordering... orderings) {
		
		this(container, container.produceClassMetadata(container.reflector().forClass(extentType)), orderings);
	}

	public SodaQueryComparator(
			LocalObjectContainer container,
			final ClassMetadata extent,
			Ordering... orderings) {
		_container = container;
		_transaction = ((LocalTransaction) _container.transaction());
		_extentType = extent;
		_orderings = orderings;
		resolveFieldPaths(orderings);
	}

	private void resolveFieldPaths(Ordering[] orderings) {
		for (Ordering fieldPath : orderings) {
			fieldPath._resolvedPath = resolveFieldPath(fieldPath.fieldPath());
		}
	}

	public List<Integer> sort(long[] ids) {
		ArrayList<Integer> idList = listFrom(ids);
		Collections.sort(idList, this);
		return idList;
	}

	private ArrayList<Integer> listFrom(long[] ids) {
		ArrayList<Integer> idList = new ArrayList<Integer>(ids.length);
		for (long id : ids) {
			idList.add((int)id);
		}
		return idList;
	}

	private List<FieldMetadata> resolveFieldPath(String[] fieldPath) {
		List<FieldMetadata> fields = new ArrayList<FieldMetadata>(fieldPath.length);
		ClassMetadata currentType = _extentType;
		for (String fieldName : fieldPath) {
			FieldMetadata field = currentType.fieldMetadataForName(fieldName);
			if(field == null){
				fields.clear();
				break;
			}
			currentType = field.fieldType();
			fields.add(field);
		}
		return fields;
	}

	public int compare(Integer x, Integer y) {
		return compare(x.intValue(), y.intValue());
	}
	
	public int compare(int x, int y) {
		for (Ordering ordering : _orderings) {
			List<FieldMetadata> resolvedPath = ordering._resolvedPath;
			if(resolvedPath.size() == 0){
				continue;
			}
			int result = compareByField(x, y, resolvedPath);
			if (result != 0) {
				return ordering.direction().equals(Direction.ASCENDING)
					? result
					: -result;
			}
		}
		return 0;
	}

	private int compareByField(int x, int y, List<FieldMetadata> path) {
		
		final Object xFieldValue = getFieldValue(x, path);
		final Object yFieldValue = getFieldValue(y, path);
		
		ensureNoManualActivationRequired(xFieldValue);
		
		final FieldMetadata field = path.get(path.size() - 1);
		return field.prepareComparison(_transaction.context(), xFieldValue).compareTo(yFieldValue);
	}

	private void ensureNoManualActivationRequired(final Object obj) {
		if (obj == null) return;
		
		if (!hasValueTypeBehavior(obj)) {
			if (!Activatable.class.isAssignableFrom(obj.getClass())) {
				throwUnsupportedOrderingException(obj.getClass(), "make it implement Activatable interface.");
			}
			
			if (!TransparentActivationSupport.isTransparentActivationEnabledOn(_container)) {
				throwUnsupportedOrderingException(obj.getClass(), "enable transparent activation support by adding TransparentActivationSupport to the configutation before opening the db.");
			}			
		}
	}

	private boolean hasValueTypeBehavior(final Object obj) {
		boolean isSimple = Platform4.isSimple(obj.getClass());
		if (isSimple) return true;
		
		ReflectClass reflectClass = _container.reflector().forObject(obj);
		if (Platform4.isStruct(reflectClass)) return true;
			
		boolean isEnum = Platform4.isEnum(_container.reflector(), reflectClass);
		if (isEnum) return true;
		
		TypeHandler4 typeHandler = _container.typeHandlerForClass(reflectClass);		
		return Handlers4.isValueType(typeHandler);
	}

	private void throwUnsupportedOrderingException(final Class clazz, String msg) {
		throw new UnsupportedOrderingException("Cannot sort on class '" + clazz.getName() + "'. If you do want to use it as a sort criteria " + msg);
	}

	private Object getFieldValue(int id, List<FieldMetadata> path) {
		for (int i = 0; i < path.size() - 1; ++i) {
			final Object obj = getFieldValue(id, path.get(i));
			if (null == obj) {
				return null;
			}
			id = _container.getID(_transaction, obj);
		}
		return getFieldValue(id, path.get(path.size() - 1));
	}

	static class FieldValueKey {
		private int _id;
		private FieldMetadata _field;

		public FieldValueKey(int id, FieldMetadata field) {
			_id = id;
			_field = field;
		}

		@Override
		public int hashCode() {
			return _field.hashCode() ^ _id;
		}

		@Override
		public boolean equals(Object obj) {
			FieldValueKey other = (FieldValueKey) obj;
			return _field == other._field && _id == other._id;
		}
	}

	private Object getFieldValue(int id, FieldMetadata field) {
		final FieldValueKey key = new FieldValueKey(id, field);

		Object cachedValue = _fieldValueCache.get(key);
		if (null != cachedValue)
			return cachedValue;

		Object fieldValue = readFieldValue(id, field);
		_fieldValueCache.put(key, fieldValue);
		return fieldValue;
	}

	private Object readFieldValue(int id, FieldMetadata field) {
		ByteArrayBuffer buffer = bufferFor(id);
		HandlerVersion handlerVersion = field.containingClass().seekToField(_transaction, buffer, field);
		if (handlerVersion == HandlerVersion.INVALID) {
			return null;
		}
		
		QueryingReadContext context = new QueryingReadContext(_transaction, handlerVersion._number, buffer, id);
		return field.read(context);
	}

	private ByteArrayBuffer bufferFor(int id) {
		ByteArrayBuffer cachedBuffer = _bufferCache.get(id);
		if (null != cachedBuffer)
			return cachedBuffer;
		
		ByteArrayBuffer buffer = _container.readBufferById(_transaction, id);
		_bufferCache.put(id, buffer);
		return buffer;
	}
}