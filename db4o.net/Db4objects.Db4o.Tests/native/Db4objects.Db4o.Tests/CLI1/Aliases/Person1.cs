/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
using System;

namespace Db4objects.Db4o.Tests.CLI1.Aliases
{
    public class Person1
    {
        public String _name;

        public Person1(String name)
        {
            _name = name;
        }

        public String Name
        {
            get { return _name; }
        }
    }
}