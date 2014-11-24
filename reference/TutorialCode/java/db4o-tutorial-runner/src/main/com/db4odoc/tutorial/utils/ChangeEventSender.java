package com.db4odoc.tutorial.utils;

import java.beans.PropertyChangeListener;

/**
 * @author roman.stoffel@gamlor.info
 * @since 27.07.2010
 */
public interface ChangeEventSender {
    void addPropertyChangeListener(PropertyChangeListener listener);

    void removePropertyChangeListener(PropertyChangeListener listener);
}
