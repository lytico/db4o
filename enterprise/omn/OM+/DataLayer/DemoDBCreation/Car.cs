/* Copyright (C) 2004 - 2007  Versant Inc.  http://www.db4o.com

This file is part of the db4o open source object database.

db4o is free software; you can redistribute it and/or modify it under
the terms of version 2 of the GNU General Public License as published
by the Free Software Foundation and as clarified by db4objects' GPL 
interpretation policy, available at
http://www.db4o.com/about/company/legalpolicies/gplinterpretation/
Alternatively you can write to db4objects, Inc., 1900 S Norfolk Street,
Suite 350, San Mateo, CA 94403, USA.

db4o is distributed in the hope that it will be useful, but WITHOUT ANY
WARRANTY; without even the implied warranty of MERCHANTABILITY or
FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License
for more details.

You should have received a copy of the GNU General Public License along
with this program; if not, write to the Free Software Foundation, Inc.,
59 Temple Place - Suite 330, Boston, MA  02111-1307, USA. */
using System;
using System.Collections;
using System.Collections.Generic;


namespace DemoDbCreation
{
    public class Car
    {
        string _carName;
        Pilot _pilot;
        IList _history;
        ArrayList _list;
        int _count;
        float[] _dblArr;
        int[] _intArrl;        
        char _character;
        decimal _decimal;
        Int16 _int16;
       
        Hashtable _htbl;
        string _string;
        char[] _charArr;



        public ArrayList List
        {
            get { return _list; }
            set { _list = value; }
        } 

        public string String
        {
            get { return _string; }
            set { _string = value; }
        }
       

        public Int16 Int16
        {
            get { return _int16; }
            set { _int16 = value; }
        }
        public int[] IntArrl
        {
            get { return _intArrl; }
            set { _intArrl = value; }
        }


        public decimal DecimalProp
        {
            get { return _decimal; }
            set { _decimal = value; }
        }
        public char Character
        {
            get { return _character; }
            set { _character = value; }
        }
     
       
        public char[] CharArr
        {
            get { return _charArr; }
            set { _charArr = value; }
        }

      

        public Hashtable Htbl
        {
            get { return _htbl; }
            set { _htbl = value; }
        }

        public float[] DblArr
        {
            get { return _dblArr; }
            set { _dblArr = value; }
        }

        public int Count
        {
            get { return _count; }
            set { _count = value; }
        }
        
        public Car(string model) : this(model, new ArrayList())
        {
        }
        
        public Car(string model, IList history)
        {
            _carName = model;
            _pilot = null;
            _history = history;
        }
        

        public Pilot Pilot
        {
            get
            {
                return _pilot;
            }
            
            set
            {
                _pilot = value;
            }
        }
        
        public string Model
        {
            get
            {
                return _carName;
            }
        }
        
        public IList History
        {
        	get
        	{
	            return _history;
	        }
            set
            {
                _history = value;
            }
        }
        
      
    }

    
}
