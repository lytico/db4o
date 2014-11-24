using System;
using System.Text;

namespace Db4oDoc.Code.Tuning.Monitoring
{
    class DataObject
    {
        private string label;

        internal DataObject(Random rnd)
        {
            this.label = NewString(rnd);
        }

        private static string NewString(Random rnd)
        {
            StringBuilder buffer = new StringBuilder();
            for (int i = 0; i < rnd.Next(4096); i++)
            {
                int charNr = 65 + rnd.Next(26);
                buffer.Append((char)charNr);
            }
            return buffer.ToString();
        }
    }
}