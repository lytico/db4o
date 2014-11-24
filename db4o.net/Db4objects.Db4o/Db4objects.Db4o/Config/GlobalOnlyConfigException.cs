/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Ext;

namespace Db4objects.Db4o.Config
{
	/// <summary>
	/// db4o-specific exception.<br /><br />
	/// This exception is thrown when a global configuration
	/// setting is attempted on an open object container.
	/// </summary>
	/// <remarks>
	/// db4o-specific exception.<br /><br />
	/// This exception is thrown when a global configuration
	/// setting is attempted on an open object container.
	/// </remarks>
	/// <seealso cref="IConfiguration.BlockSize(int)">IConfiguration.BlockSize(int)</seealso>
	/// <seealso cref="IConfiguration.Encrypt(bool)">IConfiguration.Encrypt(bool)</seealso>
	/// <seealso cref="IConfiguration.Password(string)">IConfiguration.Password(string)</seealso>
	[System.Serializable]
	public class GlobalOnlyConfigException : Db4oRecoverableException
	{
	}
}
