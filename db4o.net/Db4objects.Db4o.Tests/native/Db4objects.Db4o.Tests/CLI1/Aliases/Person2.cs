/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
using System;

namespace Db4objects.Db4o.Tests.CLI1.Aliases
{
    public class Person2 : IPerson
    {
        public String _name;

        public Person2(String name)
        {
            _name = name;
        }

        public String Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public override bool Equals(object obj)
        {
            Person2 other = obj as Person2;
            if (null == other) return false;
            return CFHelper.AreEqual(_name, other._name);
        }
    }
}