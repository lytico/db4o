package com.db4o.test;

import com.db4o.query.Query;
import java.io.DataInputStream;
import java.io.DataOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.util.Enumeration;
import javax.microedition.io.*;
import javax.microedition.io.file.*;
import javax.microedition.midlet.*;
import javax.microedition.rms.*;

import com.db4o.*;
import com.db4o.io.*;
import com.db4o.reflect.self.*;

public class Db4oTestMidlet extends MIDlet {
    
    public Db4oTestMidlet() {
        // TODO Auto-generated constructor stub
    }
    
    protected void destroyApp(boolean arg0) throws MIDletStateChangeException {
        // TODO Auto-generated method stub
        
    }
    
    protected void pauseApp() {
        // TODO Auto-generated method stub
        
    }
    
    protected void startApp() throws MIDletStateChangeException {
        try {
            runTest();
        } catch (Exception e) {
            e.printStackTrace();
        }
    }
    
    private final static boolean DELETE=true;
    private final static int RECORDSIZE=1024;
    
    private static void runTest() throws ClassNotFoundException, RecordStoreException, IOException {
        if(DELETE) {
            try {
                RecordStore.deleteRecordStore("test.yap");
            } catch (Exception e) {
            }
        }
        String[] stores=RecordStore.listRecordStores();
        System.out.println("Found "+(stores!=null ? stores.length : 0)+" stores");
        if(stores!=null) {
            for (int storeIdx = 0; storeIdx < stores.length; storeIdx++) {
                System.out.println(stores[storeIdx]);
            }
        }
        configure();
        long start=System.currentTimeMillis();
        ObjectContainer db=Db4o.openFile("test.yap");
        Dog oldDog=null;
        for(int i=0;i<100;i++) {
            Dog curDog=new Dog("Laika"+i,i,(oldDog!=null ? new Dog[]{oldDog} : new Dog[0]),new int[]{i});
            //System.out.println(curDog);
            db.set(curDog);
            oldDog=curDog;
        }
        db.commit();
        db.close();
        System.out.println("Storing 100 dogs took "+(System.currentTimeMillis()-start)+" ms");
        //------------------
        db=Db4o.openFile("test.yap");
        start=System.currentTimeMillis();
        ObjectSet result=db.query(Dog.class);
        System.out.println("Found "+result.size()+" dogs");
        System.out.println("Query took "+(System.currentTimeMillis()-start)+" ms");
        start=System.currentTimeMillis();
        while(result.hasNext()) {
            System.out.println(result.next());
        }
        System.out.println("Activation took "+(System.currentTimeMillis()-start)+" ms");
        //---------------
        start=System.currentTimeMillis();
        Query query=db.query();
        query.constrain(Dog.class);
        query.descend("_name").constrain("Laika5");
        ObjectSet res=query.execute();
        while (res.hasNext()) {
            System.out.println(" Dog: "+ res.next());
        }
        System.out.println(" Query1 /soda simple/ took "+(System.currentTimeMillis()-start)+"ms");
        //-----------------
        start=System.currentTimeMillis();
        query=db.query();
        query.constrain(Dog.class);
        query.descend("_name").constrain("Laika25").or(query.descend("_age").constrain(new Integer(27)));
        ObjectSet res2=query.execute();
        while (res2.hasNext()) {
            System.out.println(" Dog: "+ res2.next());
        }
        System.out.println(" Query2 /soda complex/ took "+(System.currentTimeMillis()-start)+"ms");
        
        //-----------------
        start=System.currentTimeMillis();
        query=db.query();
        query.constrain(Dog.class);
        query.constrain(new SodaEvaluation());
        ObjectSet res1=query.execute();
        while (res1.hasNext()) {
            System.out.println(" evaluation Dog: "+ res1.next());
        }
        System.out.println(" Query3 /soda with evaluation/ took "+(System.currentTimeMillis()-start)+"ms");
        //------------------
        
        db.close();
        
        //copyToYapFile("test.yap");
    }
    
    private static void configure() {
        IoAdapter io =
                new RecordStoreIoAdapter(RECORDSIZE);
        //new MemoryIoAdapter();
        Db4o.configure().io(io);
        Db4o.configure().reflectWith(new SelfReflector(new RegressionDogSelfReflectionRegistry()));
        Db4o.configure().objectClass(Animal.class).objectField("_name").indexed(true);
    }
    
    public static void main(String[] args) {
        try {
            runTest();
        } catch (Exception e) {
            e.printStackTrace();
        }
    }
    
    private static void copyToYapFile(String fileName) throws RecordStoreException,IOException {
        Enumeration roots=FileSystemRegistry.listRoots();
        String firstRoot=(String)roots.nextElement();
        //String url="file:///"+firstRoot+fileName;
        String url="file:///root1/output.yap";
        System.out.println(url);
        OutputConnection conn=(OutputConnection)Connector.open(url,Connector.WRITE);
        System.out.println(conn.getClass());
        OutputStream out=conn.openOutputStream();
        RecordStore store=RecordStore.openRecordStore(fileName,false);
        for(int recIdx=0;recIdx<store.getNumRecords();recIdx++) {
            byte[] page=store.getRecord(recIdx+1);
            out.write(page);
        }
        out.close();
        store.closeRecordStore();
    }
}
