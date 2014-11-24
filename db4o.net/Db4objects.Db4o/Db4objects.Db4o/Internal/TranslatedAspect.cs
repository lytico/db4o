/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Marshall;
using Db4objects.Db4o.Reflect;
using Db4objects.Db4o.Typehandlers;

namespace Db4objects.Db4o.Internal
{
	public sealed class TranslatedAspect : FieldMetadata
	{
		private IObjectTranslator _translator;

		public TranslatedAspect(ClassMetadata containingClass, string name) : this(containingClass
			)
		{
			Init(name);
		}

		public TranslatedAspect(ClassMetadata containingClass, IObjectTranslator translator
			) : this(containingClass)
		{
			InitializeTranslator(translator);
		}

		private TranslatedAspect(ClassMetadata containingClass) : base(containingClass)
		{
			SetAvailable();
		}

		public void InitializeTranslator(IObjectTranslator translator)
		{
			_translator = translator;
			InitializeFieldName();
			InitializeFieldType();
		}

		public override bool Alive()
		{
			return true;
		}

		private void InitializeFieldName()
		{
			Init(FieldNameFor(_translator));
		}

		private void InitializeFieldType()
		{
			ObjectContainerBase stream = ContainingClass().Container();
			IReflectClass storedClass = stream.Reflector().ForClass(TranslatorStoredClass(_translator
				));
			Configure(storedClass, false);
			IReflectClass baseType = Handlers4.BaseType(storedClass);
			stream.ShowInternalClasses(true);
			try
			{
				_fieldType = stream.ProduceClassMetadata(baseType);
			}
			finally
			{
				stream.ShowInternalClasses(false);
			}
			if (null == _fieldType)
			{
				throw new InvalidOperationException("Cannot produce class metadata for " + baseType
					 + "!");
			}
		}

		public static string FieldNameFor(IObjectTranslator translator)
		{
			return translator.GetType().FullName;
		}

		public override bool CanUseNullBitmap()
		{
			return false;
		}

		public override void Deactivate(IActivationContext context)
		{
			if (context.Depth().RequiresActivation())
			{
				CascadeActivation(context);
			}
			SetOn(context.Transaction(), context.TargetObject(), null);
		}

		public override object GetOn(Transaction a_trans, object a_OnObject)
		{
			try
			{
				return _translator.OnStore(a_trans.ObjectContainer(), a_OnObject);
			}
			catch (ReflectException e)
			{
				throw;
			}
			catch (Exception e)
			{
				throw new ReflectException(e);
			}
		}

		public override object GetOrCreate(Transaction a_trans, object a_OnObject)
		{
			return GetOn(a_trans, a_OnObject);
		}

		public override void Activate(UnmarshallingContext context)
		{
			object obj = Read(context);
			// Activation of members is necessary on purpose here.
			// Classes like Hashtable need fully activated members
			// to be able to calculate hashCode()
			if (obj != null)
			{
				context.Container().Activate(context.Transaction(), obj, context.ActivationDepth(
					));
			}
			SetOn(context.Transaction(), context.PersistentObject(), obj);
		}

		internal override void Refresh()
		{
		}

		// do nothing
		private void SetOn(Transaction trans, object a_onObject, object toSet)
		{
			try
			{
				_translator.OnActivate(trans.ObjectContainer(), a_onObject, toSet);
			}
			catch (Exception e)
			{
				throw new ReflectException(e);
			}
		}

		protected override object IndexEntryFor(object indexEntry)
		{
			return indexEntry;
		}

		protected override IIndexable4 IndexHandler(ObjectContainerBase stream)
		{
			return (IIndexable4)GetHandler();
		}

		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			if (obj == null || obj.GetType() != GetType())
			{
				return false;
			}
			Db4objects.Db4o.Internal.TranslatedAspect other = (Db4objects.Db4o.Internal.TranslatedAspect
				)obj;
			return _translator.Equals(other._translator);
		}

		public override int GetHashCode()
		{
			return _translator.GetHashCode();
		}

		public override Db4objects.Db4o.Internal.Marshall.AspectType AspectType()
		{
			return Db4objects.Db4o.Internal.Marshall.AspectType.Translator;
		}

		public bool IsObjectConstructor()
		{
			return _translator is IObjectConstructor;
		}

		public object Construct(ObjectReferenceContext context)
		{
			ContextState contextState = context.SaveState();
			bool fieldHasValue = ContainingClass().SeekToField(context, this);
			try
			{
				return ((IObjectConstructor)_translator).OnInstantiate(context.Container(), fieldHasValue
					 ? Read(context) : null);
			}
			finally
			{
				context.RestoreState(contextState);
			}
		}
	}
}
