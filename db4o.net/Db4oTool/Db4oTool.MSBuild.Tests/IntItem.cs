using System;
using System.Collections.Generic;
using System.Text;

namespace Db4oTool.MSBuild.Tests
{
    public class IntItem
    {
        public int _intValue;
        public IntItem _next;
        public bool _isRoot;

        public static IntItem NewIntItem(int depth)
        {
            if (depth == 0)
            {
                return null;
            }

            IntItem root = new IntItem();
            root._intValue = depth;
            root._next = NewIntItem(depth - 1);
            return root;
        }

        public int GetIntValue()
        {
            return _intValue;
        }

        public IntItem Next()
        {
            return _next;
        }
    }
}
