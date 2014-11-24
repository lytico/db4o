/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

#if !SILVERLIGHT
using System.Collections;
using System.Text;
using Db4objects.Db4o.Filestats;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Slots;

namespace Db4objects.Db4o.Filestats
{
	/// <exclude></exclude>
	public class SlotMapImpl : ISlotMap
	{
		private TreeIntObject _slots = null;

		private readonly long _fileLength;

		public SlotMapImpl(long fileLength)
		{
			_fileLength = fileLength;
		}

		public virtual void Add(Slot slot)
		{
			_slots = ((TreeIntObject)Tree.Add(_slots, new TreeIntObject(slot.Address(), slot)
				));
		}

		public virtual IList Merged()
		{
			IList mergedSlots = new ArrayList();
			ByRef mergedSlot = ByRef.NewInstance(new Slot(0, 0));
			Tree.Traverse(_slots, new _IVisitor4_32(mergedSlot, mergedSlots));
			mergedSlots.Add(((Slot)mergedSlot.value));
			return mergedSlots;
		}

		private sealed class _IVisitor4_32 : IVisitor4
		{
			public _IVisitor4_32(ByRef mergedSlot, IList mergedSlots)
			{
				this.mergedSlot = mergedSlot;
				this.mergedSlots = mergedSlots;
			}

			public void Visit(object node)
			{
				Slot curSlot = ((Slot)((TreeIntObject)node)._object);
				if (((Slot)mergedSlot.value).Address() + ((Slot)mergedSlot.value).Length() == curSlot
					.Address())
				{
					mergedSlot.value = new Slot(((Slot)mergedSlot.value).Address(), ((Slot)mergedSlot
						.value).Length() + curSlot.Length());
				}
				else
				{
					mergedSlots.Add(((Slot)mergedSlot.value));
					mergedSlot.value = curSlot;
				}
			}

			private readonly ByRef mergedSlot;

			private readonly IList mergedSlots;
		}

		public virtual IList Gaps(long length)
		{
			IList merged = Merged();
			IList gaps = new ArrayList();
			if (merged.Count == 0)
			{
				return gaps;
			}
			bool isFirst = true;
			Slot prevSlot = null;
			for (IEnumerator curSlotIter = merged.GetEnumerator(); curSlotIter.MoveNext(); )
			{
				Slot curSlot = ((Slot)curSlotIter.Current);
				if (isFirst)
				{
					prevSlot = curSlot;
					if (prevSlot.Address() > 0)
					{
						gaps.Add(new Slot(0, prevSlot.Address()));
					}
					isFirst = false;
				}
				else
				{
					int gapStart = prevSlot.Address() + prevSlot.Length();
					gaps.Add(new Slot(gapStart, curSlot.Address() - gapStart));
					prevSlot = curSlot;
				}
			}
			int afterlast = prevSlot.Address() + prevSlot.Length();
			if (afterlast < length)
			{
				gaps.Add(new Slot(afterlast, (int)(length - afterlast)));
			}
			return gaps;
		}

		public override string ToString()
		{
			StringBuilder str = new StringBuilder();
			str.Append("SLOTS:\n");
			LogSlots(Merged(), str);
			str.Append("GAPS:");
			LogSlots(Gaps(_fileLength), str);
			return str.ToString();
		}

		private void LogSlots(IEnumerable slots, StringBuilder str)
		{
			int totalLength = 0;
			for (IEnumerator gapIter = slots.GetEnumerator(); gapIter.MoveNext(); )
			{
				Slot gap = ((Slot)gapIter.Current);
				totalLength += gap.Length();
				str.Append(gap).Append("\n");
			}
			str.Append("TOTAL: ").Append(totalLength).Append("\n");
		}
	}
}
#endif // !SILVERLIGHT
