using System;
using System.Collections;
    
namespace Db4odoc.Tutorial.F1.Chapter4
{
    public class Car
    {
        string _model;
        Pilot _pilot;
        IList _history;
        
        public Car(string model) : this(model, new ArrayList())
        {
        }
        
        public Car(string model, IList history)
        {
            _model = model;
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
                return _model;
            }
        }
        
        public IList History
        {
        	get
        	{
	            return _history;
	        }
        }
        
        public void Snapshot()
        {
            _history.Add(new SensorReadout(Poll(), DateTime.Now, this));
        }
        
        protected double[] Poll()
        {
            int factor = _history.Count + 1;
            return new double[] { 0.1d*factor, 0.2d*factor, 0.3d*factor };
        }
        
        override public string ToString()
        {
			return string.Format("{0}[{1}]/{2}", _model, _pilot, _history.Count);
        }
    }
}
