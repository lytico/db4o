using System;
using System.Collections;

namespace Db4odoc.Tutorial.F1.Chapter5
{   
    public class Car
    {
        string _model;
        Pilot _pilot;
        IList _history;
        
        public Car(string model)
        {
            _model = model;
            _pilot = null;
            _history = new ArrayList();
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
                return _model;
            }
        }
        
        public SensorReadout[] GetHistory()
        {
            SensorReadout[] history = new SensorReadout[_history.Count];
            _history.CopyTo(history, 0);
            return history;
        }
        
        public void Snapshot()
        {
            _history.Add(new TemperatureSensorReadout(DateTime.Now, this, "oil", PollOilTemperature()));
            _history.Add(new TemperatureSensorReadout(DateTime.Now, this, "water", PollWaterTemperature()));
            _history.Add(new PressureSensorReadout(DateTime.Now, this, "oil", PollOilPressure()));
        }
        
        protected double PollOilTemperature()
        {
            return 0.1*_history.Count;
        }
        
        protected double PollWaterTemperature()
        {
            return 0.2*_history.Count;
        }
        
        protected double PollOilPressure()
        {
            return 0.3*_history.Count;
        }
        
        override public string ToString()
        {
			return string.Format("{0}[{1}]/{2}", _model, _pilot, _history.Count);
        }
    }
}
