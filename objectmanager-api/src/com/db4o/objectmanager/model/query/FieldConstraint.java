/*
 * Copyright (C) 2005 db4objects Inc.  http://www.db4o.com
 */
package com.db4o.objectmanager.model.query;

import com.db4o.query.Constraint;
import com.db4o.query.Query;
import com.db4o.reflect.ReflectClass;
import com.db4o.reflect.ReflectField;

public class FieldConstraint {
    
    public final ReflectField field;
    public RelationalOperator relation;
    public Object value = null;
    private QueryBuilderModel model;
    
    public FieldConstraint(ReflectField field, QueryBuilderModel model) {
        this.field = field;
        this.model = model;
        this.relation = RelationalOperator.EQUALS;
        final ReflectClass fieldType = field.getFieldType();
    }
    
    public void expand() {
        expand(model, field.getFieldType());
    }

    private void expand(QueryBuilderModel model, final ReflectClass fieldType) {
        this.value = new QueryPrototypeInstance(fieldType, model);
    }

    public void apply(Query query) {
        if (field.getFieldType().isSecondClass()) {
            Constraint constraint=query.descend(field.getName()).constrain(value);
            relation.apply(constraint);
        } else {
            // It's an object reference, we need to traverse it to its
            // QueryPrototypeInstance and apply those constraints too
            QueryPrototypeInstance referencePrototype = (QueryPrototypeInstance) value;
            referencePrototype.addUserConstraints(query.descend(field.getName()));
        }
    }

    public QueryPrototypeInstance valueProto() {
        return (QueryPrototypeInstance) value;
    }
}

