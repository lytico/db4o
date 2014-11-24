/* Copyright (C) 2010   Versant Inc.   http://www.db4o.com */
using System;
using System.Collections.Generic;

class TAActivatableDictionaryScenarios
{
	public void ConstructorWithInicialCapacity()
	{
		IDictionary<string, int> dict = new Dictionary<string, int>(10);
	}

	public void ConstructorWithDictionary(Dictionary<string, int> source)
	{
		IDictionary<string, int> dict = new Dictionary<string, int>(source);
	}

	public void CastFollowedByValuePropertyAccess()
	{
		IDictionary<int, DateTime> dic = new Dictionary<int, DateTime>();
		object values = ((Dictionary<int, DateTime>)dic).Values;
	}

	public void InitConcrete()
	{
		Dictionary<int, DateTime> dic = new Dictionary<int, DateTime>();
	}
}
