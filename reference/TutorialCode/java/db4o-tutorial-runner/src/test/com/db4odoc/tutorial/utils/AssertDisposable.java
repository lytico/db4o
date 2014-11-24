package com.db4odoc.tutorial.utils;

import org.testng.Assert;

/**
 * @author roman.stoffel@gamlor.info
 * @since 25.07.2010
 */
public final class AssertDisposable implements Disposable {
    private boolean isDisposed;

    public AssertDisposable() {
        this(false);
    }

    public AssertDisposable(boolean disposed) {
        isDisposed = disposed;
    }

    @Override
    public void dispose() {
        assertNotDisposed();
        isDisposed = true;
    }

    public void assertNotDisposed() {
        Assert.assertFalse(isDisposed);
    }

    public void assertDisposed() {
        Assert.assertTrue(isDisposed);
    }
}

