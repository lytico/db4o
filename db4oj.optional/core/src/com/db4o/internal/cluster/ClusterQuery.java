/* Copyright (C) 2004 - 2005  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.cluster;

import java.util.*;

import com.db4o.*;
import com.db4o.cluster.*;
import com.db4o.foundation.*;
import com.db4o.internal.query.*;
import com.db4o.query.*;

/**
 * 
 * @exclude
 */
public class ClusterQuery implements Query{
    
    private final Cluster _cluster;
    private final Query[] _queries;
    
    public ClusterQuery(Cluster cluster, Query[] queries){
        _cluster = cluster;
        _queries = queries;
    }

    public Constraint constrain(Object constraint) {
        synchronized(_cluster){
            Constraint[] constraints = new Constraint[_queries.length];
            for (int i = 0; i < constraints.length; i++) {
                constraints[i] = _queries[i].constrain(constraint);
            }
            return new ClusterConstraint(_cluster, constraints);
        }
    }

    public Constraints constraints() {
        synchronized(_cluster){
            Constraint[] constraints = new Constraint[_queries.length];
            for (int i = 0; i < constraints.length; i++) {
                constraints[i] = _queries[i].constraints();
            }
            return new ClusterConstraints(_cluster, constraints);
        }
    }

    public Query descend(String fieldName) {
        synchronized(_cluster){
            Query[] queries = new Query[_queries.length];
            for (int i = 0; i < queries.length; i++) {
                queries[i] = _queries[i].descend(fieldName);
            }
            return new ClusterQuery(_cluster, queries);
        }
    }

    public ObjectSet execute() {
        synchronized(_cluster){
            return new ObjectSetFacade(new ClusterQueryResult(_cluster, _queries)); 
        }
    }

    public Query orderAscending() {
        throw new NotSupportedException();
    }

    public Query orderDescending() {
    	throw new NotSupportedException();
    }

	public Query sortBy(QueryComparator comparator) {
		// FIXME
		throw new NotSupportedException();
	}

	/**
	 * @sharpen.ignore
	 */
	@decaf.Ignore(decaf.Platform.JDK11)
    public Query sortBy(Comparator comparator) {
		// FIXME
		throw new NotSupportedException();
	}
}
