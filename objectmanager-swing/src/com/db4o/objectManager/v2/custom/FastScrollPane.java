package com.db4o.objectManager.v2.custom;

import java.awt.*;

import javax.swing.*;
import javax.swing.event.*;
import javax.swing.plaf.basic.*;

/**
 * A scroll pane that only updates when you release the mouse.
 * <p/>
 * Might be possible to implement this by removing the listeners and adding custom ones instead of replacing the UI.
 * <p/>
 * User: treeder
 * Date: Aug 30, 2006
 * Time: 4:43:03 PM
 */
public class FastScrollPane extends JScrollPane {

    public FastScrollPane(JTable table) {
        super(table);
        this.setUI(new FastScrollPaneUI(table));
    }


    class FastScrollPaneUI extends BasicScrollPaneUI {

        JTable table;

        public FastScrollPaneUI(JTable table) {
            this.table = table;
        }

        protected ChangeListener createVSBChangeListener() {
            return new myVSBChangeListener();
        }

        protected ChangeListener createHSBChangeListener() {
            return new myHSBChangeListener();
        }

        protected ChangeListener createViewportChangeListener() {
            return new myViewportChangeHandler();
        }

        public class myViewportChangeHandler implements ChangeListener {
            public void stateChanged(ChangeEvent e) {
                syncScrollPaneWithViewport();
            }
        }

        public class myHSBChangeListener implements ChangeListener {
            public void stateChanged(ChangeEvent e) {
                JViewport viewport = scrollpane.getViewport();
                if (viewport != null) {
                    BoundedRangeModel model = (BoundedRangeModel) (e.getSource());
                    Point p = viewport.getViewPosition();
                    p.x = model.getValue();
                    JScrollBar sb = scrollpane.getHorizontalScrollBar();
                    if (!sb.getValueIsAdjusting()) viewport.setViewPosition(p);
                }
            }
        }

        public class myVSBChangeListener implements ChangeListener {
            public void stateChanged(ChangeEvent e) {
                JViewport viewport = scrollpane.getViewport();
                if (viewport != null) {
                    BoundedRangeModel model = (BoundedRangeModel) (e.getSource());
                    Point p = viewport.getViewPosition();
                    p.y = model.getValue();
                    JScrollBar sb = scrollpane.getVerticalScrollBar();
                    if (!sb.getValueIsAdjusting()) viewport.setViewPosition(p);
                }
            }
        }

    }
}
