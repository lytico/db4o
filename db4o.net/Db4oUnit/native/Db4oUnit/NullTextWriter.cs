using System;
using System.Text;
using System.IO;

namespace Db4oUnit
{
    class NullTextWriter : TextWriter
    {
        override public Encoding Encoding
        {
            get 
            {
                return Encoding.UTF8;
            }
        }
    }
}
