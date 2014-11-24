using Db4objects.Db4o.Query;

namespace Db4objects.Db4o.Query
{
    public interface ISodaQueryFactory
    {
        /// <summary>
        /// creates a new SODA
        /// <see cref="Db4objects.Db4o.Query.IQuery">Query</see>
        /// .
        /// <br /><br />
        /// Linq queries are the recommended main db4o query interface.
        /// <br /><br />
        /// Use
        /// <see cref="Db4objects.Db4o.IObjectContainer.QueryByExample">QueryByExample(Object template)</see>
        /// for simple Query-By-Example.<br /><br />
        /// </summary>
        /// <returns>a new IQuery object</returns>
        IQuery Query();
    }
}