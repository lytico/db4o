/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Config.Attributes;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Tests.Jre5.Annotation;

namespace Db4objects.Db4o.Tests.Jre5.Annotation
{
	public class IndexedAnnotationTestCase : AbstractDb4oTestCase
	{
		private class DataAnnotated
		{
			[Indexed]
			private int _id;

			public DataAnnotated(int id)
			{
				this._id = id;
			}

			public override string ToString()
			{
				return "DataAnnotated(" + _id + ")";
			}
		}

		private class DataNotAnnotated
		{
			private int _id;

			public DataNotAnnotated(int id)
			{
				this._id = id;
			}

			public override string ToString()
			{
				return "DataNotAnnotated(" + _id + ")";
			}
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestIndexed()
		{
			StoreData();
			AssertIndexed();
			Reopen();
			AssertIndexed();
		}

		private void StoreData()
		{
			Db().Store(new IndexedAnnotationTestCase.DataAnnotated(42));
			Db().Store(new IndexedAnnotationTestCase.DataNotAnnotated(43));
		}

		private void AssertIndexed()
		{
			AssertIndexed(typeof(IndexedAnnotationTestCase.DataNotAnnotated), false);
			AssertIndexed(typeof(IndexedAnnotationTestCase.DataAnnotated), true);
		}

		private void AssertIndexed(Type clazz, bool expected)
		{
			IStoredClass storedClass = FileSession().StoredClass(clazz);
			IStoredField storedField = storedClass.StoredField("_id", typeof(int));
			Assert.AreEqual(expected, storedField.HasIndex());
		}

		public static void Main(string[] args)
		{
			new IndexedAnnotationTestCase().RunSoloAndClientServer();
		}
	}
}
