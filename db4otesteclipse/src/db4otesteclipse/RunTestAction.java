package db4otesteclipse;

import java.util.*;

import org.eclipse.core.runtime.*;
import org.eclipse.debug.core.*;
import org.eclipse.debug.ui.*;
import org.eclipse.jface.action.*;
import org.eclipse.jface.viewers.*;
import org.eclipse.ui.*;

public abstract class RunTestAction implements IObjectActionDelegate {

	private ISelection selection=null;

	public final void setActivePart(IAction action, IWorkbenchPart targetPart) {
	}

	public final void run(IAction action) {
		try {
			TestTypeSpec spec = spec();
			List testTypes=new TypeCollector(spec).collectTestTypes(selection);
			if(testTypes.isEmpty()) {
				return;
			}
			ILaunchConfiguration config = new TestLaunchFactory(spec).getLaunchConfig(testTypes);
			DebugUITools.launch(config, ILaunchManager.RUN_MODE);
		} catch (CoreException exc) {
			Activator.log(new Status(Status.ERROR,Activator.PLUGIN_ID,0,"Could not run db4o test",exc));
		}
	}

	public final void selectionChanged(IAction action, ISelection selection) {
		this.selection=selection;
	}

	protected abstract TestTypeSpec spec();
	
	protected final ISelection selection() {
		return selection;
	}
}
