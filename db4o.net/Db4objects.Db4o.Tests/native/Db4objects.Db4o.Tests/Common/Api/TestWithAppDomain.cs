/* Copyright (C) 2004 - 2008  Versant Inc.  http://www.db4o.com */

using System;
using System.IO;
using Db4oUnit.Extensions.Util;

namespace Db4objects.Db4o.Tests.Common.Api
{
#if !CF && !SILVERLIGHT
	public class TestWithAppDomain : TestWithTempFile
	{
		protected AppDomain _domain;

		public override void SetUp()
		{
			base.SetUp();

			string applicationBase = Path.Combine(Path.GetDirectoryName(TempFile()), GetType().Name);
			IOServices.CopyEnclosingAssemblyTo(typeof(Db4oEmbedded), applicationBase);

			AppDomainSetup setup = new AppDomainSetup();
			setup.ApplicationBase = applicationBase;
			
			_domain = AppDomain.CreateDomain(GetType().Name, null, setup);

		}

		protected string Db4oAssemblyPath()
		{
			return typeof(Db4oEmbedded).Module.FullyQualifiedName;
		}

		public override void TearDown()
		{
			AppDomain.Unload(_domain);
			base.TearDown();
		}

		protected void ExecuteAssemblyInAppDomain(string assemblyFile, params string[] args)
		{
			_domain.ExecuteAssembly(assemblyFile, null, args);
		}
	}
#endif
}
