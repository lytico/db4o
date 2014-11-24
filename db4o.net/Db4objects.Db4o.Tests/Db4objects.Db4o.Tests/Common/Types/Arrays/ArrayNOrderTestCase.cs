/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Tests.Common.Types.Arrays;

namespace Db4objects.Db4o.Tests.Common.Types.Arrays
{
	public class ArrayNOrderTestCase : AbstractDb4oTestCase
	{
		public class Data
		{
			public string[][][] _strArr;

			public object[][] _objArr;

			public Data(string[][][] strArr, object[][] objArr)
			{
				this._strArr = strArr;
				this._objArr = objArr;
			}
		}

		protected override void Store()
		{
			string[][][] strArr = new string[][][] { new string[][] { new string[3], new string
				[3] }, new string[][] { new string[3], new string[3] } };
			strArr[0][0][0] = "000";
			strArr[0][0][1] = "001";
			strArr[0][0][2] = "002";
			strArr[0][1][0] = "010";
			strArr[0][1][1] = "011";
			strArr[0][1][2] = "012";
			strArr[1][0][0] = "100";
			strArr[1][0][1] = "101";
			strArr[1][0][2] = "102";
			strArr[1][1][0] = "110";
			strArr[1][1][1] = "111";
			strArr[1][1][2] = "112";
			object[][] objArr = new object[][] { new object[2], new object[2] };
			objArr[0][0] = 0;
			objArr[0][1] = "01";
			objArr[1][0] = System.Convert.ToSingle(10);
			objArr[1][1] = 1.1;
			Db().Store(new ArrayNOrderTestCase.Data(strArr, objArr));
		}

		public virtual void Test()
		{
			ArrayNOrderTestCase.Data data = (ArrayNOrderTestCase.Data)((ArrayNOrderTestCase.Data
				)RetrieveOnlyInstance(typeof(ArrayNOrderTestCase.Data)));
			Check(data);
		}

		public virtual void Check(ArrayNOrderTestCase.Data data)
		{
			Assert.AreEqual("000", data._strArr[0][0][0]);
			Assert.AreEqual("001", data._strArr[0][0][1]);
			Assert.AreEqual("002", data._strArr[0][0][2]);
			Assert.AreEqual("010", data._strArr[0][1][0]);
			Assert.AreEqual("011", data._strArr[0][1][1]);
			Assert.AreEqual("012", data._strArr[0][1][2]);
			Assert.AreEqual("100", data._strArr[1][0][0]);
			Assert.AreEqual("101", data._strArr[1][0][1]);
			Assert.AreEqual("102", data._strArr[1][0][2]);
			Assert.AreEqual("110", data._strArr[1][1][0]);
			Assert.AreEqual("111", data._strArr[1][1][1]);
			Assert.AreEqual("112", data._strArr[1][1][2]);
			Assert.AreEqual(0, data._objArr[0][0]);
			Assert.AreEqual("01", data._objArr[0][1]);
			Assert.AreEqual(System.Convert.ToSingle(10), data._objArr[1][0]);
			Assert.AreEqual(1.1, data._objArr[1][1]);
		}
	}
}
