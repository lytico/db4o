/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Activation;

namespace Db4objects.Db4o.Internal.Activation
{
	public interface IUpdateDepth
	{
		bool SufficientDepth();

		bool Negative();

		IUpdateDepth Adjust(ClassMetadata clazz);

		IUpdateDepth AdjustUpdateDepthForCascade(bool isCollection);

		IUpdateDepth Descend();

		bool CanSkip(ObjectReference @ref);
	}
}
