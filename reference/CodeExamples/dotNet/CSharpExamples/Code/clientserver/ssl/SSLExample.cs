using System;
using System.IO;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Db4objects.Db4o;
using Db4objects.Db4o.CS;
using Db4objects.Db4o.CS.Config;

namespace Db4oDoc.Code.ClientServer.SSL
{
    public class SSLExample
    {
        private const string DatabaseFileName = "database.db4o";
        private const string UserAndPassword = "sa";
        private const int PortNumber = 1337;

        public static void Main(string[] args)
        {
            TheSSLExample();
        }


        private static void TheSSLExample()
        {
            CleanUp();
            // #example: Add SSL-support to the server
            IServerConfiguration configuration = Db4oClientServer.NewServerConfiguration();
            // For the server you need a certificate. For example using a certificate from a file
            X509Certificate2 certificate = new X509Certificate2("cert.cer");
            configuration.AddConfigurationItem(new ServerSslSupport(certificate));
            // #end example

            using (IObjectServer server = OpenServer(configuration))
            {
                SSLClient();
            }
            CleanUp();
        }
        

        private static IObjectServer OpenServer(IServerConfiguration configuration)
        {
            IObjectServer server = Db4oClientServer.OpenServer(configuration, DatabaseFileName, PortNumber);
            server.GrantAccess(UserAndPassword, UserAndPassword);
            return server;
        }

        private static void SSLClient()
        {
            // #example: Add SSL-support to the client
            IClientConfiguration configuration = Db4oClientServer.NewClientConfiguration();
            configuration.AddConfigurationItem(new ClientSslSupport(CheckCertificate));
            // #end example
            using (IObjectContainer container = OpenClient(configuration))
            {
                container.Store(new Person());
                Console.Out.WriteLine("Stored person");
            }
        }

        // #example: callback for validating the certificate
        private static bool CheckCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslpolicyerrors)
        {
            // here you can check the certificates of the server and accept it if it's ok.)
            return true;
        }
        // #end example

        private static IObjectContainer OpenClient(IClientConfiguration configuration)
        {
            return Db4oClientServer.OpenClient(configuration, "localhost",
                                               PortNumber, UserAndPassword, UserAndPassword);
        }

        private static void CleanUp()
        {
            File.Delete(DatabaseFileName);
        }


        private class Person
        {
            private string name;

            public Person()
            {
                name = "Joe";
            }
        }
    }
}