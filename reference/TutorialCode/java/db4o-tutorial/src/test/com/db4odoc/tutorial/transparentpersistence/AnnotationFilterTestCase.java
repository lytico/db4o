package com.db4odoc.tutorial.transparentpersistence;


import junit.framework.Assert;
import org.testng.annotations.DataProvider;
import org.testng.annotations.Test;

public class AnnotationFilterTestCase {

    private final AnnotationFilter toTest = new AnnotationFilter();

    @Test(dataProvider = "annotatedClasses")
    public void acceptsClass(Class toAccept){
        Assert.assertTrue(toTest.accept(toAccept));
    }
    @Test(dataProvider = "notAnnotatedClasses")
    public void dontAccept(Class toAccept){
        Assert.assertFalse(toTest.accept(toAccept));
    }

    @DataProvider
    public Object[][] annotatedClasses(){
        return new Object[][]{
                new Object[]{ClassWithAnnotation.class},
                new Object[]{InheritedAnnotation.class}
        };
    }
    @DataProvider
    public Object[][] notAnnotatedClasses(){
        return new Object[][]{
                new Object[]{ClassWithoutAnnotation.class}
        };
    }
}
