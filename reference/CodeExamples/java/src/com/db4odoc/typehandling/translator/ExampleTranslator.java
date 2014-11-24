package com.db4odoc.typehandling.translator;

import com.db4o.ObjectContainer;
import com.db4o.config.ObjectConstructor;

// #example: An example translator
class ExampleTranslator implements ObjectConstructor {

    // This is called to store the object
    public Object onStore(ObjectContainer objectContainer, Object objToStore) {
        NonStorableType notStorable = (NonStorableType) objToStore;
        return notStorable.getData();
    }

    // This is called when the object is activated
    public void onActivate(ObjectContainer objectContainer, Object targetObject, Object storedObject) {
        NonStorableType notStorable = (NonStorableType) targetObject;
        notStorable.setData((String)storedObject);
    }

    // Tell db4o which type we use to store the data
    public Class storedClass() {
        return String.class;
    }

    // This method is called when a new instance is needed
    public Object onInstantiate(ObjectContainer objectContainer, Object storedObject) {
        return new NonStorableType("");
    }
}
// #end example
