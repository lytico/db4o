package com.db4o.objectManager.v2.connections;

import com.db4o.objectmanager.model.Db4oConnectionSpec;
import com.db4o.objectmanager.model.Db4oSocketConnectionSpec;
import com.db4o.objectManager.v2.resources.ResourceManager;

import javax.swing.*;
import java.awt.Component;
import java.awt.Color;

/**
 * User: treeder
 * Date: Sep 17, 2006
 * Time: 9:20:26 PM
 */
public class ConnectionListCellRenderer extends JPanel implements ListCellRenderer {
    private JLabel pathLabel;
    private JLabel imageLabel;
    private static final String IMG_LOCAL = "harddisk.png";
    private static final String IMG_NETWORKED = "client_network.png";


    public ConnectionListCellRenderer() {
        super();
        setLayout(new BoxLayout(this, BoxLayout.LINE_AXIS));

        pathLabel = new JLabel(" ");
        imageLabel = new JLabel();
        imageLabel.setOpaque(true);
        add(imageLabel);
        add(pathLabel);

    }

    public Component getListCellRendererComponent(JList list, Object value, int index, boolean isSelected, boolean cellHasFocus) {
        Db4oConnectionSpec entry = (Db4oConnectionSpec) value;
        String user = null;
        String imgIcon;
        if(entry instanceof Db4oSocketConnectionSpec){
            Db4oSocketConnectionSpec spec = (Db4oSocketConnectionSpec) entry;
            user = (spec.getUser());
            imgIcon = IMG_NETWORKED;
        } else {
            imgIcon = IMG_LOCAL;
        }
        pathLabel.setText(entry.getFullPath());
        String host = entry.getFullPath();
        if (host == null) {
            host = " ";
        }
        if (user == null) {
            user = " ";
        }
        pathLabel.setText(host);
        imageLabel.setIcon(ResourceManager.createImageIcon(ResourceManager.ICONS_16X16 + imgIcon));
        if (isSelected) {
            adjustColors(list.getSelectionBackground(),
                    list.getSelectionForeground(), this, imageLabel, pathLabel);
        } else {
            adjustColors(list.getBackground(),
                    list.getForeground(), this, imageLabel, pathLabel);
        }
        return this;
    }
    private void adjustColors(Color bg, Color fg, Component...components) {
            for (Component c : components) {
                c.setForeground(fg);
                c.setBackground(bg);
            }
        }

}
