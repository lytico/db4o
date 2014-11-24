/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Activation;
using Db4objects.Db4o.Internal.Marshall;
using Db4objects.Db4o.Internal.Slots;
using Db4objects.Db4o.Tests.Common.Internal;

namespace Db4objects.Db4o.Tests.Common.Internal
{
	public class MarshallingContextTestCase : AbstractDb4oTestCase
	{
		public static void Main(string[] arguments)
		{
			new MarshallingContextTestCase().RunSolo();
		}

		public class StringItem
		{
			public string _name;

			public StringItem(string name)
			{
				_name = name;
			}
		}

		public class StringIntItem
		{
			public string _name;

			public int _int;

			public StringIntItem(string name, int i)
			{
				_name = name;
				_int = i;
			}
		}

		public class StringIntBooleanItem
		{
			public string _name;

			public int _int;

			public bool _bool;

			public StringIntBooleanItem(string name, int i, bool @bool)
			{
				_name = name;
				_int = i;
				_bool = @bool;
			}
		}

		public virtual void TestStringItem()
		{
			MarshallingContextTestCase.StringItem writtenItem = new MarshallingContextTestCase.StringItem
				("one");
			MarshallingContextTestCase.StringItem readItem = (MarshallingContextTestCase.StringItem
				)WriteAndRead(writtenItem);
			Assert.AreEqual(writtenItem._name, readItem._name);
		}

		public virtual void TestStringIntItem()
		{
			MarshallingContextTestCase.StringIntItem writtenItem = new MarshallingContextTestCase.StringIntItem
				("one", 777);
			MarshallingContextTestCase.StringIntItem readItem = (MarshallingContextTestCase.StringIntItem
				)WriteAndRead(writtenItem);
			Assert.AreEqual(writtenItem._name, readItem._name);
			Assert.AreEqual(writtenItem._int, readItem._int);
		}

		public virtual void TestStringIntBooleanItem()
		{
			MarshallingContextTestCase.StringIntBooleanItem writtenItem = new MarshallingContextTestCase.StringIntBooleanItem
				("one", 777, true);
			MarshallingContextTestCase.StringIntBooleanItem readItem = (MarshallingContextTestCase.StringIntBooleanItem
				)WriteAndRead(writtenItem);
			Assert.AreEqual(writtenItem._name, readItem._name);
			Assert.AreEqual(writtenItem._int, readItem._int);
			Assert.AreEqual(writtenItem._bool, readItem._bool);
		}

		private object WriteAndRead(object obj)
		{
			int imaginativeID = 500;
			ObjectReference @ref = new ObjectReference(ClassMetadataForObject(obj), imaginativeID
				);
			@ref.SetObject(obj);
			MarshallingContext marshallingContext = new MarshallingContext(Trans(), @ref, Container
				().UpdateDepthProvider().ForDepth(int.MaxValue), true);
			Handlers4.Write(@ref.ClassMetadata().TypeHandler(), marshallingContext, obj);
			Pointer4 pointer = marshallingContext.AllocateSlot();
			ByteArrayBuffer buffer = marshallingContext.ToWriteBuffer(pointer);
			buffer.Seek(0);
			//        String str = new String(buffer._buffer);
			//        System.out.println(str);
			UnmarshallingContext unmarshallingContext = new UnmarshallingContext(Trans(), @ref
				, Const4.AddToIdTree, false);
			unmarshallingContext.Buffer(buffer);
			unmarshallingContext.ActivationDepth(new LegacyActivationDepth(5));
			return unmarshallingContext.Read();
		}

		private ClassMetadata ClassMetadataForObject(object obj)
		{
			return Container().ProduceClassMetadata(Reflector().ForObject(obj));
		}
	}
}
