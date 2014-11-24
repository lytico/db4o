/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Btree;
using Db4objects.Db4o.Internal.Classindex;
using Db4objects.Db4o.Internal.Query.Processor;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Reflect;

namespace Db4objects.Db4o.Internal.Query.Processor
{
	/// <summary>Class constraint on queries</summary>
	/// <exclude></exclude>
	public class QConClass : QConObject
	{
		[System.NonSerialized]
		private IReflectClass _claxx;

		private string _className;

		private bool i_equal;

		public QConClass()
		{
		}

		internal QConClass(Transaction trans, QCon parent, QField field, IReflectClass claxx
			) : base(trans, parent, field, null)
		{
			// C/S
			if (claxx != null)
			{
				ObjectContainerBase container = trans.Container();
				_classMetadata = container.ClassMetadataForReflectClass(claxx);
				if (_classMetadata == null)
				{
					// could be an aliased class, try to resolve.
					string className = claxx.GetName();
					string aliasRunTimeName = container.Config().ResolveAliasStoredName(className);
					if (!className.Equals(aliasRunTimeName))
					{
						_classMetadata = container.ClassMetadataForName(aliasRunTimeName);
					}
				}
				if (claxx.Equals(container._handlers.IclassObject))
				{
					_classMetadata = (ClassMetadata)_classMetadata.TypeHandler();
				}
			}
			_claxx = claxx;
		}

		internal QConClass(Transaction trans, IReflectClass claxx) : this(trans, null, null
			, claxx)
		{
		}

		public virtual string GetClassName()
		{
			return _claxx == null ? null : _claxx.GetName();
		}

		public override bool CanBeIndexLeaf()
		{
			return false;
		}

		internal override bool Evaluate(IInternalCandidate candidate)
		{
			bool result = true;
			QCandidates qCandidates = candidate.Candidates();
			if (qCandidates.IsTopLevel() && qCandidates.WasLoadedFromClassFieldIndex())
			{
				if (_classMetadata.GetAncestor() != null)
				{
					BTreeClassIndexStrategy index = (BTreeClassIndexStrategy)_classMetadata.Index();
					if (index == null)
					{
						return i_evaluator.Not(true);
					}
					BTree btree = index.Btree();
					object searchResult = btree.Search(candidate.Transaction(), candidate.Id());
					result = searchResult != null;
				}
			}
			else
			{
				IReflectClass claxx = candidate.ClassMetadata().ClassReflector();
				if (claxx == null)
				{
					result = false;
				}
				else
				{
					result = i_equal ? _claxx.Equals(claxx) : _claxx.IsAssignableFrom(claxx);
				}
			}
			return i_evaluator.Not(result);
		}

		internal override void EvaluateSelf()
		{
			if (i_candidates.WasLoadedFromClassIndex())
			{
				if (i_evaluator.IsDefault())
				{
					if (!HasJoins())
					{
						if (_classMetadata != null && i_candidates._classMetadata != null)
						{
							if (_classMetadata.GetHigherHierarchy(i_candidates._classMetadata) == _classMetadata)
							{
								return;
							}
						}
					}
				}
			}
			if (i_candidates.WasLoadedFromClassFieldIndex())
			{
				if (i_candidates.IsTopLevel())
				{
					if (i_evaluator.IsDefault())
					{
						if (!HasJoins())
						{
							if (CanResolveByFieldIndex())
							{
								return;
							}
						}
					}
				}
			}
			i_candidates.Filter(this);
		}

		protected override bool CanResolveByFieldIndex()
		{
			return _classMetadata != null && _classMetadata.GetAncestor() == null;
		}

		public override IConstraint Equal()
		{
			lock (StreamLock())
			{
				i_equal = true;
				return this;
			}
		}

		internal override bool IsNullConstraint()
		{
			return false;
		}

		internal override string LogObject()
		{
			return string.Empty;
		}

		internal override void Marshall()
		{
			base.Marshall();
			if (_claxx != null)
			{
				_className = Container().Config().ResolveAliasRuntimeName(_claxx.GetName());
			}
		}

		public override string ToString()
		{
			string str = "QConClass ";
			if (_claxx != null)
			{
				str += _claxx.GetName() + " ";
			}
			return str + base.ToString();
		}

		internal override void Unmarshall(Transaction a_trans)
		{
			if (i_trans == null)
			{
				base.Unmarshall(a_trans);
				if (_className != null)
				{
					_className = Container().Config().ResolveAliasStoredName(_className);
					_claxx = a_trans.Reflector().ForName(_className);
				}
			}
		}

		internal override void SetEvaluationMode()
		{
			IEnumerator children = IterateChildren();
			while (children.MoveNext())
			{
				object child = children.Current;
				if (child is QConObject)
				{
					((QConObject)child).SetEvaluationMode();
				}
			}
		}

		public override void SetProcessedByIndex(QCandidates candidates)
		{
		}
		// do nothing, QConClass needs to stay in the evaluation graph.
	}
}
