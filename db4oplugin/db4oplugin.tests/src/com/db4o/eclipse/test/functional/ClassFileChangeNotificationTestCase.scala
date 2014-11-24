package com.db4o.eclipse.test.functional

import com.db4o.eclipse.preferences._

import com.db4o.eclipse.test.util._

import org.eclipse.core.resources._

import org.junit._

class ClassFileChangeNotificationTestCase extends Db4oPluginTestCaseTrait {

  @Before
  override def setUp {
    super.setUp
    WorkspaceUtilities.setAutoBuilding(false)
  }
  
  @Test
  def testChangeNotification {
    val listener = new IResourceChangeListener() {
      override def resourceChanged(event: IResourceChangeEvent) {
        System.err.println(event.getDelta + "/" + event.getType)
      }
      
      def findChangedClassFiles(delta: IResourceDelta): List[IResource] = {
        if(delta == null) {
          return Nil
        }
        delta.accept(new IResourceDeltaVisitor() {
          override def visit(delta: IResourceDelta): Boolean = {
		    val resource = delta.getResource
		    if(resource.getType != IResource.FILE) {
		      return true
		    }
      // FIXME

		    //if(!"class".equals(resource.getFileExtension)) {
		      return false
		    //}
          }
        })
        Nil
      }
    }
    WorkspaceUtilities.getWorkspaceRoot.getWorkspace.addResourceChangeListener(listener);
    project.createCompilationUnit("one", "One.java", "public class One {}")
    project.buildProject(null)
  }
  
}
