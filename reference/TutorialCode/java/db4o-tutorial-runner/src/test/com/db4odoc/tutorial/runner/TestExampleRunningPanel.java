package com.db4odoc.tutorial.runner;


import com.db4odoc.tutorial.utils.NoArgAction;
import org.testng.Assert;
import org.testng.annotations.BeforeMethod;
import org.testng.annotations.Test;

import javax.swing.*;
import java.awt.event.ActionListener;

public class TestExampleRunningPanel {

    private static final String TEST_SNIPPET = "String test=null;";
    private RunningCodeViewModel model;
    private ExampleRunningPanel toTest;

    @BeforeMethod
    public void setup() {
        SwingInvokerTestUtil.runTestInSwingInvoker(new NoArgAction() {
            @Override
            public void invoke() {
                model = RunningCodeViewModel.create(RunPreparation.NO_PREPARATION);
                model.addSnippet(new ExampleSnippet("Test", TEST_SNIPPET));
                toTest = new ExampleRunningPanel(model);
            }
        });
    }

    @Test
    public void hasEmptyTestFromModel() {
        SwingInvokerTestUtil.runTestInSwingInvoker(new NoArgAction() {
            @Override
            public void invoke() {
                String text = toTest.getOutputTextArea().getText();
                Assert.assertEquals(text, "");
            }
        });
    }

    @Test
    public void testIsPickedUpFromModel() {
        SwingInvokerTestUtil.runTestInSwingInvoker(new NoArgAction() {
            @Override
            public void invoke() {
                model.getConsoleView().getWriter().println("Hi");
                String text = toTest.getOutputTextArea().getText();
                Assert.assertEquals(text, "Hi" + SystemInfo.NEW_LINE);
            }
        });
    }

    @Test
    public void codeWindowIsSynchonizedWithModel() {
        SwingInvokerTestUtil.runTestInSwingInvoker(new NoArgAction() {
            @Override
            public void invoke() {
                toTest.getCodeTextArea().setText("Hi");
                clickButton(TestExampleRunningPanel.this.toTest.getRunButton());
                Assert.assertEquals("Hi", model.getCodeSnippet());
            }
        });
    }
    @Test
    public void executesCode() {
        SwingInvokerTestUtil.runTestInSwingInvoker(new NoArgAction() {
            @Override
            public void invoke() {

                toTest.getCodeTextArea().setText("System.out.println(\"Hello World\");");
                clickButton(toTest.getRunButton());
                Assert.assertEquals("Hello World"+ SystemInfo.NEW_LINE, model.getConsoleView().getText());
            }
        });
    }
    @Test
    public void clearUp() {
        SwingInvokerTestUtil.runTestInSwingInvoker(new NoArgAction() {
            @Override
            public void invoke() {
                model.getConsoleView().getWriter().println("Hi");
                clickButton(toTest.getClearButton());
                Assert.assertEquals("", model.getConsoleView().getText());
            }
        });
    }
    @Test
    public void showSnippets() {
        SwingInvokerTestUtil.runTestInSwingInvoker(new NoArgAction() {
            @Override
            public void invoke() {
                Assert.assertEquals(1, toTest.getExampleList().getComponentCount());
            }
        });
    }
    @Test
    public void useSnippet() {
        SwingInvokerTestUtil.runTestInSwingInvoker(new NoArgAction() {
            @Override
            public void invoke() {
                final JButton button = (JButton) toTest.getExampleList().getComponent(0);
                clickButton(button);
                Assert.assertEquals(TEST_SNIPPET, model.getCodeSnippet());
            }
        });
    }

    private void clickButton(JButton buttonToClick) {
        for (ActionListener actionListener : buttonToClick.getActionListeners()) {
            actionListener.actionPerformed(null);
        }
    }
}
