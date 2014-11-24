/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4objects.Db4o.Activation;
using Db4objects.Db4o.TA;
using Db4objects.Db4o.Tests.Common.TA;

namespace Db4objects.Db4o.Tests.Common.TA
{
	public class ActivationBasedOnConcreteTypeTestCase : TransparentActivationTestCaseBase
	{
		public static void Main(string[] args)
		{
			new ActivationBasedOnConcreteTypeTestCase().RunNetworking();
		}

		public class NonActivatableParent
		{
		}

		public class ActivatableChild : ActivationBasedOnConcreteTypeTestCase.NonActivatableParent
			, IActivatable
		{
			[System.NonSerialized]
			private IActivator _activator;

			public virtual void Bind(IActivator activator)
			{
				if (_activator == activator)
				{
					return;
				}
				if (activator != null && _activator != null)
				{
					throw new InvalidOperationException();
				}
				_activator = activator;
			}

			public virtual void Activate(ActivationPurpose purpose)
			{
				if (_activator == null)
				{
					return;
				}
				_activator.Activate(purpose);
			}

			public int _value;

			public ActivatableChild(int value)
			{
				_value = value;
			}
		}

		public class Holder
		{
			public Holder(ActivationBasedOnConcreteTypeTestCase.NonActivatableParent @object)
			{
				_object = @object;
			}

			public ActivationBasedOnConcreteTypeTestCase.NonActivatableParent _object;
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			Store(new ActivationBasedOnConcreteTypeTestCase.Holder(new ActivationBasedOnConcreteTypeTestCase.ActivatableChild
				(42)));
		}

		public virtual void TestActivationIsBasedOnConcretType()
		{
			ActivationBasedOnConcreteTypeTestCase.Holder holder = ((ActivationBasedOnConcreteTypeTestCase.Holder
				)RetrieveOnlyInstance(typeof(ActivationBasedOnConcreteTypeTestCase.Holder)));
			Assert.IsFalse(Db().Ext().IsActive(holder._object));
		}
	}
}
