package com.db4odoc.tutorial.runner;


import com.db4odoc.tutorial.utils.Disposable;
import com.db4odoc.tutorial.utils.EventListeners;
import com.db4odoc.tutorial.utils.NoArgAction;

import java.io.*;

public class TextViewModel{
    private String text = "";
    private final EventListeners<NoArgAction> events = EventListeners.create(NoArgAction.class);
    public String getText() {
        return text;
    }

    public PrintStream getWriter() {
        return new PrintStream(new OutputStream() {
            private final StringBuilder builder = new StringBuilder();
            @Override
            public void write(int b) throws IOException {
                builder.append((char)b);
            }

            @Override
            public void flush() throws IOException{
                super.flush();
                text += builder.toString();
                builder.delete(0,builder.length());
                events.invoker().invoke();
            }
        },true);
    }

    public void clear() {
        this.text = "";
        events.invoker().invoke();
    }


    public Disposable addEventListener(NoArgAction event) {
        return events.add(event);
    }
}
