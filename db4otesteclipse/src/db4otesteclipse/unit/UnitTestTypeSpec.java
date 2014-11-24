package db4otesteclipse.unit;

import java.util.*;

import org.eclipse.core.runtime.*;
import org.eclipse.debug.core.*;
import org.eclipse.jdt.core.*;
import org.eclipse.jdt.launching.*;

import db4otesteclipse.*;

public class UnitTestTypeSpec implements TestTypeSpec {
	private static final String PLAIN_TESTLAUNCHER_NAME = "db4ounit.UnitTestMain";
	private static final String DB4O_TESTLAUNCHER_NAME = "db4ounit.extensions.Db4oUnitTestMain";
	private static final String PLAIN_TESTINTERFACE_NAME = "db4ounit.TestCase";
	private static final String DB4O_TESTINTERFACE_NAME = "db4ounit.extensions.Db4oTestCase";
	
	public boolean acceptTestType(IType type) throws JavaModelException {
		if (!type.exists() || Flags.isAbstract(type.getFlags())) {
			Activator.log("Not existent or abstract: "
					+ type.getFullyQualifiedName());
			return false;
		}
		return implementsTest(type, PLAIN_TESTINTERFACE_NAME);
	}

	private boolean implementsTest(IMember member, String typeName) throws JavaModelException {
		IType type=null;
		if(member instanceof IMethod) {
			type=((IMethod)member).getDeclaringType();
		}
		else {
			type=(IType)member;
		}
		ITypeHierarchy hierarchy=type.newSupertypeHierarchy(null);
		IType[] interfaces=hierarchy.getAllInterfaces();
		for (int idx = 0; idx < interfaces.length; idx++) {
			if(interfaces[idx].getFullyQualifiedName().equals(typeName)) {
				return true;
			}
		}
		return false;
	}

	public void configureSpecific(ILaunchConfigurationWorkingCopy workingCopy,
			List testTypes, String typeArgsList) throws CoreException {
		String testLauncherName = (needsDb4oExtensions(testTypes) ? DB4O_TESTLAUNCHER_NAME : PLAIN_TESTLAUNCHER_NAME);
		workingCopy.setAttribute(
				IJavaLaunchConfigurationConstants.ATTR_MAIN_TYPE_NAME,
				testLauncherName);
		workingCopy.setAttribute(
				IJavaLaunchConfigurationConstants.ATTR_PROGRAM_ARGUMENTS, typeArgsList);
		String vmArgs = workingCopy.getAttribute(IJavaLaunchConfigurationConstants.ATTR_VM_ARGUMENTS, "");
		String tmpDirConfig = "-Ddb4ounit.file.path=" + System.getProperty("java.io.tmpdir");
		workingCopy.setAttribute(
				IJavaLaunchConfigurationConstants.ATTR_VM_ARGUMENTS, (vmArgs.length() > 0) ? tmpDirConfig : vmArgs + " " + tmpDirConfig);
	}

	private boolean needsDb4oExtensions(List testTypes) throws JavaModelException {
		for (Iterator typeIter = testTypes.iterator(); typeIter.hasNext();) {
			IMember type = (IMember) typeIter.next();
			if(implementsTest(type,DB4O_TESTINTERFACE_NAME)) {
				return true;
			}
		}
		return false;
	}

	public boolean acceptTestMethod(IMethod method) throws CoreException {
		return acceptTestType(method.getDeclaringType())&&method.getElementName().startsWith("test");
	}
}
