package com.db4odoc.tutorial.utils;

/**
 * @author roman.stoffel@gamlor.info
 * @since 25.07.2010
 */
public interface Disposable {
    /**
     * Full cleanup of the component. Afterwards the component is ready to get cleaned by garbage collection.
     * This means no references of this component are hold anymore in the application.
     */
    public void dispose();
}
