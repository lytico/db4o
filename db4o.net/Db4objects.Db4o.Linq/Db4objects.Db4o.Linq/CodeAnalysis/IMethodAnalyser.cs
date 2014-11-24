/* Copyright (C) 2009 - 2010  Versant Inc.  http://www.db4o.com */

using System;

using Db4objects.Db4o.Linq.Internals;

namespace Db4objects.Db4o.Linq.CodeAnalysis
{
	internal interface IMethodAnalyser
	{
		void Run(QueryBuilderRecorder recorder);
	}
}
