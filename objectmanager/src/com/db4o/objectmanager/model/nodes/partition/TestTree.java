/*
 * Copyright (C) 2005 db4objects Inc.  http://www.db4o.com
 */
package com.db4o.objectmanager.model.nodes.partition;

import java.util.*;

import org.eclipse.jface.viewers.*;
import org.eclipse.swt.*;
import org.eclipse.swt.graphics.*;
import org.eclipse.swt.layout.*;
import org.eclipse.swt.widgets.*;

public class TestTree {
    
    private Integer[] _contents;
    private static int[][] treeSpec;
	private int _threshold;
    
    public TestTree(int numitems,int threshold) {
        _contents = new Integer[numitems];
		_threshold=threshold;
        for (int i = 0; i < _contents.length; i++) {
            _contents[i] = new Integer(i);
        }
    }

    private static class Node {
        private Object[] _contents;
		private int[][] _treeSpec;
		private int _level;
		private int _idx;
		private int _threshold;
		private int _start;
		private int _end;
        
        public Node(Object[] contents, int[][] treeSpec,int level, int idx,int threshold) {
            _contents = contents;
            _treeSpec = treeSpec;
            _level = level;
			_idx=idx;
			_threshold=threshold;
        }
        
        public String toString() {
            return "["+_level+","+_idx+"]";
        }
        
        public Object[] getChildren() {
			int[] spec=_treeSpec[_level];			
			int startidx=spec[_idx];
            if(_level==0) {
				int endidx=(_idx<spec.length-1 ? spec[_idx+1] : _contents.length);
				int length=endidx-startidx;
                Object[] children=new Object[length];
                System.arraycopy(_contents, startidx, children, 0, length);
                return children;
            }
			int endidx=(_idx<spec.length-1 ? spec[_idx+1] : _treeSpec[_level-1].length);
			int length=endidx-startidx;
			Object[] children=new Object[length];
			for (int childidx = 0; childidx < children.length; childidx++) {
				children[childidx]=new Node(_contents,_treeSpec,_level-1,startidx+childidx,_threshold);
			}
			return children;
        }

	}

    private IContentProvider contentProvider = new ITreeContentProvider() {

        public void dispose() {
        }

        public void inputChanged(Viewer viewer, Object oldInput, Object newInput) {
        }

        // Get the top level
        public Object[] getElements(Object inputElement) {
			int[][] treeSpec=computeTreeSpec(_contents.length,_threshold);
            return new Object[]{new Node(_contents,treeSpec,treeSpec.length-1,0,_threshold)};
        }

	    public int[][] computeTreeSpec(int numItems, int threshold) {
	        java.util.List structure=new ArrayList();
	        int curnum=numItems;
	        
	        while(curnum>threshold) {
	            int numbuckets=(int)Math.ceil((float)curnum/threshold);
	            int minbucketsize=curnum/numbuckets;
	            int numexceeding=curnum%numbuckets;
	            int[] curlevel=new int[numbuckets];
	            int startidx=0;
				curlevel[0]=0;
	            for (int bucketidx = 1; bucketidx < curlevel.length; bucketidx++) {
	                int curfillsize=minbucketsize;
	                if(bucketidx <= numexceeding) {
	                    curfillsize++;
	                }
					startidx+=curfillsize;
					curlevel[bucketidx] = startidx;
	            }
	            structure.add(curlevel);
	            curnum=numbuckets;
	        }
			structure.add(new int[]{0});
	        return (int[][])structure.toArray(new int[structure.size()][]);    
	    }

		// Get subsequent levels
        public Object[] getChildren(Object parentElement) {
            return ((Node)parentElement).getChildren();
        }

        public Object getParent(Object element) {
            return null;
        }

        public boolean hasChildren(Object element) {
            return (element instanceof Node);
        }
    };
    
    private ILabelProvider labelProvider = new ILabelProvider() {

        public void addListener(ILabelProviderListener listener) {
        }

        public void removeListener(ILabelProviderListener listener) {
        }

        public void dispose() {
            // TODO Auto-generated method stub
            
        }

        public boolean isLabelProperty(Object element, String property) {
            return true;
        }

        public Image getImage(Object element) {
            return null;
        }

        public String getText(Object element) {
            return element.toString();
        }
        
    };

    public void run() {
        Display display = new Display();
        Shell shell = new Shell(display);
        
        shell.setLayout(new FillLayout());
        
        TreeViewer tree = new TreeViewer(shell, SWT.NULL);
        tree.setContentProvider(contentProvider);
        tree.setLabelProvider(labelProvider);
//        tree.setInput(new Node(_contents,0,_contents.length,_threshold));
		tree.setInput(new Object());

        shell.open();
        
        while (!shell.isDisposed()) {
            if (!display.readAndDispatch()) display.sleep();
        }
        display.dispose();
    }
    
    public static void main(String[] args) {
        TestTree test = new TestTree(29,3);
		test.run();
//		int[][] spec=Node.computeTreeSpec(29,3);
//		System.out.println(spec);
    }

}
