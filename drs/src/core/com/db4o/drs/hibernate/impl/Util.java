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
package com.db4o.drs.hibernate.impl;

import java.io.Serializable;
import java.lang.reflect.Array;
import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.ResultSetMetaData;
import java.sql.SQLException;
import java.util.List;

import org.hibernate.Criteria;
import org.hibernate.Query;
import org.hibernate.Session;
import org.hibernate.SessionFactory;
import org.hibernate.Transaction;
import org.hibernate.cfg.Configuration;
import org.hibernate.criterion.Restrictions;

import com.db4o.drs.hibernate.metadata.MySignature;
import com.db4o.drs.hibernate.metadata.ObjectReference;
import com.db4o.drs.hibernate.metadata.ProviderSignature;
import com.db4o.drs.hibernate.metadata.Record;
import com.db4o.drs.hibernate.metadata.Uuid;
import com.db4o.ext.Db4oUUID;

public final class Util {
	public static final Class[] _metadataClasses = new Class[]{
			Record.class, ProviderSignature.class,
			ObjectReference.class};

	public static boolean isAssignableFromInternalObject(Class claxx) {
		for (Class<?> aClass : _metadataClasses)
			if (aClass.isAssignableFrom(claxx)) return true;
		return false;
	}

	public static Boolean isInstanceOfInternalObject(Object entity) {
		for (Class aClass : _metadataClasses)
			if (aClass.isInstance(entity)) return true;
		return false;
	}

	public static void dumpTable(HibernateReplicationProvider p, String s) {
		dumpTable(p.getName(), p.getSession(), s);
	}

	public static void dumpTable(String providerName, Connection con, String tableName) {
		ResultSet rs = null;

		try {
			System.out.println("providerName = " + providerName + ", table = " + tableName);
			String sql = "SELECT * FROM " + tableName;
			rs = con.createStatement().executeQuery(sql);
			ResultSetMetaData metaData = rs.getMetaData();
			int columnCount = metaData.getColumnCount();
			for (int i = 1; i <= columnCount; i++) {
				System.out.print(metaData.getColumnName(i) + "\t|");
			}
			System.out.println();
			while (rs.next()) {
				for (int i = 1; i <= columnCount; i++) {
					System.out.print(rs.getObject(i) + "\t|");
				}
				System.out.println();
			}
			System.out.println("Printing table = " + tableName + " - done");
		} catch (SQLException e) {
			throw new RuntimeException(e);
		} finally {
			closeResultSet(rs);
		}
	}

	public static void dumpTable(String providerName, Session sess, String tableName) {
		dumpTable(providerName, sess.connection(), tableName);
	}

	private static void closePreparedStatement(PreparedStatement ps) {
		if (ps != null) {
			try {
				ps.close();
			} catch (SQLException e) {
				throw new RuntimeException(e);
			}
		}
	}

	private static void closeResultSet(ResultSet rs) {
		if (rs != null) {
			try {
				rs.close();
			} catch (SQLException e) {
				throw new RuntimeException(e);
			}
		}
	}

	public static void addClass(Configuration cfg, Class aClass) {
		if (cfg.getClassMapping(aClass.getName()) == null)
			cfg.addClass(aClass);
	}

	public static MySignature genMySignature(Session session) {
		final List sigs = session.createCriteria(MySignature.class).list();
		final int mySigCount = sigs.size();

		if (mySigCount < 1) {
			MySignature out = MySignature.generateSignature();
			session.save(out);
			return out;
		} else if (mySigCount == 1)
			return (MySignature) sigs.get(0);
		else
			throw new RuntimeException("Number of MySignature should be exactly 1, but i got " + mySigCount);
	}

	public static void initMySignature(Configuration cfg) {
		SessionFactory sf = cfg.buildSessionFactory();
		Session session = sf.openSession();
		Transaction tx = session.beginTransaction();

		if (session.createCriteria(MySignature.class).list().size() < 1)
			session.save(MySignature.generateSignature());

		tx.commit();
		session.close();
		sf.close();
	}

	protected static String flattenBytes(byte[] b) {
		String out = "";
		for (int i = 0; i < b.length; i++) {
			out += ", " + b[i];
		}
		return out;
	}

	protected static void sleep(int i, String s) {
		System.out.println(s);
		try {
			Thread.sleep(i * 1000);
		} catch (InterruptedException e) {
			throw new RuntimeException(e.getMessage());
		}
	}

	public static Db4oUUID translate(Uuid uuid) {
		return new Db4oUUID(uuid.getCreated(), uuid.getProvider().getSignature());
	}

	public static long castAsLong(Serializable id) {
		if (!(id instanceof Long))
			throw new IllegalStateException("You must use 'long' as the type of the hibernate id");
		return (Long) id;
	}
	
	public static long getMaxReplicationRecordVersion(Session s) {
		String hql = "SELECT max(r." + Record.Fields.TIME + ") FROM Record r";
		
		List results = s.createQuery(hql).list();
		
		if (results.size()!=1)
			throw new RuntimeException("result size must be 1");
		
		final Object ver = results.iterator().next();
		
		if (ver==null)
			return 0;
		else
			return ((Long)ver).longValue();
	}

	static ObjectReference getObjectReferenceById(Session session, String className, long id) {
		Criteria criteria = session.createCriteria(ObjectReference.class);
		criteria.add(Restrictions.eq(ObjectReference.Fields.TYPED_ID, id));
		criteria.add(Restrictions.eq(ObjectReference.Fields.CLASS_NAME, className));
		List list = criteria.list();

		if (list.size() == 0)
			return null;
		else if (list.size() == 1)
			return (ObjectReference) list.get(0);
		else
			throw new RuntimeException("Duplicated uuid");
	}
	
	static Uuid getUuidById(Session session, String className, long id) {
		final ObjectReference ref = getObjectReferenceById(session, className, id);
		
		if (ref == null)
			return null;
		else 
			return ref.getUuid();
	}

	public static Uuid getUuid(Session session, Object obj) {
		long id = Util.castAsLong(session.getIdentifier(obj));

		ObjectReference of = getObjectReferenceById(session, obj.getClass().getName(), id);
		if (of == null) return null;
		return of.getUuid();
	}

	public static ObjectReference getByUUID(Session session, Uuid uuid) {
		String alias = "objRef";
		String uuidPath = alias + "." + ObjectReference.Fields.UUID + ".";
		String queryString = "from " + "ObjectReference"
				+ " as " + alias + " where " + uuidPath + Uuid.Fields.CREATED + "=?"
				+ " AND " + uuidPath + Uuid.Fields.PROVIDER + "." + ProviderSignature.Fields.SIG + "=?";
		Query c = session.createQuery(queryString);
		c.setLong(0, uuid.getCreated());
		c.setBinary(1, uuid.getProvider().getSignature());

		final List exisitings = c.list();
		int count = exisitings.size();

		if (count == 0)
			return null;
		else if (count > 1)
			throw new RuntimeException("Only one ObjectReference should exist");
		else {
			return (ObjectReference) exisitings.get(0);
		}
	}

	public static Object[] removeElement(Object[] array, Object element) {
		final int length = array.length;
		int index = indexOf(array, element);

		if (index > -1) {
			Object[] out = newArray(array, length - 1);
			System.arraycopy(array, 0, out, 0, index);
			if (index < length - 1) {
				System.arraycopy(array, index + 1, out, index, length - index - 1);
			}
			return out;
		} else {
			return clone(array);
		}
	}

	private static int indexOf(Object[] array, Object element) {
		for (int i = 0; i < array.length; i++)
			if (array[i].equals(element))
				return i;
		return -1;
	}

	private static Object[] clone(Object[] array) {
		Object[] out = newArray(array, array.length);
		System.arraycopy(array, 0, out, 0, array.length);
		return out;
	}

	public static Object[] add(Object[] array, Object element) {
		final int length = array.length;
		Object[] out = newArray(array, length + 1);
		System.arraycopy(array, 0, out, 0, length);
		out[length] = element;
		return out;
	}

	private static Object[] newArray(Object[] array, int length) {
		return (Object[]) Array.newInstance(array.getClass().getComponentType(), length);
	}
}
