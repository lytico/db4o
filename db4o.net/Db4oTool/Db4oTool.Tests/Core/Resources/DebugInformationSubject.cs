/* Copyright (C) 2004 - 2010 Versant Inc.  http://www.db4o.com */  
using System;

public class DebugInformationSubject
{
	public void SimpleSourceLine()
	{
		Test o = new Test();
		o.MethodCall(10);
	}

	public void SimpleIfBody(int n)
	{
		if (n > 10)
		{
			Test o = new Test();
			o.MethodCall(10);
		}
	}

	public void IfAndElseBranch(int n)
	{
		Test o = new Test();
		if (n > 10)
		{
			o.MethodCall(10);
		}
		else
		{
			n = 0;
		}
	}

	public void ElseBranch(int n)
	{
		Test o = new Test();
		if (n > 10)
		{
			n = 0;
		}
		else
		{
			o.MethodCall(10);
		}
	}

	public void AssignmentExpressionAndComparison()
	{
		Test o = new Test();
		int v;
		if ( (v = o.MethodCall(10)) > 1 )
		{
			Console.WriteLine(v);
		}
	}

	public void TryBody()
	{
		try
		{
			Test o = new Test();
			o.MethodCall(10);
		}
		finally
		{
		}
	}

	public void CatchBody()
	{
		try
		{
		}
		catch
		{
			Test o = new Test();
			o.MethodCall(10);
		}
	}
}

public class Test
{
	public int MethodCall(int n)
	{
		return 1;
	}
}
