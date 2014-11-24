package com.db4o.objectManager.v2.custom;

import java.awt.*;

import javax.swing.*;
import javax.swing.border.*;

/**
 * This doesn't extend JPanel so that subclass can support builder pattern (jgoodies)
 *
 * User: treeder
 * Date: Sep 3, 2006
 * Time: 4:25:58 PM
 */
public class BorderedPanel {

    private String title;
    private JPanel outer;

    public BorderedPanel(String title) {
        this.title = title;
        JPanel outer = new JPanel(new BorderLayout());
        Border b = new LineBorder(Color.GRAY, 1, true);
        TitledBorder b2 = new TitledBorder(b, title);
        this.outer = outer;
        this.outer.setBorder(b2);
    }


    public void add(JComponent p) {
        outer.add(p);
    }

    public JPanel getPanel(){
        return outer;
    }

}
