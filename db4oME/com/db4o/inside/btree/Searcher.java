/* Copyright (C) 2004 - 2006  db4objects Inc.  http://www.db4o.com */

package com.db4o.inside.btree;


/**
 * @exclude
 */
public class Searcher {
    
    public int _lower;
    
    public int _upper;
    
    public int _cursor;
    
    public int _cmp;
    
    private int _for;
    
    private static final int ANY = 0;
    
    private static final int HIGHEST = 1;
    
    private static final int LOWEST = -1;
    
    
    public Searcher(int count){
        start(count);
    }
    
    public void start(int count){
        _lower = 0;
        _upper = count - 1;
        _cursor = -1;
    }
    
    public boolean incomplete() {
        if (_upper < _lower) {
            return false;
        }
        int oldCursor = _cursor;
        _cursor = _lower + ((_upper - _lower) / 2);
        if (_cursor == oldCursor && _cursor == _lower && _lower < _upper) {
            _cursor ++;
        }
        return _cursor != oldCursor;
    }
    
    void resultIs(int cmp){
        
        _cmp = cmp;
        
        if(cmp > 0){
            _upper = _cursor - 1;
            if (_upper < _lower) {
                _upper = _lower;
            }
            return;
        }
        
        if (cmp < 0) {
            _lower = _cursor + 1;
            if (_lower > _upper) {
                _lower = _upper;
            }
            return;
        }
        
        if(_for == ANY){
            _lower = _cursor;
            _upper = _cursor;
            return;
        }
        
        if(_for == HIGHEST){
            _lower = _cursor;
            return;
        }

        // _for must be LOWEST here
        _upper = _cursor;
        
    }
    
    void highest(){
        _for = HIGHEST;
    }
    
    void lowest(){
        _for = LOWEST;
    }

}
