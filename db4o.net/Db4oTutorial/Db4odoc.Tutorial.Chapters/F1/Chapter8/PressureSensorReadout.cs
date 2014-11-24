using System;
using Db4objects.Db4o.Activation;

namespace Db4odoc.Tutorial.F1.Chapter8
{
    public class PressureSensorReadout : SensorReadout
    {
        private readonly double _pressure;

        public PressureSensorReadout(DateTime time, Car car, String description, double pressure)
            : base(time, car, description)
        {
            this._pressure = pressure;
        }

        public double Pressure
        {
            get
            {
				Activate(ActivationPurpose.Read);
                return _pressure;
            }
        }

        override public String ToString()
        {
            return String.Format("{0} pressure {1}", base.ToString(), _pressure);
        }
    }
}