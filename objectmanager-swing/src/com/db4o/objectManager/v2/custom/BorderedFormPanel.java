package com.db4o.objectManager.v2.custom;

import com.jgoodies.forms.builder.DefaultFormBuilder;
import com.jgoodies.forms.layout.FormLayout;

import javax.swing.*;
import java.awt.Component;

/**
 * User: treeder
 * Date: Sep 3, 2006
 * Time: 4:45:04 PM
 */
public class BorderedFormPanel extends BorderedPanel{
    String columnLayout = "left:max(60dlu;pref), 10dlu, 120dlu:grow, 7dlu";
    private DefaultFormBuilder builder;

    public BorderedFormPanel(String title) {
        super(title);

        FormLayout layout = new FormLayout(
				columnLayout, // 1st major colum
                "");                                         // add rows dynamically

        builder = new DefaultFormBuilder(layout);
        builder.setDefaultDialogBorder();
    }
     public void append(String label, Component value) {
        builder.append(label, value);
    }

    public JPanel getPanel() {
        JPanel p = builder.getPanel();
        add(p);
        return super.getPanel();

    }

}
