/* Copyright (C) 2004 - 2005  db4objects Inc.  http://www.db4o.com */

package com.db4o.inside.ix;

import com.db4o.*;
import com.db4o.foundation.*;

/**
 * @exclude
 */
public class NIxPathNode {
    
    IxTree              _tree;
    
    int                 _comparisonResult;

    int[]               _lowerAndUpperMatch;

    NIxPathNode         _next;
    

    /**
     * returns 0, if keys are equal
     * uses this - other  
     * returns positive if this is greater than a_to
     * returns negative if this is smaller than a_to
     */
    int compare(NIxPathNode other, int myType, int otherType) {
        if(_next == null){
            
            if(other._next != null){
                return other.ascending() ? -1 : 1; 
            }
            
            if(_lowerAndUpperMatch == null){
                if(Debug.ixTrees){
                    Debug.expect(other._lowerAndUpperMatch == null);
                }
                return myType - otherType;
            }
            
            if(_lowerAndUpperMatch[0] != other._lowerAndUpperMatch[0]){
                
                int res0 = _lowerAndUpperMatch[0] - other._lowerAndUpperMatch[0];
                
                if(res0 == 0){
                    return myType - otherType;
                }
                
                return res0; 
            }
            
            if(_lowerAndUpperMatch[1] != other._lowerAndUpperMatch[1]){
                
                // Will this ever happen?
                
                int res1 = _lowerAndUpperMatch[1] - other._lowerAndUpperMatch[1] ;
                
                if(res1 == 0){
                    return myType - otherType;
                }
                
                return res1; 
            }
            
            return myType - otherType;
        }
        
        if(other._next == null){
            return ascending() ? 1 : -1;
        }
        
        IxTree otherNext = other._next._tree;
        
        if(otherNext == _next._tree){
            return _next.compare(other._next, myType, otherType);
        }
        
        if(_tree._subsequent == otherNext){
            return -1;
        }

        if(Debug.ixTrees){
            Debug.expect(_tree._preceding == otherNext);
        }
        
        return 1;
    }
    
    boolean ascending(){
        return _tree._subsequent == _next._tree;
    }
    
    boolean carriesTheSame(NIxPathNode node) {
        if(node == null){
            return false;
        }
        return _tree == node._tree;
    }
    
    int countPreceding() {
        int preceding = 0;
        if (_tree._preceding != null) {
            if (_next == null || _next._tree != _tree._preceding) {
                preceding += _tree._preceding.size();
            }
        }
        if (_lowerAndUpperMatch != null) {
            preceding += _lowerAndUpperMatch[0] ;
        } else {
            if (_comparisonResult < 0 && !(_tree instanceof IxRemove)) {
                preceding++;
            }
        }
        return preceding;
    }
    
    int countMatching() {
        if (_comparisonResult == 0) {
            if (_lowerAndUpperMatch == null) {
                if (_tree instanceof IxRemove) {
                    return 0;
                }
                return 1;
            }
            return _lowerAndUpperMatch[1] - _lowerAndUpperMatch[0] + 1;
        }
        return 0;
    }
    
    int countSubsequent() {
        int subsequent = 0;
        if (_tree._subsequent != null) {
            if (_next == null || _next._tree != _tree._subsequent) {
                subsequent += _tree._subsequent.size();
            }
        }
        if (_lowerAndUpperMatch != null) {
            subsequent += ((IxFileRange) _tree)._entries
                - _lowerAndUpperMatch[1] - 1;
        } else {
            if (_comparisonResult > 0 && !(_tree instanceof IxRemove)) {
                subsequent++;
            }
        }
        return subsequent;
    }
    
    int countSpan(NIxPath greatPath, NIxPath smallPath, NIxPathNode small) {
        if (_comparisonResult != 0) {
            return 0;
        }
        
        if (_lowerAndUpperMatch == null) {
            if (_tree instanceof IxRemove) {
                return 0;
            }
            
            if(greatPath._takeMatches || smallPath._takeMatches){
                return 1;
            }
            
            return 0;
        }
        
        if(_lowerAndUpperMatch[0] == small._lowerAndUpperMatch[0]){
            
            if(greatPath._takeMatches || smallPath._takeMatches){
                //  We had the same constraint, return the match
                return _lowerAndUpperMatch[1] - _lowerAndUpperMatch[0] + 1;
            }
            return 0;
        }
            
        // We are looking at a range, let's figure out which ones we need
        
        int upper = _lowerAndUpperMatch[0] - 1;
        
        int lower = 0;
        
        if(! smallPath._takePreceding){
            lower = small._lowerAndUpperMatch[0];
        }
        
        if(! smallPath._takeMatches){
            lower = small._lowerAndUpperMatch[1] + 1;
        }
        
        return upper - lower + 1;
            
    }
    
    void traversePreceding(Visitor4Dispatch dispatcher) {
        if (_tree._preceding != null) {
            if (_next == null || _next._tree != _tree._preceding) {
                _tree._preceding.traverse(dispatcher);
            }
        }
        if (_lowerAndUpperMatch != null) {
            int[] lowerAndUpperMatch = new int[] { 0, _lowerAndUpperMatch[0] - 1};
            _tree.visit(dispatcher._target, lowerAndUpperMatch);
            return;
        } 
        if (_comparisonResult < 0 && !(_tree instanceof IxRemove)) {
            _tree.visit(dispatcher._target);
        }
    }
    
    void traverseMatching(Visitor4Dispatch dispatcher) {
        if (_comparisonResult == 0) {
            _tree.visit(dispatcher._target, _lowerAndUpperMatch);
        }
    }
    
    void traverseSubsequent(Visitor4Dispatch dispatcher) {
        if (_tree._subsequent != null) {
            if (_next == null || _next._tree != _tree._subsequent) {
                _tree._subsequent.traverse(dispatcher);
            }
        }
        if (_lowerAndUpperMatch != null) {
            int[] lowerAndUpperMatch = new int[] { _lowerAndUpperMatch[1] + 1,
                ((IxFileRange) _tree)._entries - 1};
            _tree.visit(dispatcher._target, lowerAndUpperMatch);
            return;
        }
        if (_comparisonResult > 0) {
            _tree.visit(dispatcher._target);
        }
    }
    
    void traverseSpan(NIxPath greatPath, NIxPath smallPath, NIxPathNode small, Visitor4Dispatch dispatcher) {
        if (_comparisonResult != 0) {
            return;
        }
        if (_lowerAndUpperMatch == null) {
            if(greatPath._takeMatches || smallPath._takeMatches){
                _tree.visit(dispatcher._target);
                return;
            }
            
        }
            
        if(_lowerAndUpperMatch[0] == small._lowerAndUpperMatch[0]){
            if(greatPath._takeMatches || smallPath._takeMatches){
                _tree.visit(dispatcher._target,_lowerAndUpperMatch);
            }
            return;
        }
            
        // We are looking at a range, let's figure out which ones we need
        
        int upper = _lowerAndUpperMatch[0] - 1;
        
        int lower = 0;
        
        if(! smallPath._takePreceding){
            lower = small._lowerAndUpperMatch[0];
        }
        
        if(! smallPath._takeMatches){
            lower = small._lowerAndUpperMatch[1] + 1;
        }
        
        _tree.visit(dispatcher._target,new int[]{lower, upper});
    }
    
    public String toString() {
        if(! Debug4.prettyToStrings){
            return super.toString();
        }
        return _tree.toString() + "\n cmp: " + _comparisonResult; 
    }

    
}
