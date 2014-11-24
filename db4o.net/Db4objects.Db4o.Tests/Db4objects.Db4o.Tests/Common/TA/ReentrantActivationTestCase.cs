/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o;
using Db4objects.Db4o.Activation;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Delete;
using Db4objects.Db4o.Marshall;
using Db4objects.Db4o.TA;
using Db4objects.Db4o.Tests.Common.TA;
using Db4objects.Db4o.Typehandlers;

namespace Db4objects.Db4o.Tests.Common.TA
{
	public class ReentrantActivationTestCase : AbstractDb4oTestCase
	{
		/// <exception cref="System.Exception"></exception>
		protected override void Configure(IConfiguration config)
		{
			config.Add(new TransparentPersistenceSupport());
			config.RegisterTypeHandler(new SingleClassTypeHandlerPredicate(typeof(ReentrantActivationTestCase.ReentratActivatableItem
				)), new ReentrantActivationTestCase.ReentrantActivationTypeHandler());
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			Store(new ReentrantActivationTestCase.ReentratActivatableItem());
		}

		public virtual void Test()
		{
			ReentrantActivationTestCase.ReentratActivatableItem item = ((ReentrantActivationTestCase.ReentratActivatableItem
				)RetrieveOnlyInstance(typeof(ReentrantActivationTestCase.ReentratActivatableItem
				)));
			Assert.IsFalse(item.Activated());
			item.ActivateForRead();
			Assert.IsTrue(item.Activated());
			AssertNotActivatedForWrite(item);
		}

		private void AssertNotActivatedForWrite(ReentrantActivationTestCase.ReentratActivatableItem
			 item)
		{
			Commit();
			Assert.IsFalse(item.Written());
		}

		public class ReentratActivatableItem : IActivatable
		{
			private IActivator _activator;

			[System.NonSerialized]
			private bool _activated;

			[System.NonSerialized]
			private bool _written;

			public virtual void Activate(ActivationPurpose purpose)
			{
				_activator.Activate(purpose);
				_activated = true;
			}

			public virtual void ObjectOnUpdate(IObjectContainer container)
			{
				_written = true;
			}

			public virtual void Bind(IActivator activator)
			{
				_activator = activator;
			}

			public virtual void ActivateForRead()
			{
				Activate(ActivationPurpose.Read);
			}

			public virtual void ActivateForWrite()
			{
				Activate(ActivationPurpose.Write);
			}

			public virtual bool Activated()
			{
				return _activated;
			}

			public virtual bool Written()
			{
				return _written;
			}
		}

		public class ReentrantActivationTypeHandler : IReferenceTypeHandler
		{
			public virtual void Activate(IReferenceActivationContext context)
			{
				ReentrantActivationTestCase.ReentratActivatableItem item = (ReentrantActivationTestCase.ReentratActivatableItem
					)context.PersistentObject();
				item.ActivateForWrite();
			}

			public virtual void Defragment(IDefragmentContext context)
			{
			}

			/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
			public virtual void Delete(IDeleteContext context)
			{
			}

			public virtual void Write(IWriteContext context, object obj)
			{
			}
		}
	}
}
