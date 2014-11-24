/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
using System;

namespace Db4objects.Db4o.Internal
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class TagAttribute : Attribute
    {
        public TagAttribute(string tag)
        {
            this.tag = tag;
        }

        public string Tag
        {
            get { return tag; }
        }

        private string tag;
    }
}
