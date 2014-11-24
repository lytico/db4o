using Db4objects.Db4o.Query;

using Db4odoc.Tutorial.F1.Chapter4;

namespace Db4odoc.Tutorial.F1.Chapter7
{	
	public class EvenHistoryEvaluation : IEvaluation
	{
		public void Evaluate(ICandidate candidate)
		{
			Car car=(Car)candidate.GetObject();
			candidate.Include(car.History.Count % 2 == 0);
		}
	}
}