package com.db4odoc.servlet;


import com.db4o.ObjectContainer;
import com.db4o.ObjectServer;
import com.db4o.cs.Db4oClientServer;
import com.db4o.cs.config.ServerConfiguration;

import javax.servlet.ServletException;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import java.io.Console;
import java.io.IOException;


public class ServletExample extends HttpServlet {

    @Override
    protected void doGet(HttpServletRequest req, HttpServletResponse resp) throws ServletException, IOException {
        //#example: Get the session container
        ObjectContainer container = 
                (ObjectContainer)req.getAttribute(Db4oServletListener.KEY_DB4O_SESSION);
        //#end example
        super.doGet(req, resp);
    }
}
