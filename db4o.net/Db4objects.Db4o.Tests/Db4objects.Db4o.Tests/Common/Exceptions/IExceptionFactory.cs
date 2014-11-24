/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

namespace Db4objects.Db4o.Tests.Common.Exceptions
{
	public interface IExceptionFactory
	{
		void ThrowException();

		void ThrowOnClose();
	}
}
