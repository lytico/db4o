package com.db4odoc.tutorial.runner;

import java.util.List;

/**
 * @author roman.stoffel@gamlor.info
 * @since 27.04.11
 */
public interface DemoPack {
    List<ExampleSnippet> snippets();

    RunPreparation preparation();

}
