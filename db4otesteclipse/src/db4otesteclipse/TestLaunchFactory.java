package db4otesteclipse;

import java.util.*;

import org.eclipse.core.runtime.*;
import org.eclipse.debug.core.*;
import org.eclipse.jdt.core.*;
import org.eclipse.jdt.launching.*;

public class TestLaunchFactory {
	
	private final static String LAUNCHCONFIG_ID="db4otesteclipse.launchconfig.type";
	private final static String TESTTYPES_KEY="db4otesteclipse.testtypes";

	private TestTypeSpec spec;
	
	public TestLaunchFactory(TestTypeSpec spec) {
		this.spec = spec;
	}

	public ILaunchConfiguration getLaunchConfig(List<IMember> testTypes) throws CoreException {
		IJavaProject javaProject=containerForTypes(testTypes);
		ILaunchManager launchMgr=DebugPlugin.getDefault().getLaunchManager();
		ILaunchConfigurationType launchType=launchMgr.getLaunchConfigurationType(LAUNCHCONFIG_ID);
		String name=nameForTestTypes(testTypes);
		ILaunchConfiguration config=findExistingLaunchConfig(launchMgr,launchType,testTypes,name);
		if(config!=null) {
			return config;
		}
		ILaunchConfigurationWorkingCopy workingCopy = createLaunchConfig(javaProject,launchType,name);
		configureLaunchConfig(javaProject, testTypes, workingCopy);
		try {
			return workingCopy.doSave();
		}
		catch(RuntimeException exc) {
			exc.printStackTrace();
			throw exc;
		}
	}

	private ILaunchConfiguration findExistingLaunchConfig(ILaunchManager launchMgr,ILaunchConfigurationType launchType,List<IMember> testTypes,String name) throws CoreException {
		ILaunchConfiguration[] launchConfigs=launchMgr.getLaunchConfigurations(launchType);
		Set<IMember> testTypeSet = new HashSet<IMember>(testTypes);
		for (int launchConfigIdx = 0; launchConfigIdx < launchConfigs.length; launchConfigIdx++) {
			ILaunchConfiguration launchConfig = launchConfigs[launchConfigIdx];
			if(configMatches(testTypeSet, name, launchConfig)) {
				return launchConfigs[launchConfigIdx];
			}
		}
		return null;
	}

	private boolean configMatches(Set<IMember> testTypes, String name,
			ILaunchConfiguration launchConfig) throws CoreException {
		if(!launchConfig.getName().equals(name)) {
			return false;
		}
		List<IMember> configTestTypes=testTypesFor(launchConfig);
		if(configTestTypes == null) {
			return false;
		}
		if(testTypes.equals(new HashSet<IMember>(configTestTypes))) {
			return true;
		}
		return false;
	}

	@SuppressWarnings("unchecked")
	private List<IMember> testTypesFor(ILaunchConfiguration launchConfig)
			throws CoreException {
		return (List<IMember>)launchConfig.getAttribute(TESTTYPES_KEY, (List<IMember>)null);
	}

	private List<String> namesForTestTypes(List<IMember> testTypes) {
		List<String> names=new ArrayList<String>(testTypes.size());
		for (IMember curType : testTypes) {
			names.add(curType.getElementName());
		}
		return names;
	}

	private ILaunchConfigurationWorkingCopy createLaunchConfig(IJavaProject javaProject,ILaunchConfigurationType launchType,String name) throws CoreException {
		ILaunchConfigurationWorkingCopy workingCopy = launchType.newInstance(null,name);
		return workingCopy;
	}

	private void configureLaunchConfig(IJavaProject javaProject, List<IMember> testTypes, ILaunchConfigurationWorkingCopy workingCopy) throws CoreException {
		IRuntimeClasspathEntry projectClasspath=JavaRuntime.newDefaultProjectClasspathEntry(javaProject);
		spec.configureSpecific(workingCopy, testTypes, parameterString(testTypes));
		List<String> classPath=new ArrayList<String>();
		classPath.add(projectClasspath.getMemento());
		workingCopy.setAttribute(IJavaLaunchConfigurationConstants.ATTR_CLASSPATH, classPath);
		workingCopy.setAttribute(IJavaLaunchConfigurationConstants.ATTR_DEFAULT_CLASSPATH, false);
		workingCopy.setAttribute(TESTTYPES_KEY, namesForTestTypes(testTypes));
		String workingDir = testTypes.get(0).getJavaProject().getProject().getLocation().toString();
		workingCopy.setAttribute(IJavaLaunchConfigurationConstants.ATTR_WORKING_DIRECTORY, workingDir);
	}
	
	private IJavaProject containerForTypes(List<IMember> testTypes) {
		IJavaProject container=null;
		for (IMember type : testTypes) {
			IJavaProject curContainer = type.getJavaProject();
			if(container!=null) {
				if(!container.equals(curContainer)) {
					return null;
				}
			}
			else {
				container=curContainer;
			}
		}
		return container;
	}
	
	private String parameterString(List<IMember> testTypes) {
		StringBuffer buf = new StringBuffer();
		boolean firstRun = true;
		for (IMember type : testTypes) {
			if (!firstRun) {
				buf.append(' ');
			}
			if(type instanceof IType) {
				buf.append(((IType)type).getFullyQualifiedName());
			}
			if(type instanceof IMethod) {
				IMethod method = ((IMethod)type);
				buf.append(method.getDeclaringType().getFullyQualifiedName())
					.append('#')
					.append(method.getElementName());
			}
			firstRun = false;
		}
		return buf.toString();
	}
	

	private String nameForTestTypes(List<IMember> testTypes) {
		String name=((IMember)testTypes.iterator().next()).getElementName();
		if(testTypes.size()>1) {
			name+=",...["+testTypes.size()+"]";
		}
		return name;
	}
}
