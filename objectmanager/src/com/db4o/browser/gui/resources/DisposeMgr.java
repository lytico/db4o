/*
 * Copyright (C) 2005 db4objects Inc.  http://www.db4o.com
 */
package com.db4o.browser.gui.resources;

import org.eclipse.swt.events.DisposeEvent;
import org.eclipse.swt.events.DisposeListener;
import org.eclipse.swt.graphics.Cursor;
import org.eclipse.swt.graphics.Image;
import org.eclipse.ve.sweet.reflect.DuckType;


public class DisposeMgr implements DisposeListener {

    private IDisposable disposableObject;

    public DisposeMgr(Image image) {
        this.disposableObject = (IDisposable) DuckType.implement(IDisposable.class, image);
    }
    
    public DisposeMgr(Object receiver, Image image) {
        if (!DuckType.instanceOf(IImageHolder.class, receiver)) {
            throw new ClassCastException("IImageHolder isn't a duck interface of " + receiver.getClass().getName());
        }

        this.disposableObject = (IDisposable) DuckType.implement(IDisposable.class, image);

        IImageHolder imageHolder = (IImageHolder) DuckType.implement(
                IImageHolder.class, receiver);
        imageHolder.setImage(image);
        imageHolder.addDisposeListener(this);
    }
    
    public DisposeMgr(Object receiver, Cursor cursor) {
        if (!DuckType.instanceOf(ICursorHolder.class, receiver)) {
            throw new ClassCastException("ICursorHolder isn't a duck interface of " + receiver.getClass().getName());
        }

        this.disposableObject = (IDisposable) DuckType.implement(IDisposable.class, cursor);

        ICursorHolder cursorHolder = (ICursorHolder) DuckType.implement(ICursorHolder.class, receiver);
        cursorHolder.setCursor(cursor);
        cursorHolder.addDisposeListener(this);
    }
    
    public DisposeMgr(Cursor cursor) {
        this.disposableObject = (IDisposable) DuckType.implement(IDisposable.class, cursor);
    }
    
    public DisposeMgr(Object object) {
        if (!DuckType.instanceOf(IDisposable.class, object)) {
            throw new ClassCastException("IDisposable isn't a duck interface of " + object.getClass().getName());
        }
        this.disposableObject = (IDisposable) DuckType.implement(IDisposable.class, object);
    }
    
    public void widgetDisposed(DisposeEvent e) {
        if (!disposableObject.isDisposed())
            disposableObject.dispose();
    }
    
    public Object get() {
        return disposableObject;
    }

}
