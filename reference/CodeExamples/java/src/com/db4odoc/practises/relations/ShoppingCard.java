package com.db4odoc.practises.relations;

import java.util.HashSet;
import java.util.Set;


class ShoppingCard {

    // #example: Simple 1-n relation. Navigating from the card to the items
    Set<Item> items = new HashSet<Item>();

    public void add(Item terrain) {
        items.add(terrain);
    }

    public void remove(Item o) {
        items.remove(o);
    }
    // #end example
}
