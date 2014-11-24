/* Copyright (C) 2009  Versant Inc.  http://www.db4o.com */
namespace Db4objects.Db4o.Internal.Convert.Conversions
{
	public abstract class DropClassIndexesConversion : Conversion
	{
		public override void Convert(ConversionStage.SystemUpStage stage)
		{
			LocalObjectContainer file = stage.File();
			ClassMetadataIterator i = file.ClassCollection().Iterator();
			while (i.MoveNext())
			{
				ClassMetadata classmetadata = i.CurrentClass();
				if (Accept(classmetadata))
				{
					classmetadata.DropClassIndex();
				}
			}
		}

		protected abstract bool Accept(ClassMetadata classmetadata);
	}
}
