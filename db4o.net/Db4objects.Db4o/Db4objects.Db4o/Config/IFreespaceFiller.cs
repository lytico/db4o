/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.IO;

namespace Db4objects.Db4o.Config
{
	/// <summary>Callback hook for overwriting freed space in the database file.</summary>
	/// <remarks>Callback hook for overwriting freed space in the database file.</remarks>
	public interface IFreespaceFiller
	{
		/// <summary>Called to overwrite freed space in the database file.</summary>
		/// <remarks>Called to overwrite freed space in the database file.</remarks>
		/// <param name="io">Handle for the freed slot</param>
		/// <exception cref="System.IO.IOException"></exception>
		void Fill(BlockAwareBinWindow io);
	}
}
