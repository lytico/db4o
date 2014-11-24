/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions.Fixtures;
using Db4oUnit.Extensions.Util;
using Db4objects.Db4o.Activation;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Diagnostic;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.TA;
using Db4objects.Db4o.Tests.Common.TA;
using Db4objects.Db4o.Tests.Common.TA.Diagnostics;

namespace Db4objects.Db4o.Tests.Common.TA.Diagnostics
{
	public class TransparentActivationDiagnosticsTestCase : TransparentActivationTestCaseBase
		, IOptOutMultiSession, IOptOutDefragSolo
	{
		public class SomeTAAwareData
		{
			public int _id;

			public SomeTAAwareData(int id)
			{
				_id = id;
			}
		}

		public class SomeOtherTAAwareData : IActivatable
		{
			public TransparentActivationDiagnosticsTestCase.SomeTAAwareData _data;

			public virtual void Bind(IActivator activator)
			{
			}

			public virtual void Activate(ActivationPurpose purpose)
			{
			}

			public SomeOtherTAAwareData(TransparentActivationDiagnosticsTestCase.SomeTAAwareData
				 data)
			{
				_data = data;
			}
		}

		public class NotTAAwareData
		{
			public TransparentActivationDiagnosticsTestCase.SomeTAAwareData _data;

			public NotTAAwareData(TransparentActivationDiagnosticsTestCase.SomeTAAwareData data
				)
			{
				_data = data;
			}
		}

		private class DiagnosticsRegistered
		{
			public int _registeredCount = 0;
		}

		private readonly TransparentActivationDiagnosticsTestCase.DiagnosticsRegistered _registered
			 = new TransparentActivationDiagnosticsTestCase.DiagnosticsRegistered();

		private readonly IDiagnosticListener _checker;

		private IDiagnosticConfiguration _diagnostic;

		public TransparentActivationDiagnosticsTestCase()
		{
			_checker = new _IDiagnosticListener_60(this);
		}

		private sealed class _IDiagnosticListener_60 : IDiagnosticListener
		{
			public _IDiagnosticListener_60(TransparentActivationDiagnosticsTestCase _enclosing
				)
			{
				this._enclosing = _enclosing;
			}

			public void OnDiagnostic(IDiagnostic diagnostic)
			{
				if (!(diagnostic is NotTransparentActivationEnabled))
				{
					return;
				}
				NotTransparentActivationEnabled taDiagnostic = (NotTransparentActivationEnabled)diagnostic;
				Assert.AreEqual(CrossPlatformServices.FullyQualifiedName(typeof(TransparentActivationDiagnosticsTestCase.NotTAAwareData
					)), ((ClassMetadata)taDiagnostic.Reason()).GetName());
				this._enclosing._registered._registeredCount++;
			}

			private readonly TransparentActivationDiagnosticsTestCase _enclosing;
		}

		protected override void Configure(IConfiguration config)
		{
			base.Configure(config);
			_diagnostic = config.Diagnostic();
			_diagnostic.AddListener(_checker);
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Db4oTearDownBeforeClean()
		{
			WorkaroundOsgiConfigCloningBehavior();
			Db().Ext().Configure().Diagnostic().RemoveAllListeners();
			base.Db4oTearDownBeforeClean();
		}

		private void WorkaroundOsgiConfigCloningBehavior()
		{
			// fix for Osgi config cloning behavior - see Db4oOSGiBundleFixture
			_diagnostic.RemoveAllListeners();
		}

		public virtual void TestTADiagnostics()
		{
			Store(new TransparentActivationDiagnosticsTestCase.SomeTAAwareData(1));
			Assert.AreEqual(0, _registered._registeredCount);
			Store(new TransparentActivationDiagnosticsTestCase.SomeOtherTAAwareData(new TransparentActivationDiagnosticsTestCase.SomeTAAwareData
				(2)));
			Assert.AreEqual(0, _registered._registeredCount);
			Store(new TransparentActivationDiagnosticsTestCase.NotTAAwareData(new TransparentActivationDiagnosticsTestCase.SomeTAAwareData
				(3)));
			Assert.AreEqual(1, _registered._registeredCount);
		}

		public static void Main(string[] args)
		{
			new TransparentActivationDiagnosticsTestCase().RunAll();
		}
	}
}
