package com.db4odoc.pitfalls.typehandling;

class CollectionHolder {
    private MyFixedSizeCollection<String> collection;

    CollectionHolder(String...collection) {
        this.collection = new MyFixedSizeCollection<String>(collection);
    }

    public MyFixedSizeCollection<String> getNames() {
        return collection;
    }
}
