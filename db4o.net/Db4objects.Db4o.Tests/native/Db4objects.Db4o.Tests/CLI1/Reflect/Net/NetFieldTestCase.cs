/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
namespace Db4objects.Db4o.Tests.CLI1.Reflect.Net
{
	using System;
	using Db4oUnit;
	using Db4objects.Db4o.Reflect.Net;
	
	public class CustomTransientAttribute : Attribute
	{
	    public override string ToString()
		{
			return "CustomTransient";
		}
	}

    public class WhateverAttribute : Attribute
    {   
    }
	
	public class NetFieldTestCase : ITestCase, ITestLifeCycle
	{
        public class Item
        {
            public object RawField;

            [NonSerialized]
            public object NonSerializedField;

            [Transient]
            public object Db4oTransientField;

            [CustomTransient]
            public object CustomTransientField;

            [Whatever]
            public object FieldWithCustomAttribute;
        }

        public void SetUp()
        {
			NetField.ResetTransientMarkers();
        }

        public void TearDown()
        {
        }

		public void TestIsTransientRefusesRawFields()
		{
			AssertIsNotTransient("RawField");
		}
		
		public void TestIsTransientUnderstandsNonSerialized()
		{
			AssertIsTransient("NonSerializedField");
		}
		
		public void TestIsTransientUnderstandsDb4oTransient()
		{
			AssertIsTransient("Db4oTransientField");
		}
		
		public void TestIsTransientUnderstandsCustomTransient()
		{
			AssertIsNotTransient("CustomTransientField");
			AssertIsNotTransient("FieldWithCustomAttribute");
			
            NetField.MarkTransient(typeof(CustomTransientAttribute));
			
			AssertIsTransient("CustomTransientField");
			AssertIsNotTransient("FieldWithCustomAttribute");
		}

	    private static void AssertIsNotTransient(string fieldName)
	    {
	        Assert.IsFalse(IsTransient(fieldName));
	    }

	    private static void AssertIsTransient(string fieldName)
	    {
	        Assert.IsTrue(IsTransient(fieldName));
	    }

	    private static bool IsTransient(string name)
	    {
	        return NetField.IsTransient(typeof(Item).GetField(name));
	    }
	}
}
