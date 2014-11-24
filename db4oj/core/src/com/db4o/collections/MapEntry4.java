/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.collections;

import java.util.*;


/**
 * @exclude
 * 
 * @sharpen.ignore
 */
@decaf.Ignore(unlessCompatible=decaf.Platform.JDK15)
public class MapEntry4<K, V> implements Map.Entry<K, V> {

    private K _key;

    private V _value;

    public MapEntry4(K key, V value) {
        _key = key;
        _value = value;
    }

    public K getKey() {
        return _key;
    }

    public V getValue() {
        return _value;
    }

    public V setValue(V value) {
        V oldValue = value;
        this._value = value;
        return oldValue;
    }

    @SuppressWarnings("unchecked")
    public boolean equals(Object o) {
        if (this == o) {
            return true;
        }
        if (!(o instanceof Map.Entry)) {
            return false;
        }

        MapEntry4<K, V> other = (MapEntry4<K, V>) o;

        return (_key == null ? other.getKey() == null : _key.equals(other
                .getKey())
                && _value == null ? other.getValue() == null : _value
                .equals(other.getValue()));

    }

    public int hashCode() {
        return (_key == null ? 0 : _key.hashCode())
                ^ (_value == null ? 0 : _value.hashCode());
    }
}
