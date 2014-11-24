package com.db4odoc.tutorial.runner;


import com.db4o.ObjectContainer;

import java.util.Collections;

public interface RunPreparation {

    public static RunPreparation NO_PREPARATION = new RunPreparation() {
        @Override
        public Iterable<String> packages() {
            return Collections.emptyList();
        }

        @Override
        public void prepareDB(ObjectContainer container) {
        }
    };
    Iterable<String> packages();

    void prepareDB(ObjectContainer container);


}
