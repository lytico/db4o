using Db4objects.Db4o.Query;

namespace Db4odoc.Tutorial.F1.Chapter1
{
	public class ComplexQuery : Predicate
    {
    	public bool Match(Pilot pilot)
    	{
	    	return pilot.Points > 99
                && pilot.Points < 199
                || pilot.Name=="Rubens Barrichello";
    	}
    }
}
