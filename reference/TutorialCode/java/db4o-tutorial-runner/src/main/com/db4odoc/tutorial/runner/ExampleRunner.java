package com.db4odoc.tutorial.runner;


import com.db4o.Db4oEmbedded;
import com.db4o.ObjectContainer;
import com.db4o.config.EmbeddedConfiguration;
import com.db4o.io.PagingMemoryStorage;
import com.db4o.reflect.jdk.JdkReflector;
import com.db4odoc.tutorial.faulttolerance.OperationResult;
import com.db4odoc.tutorial.utils.NoArgAction;
import com.db4odoc.tutorial.utils.OneArgAction;

import java.io.PrintStream;

import static com.db4odoc.tutorial.runner.RunPreparation.NO_PREPARATION;
import static com.db4odoc.tutorial.utils.ExceptionUtils.reThrow;

public class ExampleRunner {
    private final PrintStream writer;
    private final ExecutableClassGenerator generator = ExecutableClassGenerator.create();
    private final RunPreparation preparation;
    private final ObjectContainer container;

    private ExampleRunner(PrintStream writer, RunPreparation preparation, ObjectContainer container) {
        this.writer = writer;
        this.preparation = preparation;
        this.container = container;
    }

    public static ExampleRunner create(PrintStream writer){
        return create(writer, NO_PREPARATION);
    }
    public static ExampleRunner create(PrintStream writer,RunPreparation preparation){
        return create(writer, preparation,
                openContainer(ExampleRunner.class.getClassLoader()));
    }
    public static ExampleRunner create(PrintStream writer,RunPreparation preparation,
                                       ObjectContainer container){
        return new ExampleRunner(writer,preparation,container);
    }

    public void run(String codeToRun) {
        try {
            final OperationResult<OneArgAction<ObjectContainer>> result
                    = generator.generateWithBody(codeToRun,preparation.packages());
            if(result.wasSuccessful()){
                runSnippet(result);
            } else{
                printError(result.getException());
            }
        } catch (Exception e) {
            throw reThrow(e);
        }
    }

    private static ObjectContainer openContainer(ClassLoader loader) {
        EmbeddedConfiguration config = Db4oEmbedded.newConfiguration();
        config.common().reflectWith(new JdkReflector(loader));
        config.file().storage(new PagingMemoryStorage());
        return Db4oEmbedded.openFile(config,"!In:Memory!");
    }

    private void printError(final Exception exception) {
        writer.print("Compile error: ");
        writer.print(exception.getMessage());
        writer.flush();
    }

    private void runSnippet(final OperationResult<OneArgAction<ObjectContainer>> result) {
        withRedirectedOut(new NoArgAction() {
            @Override
            public void invoke() {
                try {
                    preparation.prepareDB(container);
                    result.getResultData().invoke(container);
                } catch (Exception e) {
                    e.printStackTrace(writer);
                }
            }
        });
    }


    private void withRedirectedOut(NoArgAction toRun) {
        PrintStream oldout = System.out;
        try {
            System.setOut(writer);
            toRun.invoke();
        } finally {
            System.setOut(oldout);
        }
    }

    public void dispose() {
        container.close();
    }
}
