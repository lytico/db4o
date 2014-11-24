/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Drs.Inside.Traversal;

namespace Db4objects.Drs.Inside.Traversal
{
	public interface ITraverser
	{
		/// <summary>
		/// Traversal will only stop when visitor.visit(...) returns false, EVEN IN
		/// THE PRESENCE OF CIRCULAR REFERENCES.
		/// </summary>
		/// <remarks>
		/// Traversal will only stop when visitor.visit(...) returns false, EVEN IN
		/// THE PRESENCE OF CIRCULAR REFERENCES. So it is up to the visitor to detect
		/// circular references if necessary. Transient fields are not visited. The
		/// fields of second class objects such as Strings and Dates are also not visited.
		/// </remarks>
		void TraverseGraph(object @object, IVisitor visitor);

		/// <summary>Should only be called during a traversal.</summary>
		/// <remarks>
		/// Should only be called during a traversal. Will traverse the graph
		/// for the received object too, using the current visitor. Used to
		/// extend the traversal to a possibly disconnected object graph.
		/// </remarks>
		void ExtendTraversalTo(object disconnected);
	}
}
