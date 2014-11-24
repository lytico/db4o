/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

using System;
using System.Globalization;
using Sharpen.Lang;
using Db4objects.Db4o;

namespace Db4objects.Db4o.Config
{
	/// <exclude />
	public class TCultureInfo : IObjectConstructor
	{
		public Object OnInstantiate(IObjectContainer store, object stored)
		{
			return new CultureInfo((string)stored);
		}

		public Object OnStore(IObjectContainer store, object obj)
		{
			CultureInfo culture = (CultureInfo)obj;
			return culture.Name;
		}

		public void OnActivate(IObjectContainer container, object applicationObject, object storedObject)
		{
		}

		public Type StoredClass()
		{		
			return typeof(string);
		}
	}
}
