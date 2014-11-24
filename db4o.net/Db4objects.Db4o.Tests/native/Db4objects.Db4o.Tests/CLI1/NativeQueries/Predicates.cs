/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
namespace Db4objects.Db4o.Tests.CLI1.NativeQueries
{
	using System;

	public enum Priority
	{	
		None,
		Low,
		Normal,
		High
	}

	public class Base
	{
		public int id;

		public virtual int Id 
		{
			get { return id; }
		}
	}

	public class Data : Base
	{
		public string name;
		public Data previous;
		private DateTime _expires;
		private Priority _priority;

		public Data(int id, string name, Data previous, DateTime expires, Priority priority) 
		{
			this.id = id;
			this.name = name;
			this.previous = previous;
			this._expires = expires;
			this._priority = priority;
		}

		public string Name
		{
			get { return name; }
		}

		public Data Previous
		{
			get { return previous; }
		}	

		public bool HasPrevious
		{
			get { return this.Previous != null; }
		}

		public DateTime Expires
		{
			get { return _expires; }
		}

		public Priority Priority
		{
			get { return _priority; }
		}

		public override string ToString()
		{
			return string.Format("Data(id={0}, name={1}, previous={2})",
				id, name, previous == null ? "null" : previous.id.ToString());
		}
	}
    
    class ReturnTruePredicate
    {
        public bool Match(Data candidate)
        {
            return true;
        }
    }

	class RightSideField
	{
		public bool Match(Data candidate)
		{
			return "Bb" == candidate.Name;
		}
	}

	class DescendRightSideField
	{
		public bool Match(Data candidate)
		{
			return null != candidate.Previous && "Aa" == candidate.Previous.Name;
		}
	}

	class WithPriority
	{
		private Priority _priority;

		public WithPriority(Priority priority)
		{
			_priority = priority;
		}

		public bool Match(Data candidate)
		{
			return candidate.Priority == _priority;
		}
	}

	class DateRange
	{
		DateTime _begin;
		DateTime _end;

		public DateRange(DateTime begin, DateTime end)
		{
			_begin = begin;
			_end = end;
		}

		public bool Match(Data candidate)
		{
			return (candidate.Expires >= _begin) && (candidate.Expires <= _end);
		}
	}

	class ConstStringFieldPredicate
	{
		public bool Match(Data candidate)
		{
			return candidate.name == "Cc";
		}
	}

	class ConstStringFieldNotEqual
	{
		public bool Match(Data candidate)
		{
			return candidate.name != "Cc";
		}
	}

	class ConstStringFieldPredicateEmpty
	{
		public bool Match(Data candidate)
		{
			return candidate.name == "ABBA";
		}
	}

	class ConstStringFieldOrPredicate
	{
		public bool Match(Data candidate)
		{
			return candidate.name == "Aa" || candidate.name == "Bb";
		}
	}

	class ConstStringFieldOrPredicateEmpty
	{
		public bool Match(Data candidate)
		{
			return candidate.name == "ABBA" || candidate.name == "MILI";
		}
	}

	class ConstIntFieldPredicate1
	{
		public bool Match(Data candidate)
		{
			return candidate.id == 1;
		}
	}

	class ConstIntFieldPredicate2
	{
		public bool Match(Data candidate)
		{
			return candidate.id == 2;
		}
	}

	class ConstIntFieldOrPredicate
	{
		public bool Match(Data candidate)
		{
			return candidate.id == 1 || candidate.id == 2;
		}
	}

	class IntFieldLessThanConst
	{
		public bool Match(Data candidate)
		{
			return candidate.id < 2;
		}
	}

	class IntFieldGreaterThanConst
	{
		public bool Match(Data candidate)
		{
			return candidate.id > 2;
		}
	}

	class IntFieldLessThanOrEqualConst
	{
		public bool Match(Data candidate)
		{
			return candidate.id <= 2;
		}
	}

	class IntFieldGreaterThanOrEqualConst
	{
		public bool Match(Data candidate)
		{
			return candidate.id >= 2;
		}
	}

	class IntGetterEqual
	{
		public bool Match(Data candidate)
		{
			return candidate.Id == 2;
		}
	}

	class IntGetterNotEqual
	{
		public bool Match(Data candidate)
		{
			return candidate.Id != 2;
		}
	}

	class IntGetterGreaterThan
	{
		public bool Match(Data candidate)
		{
			return candidate.Id > 2;
		}
	}

	class IntGetterLessThan
	{
		public bool Match(Data candidate)
		{
			return candidate.Id < 2;
		}
	}

	class IntGetterLessThanOrEqual
	{
		public bool Match(Data candidate)
		{
			return candidate.Id <= 2;
		}
	}

	class IntGetterGreaterThanOrEqual
	{
		public bool Match(Data candidate)
		{
			return candidate.Id >= 2;
		}
	}

	class StringGetterEqual
	{
		public bool Match(Data candidate)
		{
			return candidate.Name == "Cc";
		}
	}

	class CandidateNestedMethodInvocation
	{
		public bool Match(Data candidate)
		{	
			return candidate.HasPrevious;
		}
	}

	class NotIntFieldEqual
	{
		public bool Match(Data candidate) 
		{
			return !(candidate.id == 1);
		}
	}

	class NotIntGetterGreater
	{
		public bool Match(Data candidate) 
		{
			return !(candidate.Id > 2);
		}
	}

	class NotStringGetterEqual
	{
		public bool Match(Data candidate) 
		{
			return !(candidate.Name == "Cc");
		}
	}

	class IdGreaterOrEqualThan
	{
		int _id;

		public IdGreaterOrEqualThan(int id)
		{
			_id = id;
		}

		public bool Match(Data candidate)
		{
			return candidate.id >= _id;
		}
	}

	class NameEqualsTo
	{
		string _name;

		public NameEqualsTo(string name)
		{
			_name = name;
		}

		public bool Match(Data candidate)
		{
			return candidate.Name == _name;
		}
	}

	class NameOrId
	{
		string _name;
		int _id;

		public NameOrId(string name, int id)
		{
			_name = name;
			_id = id;
		}

		public bool Match(Data candidate)
		{
			return candidate.Name == _name || candidate.Id == _id;
		}
	}

	/*
		 * XXX: what to do?
		class TruePredicate
		{
			public bool Match(Data candidate)
			{
				return true;
			}
		}

		class FalsePredicate
		{
			public bool Match(Data candidate)
			{
				return false;
			}
		}*/

	class PreviousIdGreaterOrEqual
	{
		int _id;

		public PreviousIdGreaterOrEqual(int id)
		{	
			_id = id;
		}

		public bool Match(Data candidate)
		{
			return candidate.HasPrevious && candidate.previous.id >= _id;
		}
	}

	class HasPreviousWithName
	{
		string _name;

		public HasPreviousWithName(string name)
		{
			_name = name;
		}

		public bool Match(Data candidate) 
		{
			return candidate.HasPrevious && candidate.previous.name == _name;
		}
	}

	class GetterHasPreviousWithName
	{
		string _name;

		public GetterHasPreviousWithName(string name)
		{
			_name = name;
		}

		public bool Match(Data candidate) 
		{
			return candidate.HasPrevious && candidate.Previous.name == _name;
		}
	}

	class GetterGetterHasPreviousWithName
	{
		string _name;

		public GetterGetterHasPreviousWithName(string name)
		{
			_name = name;
		}

		public bool Match(Data candidate) 
		{
			return candidate.HasPrevious && candidate.Previous.Name == _name;
		}
	}

	class FieldGetterHasPreviousWithName
	{
		string _name;

		public FieldGetterHasPreviousWithName(string name)
		{
			_name = name;
		}

		public bool Match(Data candidate) 
		{
			return candidate.HasPrevious && candidate.previous.Name == _name;
		}
	}

	class IdGreaterAndNameEqual
	{
		public bool Match(Data candidate) 
		{
			return (candidate.id > 1) && candidate.Name == "Cc";
		}
	}

	class IdDisjunction
	{
		public bool Match(Data candidate) 
		{
			return (candidate.id <= 1) || (candidate.Id >= 3);
		}
	}

	class IdRange
	{
		int _begin;
		int _end;

		public IdRange(int begin, int end)
		{
			_begin = begin;
			_end = end;
		}

		public bool Match(Data candidate) 
		{
			return (candidate.id >= _begin) && (candidate.Id <= _end);
		}
	}

	class IdValidRange
	{
		public bool Match(Data candidate) 
		{
			return (candidate.id > 1) && (candidate.Id <= 2);
		}
	}

	class IdInvalidRange
	{
		public bool Match(Data candidate) 
		{
			return (candidate.id > 1) && (candidate.Id < 1);
		}
	}

	class HasPreviousWithPrevious
	{
		public bool Match(Data candidate)
		{
			return candidate.HasPrevious && candidate.Previous.HasPrevious;
		}
	}

	class NestedOr
	{
		public bool Match(Data candidate) 
		{
			return ((candidate.id >= 1) || candidate.Name == "Cc")
				&& candidate.id < 3;
		}
	}

	class NestedAnd
	{
		public bool Match(Data candidate)
		{
			return (candidate.id == 1 && candidate.Name == "Bb")
				|| (candidate.id == 3 && candidate.HasPrevious && candidate.Previous.Id == 2);
				
		}
	}

    class Identity
    {

        private Data _identity;

        public Identity(Data identity_)
        {
            _identity = identity_;
        }

        public bool Match(Data candidate)
        {
            return candidate == _identity;
        }
        
    }
}