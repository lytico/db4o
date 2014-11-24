package com.db4odoc.osgi;


import com.db4o.Db4oEmbedded;
import com.db4o.ObjectContainer;
import com.db4o.config.EmbeddedConfiguration;
import com.db4o.reflect.jdk.JdkLoader;
import com.db4o.reflect.jdk.JdkReflector;
import org.osgi.framework.Bundle;
import org.osgi.framework.BundleActivator;
import org.osgi.framework.BundleContext;

public class UsingDb4oDirectly implements BundleActivator {
    public void start(BundleContext bundleContext) throws Exception {
        // #example: Db4o with a special OSGi loader
        // Specify the bundles from which the classes are used to store objects
        Bundle[] bundles = new Bundle[]{bundleContext.getBundle()};
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        configuration.common()
                .reflectWith(new JdkReflector(new OSGiLoader(bundles)));
        // #end example
        ObjectContainer container = Db4oEmbedded.openFile(configuration, "database.db4o");

    }

    public void stop(BundleContext bundleContext) throws Exception {

    }

    // #example: Load classes from multiple bundles
    class OSGiLoader implements JdkLoader{
        private final Bundle[] bundlesToLookIn;

        OSGiLoader(Bundle... bundlesToLookIn) {
            this.bundlesToLookIn = bundlesToLookIn;
        }

        public Class loadClass(String s) {
            for (Bundle bundle : bundlesToLookIn) {
                try {
                    return bundle.loadClass(s);
                } catch (ClassNotFoundException e) {
                    // just retry on other bundles
                }
            }
            try {
                return getClass().getClassLoader().loadClass(s);
            } catch (ClassNotFoundException e) {
                return null;
            }
        }

        public Object deepClone(Object o) {
            return new OSGiLoader(bundlesToLookIn);
        }
    }
    // #end example
}
