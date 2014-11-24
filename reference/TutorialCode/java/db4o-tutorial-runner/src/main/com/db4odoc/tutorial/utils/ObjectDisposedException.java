package com.db4odoc.tutorial.utils;

/**
 * Is thrown when on {@link Disposable#dispose()} was called
 * and another call is made on the object.
 *
 * @author roman.stoffel@gamlor.info
 * @since 25.07.2010
 */
public class ObjectDisposedException extends IllegalStateException {
    public ObjectDisposedException(Object obj) {
        super("This object is all ready disposed. Object=" + obj);
    }
}

