/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

namespace Db4objects.Drs.Tests.Data
{
	public sealed class UntypedFieldData
	{
		private int id;

		public UntypedFieldData()
		{
		}

		public UntypedFieldData(int value)
		{
			SetId(value);
		}

		public override bool Equals(object obj)
		{
			Db4objects.Drs.Tests.Data.UntypedFieldData other = (Db4objects.Drs.Tests.Data.UntypedFieldData
				)obj;
			return GetId() == other.GetId();
		}

		public void SetId(int id)
		{
			this.id = id;
		}

		public int GetId()
		{
			return id;
		}
	}
}
