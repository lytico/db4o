using Db4objects.Db4o.Query;

namespace Db4odoc.Tutorial.F1.Chapter5
{
	public class RetrieveAllSensorReadoutsPredicate : Predicate 
	{
		public bool Match(SensorReadout candidate)
		{
			return true;
		}
	}
}