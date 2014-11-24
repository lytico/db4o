package com.db4o.objectManager.v2.demo;

import demo.objectmanager.model.DemoPopulator;

/**
 * User: treeder
 * Date: Dec 20, 2006
 * Time: 7:02:36 PM
 */
public class DemoDbTask {
	private DemoPopulator demoPopulator;

	public DemoDbTask() {

	}

	public DemoPopulator getPopulator() {
		return demoPopulator;
	}

	public void run() {
		demoPopulator = new DemoPopulator();
		demoPopulator.run();
	}
}
