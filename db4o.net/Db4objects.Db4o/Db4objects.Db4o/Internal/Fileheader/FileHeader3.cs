/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Fileheader;

namespace Db4objects.Db4o.Internal.Fileheader
{
	/// <exclude></exclude>
	public class FileHeader3 : FileHeader2
	{
		public override FileHeaderVariablePart CreateVariablePart(LocalObjectContainer file
			)
		{
			return new FileHeaderVariablePart3(file);
		}

		protected override byte Version()
		{
			return (byte)3;
		}

		protected override NewFileHeaderBase CreateNew()
		{
			return new FileHeader3();
		}

		public override FileHeader Convert(LocalObjectContainer file)
		{
			return this;
		}
	}
}
