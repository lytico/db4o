/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.IO;

namespace Db4objects.Db4o.Tests.Common.Defragment
{
	public abstract class SlotDefragmentTestConstants
	{
		private static readonly string Filename = Path.GetTempFileName();

		private static readonly string Backupfilename = Filename + ".backup";

		private SlotDefragmentTestConstants()
		{
		}
	}
}
