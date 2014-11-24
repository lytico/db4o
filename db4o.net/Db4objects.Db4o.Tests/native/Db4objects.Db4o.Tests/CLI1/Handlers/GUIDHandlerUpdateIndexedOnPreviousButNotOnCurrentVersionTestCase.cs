/* Copyright (C) 2012  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;

namespace Db4objects.Db4o.Tests.CLI1.Handlers
{
	internal class GUIDHandlerUpdateIndexedOnPreviousButNotOnCurrentVersionTestCase : GUIDHandlerUpdateTestCase
    {
		protected override void AssertObjectsAreReadable(Ext.IExtObjectContainer objectContainer)
		{
			Assert.IsFalse(objectContainer.Ext().StoredClass(typeof(Item)).StoredField("Typed", typeof(Guid)).HasIndex());
			base.AssertObjectsAreReadable(objectContainer);
		}

		protected override bool IndexedOnCurrentVersion
		{
			get { return false; }
		}

		protected override bool IndexedOnOldVersion
		{
			get { return true; }
		}
	}
}
