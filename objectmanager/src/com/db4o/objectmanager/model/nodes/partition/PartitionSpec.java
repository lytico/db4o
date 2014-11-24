/*
 * Copyright (C) 2005 db4objects Inc.  http://www.db4o.com
 */
package com.db4o.objectmanager.model.nodes.partition;

import java.util.LinkedList;

/**
 * PartitionSpec.  Represents a tree of nodes that allow the user to drill down
 * into a large array of elements where an given tree node may have no more 
 * than a specified number of child elements.
 * <p>
 * The computed tree is a perfectly balanced tree, meaning that the user has 
 * the fewest possible number of clicks to access any given tree element, 
 * given the specified array size and partitioning threshold.
 */
public class PartitionSpec {
    
    private int _numItems;
    private int _threshold;
    
    private int[][] _treeLayout;
    
    /**
     * Construct a PartitionSpec tree for an array of the specified number of
     * items, using the specified threshold for the maximum number of children
     * per node in the tree.
     * 
     * @param numItems Total number of elements in the array to partition
     * @param threshold The maximum number of children in any one tree node
     */
    public PartitionSpec(int numItems, int threshold) {
        _numItems = numItems;
        _threshold = threshold;
        _treeLayout = computeTreePartition(numItems, threshold);
    }
    
    /**
     * Return the initial array position covered by the specified node
     * 
     * @param nodeDepth The node's depth
     * @param nodePosition The node's position relative to all nodes at its depth
     * @return The index of the initial array element covered by the specified node.
     */
    public int getBeginPartition(int nodeDepth, int nodePosition) {
        int result = 0;
        for (int accumulatedPos = 0; accumulatedPos < nodeDepth; ++accumulatedPos) {
            result += _treeLayout[nodeDepth][accumulatedPos];
        }
        return result;
    }
    
    /**
     * Return the last array position covered by the specified node
     * 
     * @param nodeDepth The node's depth
     * @param nodePosition The node's position relative to all nodes at its depth
     * @return The index of the last array element covered by the specified node.
     */
    public int getEndPartition(int nodeDepth, int nodePosition) {
        return getBeginPartition(nodeDepth, nodePosition) + 
            _treeLayout[nodeDepth][nodePosition];
    }
    
    /**
     * Returns the number of child nodes required by the specified node
     * 
     * @param nodeDepth The node's depth
     * @param nodePosition The node's position relative to all nodes at its depth
     * @return The number of child nodes required by the specified node
     */
    public int getNumChildren(int nodeDepth, int nodePosition) {
        int childCoverage = _treeLayout[nodeDepth][nodePosition];
        int foundCoverage = 0;
        int numChildren = 0;
        int currentChild = 0;
        
        while (foundCoverage < childCoverage) {
            foundCoverage += _treeLayout[nodeDepth+1][currentChild];
            ++currentChild;
            ++numChildren;
        }
        
        return numChildren;
    }
    
    /**
     * Returns the position of the initial child node relative to all children
     * at the depth one higher than the specified node.
     * 
     * @param nodeDepth The node's depth
     * @param nodePosition The node's position relative to all nodes at its depth
     * @return The position of the initial child node relative to all children in
     * the tree at nodeDepth+1
     */
    public int getStartingChildPosition(int nodeDepth, int nodePosition) {
        int beginPartition = getBeginPartition(nodeDepth, nodePosition);
        int numChildrenCovered = 0;
        int nextRow = nodeDepth+1;
        
        int resultPosition = 0;
        while (numChildrenCovered < beginPartition) {
            numChildrenCovered += _treeLayout[nextRow][resultPosition];
            ++resultPosition;
        }
        
        return resultPosition;
    }
    
    /**
     * Return true if children of the specified node depth are also
     * partition nodes.
     * 
     * @param nodeDepth The specified node depth
     * @return true if nodeDepth's children are also partition nodes; false if they
     * are leaf nodes (are actual array elements)
     */
    public boolean hasPartitionChildren(int nodeDepth) {
        return nodeDepth < _treeLayout.length;
    }
    
    /**
     * Returns the number of nodes in the root (0th) level of the tree.
     * @return The number of nodes at depth 0
     */
    public int getNumRootNodes() {
        return _treeLayout[0].length;
    }
    
    /*
     * This method will partition the specified number of elements
     * into a balanced tree represented according to the following
     * example where each node in the tree represents the number
     * of children in the original array covered by that node in
     * the tree:
     * 
     * threshold = 3
     * 29 elements:
     *
     * xxx xxx xxx xxx xxx xxx xxx xxx xxx xx
     * ----------- ----------- ------- ------
     * ----------------------- --------------
     *
     * Results:
     * 
     * new int[][] {
     *  {18, 11}
     *  {9, 9, 6, 5},
     *  {3, 3, 3, 3, 3, 3, 3, 3, 3, 2},
     * }
     * 
     *
     */
    private int[][] computeTreePartition(int numItems, int threshold) {
        LinkedList structure=new LinkedList();
        int curnum=numItems;
        int[] lastlevel = null;
        
        while(curnum>threshold) {
            int numbuckets=(int)Math.round((float)curnum/threshold+0.5);
            int minbucketsize=curnum/numbuckets;
            int numexceeding=curnum%numbuckets;
            int[] curlevel=new int[numbuckets];
            int startidx=0;
            for (int bucketidx = 0; bucketidx < curlevel.length; bucketidx++) {
                int curfillsize=minbucketsize;
                if(bucketidx < numexceeding) {
                    curfillsize++;
                }
                if (lastlevel == null) {
                    curlevel[bucketidx] = curfillsize;
                } else {
                    for(int lastidx=startidx;lastidx<startidx+curfillsize;lastidx++) {
                        curlevel[bucketidx]+=lastlevel[lastidx];
                    }
                    startidx+=curfillsize;
                }
            }
            structure.addFirst(curlevel);
            lastlevel = curlevel;
            curnum=numbuckets;
        }
        return (int[][])structure.toArray(new int[structure.size()][]);    
    }
    
    public static void main(String[] args) {
        PartitionSpec test = new PartitionSpec(29, 3);
    }

}
