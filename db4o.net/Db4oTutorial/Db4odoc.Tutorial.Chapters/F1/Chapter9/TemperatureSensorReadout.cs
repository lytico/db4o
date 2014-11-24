using System;
using Db4objects.Db4o.Activation;

namespace Db4odoc.Tutorial.F1.Chapter9
{
    public class TemperatureSensorReadout : SensorReadout
    {
        private readonly double _temperature;

        public TemperatureSensorReadout(DateTime time, Car car, string description, double temperature)
            : base(time, car, description)
        {
            this._temperature = temperature;
        }

        public double Temperature
        {
            get
            {
				Activate(ActivationPurpose.Read);
                return _temperature;
            }
        }

        public override String ToString()
        {
			Activate(ActivationPurpose.Read);
            return string.Format("{0} temp : {1}", base.ToString(), _temperature);
        }
    }
}