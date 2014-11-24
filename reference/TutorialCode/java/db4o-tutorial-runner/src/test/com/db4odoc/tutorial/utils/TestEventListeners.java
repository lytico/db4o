package com.db4odoc.tutorial.utils;

import org.testng.Assert;
import org.testng.annotations.BeforeMethod;
import org.testng.annotations.Test;

import static org.testng.Assert.assertNotNull;


/**
 * @author roman.stoffel@gamlor.info
 * @since 26.07.2010
 */
public class TestEventListeners {
    private EventListeners<OneArgAction<String>> toTest;

    @BeforeMethod
    public void setup() {
        this.toTest = EventListeners.create(OneArgAction.class);
    }


    @Test
    public void canAddEventListener() {
        final Disposable disposable = toTest.add(new OneArgAction<String>() {
            @Override
            public void invoke(String arg) {

            }
        });
        assertNotNull(disposable);
    }

    @Test
    public void invokesHandler() {
        WasInvoked wasInvoked = new WasInvoked("argument");
        toTest.add(wasInvoked);
        toTest.invoker().invoke("argument");
        wasInvoked.assertWasInvoked();
    }

    @Test
    public void canDisposeHandler() {
        OneArgAction wasInvoked = new OneArgAction() {
            @Override
            public void invoke(Object arg) {
                Assert.fail("No invocation expected");
            }
        };
        toTest.add(wasInvoked).dispose();
        toTest.invoker().invoke("argument");
    }

    @Test
    public void disposesAll() {
        OneArgAction wasInvoked = new OneArgAction() {
            @Override
            public void invoke(Object arg) {
                Assert.fail("No invocation expected");
            }
        };
        toTest.dispose();
        toTest.add(wasInvoked);
    }
}
