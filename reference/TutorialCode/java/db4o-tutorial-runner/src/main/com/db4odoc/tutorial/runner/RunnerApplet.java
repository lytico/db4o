package com.db4odoc.tutorial.runner;


import javax.swing.*;
import java.applet.Applet;
import java.awt.*;

import static com.db4odoc.tutorial.utils.ExceptionUtils.reThrow;

public class RunnerApplet  extends Applet {

    public RunnerApplet() throws HeadlessException {
        super();
    }

    @Override
    public void init() {
        super.init();
        setLayout(new BoxLayout(this, BoxLayout.LINE_AXIS));
        setupLookAndFeel();
        final RunningCodeViewModel model = setupDemo();
        this.add(ExampleRunningPanel.create(model));
    }

    @Override
    public void start() {
        super.start();
    }

    @Override
    public void stop() {
        super.stop();
    }

    @Override
    public void destroy() {
        super.destroy();
    }

    private RunningCodeViewModel setupDemo() {
        DemoPack demoPack = DemoPackLoader.loadByName(getParameter("demoClassName"));
        final RunningCodeViewModel model = RunningCodeViewModel.create(demoPack.preparation());
        for (ExampleSnippet snippet : demoPack.snippets()) {
            model.addSnippet(snippet);
        }
        return model;
    }

    private static void setupLookAndFeel()  {
        try {
            UIManager.setLookAndFeel(UIManager.getSystemLookAndFeelClassName());
        } catch (Exception e) {
            reThrow(e);
        }
    }

}
