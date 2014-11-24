/* Copyright (C) 2012  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;

namespace Db4objects.Db4o.Tests.CLI1.Handlers
{
	internal class GUIDHandlerUpdateIndexedOnCurrentVersionTestCase : GUIDHandlerUpdateTestCase
    {
#if !SILVERLIGHT && !CF
		protected override void AssertObjectsAreReadable(Ext.IExtObjectContainer objectContainer)
		{
			if (IsEmbeeded(objectContainer))
			{
				Assert.IsTrue(objectContainer.Ext().StoredClass(typeof(Item)).StoredField("Typed", typeof(Guid)).HasIndex());
			}
			base.AssertObjectsAreReadable(objectContainer);
		}
#endif
		protected override bool IndexedOnCurrentVersion
		{
			get { return true; }
		}

		protected override bool IndexedOnOldVersion
		{
			get { return false; }
		}
    }
}
