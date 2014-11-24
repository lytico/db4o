package com.db4odoc.indexing.traverse;


import com.db4o.config.annotations.Indexed;

class Item {
    @Indexed
    private int data;

    public Item(int data) {
        this.data = data;
    }
}
