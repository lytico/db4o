package com.db4odoc.osgi;

import com.db4o.ObjectContainer;
import com.db4o.osgi.Db4oService;
import org.osgi.framework.BundleActivator;
import org.osgi.framework.BundleContext;
import org.osgi.framework.ServiceReference;


public class UsingDb4oViaService implements BundleActivator {
    public void start(BundleContext bundleContext) throws Exception {
        // #example: Get db4o osgi service
        ServiceReference reference = bundleContext.getServiceReference("com.db4o.osgi.Db4oService");
        Db4oService db4oService = (Db4oService) bundleContext.getService(reference);
        ObjectContainer container = db4oService.openFile("database.db4o");
        // #end example
    }

    public void stop(BundleContext bundleContext) throws Exception {

    }
}
