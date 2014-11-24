package com.db4odoc.tutorial.utils;


import java.util.HashSet;
import java.util.Set;

/**
 * @author roman.stoffel@gamlor.info
 * @since 25.07.2010
 */
public final class Disposer implements Disposable {
    private Set<Disposable> disposables = new HashSet<Disposable>();

    public void add(Disposable disposable) {
        checkNotDisposed();
        if (null == disposable) {
            throw new IllegalArgumentException("Cannot add a null-reference as disposable");
        }
        synchronized (this) {
            disposables.add(disposable);
        }
    }

    public void checkNotDisposed() {
        synchronized (this) {
            if (null == disposables) {
                throw new ObjectDisposedException(this);
            }
        }
    }

    @Override
    public void dispose() {
        checkNotDisposed();
        Set<Disposable> disposeRun;
        synchronized (this) {
            disposeRun = new HashSet<Disposable>(disposables);
            disposables = null;
        }
        for (Disposable toDispose : disposeRun) {
            toDispose.dispose();
        }
    }

    /**
     * Disposes this item and remove it from the dispose-list
     *
     * @param disposeAndRemove
     */
    public void dispose(Disposable disposeAndRemove) {
        synchronized (this) {
            disposables.remove(disposeAndRemove);
        }
        disposeAndRemove.dispose();
    }

    /**
     * Checks if the object is a {@link Disposable} and only adds it then.
     *
     * @param obj
     */
    public void tryAdd(Object obj) {
        if (obj instanceof Disposable) {
            synchronized (this) {
                disposables.add((Disposable) obj);
            }
        }
    }

    /**
     * Checks if the object is a {@link Disposable} and only disposes it then.
     * Further if removes it from the dispose-list
     *
     * @param obj
     */
    public void tryDispose(Object obj) {
        if (obj instanceof Disposable) {
            dispose((Disposable) obj);
        }
    }
}

