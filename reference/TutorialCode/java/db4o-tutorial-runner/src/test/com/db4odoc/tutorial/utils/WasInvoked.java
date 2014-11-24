package com.db4odoc.tutorial.utils;


import java.beans.PropertyChangeEvent;
import java.beans.PropertyChangeListener;

import static org.testng.Assert.assertTrue;
import static org.testng.AssertJUnit.assertEquals;

/**
 * @author roman.stoffel@gamlor.info
 * @since 26.07.2010
 */
public final class WasInvoked<T> implements OneArgAction<T>, NoArgAction, PropertyChangeListener {
    private int invokeCount = 0;
    private final Object[] arguments;

    public WasInvoked(Object... arguments) {
        this.arguments = arguments;
    }

    @Override
    public void invoke() {
        invokeCount += 1;
    }

    @Override
    public void invoke(Object arg) {
        invoke();
        if (arguments.length != 0) {
            assertEquals(arg, arguments[0]);
        }
    }

    public void assertWasInvoked() {
        assertTrue(invokeCount > 0);
    }

    @Override
    public void propertyChange(PropertyChangeEvent evt) {
        invoke();
    }

    public void assertWasInvoked(int amount) {
        assertEquals(invokeCount, amount);
    }
}
