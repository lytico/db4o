/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.btree;


/**
 * @exclude
 */
public final class Searcher {
    
    private int _lower;
    
    private int _upper;
    
    private int _cursor;
    
    private int _cmp;
    
    private final SearchTarget _target;
    
    private final int _count;
    
    public Searcher(SearchTarget target, int count){
    	if(count < 0){
    		throw new IllegalArgumentException();
    	}
        _target = target;
        _count = count;
        _cmp = -1;
        if(count == 0){
            complete();
            return;
        }
        _cursor = -1;
        _upper = count - 1;
        adjustCursor();
    }
    
    private final void adjustBounds(){
        if(_cmp > 0){
            _upper = _cursor - 1;
            if (_upper < _lower) {
                _upper = _lower;
            }
            return;
        }
        
        if (_cmp < 0) {
            if(_lower == _cursor && _lower < _upper){
                _lower++;
            }else{
                _lower = _cursor;
            }
            return;
        }
        
        if(_target == SearchTarget.ANY){
            _lower = _cursor;
            _upper = _cursor;
        }else if(_target == SearchTarget.HIGHEST){
            _lower = _cursor;
        }else if(_target == SearchTarget.LOWEST){
            _upper = _cursor;
        }else{
            throw new IllegalStateException("Unknown target");
        }
    }
    
    private final void adjustCursor(){
        int oldCursor = _cursor;
        if(_upper - _lower <= 1){
            if((_target == SearchTarget.LOWEST)  && (_cmp == 0)){
                _cursor = _lower;
            }else{
                _cursor = _upper;
            }
        }else{
            _cursor = _lower + ((_upper - _lower) / 2);
        }
        if(_cursor == oldCursor){
            complete();
        }
    }
    
    public final boolean afterLast(){
        if(_count == 0){
            return false;  // _cursor is 0: not after last
        }
        return (_cursor == _count -1) && _cmp < 0;
    }
    
    public final boolean beforeFirst() {
        return (_cursor == 0) && (_cmp > 0);
    }

    private final void complete(){
        _upper = -2;
    }
    
    public int count(){
        return _count;
    }
    
    public int cursor() {
        return _cursor;
    }

    public boolean foundMatch(){
        return _cmp == 0;
    }
    
    public boolean incomplete() {
        return _upper >= _lower;
    }
    
    public void moveForward() {
        _cursor++;
    }

    public final void resultIs(int cmp){
        _cmp = cmp;
        adjustBounds();
        adjustCursor();
    }

    public boolean isGreater() {
        return _cmp < 0;
    }
    
 
}
