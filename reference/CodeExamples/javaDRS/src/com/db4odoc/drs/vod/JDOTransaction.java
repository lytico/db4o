package com.db4odoc.drs.vod;


import javax.jdo.PersistenceManager;

public interface JDOTransaction {
    public void invoke(PersistenceManager manager);
}
