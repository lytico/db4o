package com.db4odoc.tutorial.runner;

import com.db4odoc.tutorial.utils.WasInvoked;
import org.testng.annotations.BeforeMethod;
import org.testng.annotations.Test;

import static org.testng.Assert.assertEquals;
import static org.testng.Assert.assertNotNull;

/**
 * @author roman.stoffel@gamlor.info
 * @since 20.04.11
 */
public class TestRunningCodeViewModel {

    private RunningCodeViewModel toTest;

    @BeforeMethod
    public void setup(){
        this.toTest = RunningCodeViewModel.create(RunPreparation.NO_PREPARATION);
    }
    @Test
    public void emptyTestAsDefault() {
        assertEquals("",toTest.getCodeSnippet());
    }
    @Test
    public void manipulateCode() {
        toTest.setCodeSnippet("42");
        assertEquals("42",toTest.getCodeSnippet());
    }
    @Test
    public void settingSnippetFiresEvent() {
        WasInvoked eventListener = new WasInvoked();
        toTest.addTextChangedListener(eventListener);
        toTest.setCodeSnippet("42");
        eventListener.assertWasInvoked();
    }
    @Test
    public void hasConsoleView() {
        assertNotNull(toTest.getConsoleView());
    }
    @Test
    public void runsCode() {
        System.out.println("Hi");
        toTest.setCodeSnippet("System.out.println(\"Hi\");");
        toTest.runCode();
        assertEquals("Hi" + SystemInfo.NEW_LINE, toTest.getConsoleView().getText());

    }
    @Test
    public void addSnippet() {
        toTest.addSnippet(new ExampleSnippet("Query Car","List<Car> cars = null;"));
        final ExampleSnippet snippet = toTest.getSnippets().iterator().next();
        assertEquals(snippet.getLabel(),"Query Car");
        assertEquals(snippet.getSnippet(),"List<Car> cars = null;");
    }
}
