/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using Db4objects.Db4o.Diagnostic;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Query;

namespace Db4objects.Db4o.Internal.Diagnostic
{
	/// <exclude>FIXME: remove me from the core and make me a facade over Events</exclude>
	public class DiagnosticProcessor : IDiagnosticConfiguration, IDeepClone
	{
		private Collection4 _listeners;

		public DiagnosticProcessor()
		{
		}

		private DiagnosticProcessor(Collection4 listeners)
		{
			_listeners = listeners;
		}

		public virtual void AddListener(IDiagnosticListener listener)
		{
			if (_listeners == null)
			{
				_listeners = new Collection4();
			}
			_listeners.Add(listener);
		}

		public virtual void CheckClassHasFields(ClassMetadata classMetadata)
		{
			if (classMetadata.AspectsAreNull() || classMetadata.DeclaredAspectCount() == 0)
			{
				string name = classMetadata.GetName();
				string[] ignoredPackages = new string[] { "java.util." };
				for (int i = 0; i < ignoredPackages.Length; i++)
				{
					if (name.IndexOf(ignoredPackages[i]) == 0)
					{
						return;
					}
				}
				if (IsDb4oClass(classMetadata))
				{
					return;
				}
				OnDiagnostic(new ClassHasNoFields(name));
			}
		}

		public virtual void CheckUpdateDepth(int depth)
		{
			if (depth > 1)
			{
				OnDiagnostic(new UpdateDepthGreaterOne(depth));
			}
		}

		public virtual object DeepClone(object context)
		{
			return new Db4objects.Db4o.Internal.Diagnostic.DiagnosticProcessor(CloneListeners
				());
		}

		public virtual void DeletionFailed()
		{
			OnDiagnostic(new Db4objects.Db4o.Diagnostic.DeletionFailed());
		}

		public virtual void DefragmentRecommended(DefragmentRecommendation.DefragmentRecommendationReason
			 reason)
		{
			OnDiagnostic(new DefragmentRecommendation(reason));
		}

		private Collection4 CloneListeners()
		{
			return _listeners != null ? new Collection4(_listeners) : null;
		}

		public virtual bool Enabled()
		{
			return _listeners != null;
		}

		private bool IsDb4oClass(ClassMetadata classMetadata)
		{
			return classMetadata.IsInternal();
		}

		public virtual void LoadedFromClassIndex(ClassMetadata classMetadata)
		{
			if (IsDb4oClass(classMetadata))
			{
				return;
			}
			OnDiagnostic(new Db4objects.Db4o.Diagnostic.LoadedFromClassIndex(classMetadata.GetName
				()));
		}

		public virtual void DescendIntoTranslator(ClassMetadata parent, string fieldName)
		{
			OnDiagnostic(new Db4objects.Db4o.Diagnostic.DescendIntoTranslator(parent.GetName(
				), fieldName));
		}

		public virtual void NativeQueryUnoptimized(Predicate predicate, Exception exception
			)
		{
			OnDiagnostic(new NativeQueryNotOptimized(predicate, exception));
		}

		public virtual void NativeQueryOptimizerNotLoaded(int reason, Exception e)
		{
			OnDiagnostic(new Db4objects.Db4o.Diagnostic.NativeQueryOptimizerNotLoaded(reason, 
				e));
		}

		public virtual void ObjectFieldDoesNotExist(string className, string fieldName)
		{
			OnDiagnostic(new Db4objects.Db4o.Diagnostic.ObjectFieldDoesNotExist(className, fieldName
				));
		}

		public virtual void ClassMissed(string className)
		{
			OnDiagnostic(new MissingClass(className));
		}

		public virtual void OnDiagnostic(IDiagnostic d)
		{
			if (_listeners == null)
			{
				return;
			}
			IEnumerator i = _listeners.GetEnumerator();
			while (i.MoveNext())
			{
				((IDiagnosticListener)i.Current).OnDiagnostic(d);
			}
		}

		public virtual void RemoveAllListeners()
		{
			_listeners = null;
		}
	}
}
