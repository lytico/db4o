package com.db4odoc.tutorial.runner;

/**
 * @author roman.stoffel@gamlor.info
 * @since 27.04.11
 */
public class ExampleSnippet {
    private final String label;
    private final String snippet;

    public ExampleSnippet(String label, String snippet) {
        this.label = label;
        this.snippet = snippet;
    }

    public String getLabel() {
        return label;
    }

    public String getSnippet() {
        return snippet;
    }
}
