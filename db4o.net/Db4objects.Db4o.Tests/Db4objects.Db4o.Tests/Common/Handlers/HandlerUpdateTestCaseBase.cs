/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Handlers;
using Db4objects.Db4o.Tests.Util;

namespace Db4objects.Db4o.Tests.Common.Handlers
{
	public abstract class HandlerUpdateTestCaseBase : FormatMigrationTestCaseBase
	{
		public class Holder
		{
			public object[] _values;

			public object _arrays;
		}

		private int _handlerVersion;

		protected override string FileNamePrefix()
		{
			return "migrate_" + TypeName() + "_";
		}

		protected override void Store(IObjectContainerAdapter objectContainer)
		{
			HandlerUpdateTestCaseBase.Holder holder = new HandlerUpdateTestCaseBase.Holder();
			holder._values = CreateValues();
			holder._arrays = CreateArrays();
			objectContainer.Store(holder);
		}

		protected override void AssertObjectsAreReadable(IExtObjectContainer objectContainer
			)
		{
			HandlerUpdateTestCaseBase.Holder holder = RetrieveHolderInstance(objectContainer);
			objectContainer.Activate(holder, int.MaxValue);
			AssertValues(objectContainer, holder._values);
			AssertArrays(objectContainer, holder._arrays);
			AssertQueries(objectContainer);
		}

		private HandlerUpdateTestCaseBase.Holder RetrieveHolderInstance(IExtObjectContainer
			 objectContainer)
		{
			IQuery q = objectContainer.Query();
			q.Constrain(typeof(HandlerUpdateTestCaseBase.Holder));
			IObjectSet objectSet = q.Execute();
			HandlerUpdateTestCaseBase.Holder holder = (HandlerUpdateTestCaseBase.Holder)objectSet
				.Next();
			InvestigateHandlerVersion(objectContainer, holder);
			return holder;
		}

		protected override void Update(IExtObjectContainer objectContainer)
		{
			HandlerUpdateTestCaseBase.Holder holder = RetrieveHolderInstance(objectContainer);
			UpdateValues(holder._values);
			UpdateArrays(holder._arrays);
			objectContainer.Store(holder, int.MaxValue);
		}

		protected override void AssertObjectsAreUpdated(IExtObjectContainer objectContainer
			)
		{
			HandlerUpdateTestCaseBase.Holder holder = RetrieveHolderInstance(objectContainer);
			AssertUpdatedValues(holder._values);
			AssertUpdatedArrays(holder._arrays);
		}

		private void InvestigateHandlerVersion(IExtObjectContainer objectContainer, object
			 obj)
		{
			_handlerVersion = VersionServices.SlotHandlerVersion(objectContainer, obj);
		}

		protected abstract string TypeName();

		protected abstract object[] CreateValues();

		protected abstract object CreateArrays();

		protected abstract void AssertValues(IExtObjectContainer objectContainer, object[]
			 values);

		protected abstract void AssertArrays(IExtObjectContainer objectContainer, object 
			obj);

		protected virtual void AssertQueries(IExtObjectContainer objectContainer)
		{
		}

		// override on demand to check queries
		protected virtual int[] CastToIntArray(object obj)
		{
			ObjectByRef byRef = new ObjectByRef(obj);
			return (int[])byRef.value;
		}

		// Bug in the oldest format: 
		// It accidentally converted int[] arrays to Integer[] arrays.
		protected virtual int Db4oHandlerVersion()
		{
			return _handlerVersion;
		}

		protected virtual void UpdateValues(object[] values)
		{
		}

		// Override to check updates also
		protected virtual void UpdateArrays(object obj)
		{
		}

		// Override to check updates also
		protected virtual void AssertUpdatedValues(object[] values)
		{
		}

		// Override to check updates also
		protected virtual void AssertUpdatedArrays(object obj)
		{
		}

		// Override to check updates also
		protected virtual bool UsesNullMarkerValue()
		{
			return Db4oHandlerVersion() == 0;
		}
	}
}
