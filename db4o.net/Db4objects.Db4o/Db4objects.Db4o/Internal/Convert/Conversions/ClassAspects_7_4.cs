/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal.Convert;

namespace Db4objects.Db4o.Internal.Convert.Conversions
{
	/// <exclude></exclude>
	public class ClassAspects_7_4 : Conversion
	{
		public const int Version = 7;

		public override void Convert(ConversionStage.SystemUpStage stage)
		{
			stage.File().ClassCollection().WriteAllClasses();
		}
	}
}
