/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Query.Processor;

namespace Db4objects.Db4o.Internal.Query.Processor
{
	public class QPrimitiveCandidate : QCandidateBase
	{
		private object _obj;

		public QPrimitiveCandidate(QCandidates candidates, object obj) : base(candidates, 
			candidates.GenerateCandidateId())
		{
			_obj = obj;
		}

		public override object GetObject()
		{
			return _obj;
		}

		public override bool Evaluate(QConObject a_constraint, QE a_evaluator)
		{
			return a_evaluator.Evaluate(a_constraint, this, a_constraint.Translate(_obj));
		}

		public override IPreparedComparison PrepareComparison(ObjectContainerBase container
			, object constraint)
		{
			Db4objects.Db4o.Internal.ClassMetadata classMetadata = ClassMetadata();
			if (classMetadata == null)
			{
				return null;
			}
			return classMetadata.PrepareComparison(container.Transaction.Context(), constraint
				);
		}

		public override Db4objects.Db4o.Internal.ClassMetadata ClassMetadata()
		{
			return Container().ClassMetadataForReflectClass(Container().Reflector().ForObject
				(_obj));
		}

		public override bool FieldIsAvailable()
		{
			return false;
		}
	}
}
