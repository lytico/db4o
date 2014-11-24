/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
using System;
using Db4oUnit;
using Db4oUnit.Extensions;

namespace Db4objects.Db4o.Tests.CLI1
{
    public class CsValueTypesTestCase : AbstractDb4oTestCase
    {
		public static decimal DECIMAL_PREC = 9999999999999999999999999999m;
		public static float FLOAT_PREC = 1.123456E+38f;
		public static double DOUBLE_PREC = 1.12345678901234E-300;

		public bool boolMin;
		public bool boolMax;

		public byte byteMin;
		public byte byteOne;
		public byte byteFuzzy;
		public byte byteMax;

		public sbyte sbyteMin;
		public sbyte sbyteNegOne;
		public sbyte sbyteOne;
		public sbyte sbyteFuzzy;
		public sbyte sbyteMax;

		public char charMin;
		public char charOne;
		public char charFuzzy;
		public char charMax;

		public decimal decimalMin;
		public decimal decimalNegOne;
		public decimal decimalOne;
		public decimal decimalPrec;
		public decimal decimalMax;

		public double doubleMin;
		public double doubleNegOne;
		public double doubleOne;
		public double doublePrec;
		public double doubleMax;

		public float floatMin;
		public float floatNegOne;
		public float floatOne;
		public float floatPrec;
		public float floatMax;

		public int intMin;
		public int intNegOne;
		public int intOne;
		public int intFuzzy;
		public int intMax;

		public uint uintMin;
		public uint uintOne;
		public uint uintFuzzy;
		public uint uintMax;

		public long longMin;
		public long longNegOne;
		public long longOne;
		public long longFuzzy;
		public long longMax;

		public ulong ulongMin;
		public ulong ulongOne;
		public ulong ulongFuzzy;
		public ulong ulongMax;

		public short shortMin;
		public short shortNegOne;
		public short shortOne;
		public short shortFuzzy;
		public short shortMax;

		public ushort ushortMin;
		public ushort ushortOne;
		public ushort ushortFuzzy;
		public ushort ushortMax;

		public DateTime dateTimeMin;
		public DateTime dateTimeOne;
		public DateTime dateTimeFuzzy;
		public DateTime dateTimeMax;

		public String name;

        override protected void Store()
        {
            CsValueTypesTestCase cs = new CsValueTypesTestCase();
            cs.name = "AllNull";
            Store(cs);
            cs = new CsValueTypesTestCase();
            cs.SetValues();
            Store(cs);
        }

        public void SetValues()
        {
            boolMin = false;
            boolMax = true;

            byteMin = byte.MinValue;
            byteOne = 1;
            byteFuzzy = 123;
            byteMax = byte.MaxValue;

            sbyteMin = sbyte.MinValue;
            sbyteNegOne = -1;
            sbyteOne = 1;
            sbyteFuzzy = 123;
            sbyteMax = sbyte.MaxValue;

            charMin = char.MinValue;
            charOne = (char)1;
            charFuzzy = (char)123;
            charMax = char.MaxValue;

            decimalMin = decimal.MinValue;
            decimalNegOne = -1;
            decimalOne = 1;
            decimalPrec = DECIMAL_PREC;
            decimalMax = decimal.MaxValue;

            doubleMin = double.MinValue;
            doubleNegOne = -1;
            doubleOne = 1;
            doublePrec = DOUBLE_PREC;
            doubleMax = double.MaxValue;

            floatMin = float.MinValue;
            floatNegOne = -1;
            floatOne = 1;
            floatPrec = FLOAT_PREC;
            floatMax = float.MaxValue;

            intMin = int.MinValue;
            intNegOne = -1;
            intOne = 1;
            intFuzzy = 1234567;
            intMax = int.MaxValue;

            uintMin = uint.MinValue;
            uintOne = 1;
            uintFuzzy = 1234567;
            uintMax = uint.MaxValue;

            longMin = long.MinValue;
            longNegOne = -1;
            longOne = 1;
            longFuzzy = 1234567891;
            longMax = long.MaxValue;

            ulongMin = ulong.MinValue;
            ulongOne = 1;
            ulongFuzzy = (ulong)87638635562;
            ulongMax = ulong.MaxValue;

            shortMin = short.MinValue;
            shortNegOne = -1;
            shortOne = 1;
            shortFuzzy = 12345;
            shortMax = short.MaxValue;

            ushortMin = ushort.MinValue;
            ushortOne = 1;
            ushortFuzzy = 12345;
            ushortMax = ushort.MaxValue;

            dateTimeMin = DateTime.MinValue;
            dateTimeOne = new DateTime(1);
            dateTimeFuzzy = new DateTime(2000, 3, 4, 2, 3, 4, 5);
            dateTimeMax = DateTime.MaxValue;

        }

        public void Test()
        {
            IObjectSet os = NewQuery(typeof (CsValueTypesTestCase)).Execute();
            Assert.AreEqual(2, os.Count);
            CsValueTypesTestCase cs1 = (CsValueTypesTestCase)os.Next();
            CsValueTypesTestCase cs2 = (CsValueTypesTestCase)os.Next();
            if (cs1.name != null)
            {
                cs1.CheckAllNull();
                cs2.CheckAllPresent();
            }
            else
            {
                cs1.CheckAllPresent();
                cs2.CheckAllNull();
            }
        }

        public void CheckAllPresent()
        {
            Assert.IsFalse(boolMin);
            Assert.IsTrue(boolMax);

            Assert.AreEqual(byte.MinValue, byteMin);
            Assert.AreEqual(1, byteOne);
            Assert.AreEqual(123, byteFuzzy);
            Assert.AreEqual(byte.MaxValue, byteMax);

            Assert.AreEqual(sbyte.MinValue, sbyteMin);
            Assert.AreEqual(-1, sbyteNegOne);
            Assert.AreEqual(1, sbyteOne);
            Assert.AreEqual(123, sbyteFuzzy);
            Assert.AreEqual(sbyte.MaxValue, sbyteMax);

            Assert.AreEqual(char.MinValue, charMin);
            Assert.AreEqual(1, charOne);
            Assert.AreEqual((char)123, charFuzzy);
            Assert.AreEqual(char.MaxValue, charMax);

            Assert.AreEqual(decimal.MinValue, decimalMin);
            Assert.AreEqual((decimal)-1, decimalNegOne);
            Assert.AreEqual((decimal)1, decimalOne);
            Assert.AreEqual(DECIMAL_PREC, decimalPrec);
            Assert.AreEqual(decimal.MaxValue, decimalMax);

            Assert.AreEqual(double.MinValue, doubleMin);
            Assert.AreEqual(-1, doubleNegOne);
            Assert.AreEqual(1, doubleOne);
            Assert.AreEqual(DOUBLE_PREC, doublePrec);
            Assert.AreEqual(double.MaxValue, doubleMax);

            Assert.AreEqual(float.MinValue, floatMin);
            Assert.AreEqual(-1f, floatNegOne);
            Assert.AreEqual(1f, floatOne);
            Assert.AreEqual(FLOAT_PREC, floatPrec);
            Assert.AreEqual(float.MaxValue, floatMax);

            Assert.AreEqual(int.MinValue, intMin);
            Assert.AreEqual(-1, intNegOne);
            Assert.AreEqual(1, intOne);
            Assert.AreEqual(1234567, intFuzzy);
            Assert.AreEqual(int.MaxValue, intMax);

            Assert.AreEqual(uint.MinValue, uintMin);
            Assert.AreEqual(1, uintOne);
            Assert.AreEqual(1234567, uintFuzzy);
            Assert.AreEqual(uint.MaxValue, uintMax);

            Assert.AreEqual(long.MinValue, longMin);
            Assert.AreEqual(-1, longNegOne);
            Assert.AreEqual(1, longOne);
            Assert.AreEqual(1234567891, longFuzzy);
            Assert.AreEqual(long.MaxValue, longMax);

            Assert.AreEqual(ulong.MinValue, ulongMin);
            Assert.AreEqual(1, ulongOne);
            Assert.AreEqual(87638635562, ulongFuzzy);
            Assert.AreEqual(ulong.MaxValue, ulongMax);

            Assert.AreEqual(short.MinValue, shortMin);
            Assert.AreEqual(-1, shortNegOne);
            Assert.AreEqual(1, shortOne);
            Assert.AreEqual(12345, shortFuzzy);
            Assert.AreEqual(short.MaxValue, shortMax);

            Assert.AreEqual(ushort.MinValue, ushortMin);
            Assert.AreEqual(1, ushortOne);
            Assert.AreEqual(12345, ushortFuzzy);
            Assert.AreEqual(ushort.MaxValue, ushortMax);

            Assert.AreEqual(DateTime.MinValue, dateTimeMin);
            Assert.AreEqual(new DateTime(1), dateTimeOne);
            Assert.AreEqual(new DateTime(2000, 3, 4, 2, 3, 4, 5), dateTimeFuzzy);
            Assert.AreEqual(DateTime.MaxValue, dateTimeMax);
        }

        public void CheckAllNull()
        {
            Assert.AreEqual("AllNull", name);
        }
    }
}
