package com.db4odoc.performance;


import com.db4o.config.annotations.Indexed;

public class ItemHolder {
    @Indexed
    private final Item indexedReference;

    public ItemHolder(Item item) {
        this.indexedReference = item;
    }

    public static ItemHolder create(Item reference){
        return new ItemHolder(reference);
    }

    public Item getIndexedReference() {
        return indexedReference;
    }
}
