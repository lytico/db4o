package com.db4odoc.tutorial.runner;

import com.db4odoc.tutorial.utils.Disposable;
import com.db4odoc.tutorial.utils.EventListeners;
import com.db4odoc.tutorial.utils.NoArgAction;

import java.util.ArrayList;
import java.util.Collections;
import java.util.List;

/**
 * @author roman.stoffel@gamlor.info
 * @since 20.04.11
 */
public class RunningCodeViewModel {

    private String codeSnippet = "";
    private final TextViewModel consoleView = new TextViewModel();
    private final ExampleRunner runner;
    private final List<ExampleSnippet> snippets = new ArrayList<ExampleSnippet>();
    private final EventListeners<NoArgAction> codeSnippetChanged = EventListeners.create(NoArgAction.class);

    RunningCodeViewModel(RunPreparation preparation) {
        runner = ExampleRunner.create(consoleView.getWriter(),preparation);
    }

    public String getCodeSnippet() {
        return codeSnippet;
    }

    public void setCodeSnippet(String codeSnippet) {
        this.codeSnippet = codeSnippet;
        codeSnippetChanged.invoker().invoke();
    }

    public static RunningCodeViewModel create(RunPreparation preparation) {
        return new RunningCodeViewModel(preparation);
    }

    public TextViewModel getConsoleView() {
        return consoleView;
    }

    public void runCode(){
        runner.run(getCodeSnippet());
    }

    public Iterable<ExampleSnippet> getSnippets() {
        return Collections.unmodifiableCollection(snippets);
    }

    public boolean addSnippet(ExampleSnippet exampleSnippet) {
        return snippets.add(exampleSnippet);
    }

    public Disposable addTextChangedListener(NoArgAction event) {
        return codeSnippetChanged.add(event);
    }
}
