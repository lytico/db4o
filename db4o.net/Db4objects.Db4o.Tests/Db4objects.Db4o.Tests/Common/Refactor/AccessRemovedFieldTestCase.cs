/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Tests.Common.Refactor;

namespace Db4objects.Db4o.Tests.Common.Refactor
{
	public class AccessRemovedFieldTestCase : AccessFieldTestCaseBase, ITestLifeCycle
	{
		private static readonly Type FieldType = typeof(int);

		private static readonly string FieldName = "_value";

		private const int FieldValue = 42;

		public virtual void TestRemovedField()
		{
			Type targetClazz = typeof(AccessRemovedFieldTestCase.RemovedFieldData);
			RenameClass(typeof(AccessRemovedFieldTestCase.OriginalData), ReflectPlatform.FullyQualifiedName
				(targetClazz));
			AssertField(targetClazz, FieldName, FieldType, FieldValue);
		}

		protected override object NewOriginalData()
		{
			return new AccessRemovedFieldTestCase.OriginalData(FieldValue);
		}

		public class OriginalData
		{
			public int _value;

			public string _name;

			public OriginalData(int value)
			{
				_value = value;
			}
		}

		public class RemovedFieldData
		{
			public string _name;
		}
	}
}
