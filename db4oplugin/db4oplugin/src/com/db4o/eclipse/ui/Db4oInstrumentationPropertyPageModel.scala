/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */

package com.db4o.eclipse
package ui

import com.db4o.eclipse.preferences._

import org.eclipse.core.resources._
import org.eclipse.jface.viewers._
import org.eclipse.jdt.core._
import org.eclipse.jdt.core.search._
import org.eclipse.swt.widgets._
import org.eclipse.jface.operation._

import java.util.regex._
import scala.collection._

import AndOrEnum.AndOr


class Db4oInstrumentationPropertyPageModel(project: IProject) {
  private var filterRegExp: String = Db4oPreferences.getFilterRegExp(project).toString
  private var filterPackages: immutable.Set[String] = Db4oPreferences.getPackageList(project)
  private var filterCombinator: AndOr = Db4oPreferences.getFilterCombinator(project)
  private var selectedPackages: Set[String] = immutable.ListSet()
  
  private var listChangeListeners: immutable.ListSet[PackageListChangeListener] = immutable.ListSet.empty
  private var selectionChangeListeners: immutable.ListSet[PackageSelectionChangeListener] = immutable.ListSet.empty

  def addPackageListChangeListener(listener: PackageListChangeListener) = listChangeListeners += listener
  def removePackageListChangeListener(listener: PackageListChangeListener) = listChangeListeners -= listener
  def addPackageSelectionChangeListener(listener: PackageSelectionChangeListener) = selectionChangeListeners += listener
  def removePackageSelectionChangeListener(listener: PackageSelectionChangeListener) = selectionChangeListeners -= listener

  private object PackageSelectionListener extends ISelectionChangedListener {
	  private var selected = false
    
	  def selectionChanged(event: SelectionChangedEvent) {
	    val selection = event.getSelection
	    if(!selection.isInstanceOf[IStructuredSelection]) {
	      selectedPackages = immutable.ListSet()
	      return
	    }
	    val structured = selection.asInstanceOf[IStructuredSelection]
	    selectedPackages = immutable.ListSet(structured.toArray.map(_.toString):_*)
	    if(selected == !selectedPackages.isEmpty) {
	      return
	    }
	    selected = !selected
	    selectionChangeListeners.foreach(_.packagesSelected(selected))
	  }
  }

  def setSelectionProvider(provider: ISelectionProvider) {
    provider.addSelectionChangedListener(PackageSelectionListener)
  }
  
  def setFilterRegExp(regExp: String) {
    filterRegExp = regExp
  }
  
  def getFilterRegExp = 
    try {
      Some(Pattern.compile(filterRegExp))
    }
    catch {
      case exc => None
    }

  def addPackages(packageNames: Set[String]) {
    filterPackages ++= packageNames
    listChangeListeners.foreach(_.packagesAdded(packageNames))
  }
  
  def removePackages(packageNames: Set[String]) {
    filterPackages --= packageNames
    listChangeListeners.foreach(_.packagesRemoved(packageNames))
  }

  def getPackages = filterPackages

  def setFilterCombinator(combinator: AndOr) {
    filterCombinator = combinator
  }
  
  def getFilterCombinator = filterCombinator

  def getSelectedPackages = selectedPackages
  
  def store(project: IProject): StoreStatus = {
    val pattern = getFilterRegExp
    if(!pattern.isDefined) {
      return StoreStatus(false, "Invalid regular expression")
    }
    pattern.map(Db4oPreferences.setFilterRegExp(project, _))
    Db4oPreferences.setPackageList(project, filterPackages)
    Db4oPreferences.setFilterCombinator(project, filterCombinator)
    StoreStatus(true, "OK")
  }

  def getPackageSelectionDialog(shell: Shell, context: IRunnableContext) = {
	val scope= SearchEngine.createJavaSearchScope(Array[IJavaElement](javaProject), IJavaSearchScope.SOURCES);
	val flags= PackageSelector.F_SHOW_PARENTS | PackageSelector.F_HIDE_DEFAULT_PACKAGE | PackageSelector.F_REMOVE_DUPLICATES
	val packageFilter = (pkg: IPackageFragment) => {
	  javaProject.equals(pkg.getJavaProject) && !getPackages.contains(pkg.getElementName)
	}
    val dialog = new PackageSelector(shell, context, flags , scope, packageFilter)
    dialog.setMultipleSelection(true)
    dialog
  }

  private def javaProject() = JavaCore.create(project)
}

case class StoreStatus(success: Boolean, message: String)

trait PackageListChangeListener {
  def packagesAdded(names: Set[String])
  def packagesRemoved(names: Set[String])
}

trait PackageSelectionChangeListener {
  def packagesSelected(state: Boolean)
}
