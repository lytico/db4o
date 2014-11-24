/* Copyright (C) 2010  Versant Inc.   http://www.db4o.com */
using System;

namespace Db4objects.Db4o.Tests.CLI2.Collections.Transparent
{
	[Serializable]
	public class AbstractCollectionElement : ICollectionElement
	{
#if SILVERLIGHT
		public string _name;
#else
		protected string _name;
#endif

		protected AbstractCollectionElement(string name)
		{
			_name = name;	
		}

		public int CompareTo(ICollectionElement other)
		{
			if(Name == null)
			{
				if(other.Name == null)
				{
					return 0;
				}
				
				return -1;
			}
		
			return Name.CompareTo(other.Name);
		}

		public string Name
		{
			get
			{
				ReadFieldAccess();
				return _name;
			}
		}

		protected virtual void ReadFieldAccess()
		{
			// Do nothing
		}
	}
}
