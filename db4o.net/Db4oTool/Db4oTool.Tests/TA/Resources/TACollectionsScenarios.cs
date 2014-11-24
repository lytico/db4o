/* Copyright (C) 2010   Versant Inc.   http://www.db4o.com */
using System;
using System.Collections.Generic;
using System.Diagnostics;

public class TACollectionsScenarios
{
	private IList<string> _ilist;
	private ICollection<string> _icollection;
	private List<string> _list;

	public TACollectionsScenarios(IList<string> items)
	{
		_ilist = items;
	}

	public static TACollectionsScenarios Create(params string[] items)
	{
		return new TACollectionsScenarios(new List<string>(items));
	}

	public void InitInterface(IList<string> list)
	{
		InitInterface(new List<string>());
	}

	public void LocalsAsIList()
	{
		IList<int> local = new List<int>();
	}

	private static IList<DateTime> CreateList()
	{
		IList<DateTime> theList = new List<DateTime>();
		return theList;
	}

	public void ParameterLessConstructor()
	{
		_ilist = new List<string>();
	}

	public void CollectionInitInterface(ICollection<string> list)
	{
		CollectionInitInterface(new List<string>());
	}

	public void CollectionLocalsAsIList()
	{
		ICollection<int> local = new List<int>();
	}

	private static ICollection<DateTime> CollectionCreateList()
	{
		ICollection<DateTime> theList = new List<DateTime>();
		theList.Add(DateTime.Now);

		return theList;
	}

	public void CollectionParameterLessConstructor()
	{
		_icollection = new List<string>();
	}

	public void ConstructorTakingIEnumerable()
	{
	    _ilist = new List<string>(new string[] {"foo", "bar" });
	}

	public void ConstructorTakingInitialSize()
	{
	    _ilist = new List<string>(10);
	}

	public void ConstructorTakingEnumerable()
	{
		IEnumerable<string> enumerable = null;
		_ilist = new List<string>(enumerable);
	}

	public List<string> PublicCreateConcreteList()
	{
		return new List<string>();
	}

	public void CastFollowedByParameterLessMethod()
	{
		// Is it a cast to List<T> ?
		// Are we calling a method / property imediatly ?
		((List<string>)_ilist).Sort();
	}

	public void CastFollowedByMethodWithSingleArgument()
	{
		((List<string>)_ilist).Add("foo");
	}

	public void CastConsumedByPropertyAccess()
	{
		int foo = ((List<string>)_ilist).Capacity;
	}

	public void CastConsumedByMethodExpectingExplicityDelegate()
	{
		((List<string>)_ilist).Sort(new Comparison<string>(Compare));
	}

	public void CastConsumedByMethodExpectingDelegate()
	{
		((List<string>)_ilist).Sort(Compare);
	}

	private int Compare(string lhs, string rhs)
	{
		return 0;
	}

	/**
	 * The following instantiations of List<> should not be exchanged by its ActivatableList<> counterparts since
	 * they are assigning directly to List<> instead of IList<>
	 */
	#region ThisMustFailOrEmitWarning

	#region ShouldNotBeExchangedByActivatableListOfT

	public void AssignmentOfConcreteListToLocal()
	{
		List<string> local = new List<string>();
	}

	public void AssignmentOfConcreteListToField()
	{
		_list = new List<string>();
	}

	public void InitConcrete(List<string> list)
	{
	    InitConcrete(new List<string>());
	}

	#endregion

#if CASTCONSUMEDBYLOCAL
	public void CastConsumedByLocal()
	{
		List<string> dubious = ((List<string>)_ilist);
	}
#endif

#if CASTCONSUMEDBYFIELD
	public void CastConsumedByField()
	{
		_list = ((List<string>)_ilist);
	}
#endif

#if CASTCONSUMEDBYARGUMENT
	public void CastConsumedByArgument(List<string> arg)
	{
		CastConsumedByArgument((List<string>)_ilist);
	}
#endif

#if CASTCONSUMEDBYMETHODRETURN
	public List<string> CastConsumedByMethodReturn()
	{
		return ((List<string>)_ilist);
	}
#endif

	#endregion
}

//TODO: Make this condition explicity. 
//      As of now the simple existence is enoght to tigger failures if the
//      code is note prepared to handle it. 
public class GenericHolder<T>
{
    private IList<T> _list;

    public void Init()
    {
        _list = new List<T>();
    }
}