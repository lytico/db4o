/* Copyright (C) 2004 - 2008  Versant Inc.  http://www.db4o.com */
using System;

namespace Sharpen.Text
{
    public class DecimalFormat
    { 
        private string _format;
        public DecimalFormat(string format)
        {
            _format = format;
        }

        public string Format(double number)
        {
            Double temp = (Double)number;
            return temp.ToString(_format);
        }
    }
}