package com.db4odoc.pitfalls.typehandling;

import java.util.AbstractCollection;
import java.util.Arrays;
import java.util.Iterator;


// #example: This collection doesn't implement all collection methods
class MyFixedSizeCollection<E> extends AbstractCollection<E>{
    private E[] items;

    public MyFixedSizeCollection(E[] items) {
        this.items = items;
    }

    @Override
    public Iterator<E> iterator() {
        return Arrays.asList(items).iterator();
    }

    @Override
    public int size() {
        return items.length;
    }
}
// #end example
