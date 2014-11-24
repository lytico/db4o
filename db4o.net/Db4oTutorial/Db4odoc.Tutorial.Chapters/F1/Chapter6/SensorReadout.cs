using System;

namespace Db4odoc.Tutorial.F1.Chapter6
{   
    public abstract class SensorReadout
    {
        DateTime _time;
        Car _car;
        string _description;
        SensorReadout _next;
        
        protected SensorReadout(DateTime time, Car car, string description)
        {
            _time = time;
            _car = car;
            _description = description;
            _next = null;            
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
        
        public SensorReadout Next
        {
            get
            {
                return _next;
            }
        }
        
        public void Append(SensorReadout sensorReadout)
        {
            if (_next == null)
            {
                _next = sensorReadout;
            }
            else
            {
                _next.Append(sensorReadout);
            }
        }
        
        public int CountElements()
        {
            return (_next == null ? 1 : _next.CountElements() + 1);
        }
        
        override public string ToString()
        {
            return string.Format("{0} : {1} : {2}", _car, _time, _description);
        }
    }
}
