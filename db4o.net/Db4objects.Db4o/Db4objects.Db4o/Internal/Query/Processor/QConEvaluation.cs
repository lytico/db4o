/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Query.Processor;

namespace Db4objects.Db4o.Internal.Query.Processor
{
	/// <exclude></exclude>
	public class QConEvaluation : QCon
	{
		[System.NonSerialized]
		private object i_evaluation;

		private byte[] i_marshalledEvaluation;

		private int i_marshalledID;

		public QConEvaluation()
		{
		}

		public QConEvaluation(Transaction a_trans, object a_evaluation) : base(a_trans)
		{
			// C/S only
			i_evaluation = a_evaluation;
		}

		internal override void EvaluateEvaluationsExec(QCandidates a_candidates, bool rereadObject
			)
		{
			//		if (rereadObject) {
			//			a_candidates.traverse(new Visitor4() {
			//				public void visit(Object a_object) {
			//					((QCandidate) a_object).useField(null);
			//				}
			//			});
			//		}
			a_candidates.Filter(this);
		}

		internal override void Marshall()
		{
			base.Marshall();
			if (!Platform4.UseNativeSerialization())
			{
				MarshallUsingDb4oFormat();
			}
			else
			{
				try
				{
					i_marshalledEvaluation = Platform4.Serialize(i_evaluation);
				}
				catch (Exception)
				{
					MarshallUsingDb4oFormat();
				}
			}
		}

		private void MarshallUsingDb4oFormat()
		{
			SerializedGraph serialized = Serializer.Marshall(Container(), i_evaluation);
			i_marshalledEvaluation = serialized._bytes;
			i_marshalledID = serialized._id;
		}

		internal override void Unmarshall(Transaction a_trans)
		{
			if (i_trans == null)
			{
				base.Unmarshall(a_trans);
				if (i_marshalledID > 0 || !Platform4.UseNativeSerialization())
				{
					i_evaluation = Serializer.Unmarshall(Container(), i_marshalledEvaluation, i_marshalledID
						);
				}
				else
				{
					i_evaluation = Platform4.Deserialize(i_marshalledEvaluation);
				}
			}
		}

		public override void Visit(object obj)
		{
			IInternalCandidate candidate = (IInternalCandidate)obj;
			// force activation outside the try block
			// so any activation errors bubble up
			ForceActivation(candidate);
			try
			{
				Platform4.EvaluationEvaluate(i_evaluation, candidate);
			}
			catch (Exception)
			{
				candidate.Include(false);
			}
			// TODO: implement Exception callback for the user coder
			// at least for test cases
			if (!candidate.Include())
			{
				DoNotInclude(candidate.GetRoot());
			}
		}

		private void ForceActivation(IInternalCandidate candidate)
		{
			candidate.GetObject();
		}

		internal virtual bool SupportsIndex()
		{
			return false;
		}

		protected override bool CanResolveByFieldIndex()
		{
			return false;
		}
	}
}
