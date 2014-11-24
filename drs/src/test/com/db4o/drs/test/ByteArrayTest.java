/* Copyright (C) 2004 - 2008  Versant Inc.  http://www.db4o.com

This file is part of the db4o open source object database.

db4o is free software; you can redistribute it and/or modify it under
the terms of version 2 of the GNU General Public License as published
by the Free Software Foundation and as clarified by db4objects' GPL 
interpretation policy, available at
http://www.db4o.com/about/company/legalpolicies/gplinterpretation/
Alternatively you can write to db4objects, Inc., 1900 S Norfolk Street,
Suite 350, San Mateo, CA 94403, USA.

db4o is distributed in the hope that it will be useful, but WITHOUT ANY
WARRANTY; without even the implied warranty of MERCHANTABILITY or
FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License
for more details.

You should have received a copy of the GNU General Public License along
with this program; if not, write to the Free Software Foundation, Inc.,
59 Temple Place - Suite 330, Boston, MA  02111-1307, USA. */
package com.db4o.drs.test;

import com.db4o.drs.test.data.*;

import db4ounit.*;

/**
 * Design of this case is copied from
 * com.db4o.db4ounit.common.types.arrays.ByteArrayTestCase.
 */
public class ByteArrayTest extends DrsTestCase {
	static final int ARRAY_LENGTH = 5;

	static byte[] initial = createByteArray();

	static byte[] modInB = new byte[] { 2, 3, 5, 68, 69 };

	static byte[] modInA = new byte[] { 15, 36, 55, 8, 9, 28, 65 };

	public void test() {
		storeInA();
		replicate();
		modifyInB();
		replicate2();
		modifyInA();
		replicate3();
	}

	private void storeInA() {
		IByteArrayHolder byteArrayHolder = new ByteArrayHolder(
				createByteArray());

		a().provider().storeNew(byteArrayHolder);
		a().provider().commit();

		ensureNames(a(), initial);
	}

	private void replicate() {
		replicateAll(a().provider(), b().provider());

		ensureNames(a(), initial);
		ensureNames(b(), initial);
	}

	private void modifyInB() {
		IByteArrayHolder c = getTheObject(b());

		c.setBytes(modInB);
		b().provider().update(c);
		b().provider().commit();

		ensureNames(b(), modInB);
	}

	private void replicate2() {
		replicateAll(b().provider(), a().provider());

		ensureNames(a(), modInB);
		ensureNames(b(), modInB);
	}

	private void modifyInA() {
		IByteArrayHolder c = getTheObject(a());

		c.setBytes(modInA);
		a().provider().update(c);
		a().provider().commit();

		ensureNames(a(), modInA);
	}

	private void replicate3() {
		replicateAll(a().provider(), b().provider());

		ensureNames(a(), modInA);
		ensureNames(b(), modInA);
	}

	private void ensureNames(DrsProviderFixture fixture, byte[] bs) {
		ensureOneInstance(fixture, ByteArrayHolder.class);
		IByteArrayHolder c = getTheObject(fixture);
		ArrayAssert.areEqual(c.getBytes(), bs);
	}

	private IByteArrayHolder getTheObject(DrsProviderFixture fixture) {
		return (ByteArrayHolder) getOneInstance(fixture,
				ByteArrayHolder.class);
	}

	static byte[] createByteArray() {
		byte[] bytes = new byte[ARRAY_LENGTH];
		for (byte i = 0; i < bytes.length; ++i) {
			bytes[i] = i;
		}
		return bytes;
	}
}