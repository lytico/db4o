package com.db4o.omplus.ui.model;

import java.util.ArrayList;
import java.util.regex.Pattern;

import org.eclipse.jface.viewers.ITreeContentProvider;
import org.eclipse.jface.viewers.Viewer;

import com.db4o.omplus.datalayer.*;
import com.db4o.omplus.datalayer.classviewer.ClassTreeBuilder;
import com.db4o.omplus.datalayer.classviewer.ClassTreeNode;
import com.db4o.omplus.ui.ClassViewer;


/**
 * 
 * Content provider for ClassViewer.
 *
 */
public class ClassViewerContentProvider implements ITreeContentProvider 
{
	private final String STAR_STR = "*";
	private final String STAR_PATTERN = ".*";
	private final String REPLACE_STR = "\\";
	private final String DOT_STR = "?";
	private final String DOT_PATTERN = ".";
	
	private StringPattern strPattern;
	private ClassTreeBuilder treeBuilder;
	
	public ClassViewerContentProvider(StringPattern strPattern) {
		this.strPattern = strPattern;
		treeBuilder = new ClassTreeBuilder();
	}

	public Object[] getChildren(Object parentElement) {
		if(parentElement == null)
			return null;
		ClassTreeNode node = (ClassTreeNode)parentElement;
		int nodeType = node.getNodeType();
		String name = node.getName();
		if( nodeType == OMPlusConstants.PACKAGE_NODE){
			ClassTreeNode []classes = treeBuilder.getClassNodesForPackage(name);
			String searchStr = strPattern.getPattern();
			if(classes != null && searchStr != null && (searchStr.trim().length() > 0)) {
				searchStr = replaceStr(searchStr);
				ArrayList<ClassTreeNode> list = getMatchingClasses(searchStr, classes);
				return list.toArray();
			}
			return classes;
		} else if( nodeType == OMPlusConstants.CLASS_NODE)
			return treeBuilder.getFieldNodesForClass(name, name);
		else
			return treeBuilder.getFieldNodesForClass(node.getType(), name);
	}

	public Object getParent(Object element)	{
		return null;
	}

	public boolean hasChildren(Object element)
	{
		if(element != null && (element instanceof ClassTreeNode)) {
			ClassTreeNode node = (ClassTreeNode)element;
			int nodeType = node.getNodeType();
			if( nodeType == OMPlusConstants.PACKAGE_NODE ||
					nodeType == OMPlusConstants.CLASS_NODE )
				return true;
			else if(node.hasChildren()){
				String className = node.getName().split(OMPlusConstants.REGEX)[0];
				if (!className.equals(node.getType()))
					return true;
			}
		}
		return false;
	}

	public Object[] getElements(Object inputElement) {
		ClassTreeNode classes[] = null;
		if(inputElement != null){
			if(inputElement.equals(ClassViewer.HIERARCHICAL_VIEW))
				classes = treeBuilder.getPackageTreeNodes();
			else if(inputElement.equals(ClassViewer.FLAT_VIEW))
				classes = treeBuilder.getClassTreeNodes();
			else
				return classes;
			ArrayList<ClassTreeNode> list = new ArrayList<ClassTreeNode>();;
			String searchStr = strPattern.getPattern();
			if(searchStr != null && classes != null && (searchStr.trim().length() > 0)) {
				searchStr = replaceStr(searchStr);
				if(inputElement.equals(ClassViewer.HIERARCHICAL_VIEW)){
					list = getMacthingPackages(searchStr, classes);
				} else{
					list = getMatchingClasses(searchStr, classes);
				}
				return list.toArray();
			}
		}
		return classes ;
	}

	private String replaceStr(String searchStr) {
		if(searchStr.contains(STAR_STR)) 
			searchStr = searchStr.replaceAll(REPLACE_STR + STAR_STR ,STAR_PATTERN);
		if(searchStr.contains(DOT_STR))
			searchStr = searchStr.replaceAll(REPLACE_STR + DOT_STR ,DOT_PATTERN);
		return searchStr.toLowerCase();
	}

	private ArrayList<ClassTreeNode> getMacthingPackages(String searchStr,
			ClassTreeNode[] packages) {
		ArrayList<ClassTreeNode> list = new ArrayList<ClassTreeNode>();
		Pattern pattern = getPattern(searchStr);
		for(ClassTreeNode pkgNode : packages ) {
			ClassTreeNode [] pkgClasses = treeBuilder.getClassNodesForPackage(pkgNode.getName());
			if(pkgClasses != null && pkgClasses.length > 0){
				for(ClassTreeNode classNode : pkgClasses ) {
					String lower = (classNode.getName()).toLowerCase();
					/*searchStr = searchStr.toLowerCase();
					if(lower.contains(searchStr)){
						list.add(pkgNode);
						break;
					}*/
					if(pattern.matcher(lower).find()){
						list.add(pkgNode);
						break;
					}	
				}
			}
		}
		return list;
	}

	private ArrayList<ClassTreeNode> getMatchingClasses(String searchStr,
													   ClassTreeNode[] classes) {
		ArrayList<ClassTreeNode> list = new ArrayList<ClassTreeNode>();
		Pattern pattern = getPattern(searchStr);
		for(ClassTreeNode classNode : classes ) {
			String lower = (classNode.getName()).toLowerCase();
			/*searchStr = searchStr.toLowerCase();
			if(lower.contains(searchStr))
				list.add(classNode);*/
			if(pattern.matcher(lower).find())
				list.add(classNode);
		}
		return list;
	}

	private Pattern getPattern(String searchStr) {
		return Pattern.compile(searchStr);
	}

	public void dispose() {
	}

	public void inputChanged(Viewer viewer, Object oldInput, Object newInput) {
	}
	
}
