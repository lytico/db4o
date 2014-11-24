/* Copyright (C) 2008   Versant Inc.   http://www.db4o.com */
using Db4objects.Db4o.Query;

namespace Db4objects.Db4o.Tests.CLI1.CrossPlatform
{
	internal class PersonEvaluator : IEvaluation
	{
		private readonly string _name;

		public PersonEvaluator(string name)
		{
			_name = name;
		}

		public void Evaluate(ICandidate candidate)
		{
			Person realCandidate = (Person)candidate.GetObject();
			candidate.Include(realCandidate.Name == _name);
		}
	}
}
