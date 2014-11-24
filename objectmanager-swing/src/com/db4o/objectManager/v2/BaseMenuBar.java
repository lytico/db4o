package com.db4o.objectManager.v2;

import com.jgoodies.looks.Options;
import com.jgoodies.looks.windows.WindowsLookAndFeel;
import com.jgoodies.looks.plastic.PlasticLookAndFeel;
import com.db4o.objectManager.v2.resources.ResourceManager;

import javax.swing.*;
import java.awt.event.ActionListener;

/**
 * User: treeder
 * Date: Aug 30, 2006
 * Time: 12:24:42 PM
 */
public class BaseMenuBar extends JMenuBar {
    public BaseMenuBar(Settings settings, ActionListener helpActionListener, ActionListener aboutActionListener) {

        putClientProperty(Options.HEADER_STYLE_KEY,
                settings.getMenuBarHeaderStyle());
        putClientProperty(PlasticLookAndFeel.BORDER_STYLE_KEY,
                settings.getMenuBarPlasticBorderStyle());
        putClientProperty(WindowsLookAndFeel.BORDER_STYLE_KEY,
                settings.getMenuBarWindowsBorderStyle());
        putClientProperty(PlasticLookAndFeel.IS_3D_KEY,
                settings.getMenuBar3DHint());
    }

    protected JMenu buildHelpMenu(ActionListener helpActionListener, ActionListener aboutActionListener) {

        JMenu menu = createMenu("Help", 'H');

        JMenuItem item;

        item = createMenuItem("Help Contents", ResourceManager.createImageIcon("help.gif"), 'H');
        if (helpActionListener != null) {
            item.addActionListener(helpActionListener);
        }
        menu.add(item);

        menu.addSeparator();
        item = createMenuItem("About", 'a');
        item.addActionListener(aboutActionListener);
        menu.add(item);

        return menu;
    }

    protected JMenu createMenu(String text, char mnemonic) {
        JMenu menu = new JMenu(text);
        menu.setMnemonic(mnemonic);
        return menu;
    }

    protected JMenuItem createMenuItem(String text) {
        return new JMenuItem(text);
    }

    protected JMenuItem createMenuItem(String text, char mnemonic) {
        return new JMenuItem(text, mnemonic);
    }

    protected JMenuItem createMenuItem(String text, char mnemonic, KeyStroke key) {
        JMenuItem menuItem = new JMenuItem(text, mnemonic);
        menuItem.setAccelerator(key);
        return menuItem;
    }

    protected JMenuItem createMenuItem(String text, Icon icon) {
        return new JMenuItem(text, icon);
    }

    protected JMenuItem createMenuItem(String text, Icon icon, char mnemonic) {
        JMenuItem menuItem = new JMenuItem(text, icon);
        menuItem.setMnemonic(mnemonic);
        return menuItem;
    }

    protected JMenuItem createMenuItem(String text, Icon icon, char mnemonic, KeyStroke key) {
        JMenuItem menuItem = createMenuItem(text, icon, mnemonic);
        menuItem.setAccelerator(key);
        return menuItem;
    }
}
