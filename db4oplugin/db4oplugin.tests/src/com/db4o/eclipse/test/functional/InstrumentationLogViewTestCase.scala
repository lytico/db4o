package com.db4o.eclipse.test.functional

import org.eclipse.ui._
import org.eclipse.ui.application._
import org.eclipse.swt._
import org.eclipse.swt.widgets._

import org.junit._

class InstrumentationLogViewTestCase extends Db4oPluginTestCaseTrait {

  private var shell: Shell = null
  
  @Before
  override def setUp() {
    super.setUp
    shell = new Shell()
  }

  @After
  override def tearDown() {
    System.err.println("TD")
    shell.dispose
    super.tearDown
  }
  
  @Test
  def testShowHide() {
    try {
	    val view = getPage.showView("db4oplugin.instrumentationlogview")
	    getPage.hideView(view)
    }
    finally {
      System.err.println("FIN")
      shell.getDisplay.close
    }
  }
  
  private def getPage() = {
    val display = shell.getDisplay
    PlatformUI.createAndRunWorkbench(display, new WorkbenchAdvisor() {
    	override def getInitialWindowPerspectiveId() = null
    })
    val workbench = PlatformUI.getWorkbench
    val window = workbench.getActiveWorkbenchWindow
    window.getActivePage    
  }
  
}
