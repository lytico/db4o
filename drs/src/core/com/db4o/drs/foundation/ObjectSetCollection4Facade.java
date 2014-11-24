/* Copyright (C) 2004 - 2008  Versant Inc.  http://www.db4o.com

This file is part of the db4o open source object database.

db4o is free software; you can redistribute it and/or modify it under
the terms of version 2 of the GNU General Public License as published
by the Free Software Foundation and as clarified by db4objects' GPL 
interpretation policy, available at
http://www.db4o.com/about/company/legalpolicies/gplinterpretation/
Alternatively you can write to db4objects, Inc., 1900 S Norfolk Street,
Suite 350, San Mateo, CA 94403, USA.

db4o is distributed in the hope that it will be useful, but WITHOUT ANY
WARRANTY; without even the implied warranty of MERCHANTABILITY or
FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License
for more details.

You should have received a copy of the GNU General Public License along
with this program; if not, write to the Free Software Foundation, Inc.,
59 Temple Place - Suite 330, Boston, MA  02111-1307, USA. */
package com.db4o.drs.foundation;

import com.db4o.foundation.*;

/**
 * @sharpen.ignore
 */
public class ObjectSetCollection4Facade extends ObjectSetAbstractFacade {
	
    Collection4 _delegate;
    private Iterator4 _currentIterator;
    private boolean _endOfIteration;

    public ObjectSetCollection4Facade(Collection4 delegate_) {
        this._delegate = delegate_;
    }

    public boolean hasNext() {
    	currentIterator();
        return _endOfIteration;
    }	

    public Object next() {
    	Object nextItem = currentIterator().current();
    	moveNext();
    	return nextItem;
    }

    public boolean contains(Object obj) {
        return _delegate.contains(obj);
    }
    
    public int size() {
        return _delegate.size();
    }
   
    private Iterator4 currentIterator() {
        if (_currentIterator == null) {
        	_currentIterator = _delegate.iterator();
        	_endOfIteration = false;
        	moveNext();
        }
        return _currentIterator;
    }
    
    private void moveNext() {
    	_endOfIteration = _currentIterator.moveNext();
    }

	public void reset() {
		_currentIterator = null;
	}
}
