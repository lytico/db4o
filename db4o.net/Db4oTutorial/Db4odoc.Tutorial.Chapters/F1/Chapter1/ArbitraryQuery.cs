using Db4objects.Db4o.Query;

namespace Db4odoc.Tutorial.F1.Chapter1
{
	public class ArbitraryQuery : Predicate
    {
    	private int[] _points;
    	
    	public ArbitraryQuery(int[] points)
    	{
    		_points=points;
    	}
    
    	public bool Match(Pilot pilot)
    	{
        	foreach (int points in _points)
        	{
        		if (pilot.Points == points)
        		{
        			return true;
        		}
        	}
        	return pilot.Name.StartsWith("Rubens");
    	}
    }
}
