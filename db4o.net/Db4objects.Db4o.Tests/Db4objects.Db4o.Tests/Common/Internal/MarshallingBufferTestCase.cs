/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.Tests.Common.Internal
{
	public class MarshallingBufferTestCase : ITestCase
	{
		private const int Data1 = 111;

		private const byte Data2 = (byte)2;

		private const int Data3 = 333;

		private const int Data4 = 444;

		private const int Data5 = 55;

		public virtual void TestWrite()
		{
			MarshallingBuffer buffer = new MarshallingBuffer();
			buffer.WriteInt(Data1);
			buffer.WriteByte(Data2);
			ByteArrayBuffer content = InspectContent(buffer);
			Assert.AreEqual(Data1, content.ReadInt());
			Assert.AreEqual(Data2, content.ReadByte());
		}

		public virtual void TestTransferLastWrite()
		{
			MarshallingBuffer buffer = new MarshallingBuffer();
			buffer.WriteInt(Data1);
			int lastOffset = Offset(buffer);
			buffer.WriteByte(Data2);
			MarshallingBuffer other = new MarshallingBuffer();
			buffer.TransferLastWriteTo(other, true);
			Assert.AreEqual(lastOffset, Offset(buffer));
			ByteArrayBuffer content = InspectContent(other);
			Assert.AreEqual(Data2, content.ReadByte());
		}

		private int Offset(MarshallingBuffer buffer)
		{
			return buffer.TestDelegate().Offset();
		}

		private ByteArrayBuffer InspectContent(MarshallingBuffer buffer)
		{
			ByteArrayBuffer bufferDelegate = buffer.TestDelegate();
			bufferDelegate.Seek(0);
			return bufferDelegate;
		}

		public virtual void TestChildren()
		{
			MarshallingBuffer buffer = new MarshallingBuffer();
			buffer.WriteInt(Data1);
			buffer.WriteByte(Data2);
			MarshallingBuffer child = buffer.AddChild();
			child.WriteInt(Data3);
			child.WriteInt(Data4);
			buffer.MergeChildren(null, 0, 0);
			ByteArrayBuffer content = InspectContent(buffer);
			Assert.AreEqual(Data1, content.ReadInt());
			Assert.AreEqual(Data2, content.ReadByte());
			int address = content.ReadInt();
			content.Seek(address);
			Assert.AreEqual(Data3, content.ReadInt());
			Assert.AreEqual(Data4, content.ReadInt());
		}

		public virtual void TestGrandChildren()
		{
			MarshallingBuffer buffer = new MarshallingBuffer();
			buffer.WriteInt(Data1);
			buffer.WriteByte(Data2);
			MarshallingBuffer child = buffer.AddChild();
			child.WriteInt(Data3);
			child.WriteInt(Data4);
			MarshallingBuffer grandChild = child.AddChild();
			grandChild.WriteInt(Data5);
			buffer.MergeChildren(null, 0, 0);
			ByteArrayBuffer content = InspectContent(buffer);
			Assert.AreEqual(Data1, content.ReadInt());
			Assert.AreEqual(Data2, content.ReadByte());
			int address = content.ReadInt();
			content.Seek(address);
			Assert.AreEqual(Data3, content.ReadInt());
			Assert.AreEqual(Data4, content.ReadInt());
			address = content.ReadInt();
			content.Seek(address);
			Assert.AreEqual(Data5, content.ReadInt());
		}

		public virtual void TestLinkOffset()
		{
			int linkOffset = 7;
			MarshallingBuffer buffer = new MarshallingBuffer();
			buffer.WriteInt(Data1);
			buffer.WriteByte(Data2);
			MarshallingBuffer child = buffer.AddChild();
			child.WriteInt(Data3);
			child.WriteInt(Data4);
			MarshallingBuffer grandChild = child.AddChild();
			grandChild.WriteInt(Data5);
			buffer.MergeChildren(null, 0, linkOffset);
			ByteArrayBuffer content = InspectContent(buffer);
			ByteArrayBuffer extendedBuffer = new ByteArrayBuffer(content.Length() + linkOffset
				);
			content.CopyTo(extendedBuffer, 0, linkOffset, content.Length());
			extendedBuffer.Seek(linkOffset);
			Assert.AreEqual(Data1, extendedBuffer.ReadInt());
			Assert.AreEqual(Data2, extendedBuffer.ReadByte());
			int address = extendedBuffer.ReadInt();
			extendedBuffer.Seek(address);
			Assert.AreEqual(Data3, extendedBuffer.ReadInt());
			Assert.AreEqual(Data4, extendedBuffer.ReadInt());
			address = extendedBuffer.ReadInt();
			extendedBuffer.Seek(address);
			Assert.AreEqual(Data5, extendedBuffer.ReadInt());
		}

		public virtual void TestLateChildrenWrite()
		{
			MarshallingBuffer buffer = new MarshallingBuffer();
			buffer.WriteInt(Data1);
			MarshallingBuffer child = buffer.AddChild(true, true);
			child.WriteInt(Data3);
			buffer.WriteByte(Data2);
			child.WriteInt(Data4);
			buffer.MergeChildren(null, 0, 0);
			ByteArrayBuffer content = InspectContent(buffer);
			Assert.AreEqual(Data1, content.ReadInt());
			int address = content.ReadInt();
			content.ReadInt();
			// length
			Assert.AreEqual(Data2, content.ReadByte());
			content.Seek(address);
			Assert.AreEqual(Data3, content.ReadInt());
			Assert.AreEqual(Data4, content.ReadInt());
		}
	}
}
