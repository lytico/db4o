using System;
namespace NewCecilPerfTest.Data
{
	public class Pilot
	{
		public string name;
		public int points;
		public Pilot (string name, int points)
		{
			this.name = name;
			this.points = points;
		}
		
		public override string ToString()
		{
			return "Pilot[" + name + ";"+points+"]";
		}
	}
}
