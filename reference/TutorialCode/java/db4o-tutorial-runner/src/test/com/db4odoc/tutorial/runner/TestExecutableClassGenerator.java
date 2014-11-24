package com.db4odoc.tutorial.runner;


import com.db4o.ObjectContainer;
import com.db4odoc.tutorial.faulttolerance.OperationResult;
import com.db4odoc.tutorial.utils.OneArgAction;
import org.testng.annotations.BeforeMethod;
import org.testng.annotations.Test;

import java.util.Arrays;

import static org.mockito.Mockito.mock;
import static org.mockito.Mockito.verify;
import static org.testng.Assert.*;

public class TestExecutableClassGenerator {


    private ExecutableClassGenerator toTest;

    @BeforeMethod
    public void setup(){
        this.toTest = ExecutableClassGenerator.create();
    }
    @Test
    public void returnsCallable() throws Exception{
        OperationResult<OneArgAction<ObjectContainer>> callable = toTest.generateWithBody("");
        assertNotNull(callable);
        assertNotNull(callable.getResultData());
    }

    @Test
    public void canCall() throws Exception{
        OperationResult<OneArgAction<ObjectContainer>> callable = toTest.generateWithBody("container.close();");
        ObjectContainer container = mock(ObjectContainer.class);
        callable.getResultData().invoke(container);
        verify(container).close();
    }

    @Test
    public void canGenerateMultipleInstances() throws Exception{
        OperationResult<OneArgAction<ObjectContainer>> callable1 = toTest.generateWithBody("container.close();");
        OperationResult<OneArgAction<ObjectContainer>> callable2 = toTest.generateWithBody("container.close();");
        assertNotNull(callable1);
        assertNotNull(callable2);
    }

    @Test
    public void canAccessList() throws Exception{
        OneArgAction<ObjectContainer> callable =
                toTest.generateWithBody("java.util.List<String> list = null;").getResultData();
        assertNotNull(callable);
    }
    @Test
    public void canDb4oClasses() throws Exception{
        OneArgAction<ObjectContainer> callable =
                toTest.generateWithBody("ObjectContainer cont = null;" +
                        "Predicate<Object> pred = null;").getResultData();
        assertNotNull(callable);
    }

    @Test
    public void canHandleImports() throws Exception{
        OneArgAction<ObjectContainer> callable =
                toTest.generateWithBody("Class list = Car.class;",
                        Arrays.asList("com.db4odoc.tutorial.demos.firststeps.*")).getResultData();
        assertNotNull(callable);
    }


    @Test
    public void failGracefully() throws Exception{
        OperationResult<OneArgAction<ObjectContainer>> result = toTest.generateWithBody("container.clos");
        assertFalse(result.wasSuccessful());
        assertTrue(result.getException().getMessage().contains("error"));
    }

}
