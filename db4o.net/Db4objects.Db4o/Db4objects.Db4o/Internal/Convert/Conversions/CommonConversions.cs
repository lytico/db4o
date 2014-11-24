/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal.Convert;
using Db4objects.Db4o.Internal.Convert.Conversions;

namespace Db4objects.Db4o.Internal.Convert.Conversions
{
	/// <exclude></exclude>
	public class CommonConversions
	{
		public static void Register(Converter converter)
		{
			converter.Register(ClassIndexesToBTrees_5_5.Version, new ClassIndexesToBTrees_5_5
				());
			converter.Register(FieldIndexesToBTrees_5_7.Version, new FieldIndexesToBTrees_5_7
				());
			converter.Register(ClassAspects_7_4.Version, new ClassAspects_7_4());
			converter.Register(ReindexNetDateTime_7_8.Version, new ReindexNetDateTime_7_8());
			converter.Register(DropEnumClassIndexes_7_10.Version, new DropEnumClassIndexes_7_10
				());
			converter.Register(DropGuidClassAndFieldIndexes_7_12.Version, new DropGuidClassAndFieldIndexes_7_12
				());
			converter.Register(DropDateTimeOffsetClassIndexes_7_12.Version, new DropDateTimeOffsetClassIndexes_7_12
				());
			converter.Register(VersionNumberToCommitTimestamp_8_0.Version, new VersionNumberToCommitTimestamp_8_0
				());
		}
	}
}
