using Db4objects.Db4o;

namespace ComputationBlock
{
	public interface IComputation
	{
		object Compute(IObjectContainer container);
	}
}
