package db4otesteclipse;

import java.util.*;

import org.eclipse.core.runtime.*;
import org.eclipse.debug.core.*;
import org.eclipse.jdt.core.*;

public interface TestTypeSpec {
	void configureSpecific(ILaunchConfigurationWorkingCopy workingCopy, List testTypes, String typeArgsList) throws CoreException;
	boolean acceptTestType(IType type) throws CoreException;
	boolean acceptTestMethod(IMethod method) throws CoreException;
}
