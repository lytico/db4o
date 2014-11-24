package com.db4odoc.tutorial.demos;

import com.db4o.ObjectContainer;
import com.db4odoc.tutorial.runner.DemoPack;
import com.db4odoc.tutorial.runner.RunPreparation;

import static java.util.Arrays.asList;

/**
 * @author roman.stoffel@gamlor.info
 * @since 27.04.11
 */
public abstract class AbstractDemoPack  implements DemoPack {


    protected void prepareDB(ObjectContainer container) {
    }

    @Override
    public final RunPreparation preparation() {
        return new RunPreparation() {
            @Override
            public Iterable<String> packages() {
                return asList(AbstractDemoPack.this.getClass().getPackage().getName()+".*");
            }

            @Override
            public void prepareDB(ObjectContainer container) {
                AbstractDemoPack.this.prepareDB(container);
            }
        };
    }
}
