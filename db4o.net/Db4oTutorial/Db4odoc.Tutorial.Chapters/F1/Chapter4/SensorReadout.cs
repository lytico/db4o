using System;
using System.Text;
    
namespace Db4odoc.Tutorial.F1.Chapter4
{   
    public class SensorReadout
    {
        double[] _values;
        DateTime _time;
        Car _car;
        
        public SensorReadout(double[] values, DateTime time, Car car)
        {
            _values = values;
            _time = time;
            _car = car;
        }
        
        public Car Car
        {
            get
            {
                return _car;
            }
        }
        
        public DateTime Time
        {
            get
            {
                return _time;
            }
        }
        
        public int NumValues
        {
            get
            {
                return _values.Length;
            }
        }
        
        public double[] Values
        {
        	get
        	{
        		return _values;
        	}
        }
        
        public double GetValue(int idx)
        {
            return _values[idx];
        }
        
        override public string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(_car);
            builder.Append(" : ");
            builder.Append(_time.TimeOfDay);
            builder.Append(" : ");
            for (int i=0; i<_values.Length; ++i)
            {
                if (i > 0)
                {
                    builder.Append(", ");
                }
                builder.Append(_values[i]);
            }
            return builder.ToString();
        }
    }
}
