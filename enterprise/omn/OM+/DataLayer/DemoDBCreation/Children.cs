using System;
using System.Collections.Generic;
using System.Text;

namespace DemoDbCreation
{
    public class Children
    {
        string _childname;
        int _childNo;
        DateTime _dateOfBirth;

        public DateTime DateOfBirth
        {
            get { return _dateOfBirth; }
            set { _dateOfBirth = value; }
        }
        public string Child_name
        {
            get { return _childname; }
            set { _childname = value; }
        }
       

        public int Child_no
        {
            get { return _childNo; }
            set { _childNo = value; }
        }

    }
}
