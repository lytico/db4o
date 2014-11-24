/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

namespace Db4objects.Db4o.Tests.Common.Reflect.Custom
{
	public class PersistentEntryTemplate
	{
		public string className;

		public string[] fieldNames;

		public object[] fieldValues;

		public PersistentEntryTemplate(string className, string[] fieldNames, object[] fieldValues
			)
		{
			this.className = className;
			this.fieldNames = fieldNames;
			this.fieldValues = fieldValues;
		}
	}
}
