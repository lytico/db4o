/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Types.Arrays;

namespace Db4objects.Db4o.Tests.Common.Types.Arrays
{
	public class ByteArrayTestCase : AbstractDb4oTestCase
	{
		public static void Main(string[] args)
		{
			new ByteArrayTestCase().RunAll();
		}

		public interface IIByteArrayHolder
		{
			byte[] GetBytes();
		}

		[System.Serializable]
		public class SerializableByteArrayHolder : ByteArrayTestCase.IIByteArrayHolder
		{
			private const long serialVersionUID = 1L;

			internal byte[] _bytes;

			public SerializableByteArrayHolder(byte[] bytes)
			{
				this._bytes = bytes;
			}

			public virtual byte[] GetBytes()
			{
				return _bytes;
			}
		}

		public class ByteArrayHolder : ByteArrayTestCase.IIByteArrayHolder
		{
			public byte[] _bytes;

			public ByteArrayHolder(byte[] bytes)
			{
				this._bytes = bytes;
			}

			public virtual byte[] GetBytes()
			{
				return _bytes;
			}
		}

		internal const int Instances = 2;

		internal const int ArrayLength = 1024;

		#if !CF && !SILVERLIGHT
		protected override void Configure(IConfiguration config)
		{
			config.ObjectClass(typeof(ByteArrayTestCase.SerializableByteArrayHolder)).Translate
				(new TSerializable());
		}
		#endif // !CF && !SILVERLIGHT

		protected override void Store()
		{
			for (int i = 0; i < Instances; ++i)
			{
				Db().Store(new ByteArrayTestCase.ByteArrayHolder(CreateByteArray()));
				Db().Store(new ByteArrayTestCase.SerializableByteArrayHolder(CreateByteArray()));
			}
		}

		#if !CF && !SILVERLIGHT
		/// <exception cref="System.Exception"></exception>
		public virtual void TestByteArrayHolder()
		{
			TimeQueryLoop("raw byte array", typeof(ByteArrayTestCase.ByteArrayHolder));
		}
		#endif // !CF && !SILVERLIGHT

		#if !CF && !SILVERLIGHT
		/// <exception cref="System.Exception"></exception>
		public virtual void TestSerializableByteArrayHolder()
		{
			TimeQueryLoop("TSerializable", typeof(ByteArrayTestCase.SerializableByteArrayHolder
				));
		}
		#endif // !CF && !SILVERLIGHT

		/// <exception cref="System.Exception"></exception>
		private void TimeQueryLoop(string label, Type clazz)
		{
			IQuery query = NewQuery(clazz);
			IObjectSet os = query.Execute();
			Assert.AreEqual(Instances, os.Count);
			while (os.HasNext())
			{
				Assert.AreEqual(ArrayLength, ((ByteArrayTestCase.IIByteArrayHolder)os.Next()).GetBytes
					().Length, label);
			}
		}

		internal virtual byte[] CreateByteArray()
		{
			byte[] bytes = new byte[ArrayLength];
			for (int i = 0; i < bytes.Length; ++i)
			{
				bytes[i] = (byte)(i % 256);
			}
			return bytes;
		}
	}
}
