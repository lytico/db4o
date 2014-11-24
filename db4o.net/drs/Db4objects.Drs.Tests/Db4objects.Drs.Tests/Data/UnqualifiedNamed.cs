/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

namespace Db4objects.Drs.Tests.Data
{
	/// <summary>This class has just the regular short name, like stated in most JDO tutorials
	/// 	</summary>
	public class UnqualifiedNamed
	{
		private string data;

		public UnqualifiedNamed(string data)
		{
			this.data = data;
		}

		public virtual string GetData()
		{
			return data;
		}

		public virtual void SetData(string data)
		{
			this.data = data;
		}
	}
}
