package com.db4o.eclipse.test.util;

import org.eclipse.core.resources.*;
import org.eclipse.jdt.core.*;

import java.util.*;

public class JavaModelUtility {

	public static void collectCompilationUnits(List<ICompilationUnit> result, IPackageFragmentRoot root) throws JavaModelException {
		IJavaElement[] elements = root.getChildren();
		for (int j = 0; j < elements.length; ++j) {
			IPackageFragment p = (IPackageFragment)elements[j];
			result.addAll(Arrays.asList(p.getCompilationUnits()));
		}
	}
	
	public static List<ICompilationUnit> collectCompilationUnits(IJavaProject project) throws JavaModelException {
		
		List<ICompilationUnit> result = new ArrayList<ICompilationUnit>();
		
		IPackageFragmentRoot[] roots = project.getAllPackageFragmentRoots();
		for (int i = 0; i < roots.length; ++i) {
			IPackageFragmentRoot root = roots[i];
			if (IPackageFragmentRoot.K_SOURCE == root.getKind()) {
				collectCompilationUnits(result, root);
			}
		}
		
		return result;
	}
	
	public static List<ICompilationUnit> collectCompilationUnits(IPackageFragmentRoot root) throws JavaModelException {
		List<ICompilationUnit> result = new ArrayList<ICompilationUnit>();
		collectCompilationUnits(result, root);
		return result;
	}

	public static IWorkspaceRoot workspaceRoot() {
		return ResourcesPlugin.getWorkspace().getRoot();
	}
}
