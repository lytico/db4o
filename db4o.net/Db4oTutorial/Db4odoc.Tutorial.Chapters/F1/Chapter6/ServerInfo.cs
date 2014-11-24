namespace Db4odoc.Tutorial.F1.Chapter6
{
    /// <summary>
    /// Configuration used for StartServer and StopServer.
    /// </summary>
    public class ServerInfo
    {
        /// <summary>
        /// the host to be used.
        /// If you want to run the client server examples on two computers,
        /// enter the computer name of the one that you want to use as server. 
        /// </summary>
        public const string HOST = "localhost";  

        /// <summary>
        /// the database file to be used by the server.
        /// </summary>
        public const string FILE = "formula1.yap";

        /// <summary>
        /// the port to be used by the server.
        /// </summary>
        public const int PORT = 4488;

        /// <summary>
        /// the user name for access control.
        /// </summary>
        public const string USER = "db4o";
    
        /// <summary>
        /// the pasword for access control.
        /// </summary>
        public const string PASS = "db4o";
    }
}