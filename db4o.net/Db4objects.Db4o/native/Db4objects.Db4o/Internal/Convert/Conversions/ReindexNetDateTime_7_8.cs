/* Copyright (C) 2004 - 2009   Versant Inc.   http://www.db4o.com */
using System;

namespace Db4objects.Db4o.Internal.Convert.Conversions
{
    partial class ReindexNetDateTime_7_8 : Conversion
    {
		public override void Convert(ConversionStage.SystemUpStage stage)
		{
			ReindexDateTimeFields(stage);
		}

		private static void ReindexDateTimeFields(ConversionStage stage)
		{
			var reindexer = new FieldReindexer<DateTime>();
			
			ClassMetadataIterator i = stage.File().ClassCollection().Iterator();
			while (i.MoveNext())
			{
				ClassMetadata classmetadata = i.CurrentClass();
				classmetadata.TraverseDeclaredFields(reindexer);
			}
		}
    }
}
