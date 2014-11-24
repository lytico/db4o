namespace Db4odoc.Tutorial.F1.Chapter10
{
    public class Car
    {
        string _model;
        Pilot _pilot;
        
        public Car(string model, Pilot pilot)
        {
            _model = model;
            _pilot = pilot;
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
        
        override public string ToString()
        {
			return string.Format("{0}[{1}]", _model, _pilot);
        }
    }
}
