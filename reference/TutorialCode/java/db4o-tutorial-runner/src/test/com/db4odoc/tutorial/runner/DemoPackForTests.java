package com.db4odoc.tutorial.runner;

import java.util.Collections;
import java.util.List;

/**
 * @author roman.stoffel@gamlor.info
 * @since 27.04.11
 */
public class DemoPackForTests implements DemoPack{
    @Override
    public List<ExampleSnippet> snippets() {
        return Collections.emptyList();
    }

    @Override
    public RunPreparation preparation() {
        return RunPreparation.NO_PREPARATION;
    }
}
