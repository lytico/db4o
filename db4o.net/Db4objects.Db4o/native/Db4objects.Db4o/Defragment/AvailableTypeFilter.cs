/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */

using Db4objects.Db4o.Ext;

using System;

namespace Db4objects.Db4o.Defragment
{
	public class AvailableTypeFilter : IStoredClassFilter
	{
		
		/// <param name="storedClass">StoredClass instance to be checked</param>
		/// <returns>true, if the given StoredClass instance should be accepted, false otherwise.
		/// 	</returns>
		public bool Accept(IStoredClass storedClass)
		{
			return System.Type.GetType(storedClass.GetName(),false)!=null;
		}
		
	}
}
