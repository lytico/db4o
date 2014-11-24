using System;
using Db4objects.Db4o;
using Db4objects.Db4o.Activation;
using Db4objects.Db4o.TA;

namespace Db4odoc.Tutorial.F1.Chapter8
{
    public class Pilot : IActivatable
    {
        private readonly String _name;
        private int _points;
        
        [Transient]
        private IActivator _activator;

        public Pilot(String name,int points) 
        {
            this._name=name;
            this._points=points;
        }

        public int Points
        {
            get
            {
				Activate(ActivationPurpose.Read);
                return _points;
            }

            set
            {
				Activate(ActivationPurpose.Write);
                _points += value;
            }
        }

        public String Name
        {
            get
            {
				Activate(ActivationPurpose.Read);
                return _name;
            }
        }

        public override string  ToString()
        {
			Activate(ActivationPurpose.Read);
            return String.Format("{0}/{1}", _name, _points);
        }
        
        public void Activate(ActivationPurpose purpose) 
        {
            if(_activator != null) 
            {
                _activator.Activate(purpose);
            }
        }

        public void Bind(IActivator activator) 
        {
            if (_activator == activator)
            {
                return;
            }
            if (activator != null && null != _activator)
            {
                throw new System.InvalidOperationException();
            }
            _activator = activator;
        }
    }
}