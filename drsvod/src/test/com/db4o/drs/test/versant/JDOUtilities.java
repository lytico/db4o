package com.db4o.drs.test.versant;


import javax.jdo.PersistenceManager;
import javax.jdo.PersistenceManagerFactory;

class JDOUtilities{
	
    private JDOUtilities(){}


    public static void withTransaction(PersistenceManagerFactory factory,
                                JDOTransactionClosure transactionalOperation) {
        PersistenceManager session = factory.getPersistenceManager();
        try{
            session.currentTransaction().begin();
            transactionalOperation.invoke(session);
            session.currentTransaction().commit();
        } catch (Exception e){
            session.currentTransaction().rollback();
            reThrow(e);
        } finally {
            session.close();
        }
    }

    static void reThrow(Exception e) {
        JDOUtilities.<RuntimeException>throwsUnchecked(e);
    }

    static <T extends Exception> void throwsUnchecked(Exception toThrow) throws T{
        // Since the type is erased, this cast actually does nothing!!!
        // we can throw any exception
        throw (T) toThrow;
    }

}



