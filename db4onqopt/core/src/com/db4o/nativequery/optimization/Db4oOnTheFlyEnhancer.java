/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.nativequery.optimization;

import java.util.ArrayList;

import EDU.purdue.cs.bloat.context.CachingBloatContext;
import EDU.purdue.cs.bloat.editor.ClassEditor;
import EDU.purdue.cs.bloat.editor.EditorContext;
import EDU.purdue.cs.bloat.file.ClassFileLoader;

import com.db4o.instrumentation.bloat.*;
import com.db4o.instrumentation.core.*;
import com.db4o.internal.query.Db4oNQOptimizer;
import com.db4o.nativequery.expr.Expression;
import com.db4o.query.*;
import com.db4o.reflect.Reflector;

// only introduced to keep Db4oListFacade clean of Bloat references
public class Db4oOnTheFlyEnhancer implements Db4oNQOptimizer {
	private transient BloatLoaderContext bloatUtil;
	private transient EditorContext context;
	private Reflector reflector;
	
	public Db4oOnTheFlyEnhancer() {
		this(new ClassFileLoader());
	}
	
	public Db4oOnTheFlyEnhancer(Reflector reflector) {
		this(new ClassFileLoader(new Db4oClassSource(new JdkReverseLookupClassFactory(reflector))));
		this.reflector = reflector;
	}
	
	private Db4oOnTheFlyEnhancer(ClassFileLoader loader) {
		this.context=new CachingBloatContext(loader,new ArrayList(),false);
		this.bloatUtil =new BloatLoaderContext(context);
	}
	
	public Object optimize(Query query,Predicate filter) {
		try {
			//long start=System.currentTimeMillis();
			Expression expr = analyzeInternal(filter);
			//System.err.println((System.currentTimeMillis()-start)+" ms");
			//System.err.println(expr);
			if(expr==null) {
				throw new RuntimeException("Could not analyze "+filter);
			}
			//start=System.currentTimeMillis();
			final JdkReverseLookupClassFactory classFactory = new JdkReverseLookupClassFactory(reflector);
			new SODAQueryBuilder().optimizeQuery(expr, query, filter, classFactory, new BloatReferenceResolver(classFactory));
			//System.err.println((System.currentTimeMillis()-start)+" ms");
			return expr;
		} catch (ClassNotFoundException exc) {
			throw new RuntimeException(exc.getMessage());
		}
	}

	private Expression analyzeInternal(Predicate filter) throws ClassNotFoundException {
		ClassEditor classEditor = context.editClass(filter.getClass().getName());
		return new NativeQueryEnhancer().analyze(bloatUtil,classEditor,PredicatePlatform.PREDICATEMETHOD_NAME,null);
	}
	
	public static Expression analyze(Predicate filter) throws ClassNotFoundException {
		return new Db4oOnTheFlyEnhancer().analyzeInternal(filter);
	}
}
