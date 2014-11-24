/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit.Extensions;

namespace Db4objects.Db4o.Tests.Common.Assorted
{
	public class UnavailableClassTestCaseBase : AbstractDb4oTestCase
	{
		public UnavailableClassTestCaseBase() : base()
		{
		}

		/// <exception cref="System.Exception"></exception>
		protected virtual void ReopenHidingClasses(Type[] classes)
		{
			Fixture().Close();
			Fixture().Config().ReflectWith(new ExcludingReflector(classes));
			Open();
		}

		/// <exception cref="System.Exception"></exception>
		private void Open()
		{
			Fixture().Open(this);
		}

		/// <exception cref="System.Exception"></exception>
		private void CloseAndResetConfig()
		{
			Fixture().Close();
			Fixture().ResetConfig();
		}
	}
}
