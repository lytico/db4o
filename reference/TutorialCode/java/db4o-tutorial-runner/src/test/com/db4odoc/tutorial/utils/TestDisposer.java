package com.db4odoc.tutorial.utils;

import org.testng.Assert;
import org.testng.annotations.Test;

/**
 * @author roman.stoffel@gamlor.info
 * @since 25.07.2010
 */
public class TestDisposer {

    @Test
    public void expectNoDisposeOnAdd() {
        AssertDisposable dsp1 = new AssertDisposable();
        final Disposer toTest = new Disposer();
        toTest.add(dsp1);
        dsp1.assertNotDisposed();
    }

    @Test
    public void canEmptyDispose() {
        new Disposer().dispose();
    }

    @Test(expectedExceptions = ObjectDisposedException.class)
    public void throwOnDisposed() {
        final Disposer toTest = new Disposer();
        toTest.dispose();
        toTest.checkNotDisposed();
    }

    @Test(expectedExceptions = ObjectDisposedException.class)
    public void throwOnAdd() {
        final Disposer toTest = new Disposer();
        toTest.dispose();
        toTest.add(new AssertDisposable());
    }

    @Test
    public void canDisposeDirectly() {
        AssertDisposable disp1 = new AssertDisposable();
        AssertDisposable disp2 = new AssertDisposable();
        final Disposer toTest = new Disposer();
        toTest.add(disp1);
        toTest.add(disp2);
        toTest.dispose(disp1);
        disp1.assertDisposed();
        disp2.assertNotDisposed();
        toTest.dispose();
        disp2.assertDisposed();
    }

    @Test
    public void canCastAdd() {
        AssertDisposable disp1 = new AssertDisposable();
        Object asObject = disp1;
        final Disposer toTest = new Disposer();
        toTest.tryAdd(asObject);
        toTest.tryAdd(new Object() {
            public void dispose() {
                Assert.fail("Do not use reflection to call non-disposables");
            }
        });
        toTest.dispose();
        disp1.assertDisposed();
    }

    @Test
    public void canCastDispose() {
        AssertDisposable disp1 = new AssertDisposable();
        Object asOjb = disp1;
        final Disposer toTest = new Disposer();
        toTest.add(disp1);
        toTest.tryDispose((asOjb));
        disp1.assertDisposed();
    }

    @Test(expectedExceptions = ObjectDisposedException.class)
    public void expectExceptionOnSecondDispose() {
        final Disposer toTest = new Disposer();
        toTest.dispose();
        toTest.dispose();

    }


    @Test
    public void expectDisposes() {
        AssertDisposable dsp1 = new AssertDisposable();
        AssertDisposable dsp2 = new AssertDisposable();
        AssertDisposable dsp3 = new AssertDisposable();
        final Disposer toTest = new Disposer();
        toTest.add(dsp1);
        toTest.add(dsp2);
        toTest.add(dsp3);
        toTest.dispose();
        dsp1.assertDisposed();
        dsp2.assertDisposed();
        dsp3.assertDisposed();
    }

}
