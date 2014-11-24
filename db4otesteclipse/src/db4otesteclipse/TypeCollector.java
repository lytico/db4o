package db4otesteclipse;

import java.util.*;

import org.eclipse.core.runtime.*;
import org.eclipse.jdt.core.*;
import org.eclipse.jface.viewers.*;

public class TypeCollector {
	private TestTypeSpec predicate;
	
	public TypeCollector(TestTypeSpec predicate) {
		this.predicate = predicate;
	}

	public List collectTestTypes(ISelection selection) throws CoreException {
		Set testTypes=new HashSet();
		if(selection instanceof IStructuredSelection) {
			IStructuredSelection structured=(IStructuredSelection)selection;
			for(Iterator iter=structured.iterator();iter.hasNext();) {
				collectTestTypes(iter.next(),testTypes);
			}
		}
		List sorted=new ArrayList(testTypes);
		Collections.sort(sorted,new Comparator() {
			public int compare(Object a, Object b) {
				return ((IMember)a).getElementName().compareTo(((IMember)b).getElementName());
			}
		});
		return sorted;
	}

	private void collectTestTypes(Object selected,Set testTypes) throws CoreException {
		if(selected instanceof IType) {
			addTestType(testTypes,(IType)selected);
			return;
		}
		if(selected instanceof ICompilationUnit) {
			addTestTypes(testTypes, (ICompilationUnit)selected);
			return;
		}
		if(selected instanceof IPackageFragment) {
			addTestTypes(testTypes, (IPackageFragment)selected);
			return;
		}
		if(selected instanceof IMethod) {
			addTestType(testTypes, (IMethod)selected);
			return;
		}
	}

	private void addTestTypes(Set testTypes, IPackageFragment packageFrag) throws JavaModelException, CoreException {
		ICompilationUnit[] cus=packageFrag.getCompilationUnits();
		for (int cuIdx = 0; cuIdx < cus.length; cuIdx++) {
			addTestTypes(testTypes, cus[cuIdx]);
		}
	}

	private void addTestTypes(Set testTypes, ICompilationUnit cu) throws JavaModelException, CoreException {
		IType[] types=cu.getTypes();
		for (int typeIdx = 0; typeIdx < types.length; typeIdx++) {
			addTestType(testTypes, types[typeIdx]);
		}
	}

	private void addTestType(Set testTypes, IType type) throws CoreException {
		if(predicate.acceptTestType(type)) {
			testTypes.add(type);
		}
	}

	private void addTestType(Set testTypes, IMethod method) throws CoreException {
		if(predicate.acceptTestMethod(method)) {
			testTypes.add(method);
		}
	}
}
