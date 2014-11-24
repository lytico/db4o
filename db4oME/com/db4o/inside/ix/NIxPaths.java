/* Copyright (C) 2004 - 2005  db4objects Inc.  http://www.db4o.com */

package com.db4o.inside.ix;

import com.db4o.*;
import com.db4o.foundation.*;

/**
 * 
 * A note on the logic of #count() and #traverse():
 * 
 * Within the visitor we are always looking at two NIxPath: last[0] and current.
 * Each run of the visitor takes care of all nodes:
 * - smaller than last[0] for the first run only 
 * - equal to last[0]
 * - between last[0] and current
 * - but *NOT* equal to current, which is handled in the next run. 
 * 
 * @exclude
 */
public class NIxPaths {
    
    Tree _paths;
    
    void add(NIxPath path){
        path._size = 1;
        path._preceding = null;
        path._subsequent = null;
        _paths = Tree.add(_paths, path);
    }
    
    void removeRedundancies(){
        
        // FIXME: removeRedundancies not in function yet
        // This is written very simple for ANDs only first
        // and worst of all it doesn't work yet.
        
        final Collection4 add = new Collection4();
        final boolean[] stop = new boolean[]{false};
        
        _paths.traverse(new Visitor4() {
            public void visit(Object a_object) {
                if(! stop[0]){
                    NIxPath path = (NIxPath)a_object;
                    if(! path._takePreceding){
                        add.clear();
                    }
                    add.add(path);
                    if(! path._takeSubsequent){
                        stop[0] = true;
                    }
                }
            }
        });
        
        _paths = null;
        Iterator4 i = add.iterator();
        while(i.hasNext()){
            this.add((NIxPath)i.next());
        }
    }
    
    int count(){
        final NIxPath[] last = new NIxPath[] {null};
        final int[] sum = new int[] { 0 };
        _paths.traverse(new Visitor4() {
            public void visit(Object a_object) {
                NIxPath current = (NIxPath)a_object;
                if(last[0] == null){
                    if(current._takePreceding){
                        sum[0] += countAllPreceding(current._head);
                    }
                }else{
                    if(    (last[0]._takeSubsequent || last[0]._takeMatches) 
                        && (current._takePreceding || current._takeMatches) ){
                        sum[0] += countSpan(current, last[0], current._head, last[0]._head, current._head._next, last[0]._head._next,0);
                    } else if(last[0]._takeMatches){
                        sum[0] += countAllMatching(last[0]._head);
                    }
                }
                last[0] = current;
            }
        });
        if(last[0]._takeMatches){
            sum[0] += countAllMatching(last[0]._head);
        }
        if(last[0]._takeSubsequent){
            sum[0] += countAllSubsequent(last[0]._head);
        }
        return sum[0];
    }
    
    private int countAllPreceding(NIxPathNode head){
        int count = 0;
        while (head != null) {
            count += head.countPreceding();
            head = head._next;
        }
        return count;
    }
    
    private int countAllMatching(NIxPathNode head){
        int count = 0;
        while (head != null) {
            count += head.countMatching();
            head = head._next;
        }
        return count;
    }
    
    private int countAllSubsequent(NIxPathNode head){
        int count = 0;
        while (head != null) {
            count += head.countSubsequent();
            head = head._next;
        }
        return count;
    }
    
    /** see documentation to this class for behaviour **/
    private int countSpan(NIxPath greatPath, NIxPath smallPath, NIxPathNode a_previousGreat, NIxPathNode a_previousSmall,  NIxPathNode a_great, NIxPathNode a_small, int sum) {
        sum +=  a_previousGreat.countSpan(greatPath, smallPath, a_previousSmall);
        if(a_great != null && a_great.carriesTheSame(a_small)){
            return countSpan(greatPath, smallPath, a_great, a_small, a_great._next, a_small._next, sum);
        }
        return sum + countGreater(a_small, 0) + countSmaller(a_great, 0);
    }
    
    private int countSmaller(NIxPathNode a_path, int a_sum) {
        if(a_path == null){
            return a_sum;
        }
        if (a_path._next == null) {
            return a_sum + countPreceding(a_path);
        } 
        if (a_path._next._tree == a_path._tree._subsequent) {
            a_sum += countPreceding(a_path);
        } else {
            a_sum += a_path.countMatching();
        }
        return countSmaller(a_path._next, a_sum);
    }

    private int countGreater(NIxPathNode a_path, int a_sum) {
        if(a_path == null){
            return a_sum;
        }
        if (a_path._next == null) {
            return a_sum + countSubsequent(a_path);
        } 
        if (a_path._next._tree == a_path._tree._preceding) {
            a_sum += countSubsequent(a_path);
        } else {
            a_sum += a_path.countMatching();
        }
        return countGreater(a_path._next, a_sum);
    }
    
    private int countPreceding(NIxPathNode a_path) {
        return Tree.size(a_path._tree._preceding) + a_path.countMatching();
    }
    
    private int countSubsequent(NIxPathNode a_path) {
        return Tree.size(a_path._tree._subsequent) + a_path.countMatching();
    }

    
    void traverse(Visitor4 visitor) {
        final NIxPath[] last = new NIxPath[] {null};
        
        final Visitor4Dispatch dispatcher = new Visitor4Dispatch(visitor);
        
        _paths.traverse(new Visitor4() {
            public void visit(Object a_object) {
                NIxPath current = (NIxPath)a_object;
                if(last[0] == null){
                    if(current._takePreceding){
                        traverseAllPreceding(current._head, dispatcher);
                    }
                }else{
                    if(    (last[0]._takeSubsequent || last[0]._takeMatches) 
                        && (current._takePreceding || current._takeMatches) ){
                        traverseSpan(current, last[0], current._head, last[0]._head, current._head._next, last[0]._head._next, dispatcher);
                    } else if(last[0]._takeMatches){
                        traverseAllMatching(last[0]._head, dispatcher);
                    }
                }
                last[0] = current;
            }
        });
        if(last[0]._takeMatches){
            traverseAllMatching(last[0]._head, dispatcher);
        }
        if(last[0]._takeSubsequent){
            traverseAllSubsequent(last[0]._head, dispatcher);
        }
    }
    
    private void traverseAllPreceding(NIxPathNode head, Visitor4Dispatch dispatcher){
        while (head != null) {
            head.traversePreceding(dispatcher);
            head = head._next;
        }
    }
    
    private void traverseAllMatching(NIxPathNode head, Visitor4Dispatch dispatcher){
        while (head != null) {
            head.traverseMatching(dispatcher);
            head = head._next;
        }
    }
    
    private void traverseAllSubsequent(NIxPathNode head, Visitor4Dispatch dispatcher){
        while (head != null) {
            head.traverseSubsequent(dispatcher);
            head = head._next;
        }
    }
    
    /** see documentation to this class for behaviour **/
    private void traverseSpan(NIxPath greatPath, NIxPath smallPath, NIxPathNode a_previousGreat, NIxPathNode a_previousSmall,  NIxPathNode a_great, NIxPathNode a_small, Visitor4Dispatch dispatcher) {
        a_previousGreat.traverseSpan(greatPath, smallPath, a_previousSmall, dispatcher);
        if(a_great != null && a_great.carriesTheSame(a_small)){
            traverseSpan(greatPath, smallPath, a_great, a_small, a_great._next, a_small._next, dispatcher);
            return;
        }
        traverseGreater(a_small, dispatcher);
        traverseSmaller(a_great, dispatcher);
    }
    
    private void traverseSmaller(NIxPathNode a_path, Visitor4Dispatch dispatcher) {
        if(a_path == null){
            return;
        }
        if (a_path._next == null) {
            traversePreceding(a_path, dispatcher);
            return; 
        } 
        if (a_path._next._tree == a_path._tree._subsequent) {
            traversePreceding(a_path, dispatcher);
        } else {
            a_path.traverseMatching(dispatcher);
        }
        traverseSmaller(a_path._next, dispatcher);
    }

    private void traverseGreater(NIxPathNode a_path, Visitor4Dispatch dispatcher) {
        if(a_path == null){
            return;
        }
        if (a_path._next == null) {
            traverseSubsequent(a_path, dispatcher);
            return;
        } 
        if (a_path._next._tree == a_path._tree._preceding) {
            traverseSubsequent(a_path, dispatcher);
        } else {
            a_path.traverseMatching(dispatcher);
        }
        traverseGreater(a_path._next, dispatcher);
    }
    
    private void traversePreceding(NIxPathNode a_path, Visitor4Dispatch dispatcher) {
        a_path.traverseMatching(dispatcher);
        Tree.traverse(a_path._tree._preceding, dispatcher);
    }
    
    private void traverseSubsequent(NIxPathNode a_path, Visitor4Dispatch dispatcher) {
        a_path.traverseMatching(dispatcher);
        Tree.traverse(a_path._tree._subsequent, dispatcher);
    }
}
