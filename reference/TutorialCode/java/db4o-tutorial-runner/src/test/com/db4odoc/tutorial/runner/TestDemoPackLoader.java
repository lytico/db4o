package com.db4odoc.tutorial.runner;


import static org.testng.Assert.assertNotNull;

/**
 * @author roman.stoffel@gamlor.info
 * @since 27.04.11
 */
public class TestDemoPackLoader {

    @org.testng.annotations.Test
    public void canLoadDemoPack(){
        final DemoPack demoPack = DemoPackLoader.loadByName(DemoPackForTests.class.getName());
        assertNotNull(demoPack.preparation());
        assertNotNull(demoPack.snippets());
    }
}
