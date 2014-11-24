/*
 * Copyright (C) 2005 db4objects Inc.  http://www.db4o.com
 */
package com.db4o.objectmanager.model.query;

import com.db4o.query.Constraint;

public interface RelationalOperator {
    public static abstract class RelationOperatorImpl implements RelationalOperator {
        private String name;

        private RelationOperatorImpl(String name) {
            this.name = name;
        }
        
        public String name() {
            return name;
        }
    }
    
    public final static RelationalOperator EQUALS=new RelationOperatorImpl("=") {
        public void apply(Constraint constraint) {
        }        
    };

    public final static RelationalOperator IDENTITY=new RelationOperatorImpl("ID") {
        public void apply(Constraint constraint) {
            constraint.identity();
        }        
    };

    public final static RelationalOperator SMALLER=new RelationOperatorImpl("<") {
        public void apply(Constraint constraint) {
            constraint.smaller();
        }        
    };

    public final static RelationalOperator GREATER=new RelationOperatorImpl(">") {
        public void apply(Constraint constraint) {
            constraint.greater();
        }        
    };

    public final static RelationalOperator LIKE=new RelationOperatorImpl("~") {
        public void apply(Constraint constraint) {
            constraint.like();
        }        
    };
    
    public static RelationalOperator[] OPERATORS= {
        EQUALS,IDENTITY,GREATER,SMALLER,LIKE
    };
    String name();
    void apply(Constraint query);
}
