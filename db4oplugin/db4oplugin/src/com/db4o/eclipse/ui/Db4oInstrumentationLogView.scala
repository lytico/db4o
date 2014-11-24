/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */

package com.db4o.eclipse
package ui

import com.db4o.instrumentation.core._
import com.db4o.instrumentation.main._
import com.db4o.instrumentation.file._

import org.eclipse.swt._
import org.eclipse.ui.part._
import org.eclipse.swt.widgets._
import org.eclipse.jface.viewers._
import org.eclipse.core.resources._

class Db4oInstrumentationLogView extends ViewPart {

  private val MAX_ENTRIES = 100
  private var view: TableViewer = null
  
  private object LogViewListener extends Db4oInstrumentationListener with IResourceChangeListener {
    
	override def resourceChanged(event: IResourceChangeEvent) {
		if(event.getType == IResourceChangeEvent.PRE_BUILD) {
			asyncExec(() => view.getTable.removeAll)
		}
	}

	override def notifyStartProcessing(root: FilePathRoot) {
	}
 
	override def notifyEndProcessing(root: FilePathRoot) {
	}
    
    override def notifyProcessed(source: InstrumentationClassSource, status: InstrumentationStatus) {
    	asyncExec(() => view.add(source + ": " + status))
    }
    
    private def asyncExec(block: () => Unit) {
      Display.getDefault.asyncExec(new Runnable() {
        override def run() {
          block()
        }
      })
    }
  }
  
  override def createPartControl(parent: Composite) = {
    val table= new Table(parent, SWT.BORDER | SWT.MULTI | SWT.H_SCROLL | SWT.V_SCROLL)
    table.setFont(parent.getFont())
    table.setLayout(new TableLayout)
    view = new TableViewer(table)
    Db4oPluginActivator.getDefault.addInstrumentationListener(LogViewListener)
    ResourcesPlugin.getWorkspace.addResourceChangeListener(LogViewListener, IResourceChangeEvent.PRE_BUILD)
  }

  override def setFocus() {
  }

  override def dispose() {
    ResourcesPlugin.getWorkspace.removeResourceChangeListener(LogViewListener)
    Db4oPluginActivator.getDefault.removeInstrumentationListener(LogViewListener)
    super.dispose
  }
  
}
