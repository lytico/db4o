/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;

namespace Db4oUnit
{
	public class ClassLevelFixtureTestSuite : OpaqueTestSuiteBase
	{
		public static readonly string TeardownMethodName = "classTearDown";

		public static readonly string SetupMethodName = "classSetUp";

		private readonly Type _clazz;

		public ClassLevelFixtureTestSuite(Type clazz, IClosure4 tests) : base(tests)
		{
			_clazz = clazz;
		}

		/// <exception cref="System.Exception"></exception>
		protected override void SuiteSetUp()
		{
			Reflection4.InvokeStatic(_clazz, SetupMethodName);
		}

		/// <exception cref="System.Exception"></exception>
		protected override void SuiteTearDown()
		{
			Reflection4.InvokeStatic(_clazz, TeardownMethodName);
		}

		public override string Label()
		{
			return _clazz.FullName;
		}

		protected override OpaqueTestSuiteBase Transmogrified(IClosure4 tests)
		{
			return new Db4oUnit.ClassLevelFixtureTestSuite(_clazz, tests);
		}
	}
}
