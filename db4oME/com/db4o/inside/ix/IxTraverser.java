/* Copyright (C) 2004 - 2005  db4objects Inc.   http://www.db4o.com */

package com.db4o.inside.ix;

import com.db4o.*;
import com.db4o.foundation.*;
import com.db4o.inside.freespace.*;

/**
 * @exclude
 */
public class IxTraverser{
    
    // for debugging purposes, search for _constraint to uncomment 
    // public Object _constraint;

    private IxPath i_appendHead;
    private IxPath i_appendTail;

    private IxPath i_greatHead;
    private IxPath i_greatTail;

    Indexable4 i_handler;

    private IxPath i_smallHead;
    private IxPath i_smallTail;

    // Bitmap that denotes, which elements to take, consisting of four booleans:
    // [0] take nulls 
    // [1] take smaller
    // [2] take equal
    // [3] take greater
    
    boolean[] i_take;
    
    public static final int NULLS = 0;
    public static final int SMALLER = 1;
    public static final int EQUAL = 2;
    public static final int GREATER = 3;
    
    private void add(Visitor4 visitor, IxPath a_previousPath, IxPath a_great, IxPath a_small) {
        addPathTree(visitor, a_previousPath);
        if (a_great != null && a_small != null && a_great.carriesTheSame(a_small)) {
            add(visitor, a_great, a_great.i_next, a_small.i_next);
            return;
        }
        addGreater(visitor, a_small);
        addSmaller(visitor, a_great);
    }

    private void addAll(Visitor4 visitor, Tree a_tree){
        if(a_tree != null){
            ((IxTree)a_tree).visit(visitor, null);
            addAll(visitor, a_tree._preceding);
            addAll(visitor, a_tree._subsequent);
        }
    }

    private void addGreater(Visitor4 visitor, IxPath a_path) {
        if (a_path != null) {
            if (a_path.i_next == null) {
                addSubsequent(visitor, a_path);
            } else {
                if (a_path.i_next.i_tree == a_path.i_tree._preceding) {
                    addSubsequent(visitor, a_path);
                } else {
                    addPathTree(visitor, a_path);
                }
                addGreater(visitor, a_path.i_next);
            }
        }
    }

    private void addPathTree(Visitor4 visitor, IxPath a_path) {
        if (a_path != null) {
            a_path.add(visitor);
        }
    }

    private void addPreceding(Visitor4 visitor, IxPath a_path) {
        addPathTree(visitor, a_path);
        addAll(visitor, a_path.i_tree._preceding);
    }

    private void addSmaller(Visitor4 visitor, IxPath a_path) {
        if (a_path != null) {
            if (a_path.i_next == null) {
                addPreceding(visitor, a_path);
            } else {
                if (a_path.i_next.i_tree == a_path.i_tree._subsequent) {
                    addPreceding(visitor, a_path);
                } else {
                    addPathTree(visitor, a_path);
                }
                addSmaller(visitor, a_path.i_next);
            }
        }
    }

    private void addSubsequent(Visitor4 visitor, IxPath a_path) {
        addPathTree(visitor, a_path);
        addAll(visitor, a_path.i_tree._subsequent);
    }
    
    private NIxPath createNIxPath(NIxPathNode head, boolean takePreceding, boolean takeMatches, boolean takeSubsequent, int pathType){
        NIxPath np = new NIxPath(head, takePreceding, takeMatches, takeSubsequent, pathType );
        // for debugging
        // np._constraint = _constraint;
        return np;
    }
    
    public NIxPaths convert(){
        NIxPaths res = new NIxPaths();
        if(i_take[NULLS] || i_take[SMALLER] || i_take[EQUAL]){
            NIxPath smaller = createNIxPath(i_smallHead.convert(), i_take[SMALLER], i_take[EQUAL], i_take[GREATER], SMALLER); 
            res.add(smaller);
        }
        if(i_take[EQUAL] || i_take[GREATER]){
            NIxPath greater = createNIxPath(i_greatHead.convert(), i_take[SMALLER],i_take[EQUAL], i_take[GREATER], GREATER);
            res.add(greater);
        }
        if(i_take[SMALLER] || i_take[NULLS]){
            if(i_smallHead != null){
                if( i_smallHead.i_tree.index()._nullHandling){ 
                    IxPath nullPath = findNullPath();
                    NIxPath np = createNIxPath(nullPath.convert(), i_take[NULLS], i_take[NULLS], i_take[SMALLER], NULLS);
                    res.add(np);
                }
            }
        }
        return res;
    }
    
    private int countGreater(IxPath a_path, int a_sum) {
        if (a_path.i_next == null) {
            return a_sum + countSubsequent(a_path);
        } else {
            if (a_path.i_next.i_tree == a_path.i_tree._preceding) {
                a_sum += countSubsequent(a_path);
            } else {
                a_sum += a_path.countMatching();
            }
            return countGreater(a_path.i_next, a_sum);
        }
    }
    
    private int countPreceding(IxPath a_path) {
        return Tree.size(a_path.i_tree._preceding) + a_path.countMatching();
    }
    
    private int countSmaller(IxPath a_path, int a_sum) {
        if (a_path.i_next == null) {
            return a_sum + countPreceding(a_path);
        } else {
            if (a_path.i_next.i_tree == a_path.i_tree._subsequent) {
                a_sum += countPreceding(a_path);
            } else {
                a_sum += a_path.countMatching();
            }
            return countSmaller(a_path.i_next, a_sum);
        }
    }

    private int countSpan(IxPath a_previousPath, IxPath a_great, IxPath a_small) {
        if (a_great == null) {
            if (a_small == null) {
                return a_previousPath.countMatching();
            } else {
                return countGreater(a_small, a_previousPath.countMatching());
            }
        } else if (a_small == null) {
            return countSmaller(a_great, a_previousPath.countMatching());
        }
        if (a_great.carriesTheSame(a_small)) {
            return countSpan(a_great, a_great.i_next, a_small.i_next);
        }
        return a_previousPath.countMatching() + countGreater(a_small, 0) + countSmaller(a_great, 0);
    }

    private int countSubsequent(IxPath a_path) {
        return Tree.size(a_path.i_tree._subsequent) + a_path.countMatching();
    }

    private void delayedAppend(IxTree a_tree, int a_comparisonResult, int[] lowerAndUpperMatch) {
        if (i_appendHead == null) {
            i_appendHead = new IxPath(this, null, a_tree, a_comparisonResult, lowerAndUpperMatch);
            i_appendTail = i_appendHead;
        } else {
            i_appendTail = i_appendTail.append(a_tree, a_comparisonResult, lowerAndUpperMatch);
        }
    }

    private void findBoth() {
        if (i_greatTail.i_comparisonResult == 0) {
            findSmallestEqualFromEqual((IxTree)i_greatTail.i_tree._preceding);
            resetDelayedAppend();
            findGreatestEqualFromEqual((IxTree)i_greatTail.i_tree._subsequent);
        } else if (i_greatTail.i_comparisonResult < 0) {
            findBoth1((IxTree)i_greatTail.i_tree._subsequent);
        } else {
            findBoth1((IxTree)i_greatTail.i_tree._preceding);
        }
    }

    private void findBoth1(IxTree a_tree) {
        if (a_tree != null) {
            int res = a_tree.compare(null);
            int[] lowerAndUpperMatch = a_tree.lowerAndUpperMatch();
            i_greatTail = i_greatTail.append(a_tree, res, lowerAndUpperMatch);
            i_smallTail = i_smallTail.append(a_tree, res, lowerAndUpperMatch);
            findBoth();
        }
    }
    
    private IxPath findNullPath(){
        resetDelayedAppend();
        i_handler.prepareComparison(null);
        IxTree tree = i_greatHead.i_tree;
        int res = tree.compare(null);
        IxPath nullHead = new IxPath(this, null, tree, res, tree.lowerAndUpperMatch());
        IxPath[] headTail = new IxPath[]{nullHead, nullHead};
        findNullPath1(headTail);
        return headTail[0];
    }
    
    private void findNullPath1(IxPath[] headTail) {
        if(headTail[1].i_comparisonResult == 0){
            findGreatestNullFromNull(headTail, (IxTree)headTail[1].i_tree._subsequent);
        } else if (headTail[1].i_comparisonResult < 0) {
            findNullPath2(headTail, (IxTree)headTail[1].i_tree._subsequent);
        } else {
            findNullPath2(headTail, (IxTree)headTail[1].i_tree._preceding);
        }
    }
    
    private void findNullPath2(IxPath[] headTail, IxTree tree) {
        if (tree != null) {
            int res = tree.compare(null);
            headTail[1] = headTail[1].append(tree, res, tree.lowerAndUpperMatch());
            findNullPath1(headTail);
        }
    }
    
    private void findGreatestNullFromNull(IxPath[] headTail, IxTree tree) {
        if (tree != null) {
            int res = tree.compare(null);
            delayedAppend(tree, res, tree.lowerAndUpperMatch());
            if (res == 0) {
                headTail[1] = headTail[1].append(i_appendHead, i_appendTail);
                resetDelayedAppend();
            }
            if (res > 0) {
                findGreatestNullFromNull(headTail, (IxTree)tree._preceding);
            } else {
                findGreatestNullFromNull(headTail, (IxTree)tree._subsequent);
            }
        }
    }
    

    public int findBounds(Object a_constraint, IxTree a_tree) {

        if (a_tree != null) {
            
            
            // for debugging
            // _constraint = a_constraint;
            

            i_handler = a_tree.handler();
            i_handler.prepareComparison(a_constraint);

            // TODO: Only use small or big path where necessary.

            int res = a_tree.compare(null);
            
            i_greatHead = new IxPath(this, null, a_tree, res, a_tree.lowerAndUpperMatch());
            i_greatTail = i_greatHead;
            
            i_smallHead = (IxPath)i_greatHead.shallowClone();
            i_smallTail = i_smallHead;

            findBoth();

            int span = 0;

            if (i_take[EQUAL]) {
                span += countSpan(i_greatHead, i_greatHead.i_next, i_smallHead.i_next);
            }
            if (i_take[SMALLER]) {
                IxPath head = i_smallHead;
                while (head != null) {
                    span += head.countPreceding(i_take[NULLS]);
                    head = head.i_next;
                }
            }
            if (i_take[GREATER]) {
                IxPath head = i_greatHead;
                while (head != null) {
                    span += head.countSubsequent();
                    head = head.i_next;
                }
            }
            return span;
        }
        return 0;
    }

    public int findBoundsExactMatch(Object a_constraint, IxTree a_tree){
        i_take = new boolean[] { false, false, false, false};
        i_take[EQUAL] = true;
        return findBounds(a_constraint, a_tree);
    }
    
    public int findBoundsQuery(QCon a_qcon, Object constraint) {
        if (!a_qcon.i_evaluator.supportsIndex()) {
            return -1;
        }
        i_take = new boolean[] { false, false, false, false};
        a_qcon.i_evaluator.indexBitMap(i_take);
        return findBounds(constraint, a_qcon.indexRoot());
    }
    
    private void findGreatestEqual(IxTree a_tree) {
        int res = a_tree.compare(null);
        i_greatTail = i_greatTail.append(a_tree, res, a_tree.lowerAndUpperMatch());
        if (res == 0) {
            findGreatestEqualFromEqual(a_tree);
        } else if (res < 0) {
            if (a_tree._subsequent != null) {
                findGreatestEqual((IxTree)a_tree._subsequent);
            }
        } else {
            if (a_tree._preceding != null) {
                findGreatestEqual((IxTree)a_tree._preceding);
            }
        }
    }
    
    private void findGreatestEqualFromEqual(IxTree a_tree) {
        if (a_tree != null) {
            int res = a_tree.compare(null);
            delayedAppend(a_tree, res, a_tree.lowerAndUpperMatch());
            if (res == 0) {
                i_greatTail = i_greatTail.append(i_appendHead, i_appendTail);
                resetDelayedAppend();
            }
            if (res > 0) {
                findGreatestEqualFromEqual((IxTree)a_tree._preceding);
            } else {
                findGreatestEqualFromEqual((IxTree)a_tree._subsequent);
            }
        }
    }
    
    private void findSmallestEqual(IxTree a_tree) {
        int res = a_tree.compare(null);
        i_smallTail = i_smallTail.append(a_tree, res, a_tree.lowerAndUpperMatch());
        if (res == 0) {
            findSmallestEqualFromEqual(a_tree);
        } else if (res < 0) {
            if (a_tree._subsequent != null) {
                findSmallestEqual((IxTree)a_tree._subsequent);
            }
        } else {
            if (a_tree._preceding != null) {
                findSmallestEqual((IxTree)a_tree._preceding);
            }
        }
    }
    
    private void findSmallestEqualFromEqual(IxTree a_tree) {
        if (a_tree != null) {
            int res = a_tree.compare(null);
            delayedAppend(a_tree, res, a_tree.lowerAndUpperMatch());
            if (res == 0) {
                i_smallTail = i_smallTail.append(i_appendHead, i_appendTail);
                resetDelayedAppend();
            }
            if (res < 0) {
                findSmallestEqualFromEqual((IxTree)a_tree._subsequent);
            } else {
                findSmallestEqualFromEqual((IxTree)a_tree._preceding);
            }
        }
    }
    
    private void resetDelayedAppend() {
        i_appendHead = null;
        i_appendTail = null;
    }
    
    public void visitAll(Visitor4 visitor) {
        if (i_take[EQUAL]) {
            if (i_greatHead != null) {
                add(visitor, i_greatHead, i_greatHead.i_next, i_smallHead.i_next);
            }
        }
        if (i_take[SMALLER]) {
            IxPath head = i_smallHead;
            while (head != null) {
                head.addPrecedingToCandidatesTree(visitor);
                head = head.i_next;
            }
        }
        if (i_take[GREATER]) {
            IxPath head = i_greatHead;
            while (head != null) {
                head.addSubsequentToCandidatesTree(visitor);
                head = head.i_next;
            }
        }
    }
    
    public void visitPreceding(FreespaceVisitor visitor){
        if(i_smallHead != null){
            i_smallHead.visitPreceding(visitor);    
        }
    }
    
    public void visitSubsequent(FreespaceVisitor visitor){
        if(i_greatHead != null){
            i_greatHead.visitSubsequent(visitor);
        }
    }
    
    public void visitMatch(FreespaceVisitor visitor){
        if(i_smallHead != null){
            i_smallHead.visitMatch(visitor);    
        }
        
    }

}
