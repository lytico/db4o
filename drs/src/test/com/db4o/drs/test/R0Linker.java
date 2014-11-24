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

import com.db4o.drs.inside.TestableReplicationProviderInside;
import com.db4o.drs.test.data.*;


class R0Linker {

	R0 r0;
	R1 r1;
	R2 r2;
	R3 r3;
	R4 r4;

	R0Linker() {
		r0 = new R0();
		r1 = new R1();
		r2 = new R2();
		r3 = new R3();
		r4 = new R4();
	}

	void setNames(String name) {
		r0.setName("0" + name);
		r1.setName("1" + name);
		r2.setName("2" + name);
		r3.setName("3" + name);
		r4.setName("4" + name);
	}

	void linkCircles() {
		linkList();
		r1.setCircle1(r0);
		r2.setCircle2(r0);
		r3.setCircle3(r0);
		r4.setCircle4(r0);
	}

	void linkList() {
		r0.setR1(r1);
		r1.setR2(r2);
		r2.setR3(r3);
		r3.setR4(r4);
	}

	void linkThis() {
		r0.setR0(r0);
		r1.setR1(r1);
		r2.setR2(r2);
		r3.setR3(r3);
		r4.setR4(r4);
	}

	void linkBack() {
		r1.setR0(r0);
		r2.setR1(r1);
		r3.setR2(r2);
		r4.setR3(r3);
	}

	public void store(TestableReplicationProviderInside provider) {
		provider.storeNew(r4);
		provider.storeNew(r3);
		provider.storeNew(r2);
		provider.storeNew(r1);
		provider.storeNew(r0);
	}
}
