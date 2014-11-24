/* Copyright (C) 2009 - 2010  Versant Inc.  http://www.db4o.com */

#if !CF && !SILVERLIGHT

using System;
using System.Reflection;
using System.Reflection.Emit;

using Mono.Reflection;

using Db4objects.Db4o.Activation;
using Db4objects.Db4o.Linq.Caching;
using Db4objects.Db4o.Linq.Internals;
using Db4objects.Db4o.Internal.Caching;

namespace Db4objects.Db4o.Linq.CodeAnalysis
{
	internal class ReflectionMethodAnalyser : IMethodAnalyser
	{
		private static ICache4<MethodInfo, FieldInfo> _fieldCache =
			CacheFactory<MethodInfo, FieldInfo>.For(CacheFactory.New2QXCache(5));

		private static ILPattern ActivateCall()
		{
			return new ActivateCallPattern();
		}

		private class ActivateCallPattern : ILPattern
		{
			static ILPattern pattern = ILPattern.Sequence(
				ILPattern.Optional(OpCodes.Nop),
				ILPattern.OpCode(OpCodes.Ldarg_0),
				ILPattern.OpCode(OpCodes.Ldc_I4_0),
				ILPattern.Either(
					ILPattern.OpCode(OpCodes.Call),
					ILPattern.OpCode(OpCodes.Callvirt)));

			public override void Match(MatchContext context)
			{
				pattern.Match(context);
				if (!context.IsMatch) return;

				var match = GetLastMatchingInstruction(context);
				var method = (MethodInfo)match.Operand;
				if (!IsActivateCall(method)) context.IsMatch = false;
			}

			private static bool IsActivateCall(MethodInfo method)
			{
				if (method == null) return false;
				if (method.IsStatic) return false;
				if (method.Name != "Activate") return false;

				var parameters = method.GetParameters();
				if (parameters.Length != 1) return false;
				if (parameters[0].ParameterType != typeof(ActivationPurpose)) return false;

				return true;
			}
		}

		private static ILPattern BackingField()
		{
			return new BackingFieldPattern();
		}

		private class BackingFieldPattern : ILPattern
		{
			public static readonly object BackingFieldKey = new object();

			private static ILPattern pattern = ILPattern.Sequence(
				ILPattern.Optional(OpCodes.Nop),
				ILPattern.OpCode(OpCodes.Ldarg_0),
				ILPattern.OpCode(OpCodes.Ldfld));

			public override void Match(MatchContext context)
			{
				pattern.Match (context);
				if (!context.IsMatch) return;

				var match = GetLastMatchingInstruction(context);
				var field = (FieldInfo)match.Operand;
				context.AddData(BackingFieldKey, field);
			}
		}

		private static ILPattern _getterPattern =
			ILPattern.Sequence(
				ILPattern.Optional(ActivateCall()),
				BackingField(),
				ILPattern.Optional(
					OpCodes.Stloc_0,
					OpCodes.Br_S,
					OpCodes.Ldloc_0),
				ILPattern.OpCode(OpCodes.Ret));

		private MethodInfo _method;

		private ReflectionMethodAnalyser(MethodInfo method)
		{
			_method = method;
		}

		private static MatchContext MatchGetter(MethodInfo method)
		{
			return ILPattern.Match(method, _getterPattern);
		}

		public void Run(QueryBuilderRecorder recorder)
		{
			RecordField(recorder, GetBackingField(_method));
		}

		private static void RecordField(QueryBuilderRecorder recorder, FieldInfo field)
		{
			recorder.Add(ctx => {
				ctx.Descend(field.Name);
				ctx.PushDescendigFieldEnumType(field.FieldType.IsEnum ? field.FieldType : null);
			});
		}

		private static FieldInfo GetBackingField(MethodInfo method)
		{
			return _fieldCache.Produce(method, ResolveBackingField);
		}

		private static FieldInfo ResolveBackingField(MethodInfo method)
		{
			var context = MatchGetter(method);
			if (!context.IsMatch) throw new QueryOptimizationException("Analysed method is not a simple getter");

			return GetFieldFromContext (context);
		}

		private static FieldInfo GetFieldFromContext(MatchContext context)
		{
			object data;
			if (!context.TryGetData(BackingFieldPattern.BackingFieldKey, out data)) throw new NotSupportedException();

			return (FieldInfo)data;
		}

		public static IMethodAnalyser FromMethod(MethodInfo method)
		{
			return new ReflectionMethodAnalyser(method);
		}
	}
}

#endif
