package com.db4odoc.tutorial.utils;

import junit.framework.Assert;
import org.testng.annotations.BeforeMethod;
import org.testng.annotations.Test;

import java.beans.PropertyChangeEvent;
import java.beans.PropertyChangeListener;

/**
 * @author roman.stoffel@gamlor.info
 * @since 27.07.2010
 */
public class TestPropertyEventsSupport {
    private PropertyEventsSupport toTest;

    @BeforeMethod
    public void setup() {
        this.toTest = new PropertyEventsSupport(this);
    }

    @Test
    public void canAddListener() {
        toTest.addPropertyChangeListener(new PropertyChangeListener() {
            @Override
            public void propertyChange(PropertyChangeEvent evt) {

            }
        });
    }

    @Test
    public void listenerIsFired() {
        final WasInvoked wasInvoked = new WasInvoked();
        toTest.addPropertyChangeListener(new PropertyChangeListener() {
            @Override
            public void propertyChange(PropertyChangeEvent evt) {
                wasInvoked.invoke();
            }
        });
        toTest.firePropertyChange("testProperty", "newValue");
        wasInvoked.assertWasInvoked();
    }

    @Test
    public void canRemoveListener() {
        final PropertyChangeListener listener = new PropertyChangeListener() {
            @Override
            public void propertyChange(PropertyChangeEvent evt) {
                Assert.fail("No invocation expected");
            }
        };
        toTest.addPropertyChangeListener(listener);
        toTest.removePropertyChangeListener(listener);
        toTest.firePropertyChange("testProperty", "newValue");
    }


    @Test
    public void disposes() {
        final PropertyChangeListener listener = new PropertyChangeListener() {
            @Override
            public void propertyChange(PropertyChangeEvent evt) {
                Assert.fail("No invocation expected");
            }
        };
        toTest.addPropertyChangeListener(listener);
        toTest.dispose();
        toTest.firePropertyChange("testProperty", "newValue");
    }
}
