using Db4objects.Db4o;
using Db4objects.Db4o.CS;

namespace Db4oDoc.Code.ClientServer.Basics
{
    public class Db4oClientExample
    {
        public static void main(string[] args)
        {
            // #example: Connect to the server
            using (IObjectContainer container
                = Db4oClientServer.OpenClient("localhost", 8080, "user", "password"))
            {
                // Your operations
            }
            // #end example
        }
    }
}