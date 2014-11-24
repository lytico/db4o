package com.db4o.objectManager.v2;

import java.awt.*;

import com.db4o.*;

/**
 * User: treeder
 * Date: Dec 3, 2006
 * Time: 10:29:29 AM
 */
public interface UISession {
	/**
	 * This will reopen the ObjectContainer in place and keep the user in the same spot as much as possible, with the
	 * following caveats:
	 * <br/>
	 * <ol>
	 * <li>All Query tabs will close (since they will get disconnected</li>
	 * </ol>
	 */
	void reopen();

	ObjectContainer getObjectContainer();

	void addTab(TabType objectTree, String label, Component component);

	boolean isLocal();
}
