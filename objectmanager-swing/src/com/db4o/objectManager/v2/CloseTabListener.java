package com.db4o.objectManager.v2;

import javax.swing.*;
import java.awt.event.MouseListener;
import java.awt.event.MouseEvent;
import java.awt.event.ActionListener;
import java.awt.event.ActionEvent;
import java.awt.Point;
import java.awt.Rectangle;

/**
 * User: treeder
 * Date: Sep 3, 2006
 * Time: 5:41:30 PM
 */
public class CloseTabListener implements MouseListener, ActionListener {
    private JTabbedPane tabbedPane;
    JPopupMenu popup;
    private Object source;
    private Point clickPoint;

    public CloseTabListener(JTabbedPane tabbedPane) {

        this.tabbedPane = tabbedPane;

        popup = new JPopupMenu();
        JMenuItem menuItem = new JMenuItem("Close Tab");
        menuItem.addActionListener(this);
        popup.add(menuItem);
    }

    public void mouseClicked(MouseEvent e) {

    }

    public void mousePressed(MouseEvent e) {
        showPopup(e);
    }

    private void showPopup(MouseEvent e) {
        if (e.isPopupTrigger()) {
            source = e.getSource();
            clickPoint = e.getPoint();
            popup.show(e.getComponent(),
                    e.getX(), e.getY());
        }
    }

    public void mouseReleased(MouseEvent e) {
        showPopup(e); // here because different platforms handle popups differently
    }

    public void mouseEntered(MouseEvent e) {
        
    }

    public void mouseExited(MouseEvent e) {

    }

    public void actionPerformed(ActionEvent e) {
        for(int i = 1; i < tabbedPane.getTabCount(); i++){// skip Home
            Rectangle rect = tabbedPane.getUI().getTabBounds(tabbedPane, i);
            if(rect.contains(clickPoint)){
                tabbedPane.remove(i);
            }
        }
    }
}
