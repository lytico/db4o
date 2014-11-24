package com.db4o.drs.test.versant.jdo.reflect;

import javax.jdo.*;
import javax.jdo.spi.*;

public class NotImplementedPersistenceCapable implements PersistenceCapable {

	public void jdoCopyFields(Object arg0, int[] arg1) {
		throw new java.lang.UnsupportedOperationException();
	}

	public void jdoCopyKeyFieldsFromObjectId(ObjectIdFieldConsumer arg0, Object arg1) {
		throw new java.lang.UnsupportedOperationException();
	}

	public void jdoCopyKeyFieldsToObjectId(Object arg0) {
		throw new java.lang.UnsupportedOperationException();
	}

	public void jdoCopyKeyFieldsToObjectId(ObjectIdFieldSupplier arg0, Object arg1) {
		throw new java.lang.UnsupportedOperationException();
	}

	public Object jdoGetObjectId() {
		throw new java.lang.UnsupportedOperationException();
	}

	public PersistenceManager jdoGetPersistenceManager() {
		throw new java.lang.UnsupportedOperationException();
	}

	public Object jdoGetTransactionalObjectId() {
		throw new java.lang.UnsupportedOperationException();
	}

	public Object jdoGetVersion() {
		throw new java.lang.UnsupportedOperationException();
	}

	public boolean jdoIsDeleted() {
		throw new java.lang.UnsupportedOperationException();
	}

	public boolean jdoIsDetached() {
		throw new java.lang.UnsupportedOperationException();
	}

	public boolean jdoIsDirty() {
		throw new java.lang.UnsupportedOperationException();
	}

	public boolean jdoIsNew() {
		throw new java.lang.UnsupportedOperationException();
	}

	public boolean jdoIsPersistent() {
		throw new java.lang.UnsupportedOperationException();
	}

	public boolean jdoIsTransactional() {
		throw new java.lang.UnsupportedOperationException();
	}

	public void jdoMakeDirty(String arg0) {
		throw new java.lang.UnsupportedOperationException();
	}

	public PersistenceCapable jdoNewInstance(StateManager arg0) {
		throw new java.lang.UnsupportedOperationException();
	}

	public PersistenceCapable jdoNewInstance(StateManager arg0, Object arg1) {
		throw new java.lang.UnsupportedOperationException();
	}

	public Object jdoNewObjectIdInstance() {
		throw new java.lang.UnsupportedOperationException();
	}

	public Object jdoNewObjectIdInstance(Object arg0) {
		throw new java.lang.UnsupportedOperationException();
	}

	public void jdoProvideField(int arg0) {
		throw new java.lang.UnsupportedOperationException();
	}

	public void jdoProvideFields(int[] arg0) {
		throw new java.lang.UnsupportedOperationException();
	}

	public void jdoReplaceField(int arg0) {
		throw new java.lang.UnsupportedOperationException();
	}

	public void jdoReplaceFields(int[] arg0) {
		throw new java.lang.UnsupportedOperationException();
	}

	public void jdoReplaceFlags() {
		throw new java.lang.UnsupportedOperationException();
	}

	public void jdoReplaceStateManager(StateManager arg0) throws SecurityException {
		throw new java.lang.UnsupportedOperationException();
	}

}
