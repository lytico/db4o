package com.db4o.objectManager.v2.custom;

import java.awt.*;
import java.net.*;

import javax.imageio.*;
import javax.swing.*;

import com.db4o.objectManager.v2.*;

/**
 * User: treeder
 * Date: Aug 8, 2006
 * Time: 11:42:46 AM
 */
public class BackgroundPanel extends JPanel {
    private Image bgImage;


    public BackgroundPanel() {
        setLayout(new BoxLayout(this, BoxLayout.PAGE_AXIS));
        bgImage = getImage(Dashboard.class.getResource("resources/images/gradient.png"));
    }

    // Since we're always going to fill our entire
    // bounds, allow Swing to optimize painting of us
    public boolean isOpaque() {
        return true;
    }

    protected void paintComponent(Graphics g) {
        super.paintComponent(g);
        // We'll do our own painting, so leave out
        // a call to the superclass behavior

        // We're telling Swing that we're opaque, and
        // we'll honor this by always filling our
        // our entire bounds with solid colors.
/*
        int w = getWidth();
        int h = getHeight();
        int leftW = w / 2;
        int topH = h / 2;

        // Paint the top left and bottom right in red.
        g.setColor(Color.RED);
        g.fillRect(0, 0, leftW, topH);
        g.fillRect(leftW, topH, w - leftW, h - topH);

        // Paint the bottom left and top right in white.
        g.setColor(Color.WHITE);
        g.fillRect(0, topH, leftW, h - topH);
        g.fillRect(leftW, 0, w - leftW, topH);*/

        paintBackground(g);

    }

    private void paintBackground(Graphics g) {
        // if a background image exists, paint it
        if (bgImage != null) {
            int width = this.getWidth();
            int height = this.getHeight();
            int imageW = bgImage.getWidth(null);
            int imageH = bgImage.getHeight(null);

            // we'll tile the image to fill our area
            for (int x = 0; x < width; x += imageW) {
                for (int y = 0; y < height; y += imageH) {
                    g.drawImage(bgImage, x, y, this);
                }
            }
        }
    }

    /**
     * Convenience method to load an image from the given URL.
     * This implementation uses <CODE>ImageIO</CODE> to load
     * the image and thus returns <CODE>BufferedImages</CODE>.
     *
     * @param imageURL the URL to an image
     * @return the image or null if the image couldn't be loaded
     */
    protected static Image getImage(URL imageURL) {
        Image image = null;

        try {
            // use ImageIO to read in the image
            image = ImageIO.read(imageURL);
        } catch (Exception ioe) {
            ioe.printStackTrace();
        }

        return image;
    }
}
