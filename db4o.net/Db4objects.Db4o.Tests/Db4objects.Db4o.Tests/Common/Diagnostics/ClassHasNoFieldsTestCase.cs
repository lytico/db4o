/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Fixtures;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Diagnostic;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Tests.Common.Diagnostics;

namespace Db4objects.Db4o.Tests.Common.Diagnostics
{
	public class ClassHasNoFieldsTestCase : AbstractDb4oTestCase, ICustomClientServerConfiguration
	{
		/// <exception cref="System.Exception"></exception>
		protected override void Configure(IConfiguration config)
		{
			config.Diagnostic().AddListener(_collector);
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void ConfigureClient(IConfiguration config)
		{
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void ConfigureServer(IConfiguration config)
		{
			Configure(config);
		}

		public virtual void TestDiagnostic()
		{
			Store(new ClassHasNoFieldsTestCase.Item());
			IList diagnostics = NativeCollections.Filter(_collector.Diagnostics(), new _IPredicate4_34
				());
			Assert.AreEqual(1, diagnostics.Count);
			ClassHasNoFields diagnostic = (ClassHasNoFields)((IDiagnostic)diagnostics[0]);
			Assert.AreEqual(ReflectPlatform.FullyQualifiedName(typeof(ClassHasNoFieldsTestCase.Item
				)), diagnostic.Reason());
		}

		private sealed class _IPredicate4_34 : IPredicate4
		{
			public _IPredicate4_34()
			{
			}

			public bool Match(object candidate)
			{
				return ((IDiagnostic)candidate) is ClassHasNoFields;
			}
		}

		private DiagnosticCollector _collector = new DiagnosticCollector();

		public class Item
		{
		}
	}
}
