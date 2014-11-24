/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */

package com.db4o.eclipse

import com.db4o.instrumentation.main._

import org.eclipse.jface.resource._;
import org.eclipse.ui.plugin._;
import org.osgi.framework._;

import scala.collection._

object Db4oPluginActivator {
	val PLUGIN_ID = "db4oplugin"

 	var plugin: Db4oPluginActivator = null

	def getDefault = plugin

	def getImageDescriptor(path: String) = AbstractUIPlugin.imageDescriptorFromPlugin(PLUGIN_ID, path)
}

class Db4oPluginActivator extends AbstractUIPlugin {

	private var listeners: immutable.ListSet[Db4oInstrumentationListener] = immutable.ListSet.empty 
  
	override def start(context: BundleContext) {
		super.start(context);
		Db4oPluginActivator.plugin = this;
	}

	override def stop(context: BundleContext) {
		Db4oPluginActivator.plugin = null;
		super.stop(context);
	}
 
	def addInstrumentationListener(listener: Db4oInstrumentationListener) {
	  listeners += listener
	}

	def removeInstrumentationListener(listener: Db4oInstrumentationListener) {
	  listeners -= listener
	}
 
	def getInstrumentationListeners() = listeners
}
