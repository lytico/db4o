/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal.Convert;

namespace Db4objects.Db4o.Internal.Convert
{
	/// <exclude></exclude>
	public abstract class Conversion
	{
		/// <param name="stage"></param>
		public virtual void Convert(ConversionStage.ClassCollectionAvailableStage stage)
		{
		}

		/// <param name="stage"></param>
		public virtual void Convert(ConversionStage.SystemUpStage stage)
		{
		}
	}
}
