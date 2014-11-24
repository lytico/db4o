/*******************************************************************************
 * Copyright (c) 2000, 2007 IBM Corporation and others.
 * All rights reserved. This program and the accompanying materials
 * are made available under the terms of the Eclipse Public License v1.0
 * which accompanies this distribution, and is available at
 * http://www.eclipse.org/legal/epl-v10.html
 *
 * Contributors:
 *     IBM Corporation - initial API and implementation
 *     Versant Inc. - Scala port and minor additions for db4o plugin
 *******************************************************************************/

package com.db4o.eclipse
package ui

import scala._
import scala.collection._

import org.eclipse.core.runtime._
import org.eclipse.jdt.core._
import org.eclipse.jdt.core.search._
import org.eclipse.jdt.core.search.SearchPattern
import org.eclipse.jdt.ui._
import org.eclipse.jface.dialogs._
import org.eclipse.jface.operation._
import org.eclipse.jface.viewers._
import org.eclipse.swt.graphics._
import org.eclipse.swt.widgets._
import org.eclipse.ui.dialogs._
import org.eclipse.jface.window._

object PackageSelector {
	val F_REMOVE_DUPLICATES= 1
	val F_SHOW_PARENTS= 2
	val F_HIDE_DEFAULT_PACKAGE= 4
	val F_HIDE_EMPTY_INNER= 8

	def createLabelProvider(dialogFlags: Int) = {
		var flags = JavaElementLabelProvider.SHOW_DEFAULT
		if ((dialogFlags & F_REMOVE_DUPLICATES) == 0) {
			flags= flags | JavaElementLabelProvider.SHOW_ROOT
		}
		new JavaElementLabelProvider(flags)
	}
}

class PackageSelector(parent: Shell, context: IRunnableContext, flags: Int, searchScope: IJavaSearchScope, packageFilter: (IPackageFragment) => Boolean) 
		extends ElementListSelectionDialog(parent, PackageSelector.createLabelProvider(flags)) {

	var dialogLocation: Point = null
	var dialogSize: Point = null
	
	
	override def open(): Int = {
		val runnable = new PackageSearchRunner()
		try {
			context.run(true, true, runnable)
		} 
		catch {
		  case e: java.lang.reflect.InvocationTargetException => {
			// FIXME
			//ExceptionHandler.handle(e, JavaUIMessages.PackageSelectionDialog_error_title, JavaUIMessages.PackageSelectionDialog_error3Message); 
			e.printStackTrace
			return Window.CANCEL
		  } 
		  case e: InterruptedException => {
			// cancelled by user
			e.printStackTrace
			return Window.CANCEL
		  }
		}
		
		val packageList = runnable.getPackageList
		if (packageList.isEmpty) {
			val title= "No packages found"
			val message= "There are no packages for this project"
			MessageDialog.openInformation(getShell, title, message)
			return Window.CANCEL
		}
		
		setElements(packageList.toArray)

		super.open
	}
	
	override def configureShell(newShell: Shell) {
		super.configureShell(newShell)
		// FIXME
		//PlatformUI.getWorkbench().getHelpSystem().setHelp(newShell, IJavaHelpContextIds.OPEN_PACKAGE_DIALOG);
	}

	override def close() = {
		writeSettings
		super.close
	}

	override def createContents(parent: Composite) = {
		val control = super.createContents(parent)
		readSettings
		control
	}
	
	override def getInitialSize(): Point = {
		val result = super.getInitialSize()
		if (dialogSize == null) {
		  return result
		}
		result.x= Math.max(result.x, dialogSize.x)
		result.y= Math.max(result.y, dialogSize.y)
		val display= getShell.getDisplay.getClientArea
		result.x= Math.min(result.x, display.width)
		result.y= Math.min(result.y, display.height)
		result
	}
	
	override def getInitialLocation(initialSize: Point): Point = {
		val result = super.getInitialLocation(initialSize)
		if (dialogLocation == null) {
		  return result
		}
		result.x = dialogLocation.x
		result.y = dialogLocation.y
		val display = getShell.getDisplay.getClientArea
		val xe = result.x + initialSize.x
		if (xe > display.width) {
			result.x -= xe - display.width 
		}
		val ye = result.y + initialSize.y
		if (ye > display.height) {
			result.y -= ye - display.height
		}
		result
	}

	def readSettings() {
		val s = getDialogSettings
		try {
			val x = s.getInt("x")
			val y = s.getInt("y")
			dialogLocation = new Point(x, y)
			val width = s.getInt("width")
			val height = s.getInt("height")
			dialogSize = new Point(width, height)
		} 
		catch {
		  case e: NumberFormatException => {
			dialogLocation = null
			dialogSize = null
		  }
		}
	}

	def writeSettings() {
		val s = getDialogSettings
		val location = getShell.getLocation
		s.put("x", location.x)
		s.put("y", location.y)
		val size = getShell.getSize;
		s.put("width", size.x)
		s.put("height", size.y)
	}

	def getDialogSettings() = {
		val settings = Db4oPluginActivator.getDefault.getDialogSettings
		val sectionName = getClass.getName
		var subSettings = settings.getSection(sectionName)
		if (subSettings == null)
			settings.addNewSection(sectionName)
		else
			subSettings
	}
 
	private class PackageSearchRunner extends IRunnableWithProgress {
	  
		private var packageList: scala.List[IPackageFragment] = Nil
   
		def getPackageList() = packageList
	  
		override def run(withMonitor: IProgressMonitor) {
			val monitor = withMonitor match {
			  case null => new NullProgressMonitor()
			  case _ => withMonitor 
			}
			val hideEmpty= (flags & PackageSelector.F_HIDE_EMPTY_INNER) != 0
			monitor.beginTask("Collecting Packages", if(hideEmpty) 2 else 1)
			try {
				val requestor= new PackageSearchRequestor()
				val pattern= SearchPattern.createPattern("*",
						IJavaSearchConstants.PACKAGE, IJavaSearchConstants.DECLARATIONS,
						SearchPattern.R_PATTERN_MATCH | SearchPattern.R_CASE_SENSITIVE)
				new SearchEngine().search(pattern, Array(SearchEngine.getDefaultSearchParticipant), searchScope, requestor, new SubProgressMonitor(monitor, 1))
				
				if (monitor.isCanceled()) {
					throw new InterruptedException()
				}

				packageList = requestor.getPackageList
				if (hideEmpty) {
					packageList = removeEmptyPackages(new SubProgressMonitor(monitor, 1), packageList)
				}
			} catch {
			  	case e: CoreException => throw new java.lang.reflect.InvocationTargetException(e)
			  	case e: OperationCanceledException => throw new InterruptedException()
			} 
			finally {
				monitor.done()
			}
		}
			
		private def removeEmptyPackages(monitor: IProgressMonitor, packageList: scala.List[IPackageFragment]) = {
			monitor.beginTask("Filtering Empty Packages", packageList.size)
			try {
				packageList.filter((pkg) => {
					monitor.worked(1)
					if (monitor.isCanceled()) {
						throw new InterruptedException()
					}
					pkg.hasChildren || !pkg.hasSubpackages
				})
			} 
			finally {
				monitor.done()
			}
		}
	}
 
	private class PackageSearchRequestor extends SearchRequestor {
	  
		private var packageList: scala.List[IPackageFragment] = Nil

		private val fSet = mutable.HashSet[String]();
		private val fAddDefault= (flags & PackageSelector.F_HIDE_DEFAULT_PACKAGE) == 0
		private val fDuplicates= (flags & PackageSelector.F_REMOVE_DUPLICATES) == 0
		private val fIncludeParents= (flags & PackageSelector.F_SHOW_PARENTS) != 0

		def getPackageList() = packageList
  
		override def acceptSearchMatch(searchMatch: SearchMatch) {
			val enclosingElement = searchMatch.getElement().asInstanceOf[IJavaElement]
			val name = enclosingElement.getElementName
			if (!fAddDefault && name.length() == 0) {
				return
			}
			if (!fDuplicates && fSet.contains(name)) {
				return
			}
			fSet += name
			val packageRoot = packageFragmentRootFor(enclosingElement)

			val packageFragment = enclosingElement.asInstanceOf[IPackageFragment]
			addPackageFragment(packageFragment)
			if (fIncludeParents) {
				addParentPackages(enclosingElement, name)
			}
		}

		private def packageFragmentRootFor(javaElement: IJavaElement): IPackageFragmentRoot = {
			if(javaElement.isInstanceOf[IPackageFragmentRoot]) {
				return javaElement.asInstanceOf[IPackageFragmentRoot]
			}
			if(javaElement == null) {
				return null
			}
			packageFragmentRootFor(javaElement.getParent)
		}
		
		private def addParentPackages(enclosingElement: IJavaElement, name: String) {
			var nameFragment = name
			val root = enclosingElement.getParent.asInstanceOf[IPackageFragmentRoot]
			var idx= nameFragment.lastIndexOf('.')
			while (idx != -1) {
				nameFragment = nameFragment.substring(0, idx)
				if (fDuplicates || !fSet.contains(nameFragment)) {
					fSet += nameFragment
					addPackageFragment(root.getPackageFragment(nameFragment))
				}
				idx = nameFragment.lastIndexOf('.')
			}
		}
  
		private def addPackageFragment(fragment: IPackageFragment) {
		  if(packageFilter(fragment)) {
			packageList ++= scala.List(fragment)
		  }
		}
	}
}
