/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Convert;

namespace Db4objects.Db4o.Internal.Convert
{
	/// <exclude></exclude>
	public abstract class ConversionStage
	{
		public sealed class ClassCollectionAvailableStage : ConversionStage
		{
			public ClassCollectionAvailableStage(LocalObjectContainer file) : base(file)
			{
			}

			public override void Accept(Conversion conversion)
			{
				conversion.Convert(this);
			}
		}

		public sealed class SystemUpStage : ConversionStage
		{
			public SystemUpStage(LocalObjectContainer file) : base(file)
			{
			}

			public override void Accept(Conversion conversion)
			{
				conversion.Convert(this);
			}
		}

		private LocalObjectContainer _file;

		protected ConversionStage(LocalObjectContainer file)
		{
			_file = file;
		}

		public virtual LocalObjectContainer File()
		{
			return _file;
		}

		public virtual int ConverterVersion()
		{
			return _file.SystemData().ConverterVersion();
		}

		public abstract void Accept(Conversion conversion);
	}
}
