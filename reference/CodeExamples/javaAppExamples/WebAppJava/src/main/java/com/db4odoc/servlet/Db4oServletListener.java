package com.db4odoc.servlet;

import com.db4o.Db4oEmbedded;
import com.db4o.EmbeddedObjectContainer;
import com.db4o.ObjectContainer;

import javax.servlet.*;


public class Db4oServletListener implements ServletContextListener, ServletRequestListener {
    public static final String KEY_DB4O_FILE_NAME = "database-file-name";

    public static final String KEY_DB4O_SERVER = "db4oServer";
    public static final String KEY_DB4O_SESSION = "db4oSession";

    //#example: db4o-instance for the web-application
    @Override
    public void contextInitialized(ServletContextEvent event) {
        ServletContext context = event.getServletContext();
        String filePath = context.getRealPath("WEB-INF/"
                + context.getInitParameter(KEY_DB4O_FILE_NAME));
        EmbeddedObjectContainer rootContainer = Db4oEmbedded.openFile(filePath);
        context.setAttribute(KEY_DB4O_SERVER, rootContainer);
        context.log("db4o startup on " + filePath);
    }
    
    
    @Override
    public void contextDestroyed(ServletContextEvent event) {
        ServletContext context = event.getServletContext();
        ObjectContainer rootContainer = (ObjectContainer) context.getAttribute(KEY_DB4O_SERVER);
        context.removeAttribute(KEY_DB4O_SERVER);
        close(rootContainer);
        context.log("db4o shutdown");
    }
    // #end example
    
    //#example: a db4o-session for each request
    @Override
    public void requestInitialized(ServletRequestEvent requestEvent) {
        EmbeddedObjectContainer rootContainer = (EmbeddedObjectContainer) requestEvent
                .getServletContext().getAttribute(Db4oServletListener.KEY_DB4O_SERVER);
    
        ObjectContainer session = rootContainer.openSession();
        requestEvent.getServletRequest().setAttribute(KEY_DB4O_SESSION, session);
    }
    
    
    @Override
    public void requestDestroyed(ServletRequestEvent requestEvent) {
        ObjectContainer session = (ObjectContainer) requestEvent
                .getServletRequest().getAttribute(KEY_DB4O_SESSION);
    
        close(session);
    }
    //#end example
    
    
    private void close(ObjectContainer container) {
        if (container != null) {
            container.close();
        }
        container = null;
    }
}
