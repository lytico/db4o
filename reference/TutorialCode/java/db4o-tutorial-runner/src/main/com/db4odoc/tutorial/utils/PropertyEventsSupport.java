package com.db4odoc.tutorial.utils;

import java.beans.PropertyChangeListener;
import java.beans.PropertyChangeSupport;

/**
 * @author roman.stoffel@gamlor.info
 * @since 27.07.2010
 */
public final class PropertyEventsSupport implements Disposable {
    private transient PropertyChangeSupport changeSupport;

    public PropertyEventsSupport(Object forObject) {
        this.changeSupport = new PropertyChangeSupport(forObject);
    }

    public void addPropertyChangeListener(PropertyChangeListener listener) {
        changeSupport.addPropertyChangeListener(listener);
    }

    public void removePropertyChangeListener(PropertyChangeListener listener) {
        changeSupport.removePropertyChangeListener(listener);
    }

    public void firePropertyChange(String propertyName, Object newValue) {
        changeSupport.firePropertyChange(propertyName, new Object(), newValue);
    }

    @Override
    public void dispose() {
        final PropertyChangeListener[] changeListeners = changeSupport.getPropertyChangeListeners();
        for (PropertyChangeListener changeListener : changeListeners) {
            changeSupport.removePropertyChangeListener(changeListener);
        }
    }
}
