using System;
using Db4objects.Db4o.Config.Attributes;

namespace Db4oDoc.Code.Performance
{
    public class Item
    {
        [Indexed] private readonly string indexedString;
        [Indexed] private readonly int indexNumber;
        [Indexed] private readonly DateTime indexDate;

        public event Action AnEvent;

        public Item(int number)
        {
            this.indexedString = DataString(number);
            this.indexNumber = number;
            this.indexDate = new DateTime(number);
        }


        public static string DataString(int number)
        {
            return "data for " + number;
        }

        public bool ComplexMethod()
        {
            return indexedString.Contains((indexNumber%42).ToString());
        }

        public string IndexedString
        {
            get { return indexedString; }
        }

        public string PropertyWhichFiresEvent  
        {
            get
            {
                if (null != AnEvent)
                {
                    AnEvent();
                }
                return indexedString;
            }
        }

        public int IndexNumber
        {
            get { return indexNumber; }
        }

        public DateTime IndexDate
        {
            get { return indexDate; }
        }
    }
}