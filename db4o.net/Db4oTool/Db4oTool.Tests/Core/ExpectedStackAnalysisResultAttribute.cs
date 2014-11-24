/* Copyright (C) 2010   Versant Inc.   http://www.db4o.com */
using System;

namespace Db4oTool.Tests.Core
{
	public class ExpectedStackAnalysisResultAttribute : Attribute
	{
		public String OpCode;
		public bool Match;
		public int Offset;
		public int StackHeight;
	}
}
