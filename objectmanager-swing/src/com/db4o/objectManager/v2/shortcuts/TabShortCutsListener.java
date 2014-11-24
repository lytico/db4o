package com.db4o.objectManager.v2.shortcuts;

import javax.swing.*;
import java.awt.event.KeyListener;
import java.awt.event.KeyEvent;

/**
 * User: treeder
 * Date: Aug 28, 2006
 * Time: 2:26:36 PM
 */
public class TabShortCutsListener implements KeyListener {
    private JTabbedPane tabbedPane;

    public TabShortCutsListener(JTabbedPane tabbedPane) {
        this.tabbedPane = tabbedPane;
    }

    public void keyTyped(KeyEvent e) {
        if (e.getKeyChar() == '') { // ctrl-w - close tab
            if (tabbedPane.getSelectedIndex() > 0) { // never remove Home tab
                tabbedPane.remove(tabbedPane.getSelectedIndex());
            }
        }
    }

    public void keyPressed(KeyEvent e) {

    }

    public void keyReleased(KeyEvent e) {

    }
}
