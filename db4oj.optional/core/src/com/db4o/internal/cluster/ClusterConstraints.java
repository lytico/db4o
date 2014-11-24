/* Copyright (C) 2004 - 2005  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.cluster;

import com.db4o.cluster.*;
import com.db4o.foundation.*;
import com.db4o.query.*;

/**
 * 
 * @exclude
 */
public class ClusterConstraints  extends ClusterConstraint implements Constraints{
    
    public ClusterConstraints(Cluster cluster, Constraint[] constraints){
        super(cluster, constraints);
    }

    public Constraint[] toArray() {
        synchronized(_cluster){
            Collection4 all = new Collection4();
            for (int i = 0; i < _constraints.length; i++) {
                ClusterConstraint c = (ClusterConstraint)_constraints[i];
                for (int j = 0; j < c._constraints.length; j++) {
                    all.add(c._constraints[j]);
                }
            }
            Constraint[] res = new Constraint[all.size()];
            all.toArray(res);
            return res;
        }
    }
}

