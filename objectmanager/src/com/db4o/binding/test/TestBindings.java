/*
 * Copyright (C) 2005 db4objects Inc.  http://www.db4o.com
 */
package com.db4o.binding.test;

import java.io.DataInputStream;
import java.io.IOException;

import org.eclipse.swt.events.ShellAdapter;
import org.eclipse.swt.events.ShellEvent;
import org.eclipse.swt.layout.GridLayout;
import org.eclipse.swt.widgets.Composite;
import org.eclipse.ve.sweet.CannotSaveException;
import org.eclipse.ve.sweet.objectviewer.IObjectViewer;
import org.eclipse.ve.sweet.objectviewer.ObjectViewerFactory;

import com.db4o.Db4o;
import com.db4o.ObjectContainer;
import com.db4o.ObjectSet;
import com.db4o.binding.dataeditors.db4o.Db4oObjectEditorFactory;
import com.db4o.browser.gui.controllers.detail.generator.StringInputStreamFactory;
import com.db4o.browser.gui.standalone.IControlFactory;
import com.db4o.browser.gui.standalone.SWTProgram;
import com.swtworkbench.community.xswt.XSWT;
import com.swtworkbench.community.xswt.XSWTException;

public class TestBindings implements IControlFactory {

    private ObjectContainer database;

    public String resourceFile(String fileName) {
        DataInputStream input = new DataInputStream(getClass().getResourceAsStream(fileName));
        
        StringBuffer result = new StringBuffer();
        byte[] buffer = new byte[4096];
        int bytesRead = -1;
        do {
            try {
                bytesRead = input.read(buffer);
                if (bytesRead > 0) {
                    char chars[] = new char[bytesRead];
                    for (int i = 0; i < chars.length; i++) {
                        chars[i] = (char) buffer[i];
                    }
                    result.append(chars);
                }
            } catch (IOException e) {
                throw new RuntimeException("Error reading resource file", e);
            }
        } while (bytesRead >= 0);
        
        return result.toString();
    }
    
    private String objectEditor = resourceFile("TestBindingsObject.xswt");

    private ITestBindingsObject constructEditor(Composite parent) {
        try {
            return (ITestBindingsObject) XSWT.create(parent, StringInputStreamFactory.construct(objectEditor), ITestBindingsObject.class);
        } catch (XSWTException e) {
            e.printStackTrace();
            return null;
        }
    }

    public void createContents(Composite parent) {
        
        database = Db4o.openFile("TestBindings.yap");
        ObjectSet people = database.get(Person.class);
        
        parent.setLayout(new GridLayout());
        ITestBindingsForm ui = (ITestBindingsForm) XSWT.createl(parent, "TestBindingsForm.xswt", getClass(), ITestBindingsForm.class);
        
        ObjectViewerFactory.factory = new Db4oObjectEditorFactory(database);
        
        Person person = (Person) people.next();
        if (person == null) {
            person = new Person();
        }

        ITestBindingsObject editor = constructEditor(ui.getDataArea());
        final IObjectViewer personObjectEditor1 = ObjectViewerFactory.edit(person);
        personObjectEditor1.bind(editor.getName(), "Name");
        personObjectEditor1.bind(editor.getAge(), "Age");
        
        person = (Person) people.next();
        if (person == null) {
            person = new Person();
        }

        editor = constructEditor(ui.getDataArea());
        final IObjectViewer personObjectEditor2 = ObjectViewerFactory.edit(person);
        personObjectEditor2.bind(editor.getName(), "Name");
        personObjectEditor2.bind(editor.getAge(), "Age");
        
        parent.getShell().addShellListener(new ShellAdapter() {
            public void shellClosed(ShellEvent e) {
                try {
                    personObjectEditor1.commit();
                    personObjectEditor2.commit();
                } catch (CannotSaveException e1) {
                    e1.printStackTrace();
                }
            }
        });
    }
    
    private void close() {
        database.close();
    }

    public static void main(String[] args) {
        TestBindings program = new TestBindings();
        SWTProgram.runWithLog(program);
        program.close();
    }

}
