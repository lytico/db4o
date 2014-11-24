/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */

package com.db4o.eclipse
package preferences

import org.eclipse.core.resources._

import scala.collection._

import java.util.regex._

import AndOrEnum.AndOr

object Db4oPreferences {

  val FILTER_REGEXP_PROPERTY_ID = "filter.regexp" 
  val DEFAULT_REGEXP = ".*"
  
  val PACKAGE_LIST_PROPERTY_ID = "filter.packages" 
  val DEFAULT_PACKAGE_LIST = ""
  val PACKAGE_SEP = ","

  val FILTER_COMBINATOR_PROPERTY_ID = "filter.combinator"
  val DEFAULT_COMBINATOR = AndOrEnum.Or
  
  def setFilterRegExp(project: IProject, filterRegExp: Pattern) {
    setPreference(project, FILTER_REGEXP_PROPERTY_ID, filterRegExp.toString)
  }

  def getFilterRegExp(project: IProject) =
    Pattern.compile(getPreference(project, FILTER_REGEXP_PROPERTY_ID, DEFAULT_REGEXP))
  
  def setPackageList(project: IProject, packages: Set[String]) {
    setPreference(project, PACKAGE_LIST_PROPERTY_ID, packages.mkString(PACKAGE_SEP))
  }

  def getPackageList(project: IProject) = {
    var packages = immutable.ListSet.empty[String]
    val packageStr = getPreference(project, PACKAGE_LIST_PROPERTY_ID, DEFAULT_PACKAGE_LIST)
    if(!packageStr.isEmpty) {
      packageStr.split(PACKAGE_SEP).foreach(packages += _)
    }
    packages
  }
  
  def setFilterCombinator(project: IProject, value: AndOr) {
    setPreference(project, FILTER_COMBINATOR_PROPERTY_ID, value.toString)
  }
  
  def getFilterCombinator(project: IProject): AndOr = 
    AndOrEnum.valueOf(getPreference(project, FILTER_COMBINATOR_PROPERTY_ID, DEFAULT_COMBINATOR.toString)).getOrElse(DEFAULT_COMBINATOR)
  
  def getPreference(project: IProject, key: String, defaultValue: String) = {
    projectPreferences(project).map(_.get(key, defaultValue))
        .getOrElse(defaultValue)
  }  

  private def setPreference(project: IProject, key: String, value: String) {
    projectPreferences(project).map((node) => {
      node.put(key, value);
      node.flush();
    })
  }

  private def projectPreferences(project: IProject) =
    new ProjectScope(project).getNode(Db4oPluginActivator.PLUGIN_ID) match {
      case null => None
      case x => Some(x)
    }

}
