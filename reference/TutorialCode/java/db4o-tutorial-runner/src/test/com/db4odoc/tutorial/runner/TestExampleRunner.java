package com.db4odoc.tutorial.runner;


import com.db4o.ObjectContainer;
import com.db4odoc.tutorial.demos.firststeps.Car;
import org.testng.Assert;
import org.testng.annotations.BeforeMethod;
import org.testng.annotations.Test;

import static java.util.Arrays.asList;
import static org.mockito.Mockito.mock;
import static org.mockito.Mockito.verify;

public class TestExampleRunner {

    private ExampleRunner toTest;
    private TextViewModel viewModel;

    @BeforeMethod
    public void setup(){
        this.viewModel = new TextViewModel();
        this.toTest = ExampleRunner.create(viewModel.getWriter());
    }

    @Test
    public void runsEmptyCode(){
        toTest.run("");
        Assert.assertEquals(viewModel.getText(), "");
    }
    @Test
    public void printsIntoModel(){
        toTest.run("System.out.println(\"Hi\");");
        Assert.assertEquals(viewModel.getText(),"Hi"+ SystemInfo.NEW_LINE);
    }
    @Test
    public void printsCompileError(){
        toTest.run("System.out.");
        Assert.assertTrue(viewModel.getText().contains("Compile error:"));
    }
    @Test
    public void runtimeErrorIsPrinted(){
        toTest.run("throw new RuntimeException(\"test\");");
        Assert.assertTrue(viewModel.getText().contains("RuntimeException"));
    }
    @Test
    public void shortenStackTraceOnCompileError(){
        toTest.run("System.out.");
        Assert.assertFalse(viewModel.getText().contains("TestExampleRunner"));
    }


    @Test
    public void allowsPreparation(){
        toTest = ExampleRunner.create(viewModel.getWriter(),new RunPreparation(){
            @Override
            public Iterable<String> packages() {
                return asList("com.db4odoc.tutorial.demos.firststeps.*");
            }

            @Override
            public void prepareDB(ObjectContainer container) {
                container.store(new Car("Car"));
            }
        });
        toTest.run("List<Car> cars = container.query(Car.class); System.out.println(cars.size());");
        Assert.assertEquals(viewModel.getText(),"1"+ SystemInfo.NEW_LINE);
    }
    @Test
    public void disposesContainer(){
        ObjectContainer container = mock(ObjectContainer.class);
        ExampleRunner runner = ExampleRunner.create(viewModel.getWriter(), RunPreparation.NO_PREPARATION, container);
        runner.dispose();
        verify(container).close();
    }
}
