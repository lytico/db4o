package com.db4odoc.performance;


import com.db4o.config.annotations.Indexed;

public class GenericItemHolder<T> {
    @Indexed
    private final T indexedReference;

    public GenericItemHolder(T item) {
        this.indexedReference = item;
    }

    public static <T> GenericItemHolder<T> create(T reference){
        return new GenericItemHolder<T>(reference);
    }

    public T getIndexedReference() {
        return indexedReference;
    }
}
