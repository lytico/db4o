package com.db4odoc.tutorial.runner;


import com.db4odoc.tutorial.utils.WasInvoked;
import org.testng.annotations.BeforeMethod;
import org.testng.annotations.Test;

import java.io.PrintStream;

import static org.testng.Assert.assertEquals;
import static org.testng.Assert.assertNotNull;

public class TestTextViewModel {

    private static final String TEXT_TO_WRITE = "Hi there";
    private TextViewModel toTest;

    @BeforeMethod
    public void setup(){
        this.toTest = new TextViewModel();
    }

    @Test
    public void startsEmpty(){
        String text = toTest.getText();
        assertEquals(text, "");
    }
    @Test
    public void hasPrintWriter(){
        PrintStream writer = toTest.getWriter();
        assertNotNull(writer);
    }
    @Test
    public void writeIsReflectedInText(){
        PrintStream writer = toTest.getWriter();
        writer.println(TEXT_TO_WRITE);
        assertEquals(toTest.getText(), TEXT_TO_WRITE+ SystemInfo.NEW_LINE);
    }
    @Test
    public void writeMultipleTimes(){
        PrintStream writer = toTest.getWriter();
        writer.println("42");
        writer.println(TEXT_TO_WRITE);
        assertEquals(toTest.getText(),"42"+ SystemInfo.NEW_LINE+ TEXT_TO_WRITE+ SystemInfo.NEW_LINE);
    }
    @Test
    public void writeMultipleTimesFromDifferentStreams(){
        toTest.getWriter().println("42");
        toTest.getWriter().println(TEXT_TO_WRITE);
        assertEquals(toTest.getText(),"42"+ SystemInfo.NEW_LINE+ TEXT_TO_WRITE+ SystemInfo.NEW_LINE);
    }
    @Test
    public void firesEventOnWrite(){
        final WasInvoked check = new WasInvoked();
        toTest.addEventListener(check);
        toTest.getWriter().println("42");
        check.assertWasInvoked();
    }
    @Test
    public void clearFiredEvent(){
        final WasInvoked check = new WasInvoked();
        toTest.addEventListener(check);
        toTest.clear();
        check.assertWasInvoked();
    }
    @Test
    public void canClear(){
        PrintStream writer = toTest.getWriter();
        writer.println("42");
        writer.println(TEXT_TO_WRITE);
        toTest.clear();
        assertEquals(toTest.getText(),"");
    }
}
