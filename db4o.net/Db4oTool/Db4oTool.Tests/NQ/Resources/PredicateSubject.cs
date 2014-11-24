/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
using System;
using System.Collections;
using Db4objects.Db4o.Query;
using Db4oUnit;

public enum Conditioning
{
	Unknown,
	Athlete,
	Excelent,
	Normal,
	Overweight,
	Fat
}

public class Person
{
	private int _age;
	private string _name;
	private Person _spouse;
	private Conditioning _conditioning;

	public Conditioning Conditioning
	{
		get { return _conditioning; }
		set { _conditioning = value; }
	}

	public int Age
	{
		get { return _age; }
		set { _age = value; }
	}

	public string Name
	{
		get { return _name; }
		set { _name = value; }
	}
	
	public Person Spouse
	{
		get { return _spouse; }
		set
		{	
			if (value == this) throw new ArgumentException("Spouse");			
			_spouse = value;
		}
	}
	
	public Person (int age, string name, Conditioning conditioning)
	{
		_age = age;
		_name = name;
		_conditioning = conditioning;
	}
}

class PersonByName : Predicate
{
	string _name;

	public PersonByName(string name)
	{
		_name = name;
	}

	public bool Match(Person item)
	{
		return item.Name == _name;
	}
}

class PersonByAge : Predicate
{
	int _age;

	public PersonByAge(int age)
	{
		_age = age;
	}

	public bool Match(Person item)
	{
		return item.Age == _age;
	}
}

class PersonBySpouseName : Predicate
{
	string _name;
	
	public PersonBySpouseName(string name)
	{
		_name = name;
	}
	
	public bool Match(Person candidate)
	{
		return candidate.Spouse.Name == _name;
	}
}

class PersonByAgeOrSpouseName : Predicate
{
	string _name;

	int _age;

	public PersonByAgeOrSpouseName(int age, string name)
	{
		_age = age;
		_name = name;
	}

	public bool Match(Person candidate)
	{
		return candidate.Age == _age
			|| candidate.Spouse.Name == _name;
	}
}

class PersonByAgeAndName : Predicate
{
	string _name;

	int _age;

	public PersonByAgeAndName(int age, string name)
	{
		_age = age;
		_name = name;
	}

	public bool Match(Person candidate)
	{
		return candidate.Age == _age
			&& candidate.Name == _name;
	}
}

class PersonByAgeOrNames : Predicate
{
	string _name1;

	string _name2;

	int _age;

	public PersonByAgeOrNames(int age, string name1, string name2)
	{
		_name1 = name1;
		_name2 = name2;
		_age = age;
	}

	public bool Match(Person candidate)
	{
		return candidate.Age == _age
			&& (candidate.Name == _name1 || candidate.Name == _name2);
	}
}

class PersonByAgeRange : Predicate
{
	int _begin;
	int _end;

	public PersonByAgeRange(int begin, int end)
	{
		_begin = begin;
		_end = end;
	}

	public bool Match(Person candidate)
	{
		return candidate.Age >= _begin && candidate.Age <= _end;
	}
}

class TrustworthyPeople : Predicate
{
	public bool Match(Person candidate)
	{
		return candidate.Age < 29 || candidate.Name == "ma";
	}
}

class ByNameStartAndEnd : Predicate
{
	string _begin;
	string _end;

	public ByNameStartAndEnd(string begin, string end)
	{
		_begin = begin;
		_end = end;
	}

	public bool Match(Person candidate)
	{
		return candidate.Name.StartsWith(_begin) && candidate.Name.EndsWith(_end);
	}
}

class ByNameSubstring : Predicate
{
	string _substring;

	public ByNameSubstring(string substring)
	{
		_substring = substring;
	}

	public bool Match(Person candidate)
	{
		return candidate.Name.Contains(_substring);
	}
}

class OverweightPeople : Predicate
{
	public bool Match(Person candidate)
	{
		return candidate.Conditioning == Conditioning.Overweight
			|| candidate.Conditioning == Conditioning.Fat;
	}
}

public class PredicateSubject : Db4oTool.Tests.Core.InstrumentedTestCase
{
	override public void SetUp()
	{
		_container.Store(new Person(23, "jbe", Conditioning.Normal));
		_container.Store(new Person(23, "Ronaldinho", Conditioning.Fat));
		
		Person rbo = new Person(30, "rbo", Conditioning.Overweight);
		rbo.Spouse = new Person(29, "ma", Conditioning.Normal);
		_container.Store(rbo);
	}

	public void _TestByConstEnum()
	{
		AssertResult(new OverweightPeople(), "Ronaldinho", "rbo");
	}

	public void TestByNameSubstring()
	{
		AssertResult(new ByNameSubstring("di"), "Ronaldinho");
		AssertResult(new ByNameSubstring("o"), "Ronaldinho", "rbo");
	}

	public void TestByNameStartAndEnd()
	{
		AssertResult(new ByNameStartAndEnd("r", "o"), "rbo"); // , "Ronaldinho");		
	}

	public void TestByConstValue()
	{
		AssertResult(new TrustworthyPeople(), "jbe", "Ronaldinho", "ma");
	}

	public void TestByAgeRange()
	{
		AssertResult(new PersonByAgeRange(23, 29), "jbe", "Ronaldinho", "ma");
		AssertResult(new PersonByAgeRange(28, 30), "rbo", "ma");
	}

	public void TestByAgeOrNames()
	{
		AssertResult(new PersonByAgeOrNames(23, "jbe", "Ronaldinho"), "jbe", "Ronaldinho");
		AssertResult(new PersonByAgeOrNames(23, "jbe", "rbo"), "jbe");
		AssertResult(new PersonByAgeOrNames(30, "jbe", "Ronaldinho"));
	}

	public void TestByAgeAndName()
	{
		AssertResult(new PersonByAgeAndName(23, "jbe"), "jbe");
	}

	public void TestByAgeOrSpouseName()
	{
		AssertResult(new PersonByAgeOrSpouseName(23, "ma"), "jbe", "rbo", "Ronaldinho");
	}
	
	public void TestByName()
	{
		AssertResult(new PersonByName("jbe"), "jbe");
	}

	public void TestByAge()
	{
		AssertResult(new PersonByAge(30), "rbo");
		AssertResult(new PersonByAge(23), "jbe", "Ronaldinho");
	}
	
	public void TestBySpouseName()
	{
		AssertResult(new PersonBySpouseName("ma"), "rbo");
	}

	void AssertResult(Predicate predicate, params string[] expected)
	{
		AssertResult(_container.Query(predicate), expected);
	}

	void AssertResult(IList result, params string[] expected)
	{
		Assert.AreEqual(expected.Length, result.Count);
		foreach (string name in expected)
		{
			AssertContains(result, name);
		}	
	}

	void AssertContains(IList result, string expected)
	{
		foreach (Person p in result)
		{
			if (p.Name == expected) return;
		}
		Assert.Fail("Expected '" + expected + "'.");
	}
}

