/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */

package com.db4o.eclipse
package ui

import com.db4o.eclipse.nature._
import org.eclipse.core.resources._
import org.eclipse.core.runtime	._
import org.eclipse.ui._
import org.eclipse.jface.viewers._
import org.eclipse.jface.action._

class ToggleDb4oNatureAction extends IObjectActionDelegate {

  private var project: Option[IProject] = None

  override def run(action: IAction) {
    project.map(Db4oNature.toggleNature(_))
  }
    
  override def selectionChanged(action: IAction, selection: ISelection) {
    if (!selection.isInstanceOf[IStructuredSelection]) {
      project = None
      return
	}
    val selections = selection.asInstanceOf[IStructuredSelection].toArray
    if(selections.length != 1) {
      project = None
      return
    }
    project = asProject(selections(0))
    action.setEnabled(project.isDefined)
    action.setChecked(project.map(Db4oNature.hasDb4oNature(_)).getOrElse(false))
  }

  override def setActivePart(action: IAction, targetPart: IWorkbenchPart) {
  }

  private def asProject(obj: Object): Option[IProject] = {
    if (obj.isInstanceOf[IProject]) {
      return Some(obj.asInstanceOf[IProject])
    } 
    if (obj.isInstanceOf[IAdaptable]) {
      val adapted = (obj.asInstanceOf[IAdaptable]).getAdapter(classOf[IProject]).asInstanceOf[IProject]
      if(adapted != null) {
        return Some(adapted)
      }
    }
    None
  }

}
