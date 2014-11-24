/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o;

/**
 * @exclude
 */
public abstract class QEAbstract extends QE{
	
	QE add(QE evaluator){
		QE qe = new QEMulti();
		qe.add(this);
		qe.add(evaluator);
		return qe;
	}
    
    boolean isDefault(){
        return false;
    }

}

