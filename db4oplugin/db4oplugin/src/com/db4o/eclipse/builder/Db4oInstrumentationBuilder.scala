/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */

package com.db4o.eclipse
package builder

import com.db4o.eclipse.preferences._

import com.db4o.instrumentation.main._
import com.db4o.ta.instrumentation._
import com.db4o.instrumentation.core._
import com.db4o.instrumentation.classfilter._
import com.db4o.instrumentation.file._
import EDU.purdue.cs.bloat.file._
import com.db4o.instrumentation.util._

import AndOrEnum.AndOr

import org.eclipse.core.resources._
import org.eclipse.core.runtime._
import org.eclipse.jdt.core._

import scala.collection._

import java.io._
import java.util.regex._

object Db4oInstrumentationBuilder {
  val BUILDER_ID = "db4oplugin.db4oBuilder"
}

class Db4oInstrumentationBuilder extends IncrementalProjectBuilder {

  override def build(kind: Int, args: java.util.Map[_,_], monitor: IProgressMonitor): Array[IProject] = {
	if (kind == IncrementalProjectBuilder.FULL_BUILD) {
	  fullBuild(monitor)
	  return null
	} 
	val delta = getDelta(getProject)
	if (delta == null) {
	  fullBuild(monitor)
	  return null
	}
	incrementalBuild(delta, monitor)
	null
  }

  private def fullBuild(monitor: IProgressMonitor) {
    val javaProject = JavaCore.create(getProject)
    if(javaProject == null) {
      return
    }
    roots(javaProject).foreach((root) => {
      val fileRoot = new FullFilePathRoot(javaProject.getProject, root)
      val classPath = PDEUtil.classPath(javaProject).map(PDEUtil.workspacePath(_).toOSString)
      enhance(fileRoot, javaProject, PDEUtil.workspacePath(root).toOSString)
    })
  }

  private def incrementalBuild(delta: IResourceDelta, monitor: IProgressMonitor) {
    val visitor = new ModifiedClassFileCollectorVisitor
    delta.accept(visitor)
    val classSources = visitor.getClassSources
    val classPathRoots = collectClassPathRoots(classSources).map(_.toOSString)

    val partitioned = partitionBy(classSources, (source: SelectionClassSource) => source.binaryRoot)
    val classPathRootsArr = classPathRoots.toList.toArray
    partitioned.keys.foreach((binaryRoot) => {
      val instrumentationRoot = new SelectionFilePathRoot(classPathRoots, partitioned.get(binaryRoot).get.toList.removeDuplicates)
      enhance(instrumentationRoot, binaryRoot.javaProject, PDEUtil.workspacePath(binaryRoot.path).toOSString)
    })
  }

  private def enhance(root: FilePathRoot, javaProject: IJavaProject, outPath: String) {
	  val project = javaProject.getProject
	  val classPathRoots = PDEUtil.classPath(javaProject).map(PDEUtil.workspacePath(_).toOSString)
      val instrumentor = new Db4oFileInstrumentor(new InjectTransparentActivationEdit(new PreferenceBasedFilter(project)))
      Db4oPluginActivator.getDefault.getInstrumentationListeners.foreach(instrumentor.addInstrumentationListener(_))
      try {
        instrumentor.enhance(new BundleClassSource, root, new File(outPath), classPathRoots.toList.toArray, getClass.getClassLoader)
      }
      catch {
        case exc => {
          exc.printStackTrace
          throw new CoreException(new Status(IStatus.ERROR, Db4oPluginActivator.PLUGIN_ID, "Error instrumenting folder " + outPath, exc))
        }
      }
  }
  
  private def partitionBy[T,K](iterable: Iterable[T], selector: ((T) => K)) = {
    val agg = new mutable.HashMap[K, mutable.Set[T]]() with mutable.MultiMap[K,T]
    iterable.foreach((t) => agg.add(selector(t), t))
    agg
  }
  
  // FIXME we're in a single project, anyway
  private def collectClassPathRoots(classFiles: Iterable[SelectionClassSource]) = {
    classFiles.foldLeft(scala.collection.mutable.HashSet[IPath]())((roots, classSource) => {
        val javaProject = classSource.binaryRoot.javaProject
        roots ++ (PDEUtil.binaryRoots(javaProject).map(PDEUtil.workspacePath(_)))
    })
  }
  
  private class BundleClassSource extends ClassSource {
    def loadClass(name: String) = Db4oPluginActivator.getDefault.getBundle.loadClass(name)
  }
  
  private class SelectionFilePathRoot(roots: Iterable[String], sources: Iterable[_ <: InstrumentationClassSource]) extends FilePathRoot {
	override def rootDirs = roots.toList.toArray
	override def iterator = java.util.Arrays.asList(sources.toList.toArray: _*).iterator
    override def toString = roots.toString + ": " + sources.toString
  }
  
  private class FullFilePathRoot(project: IProject, path: IPath) extends FilePathRoot {
    override def rootDirs = List(path.toOSString).toArray

    override def iterator =
      java.util.Arrays.asList(classSources.toArray: _*).iterator
    
    private def classSources(): List[InstrumentationClassSource] = {
      val folderFile = PDEUtil.workspaceFile(path)
      allClassFiles(project.getFolder(path.removeFirstSegments(1)))
          .map((resource) => PDEUtil.workspaceFile(resource.getFullPath))
          .map(new FileInstrumentationClassSource(folderFile, _))
    }
    
    private def allClassFiles(folder: IFolder) = {
      var files: List[IResource] = Nil
      folder.accept(new IResourceVisitor() {
        override def visit(resource: IResource) = {
          if(resource.getType == IResource.FILE && "class".equals(resource.getFileExtension)) {
            files ++= List(resource)
          }
          true
        }
      })
      files
    }
    
  }
  
  private def roots(project: IJavaProject) = 
    project.getPackageFragmentRoots
        .filter(_.getKind == IPackageFragmentRoot.K_SOURCE)
        .filter(_.getRawClasspathEntry.getOutputLocation != null)
        .map(_.getRawClasspathEntry.getOutputLocation).toList.removeDuplicates ++ List(project.getOutputLocation)

  private class PreferenceBasedFilter(project: IProject) extends ClassFilter {

    private val regExp = Db4oPreferences.getFilterRegExp(project)
    private val packages = Db4oPreferences.getPackageList(project)
    private val combinator = Db4oPreferences.getFilterCombinator(project)
    
    override def accept(clazz: Class[_]): Boolean = {
      if(BloatUtil.isPlatformClassName(clazz.getName())) {
        return false
      }
      val matchesRegExp = regExp.matcher(clazz.getName).matches
      // FIXME should be cur + "."
      val matchesPackage = packages.foldLeft(false)((agg, cur) => agg || clazz.getName.startsWith(cur))
      combinator(matchesRegExp, matchesPackage)
    }
  }
}
