/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Drs.Tests.Data;

namespace Db4objects.Drs.Tests.Data
{
	public class Car
	{
		private string _model;

		private Pilot _pilot;

		public Car()
		{
		}

		public Car(string model)
		{
			_model = model;
		}

		public virtual string GetModel()
		{
			return _model;
		}

		public virtual void SetModel(string model)
		{
			_model = model;
		}

		public virtual Pilot GetPilot()
		{
			return _pilot;
		}

		public virtual void SetPilot(Pilot newPilot)
		{
			_pilot = newPilot;
		}
	}
}
