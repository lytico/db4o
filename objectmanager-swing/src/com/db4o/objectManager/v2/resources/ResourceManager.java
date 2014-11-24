package com.db4o.objectManager.v2.resources;

import com.db4o.objectManager.v2.Dashboard;

import javax.swing.*;

/**
 * User: treeder
 * Date: Aug 21, 2006
 * Time: 6:14:49 PM
 */
public class ResourceManager {
	public static final String ICONS_16X16 = "icons/16x16/";
	public static final String ICONS_PLAIN_16X16 = "icons/plain/16x16/";
	public static final String ICONS_32X32 = "icons/32x32/";

	public static ImageIcon createImageIcon(String filename, String description) {
        java.net.URL imgURL = Dashboard.class.getResource("resources/images/" + filename);
        if (imgURL != null) {
            return new ImageIcon(imgURL, description);
        } else {
            System.err.println("Couldn't find file: " + filename);
            return null;
        }
    }

    public static Icon createImageIcon(String filename) {
        return createImageIcon(filename, null);
    }
}
