/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Tests.Common.Refactor;

namespace Db4objects.Db4o.Tests.Common.Refactor
{
	public class AccessOldFieldVersionsTestCase : AccessFieldTestCaseBase, ITestLifeCycle
	{
		private static readonly Type OrigType = typeof(int);

		private static readonly string FieldName = "_value";

		private const int OrigValue = 42;

		public virtual void TestRetypedField()
		{
			Type targetClazz = typeof(AccessOldFieldVersionsTestCase.RetypedFieldData);
			RenameClass(typeof(AccessOldFieldVersionsTestCase.OriginalData), ReflectPlatform.
				FullyQualifiedName(targetClazz));
			AssertField(targetClazz, FieldName, OrigType, OrigValue);
		}

		protected override object NewOriginalData()
		{
			return new AccessOldFieldVersionsTestCase.OriginalData(OrigValue);
		}

		public class OriginalData
		{
			public int _value;

			public OriginalData(int value)
			{
				_value = value;
			}
		}

		public class RetypedFieldData
		{
			public string _value;

			public RetypedFieldData(string value)
			{
				_value = value;
			}
		}
	}
}
