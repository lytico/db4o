package com.db4o.objectManager.v2.uiHelper;

import com.db4o.objectManager.v2.resources.ResourceManager;

import javax.swing.*;
import java.awt.Component;

/**
 * User: treeder
 * Date: Sep 11, 2006
 * Time: 4:15:49 PM
 */
public class OptionPaneHelper {
    public static void showErrorMessage(Component frame, String msg, String title){
        JOptionPane.showMessageDialog(frame, msg, title, JOptionPane.ERROR_MESSAGE, ResourceManager.createImageIcon(ResourceManager.ICONS_32X32 + "warning.png"));
    }

    public static void showSuccessDialog(Component frame, String msg, String title) {
        JOptionPane.showMessageDialog(frame, msg, title, JOptionPane.INFORMATION_MESSAGE, ResourceManager.createImageIcon(ResourceManager.ICONS_32X32 + "warning.png"));
    }
    public static void showConfirmWarning(Component frame, String msg, String title) {
        JOptionPane.showConfirmDialog(frame, msg, title, JOptionPane.WARNING_MESSAGE, JOptionPane.YES_NO_OPTION, ResourceManager.createImageIcon(ResourceManager.ICONS_32X32 + "warning.png"));
    }
}
