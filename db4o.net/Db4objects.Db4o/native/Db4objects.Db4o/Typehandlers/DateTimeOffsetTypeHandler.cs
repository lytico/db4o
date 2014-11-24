/* Copyright (C) 2009  Versant Inc.  http://www.db4o.com */
#if !CF
using System;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Delete;
using Db4objects.Db4o.Internal.Marshall;
using Db4objects.Db4o.Marshall;
using Db4objects.Db4o.native.Db4objects.Db4o.Typehandlers;

namespace Db4objects.Db4o.Typehandlers
{
	public class DateTimeOffsetTypeHandler : IValueTypeHandler, IQueryableTypeHandler, IIndexableTypeHandler
	{
		#region Implementation of ITypeHandler4

		void ITypeHandler4.Delete(IDeleteContext context)
		{
			context.Seek(context.Offset() + LinkLength());
		}

		void ITypeHandler4.Defragment(IDefragmentContext context)
		{
			IncrementOffset(context);
		}

		void ITypeHandler4.Write(IWriteContext context, object obj)
		{
			Write(context, (DateTimeOffset)obj);
		}

		bool IQueryableTypeHandler.DescendsIntoMembers()
		{
			return false;
		}

		object IValueTypeHandler.Read(IReadContext context)
		{
			return ReadFrom(context);
		}

		#endregion

		#region Implementation of IComparable4

		IPreparedComparison IComparable4.PrepareComparison(IContext context, object obj)
		{
			return new ComparablePreparedComparison<DateTimeOffset>(obj);
		}

		#endregion

		#region Implementation of ILinkLengthAware

		int ILinkLengthAware.LinkLength()
		{
			return LinkLength();
		}

		#endregion

		#region Implementation of IIndexable4

		object IIndexable4.ReadIndexEntry(IContext context, ByteArrayBuffer reader)
		{
			return ReadFrom(reader);
		}

		void IIndexable4.WriteIndexEntry(IContext context, ByteArrayBuffer writer, object obj)
		{
			Write(writer, (DateTimeOffset) obj);
		}

		void IIndexable4.DefragIndexEntry(DefragmentContextImpl context)
		{
			IncrementOffset(context);
		}

		#endregion

		#region Implementation of IIndexableTypeHandler

		object IIndexableTypeHandler.IndexEntryToObject(IContext context, object indexEntry)
		{
			if (indexEntry.GetType() != typeof(DateTimeOffset))
			{
				throw new InvalidOperationException();
			}

			return indexEntry;
		}

		object IIndexableTypeHandler.ReadIndexEntryFromObjectSlot(MarshallerFamily mf, StatefulBuffer buffer)
		{
			return ReadFrom(buffer);
		}

		object IIndexableTypeHandler.ReadIndexEntry(IObjectIdContext context)
		{
			return ReadFrom(context);
		}

		#endregion
		
		private static void Write(IWriteBuffer context, DateTimeOffset dateTimeOffset)
		{
			context.WriteLong(dateTimeOffset.Ticks);
			context.WriteLong(dateTimeOffset.Offset.Ticks);
		}
		
		private static DateTimeOffset ReadFrom(IReadBuffer buffer)
		{
			long ticks = buffer.ReadLong();
			long timeSpanTicks = buffer.ReadLong();
			return new DateTimeOffset(ticks, new TimeSpan(timeSpanTicks));
		}
		
		private static void IncrementOffset(IDefragmentContext context)
		{
			context.IncrementOffset(LinkLength());
		}
		
		private static int LinkLength()
		{
			return Const4.LongLength + Const4.LongLength;
		}
	}
}
#endif