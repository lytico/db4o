package com.db4odoc.performance;


import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;


public class CollectionHolder {
    private List<Item> items;

    CollectionHolder(List<Item> items) {
        this.items = items;
    }

    public static CollectionHolder create(Item...itemsToAdd){
        return new CollectionHolder(new ArrayList<Item>(Arrays.asList(itemsToAdd)));
    }

    public List<Item> getItems() {
        return items;
    }
}
