package db4otesteclipse.regression;

import java.util.*;

import org.eclipse.core.runtime.*;
import org.eclipse.debug.core.*;
import org.eclipse.jdt.core.*;
import org.eclipse.jdt.launching.*;

import db4otesteclipse.*;

public class RegressionTestTypeSpec implements TestTypeSpec {
	private static final String TESTCLASS_NAME = "com.db4o.test.AllTests";
	
	private String mode;
	
	public RegressionTestTypeSpec(String mode) {
		this.mode = mode;
	}

	public boolean acceptTestType(IType type) throws JavaModelException {
		if (!type.exists() || !Flags.isPublic(type.getFlags())
				|| Flags.isAbstract(type.getFlags())) {
			Activator.log("Not public or abstract: "
					+ type.getFullyQualifiedName());
			return false;
		}
		IMethod[] methods = type.getMethods();
		boolean declaresDefaultConstructor = false;
		boolean declaresTestMethod = false;
		boolean declaresConstructor = false;
		for (int methodIdx = 0; methodIdx < methods.length; methodIdx++) {
			IMethod method = methods[methodIdx];
			if (method.getElementName().startsWith("test")
					&& Flags.isPublic(method.getFlags())) {
				declaresTestMethod = true;
			}
			if (method.getElementName().equals(type.getElementName())) {
				declaresConstructor = true;
				if (method.getParameterNames().length == 0
						&& Flags.isPublic(method.getFlags())) {
					declaresDefaultConstructor = true;
				}
			}
		}
		if (!declaresTestMethod) {
			Activator.log("No testMethod: " + type.getFullyQualifiedName());
			return false;
		}
		if (declaresConstructor && !declaresDefaultConstructor) {
			Activator.log("No default constructor: "
					+ type.getFullyQualifiedName());
			return false;
		}
		return true;
	}

	public void configureSpecific(ILaunchConfigurationWorkingCopy workingCopy,
			List testTypes, String typeArgsList) {
		workingCopy.setAttribute(
				IJavaLaunchConfigurationConstants.ATTR_MAIN_TYPE_NAME,
				TESTCLASS_NAME);
		workingCopy.setAttribute(
				IJavaLaunchConfigurationConstants.ATTR_PROGRAM_ARGUMENTS, "-"
						+ mode + " " + typeArgsList);
	}

	public boolean acceptTestMethod(IMethod method) throws CoreException {
		return false;
	}
}
