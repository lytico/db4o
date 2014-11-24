package com.db4odoc.exampleapp.web.db;

import com.db4o.ObjectContainer;
import com.db4odoc.servlet.Db4oServletListener;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.context.annotation.Scope;
import org.springframework.stereotype.Component;

import javax.servlet.http.HttpServletRequest;


@Component("db4o-context")
@Scope(value = "request")
public class Db4oContext {

    @Autowired(required=true)
    private HttpServletRequest request;

    public Db4oContext() {
    }

    public ObjectContainer objectContainer(){
        return (ObjectContainer)request.getAttribute(Db4oServletListener.KEY_DB4O_SESSION);
    }
}
