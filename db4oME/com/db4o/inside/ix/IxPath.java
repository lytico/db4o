/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o.inside.ix;

import com.db4o.foundation.*;
import com.db4o.inside.freespace.*;

/**
 * Index Path to represent a list of traversed index tree entries,
 * used by IxTraverser
 */
class IxPath implements ShallowClone,Visitor4 {

    int                 i_comparisonResult;

    int[]               i_lowerAndUpperMatch;
    int                 i_upperNull = -1;

    IxPath              i_next;

    IxTraverser         i_traverser;
    IxTree              i_tree;
    
    Visitor4            _visitor;

    IxPath(IxTraverser a_traverser, IxPath a_next, IxTree a_tree,
        int a_comparisonResult, int[] lowerAndUpperMatch) {
        i_traverser = a_traverser;
        i_next = a_next;
        i_tree = a_tree;
        i_comparisonResult = a_comparisonResult;
        i_lowerAndUpperMatch = lowerAndUpperMatch;
    }
    
    public NIxPathNode convert() {
        NIxPathNode res = new NIxPathNode();
        res._comparisonResult = i_comparisonResult;
        res._lowerAndUpperMatch = i_lowerAndUpperMatch;
        res._tree = i_tree;
        if(i_next != null){
            res._next = i_next.convert();
        }
        return res;
    }

    void add(Visitor4 visitor) {
        if (i_comparisonResult == 0 && i_traverser.i_take[IxTraverser.EQUAL]) {
            i_tree.visit(visitor, i_lowerAndUpperMatch);
        }
    }

    void addPrecedingToCandidatesTree(Visitor4 visitor) {
        _visitor = visitor;
        if (i_tree._preceding != null) {
            if (i_next == null || i_next.i_tree != i_tree._preceding) {
                i_tree._preceding.traverse(this);
            }
        }
        if (i_lowerAndUpperMatch != null) {
            int[] lowerAndUpperMatch = new int[] { i_upperNull,
                i_lowerAndUpperMatch[0] - 1};
            i_tree.visit(visitor, lowerAndUpperMatch);
        } else {
            if (i_comparisonResult < 0) {
                visit(i_tree);
            }
        }
    }

    void addSubsequentToCandidatesTree(Visitor4 visitor) {
        _visitor = visitor;
        if (i_tree._subsequent != null) {
            if (i_next == null || i_next.i_tree != i_tree._subsequent) {
                i_tree._subsequent.traverse(this);
            }
        }
        if (i_lowerAndUpperMatch != null) {
            int[] lowerAndUpperMatch = new int[] { i_lowerAndUpperMatch[1] + 1,
                ((IxFileRange) i_tree)._entries - 1};
            i_tree.visit(visitor, lowerAndUpperMatch);
        } else {
            if (i_comparisonResult > 0) {
                visit(i_tree);
            }
        }
    }

    IxPath append(IxPath a_head, IxPath a_tail) {
        if (a_head == null) {
            return this;
        }
        i_next = a_head;
        return a_tail;
    }

    IxPath append(IxTree a_tree, int a_comparisonResult, int[] lowerAndUpperMatch) {
        i_next = new IxPath(i_traverser, null, a_tree, a_comparisonResult, lowerAndUpperMatch);
        i_next.i_tree = a_tree;
        return i_next;
    }

    boolean carriesTheSame(IxPath a_path) {
        return i_tree == a_path.i_tree;
    }

    private void checkUpperNull() {
        if (i_upperNull == -1) {
            i_upperNull = 0;
            i_traverser.i_handler.prepareComparison(null);
            int res = i_tree.compare(null);
            if(res != 0){
                return;
            }
            int[] nullMatches = i_tree.lowerAndUpperMatch();  
            if (nullMatches[0] == 0) {
                i_upperNull = nullMatches[1] + 1;
            } else {
                i_upperNull = 0; 
            }
        }
    }
    
    public void visitMatch(FreespaceVisitor visitor){
        if(i_next != null){
            i_next.visitMatch(visitor);
        }
        if(visitor.visited()){
            return;
        }
        if (i_comparisonResult != 0){
            return;
        }
        
        if (i_lowerAndUpperMatch == null) {
            i_tree.freespaceVisit(visitor, 0);
            return;
        }
        
        if(i_lowerAndUpperMatch[1] < i_lowerAndUpperMatch[0]){
            return;
        }
        
        int ix = i_lowerAndUpperMatch[0]; 
        if(ix >= 0){
            i_tree.freespaceVisit(visitor, ix);
        }
    }
    
    public void visitPreceding(FreespaceVisitor visitor){
        if(i_next != null){
            i_next.visitPreceding(visitor);
            if(visitor.visited()){
                return;
            }
        }
        if (i_lowerAndUpperMatch != null) {
            int ix = i_lowerAndUpperMatch[0] - 1;
            if(ix >= 0){
                i_tree.freespaceVisit(visitor, ix);
            }
        }else{
            if (i_comparisonResult < 0) {
                i_tree.freespaceVisit(visitor, 0);
            }
        }
        if(visitor.visited()){
            return;
        }
        if(i_tree._preceding != null){
            if (i_next == null || i_next.i_tree != i_tree._preceding) {
                ((IxTree)i_tree._preceding).visitLast(visitor);
            }
        }
    }
    
    public void visitSubsequent(FreespaceVisitor visitor){
        if(i_next != null){
            i_next.visitSubsequent(visitor);
            if(visitor.visited()){
                return;
            }
        }
        if (i_lowerAndUpperMatch != null) {
            int ix = i_lowerAndUpperMatch[1] + 1;
            if(ix < ((IxFileRange) i_tree)._entries){
                i_tree.freespaceVisit(visitor, ix);
            }
        }else{
            if (i_comparisonResult > 0) {
                i_tree.freespaceVisit(visitor, 0);
            }
        }
        if(visitor.visited()){
            return;
        }
        if(i_tree._subsequent != null){
            if (i_next == null || i_next.i_tree != i_tree._subsequent) {
                ((IxTree)i_tree._subsequent).visitFirst(visitor);
            }
        }
    }

    int countMatching() {
        if (i_comparisonResult == 0) {
            if (i_lowerAndUpperMatch == null) {
                if (i_tree instanceof IxRemove) {
                    return 0;
                }
                return 1;
            }
            return i_lowerAndUpperMatch[1] - i_lowerAndUpperMatch[0] + 1;
        }
        return 0;
    }

    int countPreceding(boolean a_takenulls) {
        int preceding = 0;
        if (i_tree._preceding != null) {
            if (i_next == null || i_next.i_tree != i_tree._preceding) {
                preceding += i_tree._preceding.size();
            }
        }
        if (i_lowerAndUpperMatch != null) {
            if(a_takenulls) {
                i_upperNull = 0;
            }else {
                checkUpperNull();
            }
            preceding += i_lowerAndUpperMatch[0] - i_upperNull;
        } else {
            if (i_comparisonResult < 0 && !(i_tree instanceof IxRemove)) {
                preceding++;
            }
        }
        return preceding;
    }

    int countSubsequent() {
        int subsequent = 0;
        if (i_tree._subsequent != null) {
            if (i_next == null || i_next.i_tree != i_tree._subsequent) {
                subsequent += i_tree._subsequent.size();
            }
        }
        if (i_lowerAndUpperMatch != null) {
            subsequent += ((IxFileRange) i_tree)._entries
                - i_lowerAndUpperMatch[1] - 1;
        } else {
            if (i_comparisonResult > 0 && !(i_tree instanceof IxRemove)) {
                subsequent++;
            }
        }
        return subsequent;
    }

    public Object shallowClone() {
    	int[] lowerAndUpperMatch=null;
    	if(i_lowerAndUpperMatch!=null) {
    		lowerAndUpperMatch=new int[]{i_lowerAndUpperMatch[0],i_lowerAndUpperMatch[1]};
    	}
    	IxPath ret=new IxPath(i_traverser,i_next,i_tree,i_comparisonResult,lowerAndUpperMatch);
    	ret.i_upperNull=i_upperNull;
    	ret._visitor=_visitor;
        return ret;
    }

    public String toString() {
        if(! Debug4.prettyToStrings){
            return super.toString();
        }
        return i_tree.toString();
    }

    public void visit(Object a_object) {
        ((Visitor4) a_object).visit(_visitor);
    }

}
