package com.db4o.objectManager.v2.shortcuts;

import java.awt.event.KeyListener;
import java.awt.event.KeyEvent;

/**
 * User: treeder
 * Date: Aug 28, 2006
 * Time: 2:16:17 PM
 */
public class ShortcutsListener implements KeyListener {
    public void keyTyped(KeyEvent e) {
        System.out.println(e);
        
    }

    public void keyPressed(KeyEvent e) {
        //System.out.println(e);
    }

    public void keyReleased(KeyEvent e) {
        //System.out.println(e);
    }
}
