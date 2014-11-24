using System;
using System.Collections.Generic;
using System.IO;
using Db4objects.Db4o;
using Db4objects.Db4o.CS;

namespace Db4oDoc.Code.Container.Sessions
{
    public class Db4oSessions
    {
        private const string DatabaseFileName = "database.db4o";

        public static void Main(string[] args)
        {
            Sessions();
            SessionsIsolation();
            SessionCache();
            EmbeddedClient();
        }


        private static void Sessions()
        {
            CleanUp();
            // #example: Session object container
            using (IObjectContainer rootContainer = Db4oEmbedded.OpenFile(DatabaseFileName))
            {
                // open the db4o-session. For example at the beginning for a web-request
                using (IObjectContainer session = rootContainer.Ext().OpenSession())
                {
                    // do the operations on the session-container
                    session.Store(new Person("Joe"));
                }
            }
            // #end example
        }

        private static void SessionsIsolation()
        {
            CleanUp();
            using (IObjectContainer rootContainer = Db4oEmbedded.OpenFile(DatabaseFileName))
            {
                using (IObjectContainer session1 = rootContainer.Ext().OpenSession(),
                                        session2 = rootContainer.Ext().OpenSession())
                {
                    // #example: Session are isolated from each other
                    session1.Store(new Person("Joe"));
                    session1.Store(new Person("Joanna"));

                    // the second session won't see the changes until the changes are committed
                    PrintAll(session2.Query<Person>());

                    session1.Commit();

                    // new the changes are visiable for the second session
                    PrintAll(session2.Query<Person>());
                    // #end example
                }
            }
        }

        
    private static void SessionCache(){
        CleanUp();
        using(IObjectContainer rootContainer = Db4oEmbedded.OpenFile(DatabaseFileName)){
            using(IObjectContainer session1 = rootContainer.Ext().OpenSession(),
                 session2 = rootContainer.Ext().OpenSession())
            {
                StoreAPerson(session1);

                // #example: Each session does cache the objects
                Person personOnSession1 = session1.Query<Person>()[0];
                Person personOnSession2 = session2.Query<Person>()[0];

                personOnSession1.Name = "NewName";
                session1.Store(personOnSession1);
                session1.Commit();


                // the second session still sees the old value, because it was cached
                Console.WriteLine(personOnSession2.Name);
                // you can explicitly refresh it
                session2.Ext().Refresh(personOnSession2,int.MaxValue);
                Console.WriteLine(personOnSession2.Name);
                // #end example
            }

        }
    }

        private static void EmbeddedClient()
        {
            CleanUp();
            // #example: Embedded client
            using (IObjectServer server = Db4oClientServer.OpenServer(DatabaseFileName, 0))
            {
                // open the db4o-embedded client. For example at the beginning for a web-request
                using (IObjectContainer container = server.OpenClient())
                {
                    // do the operations on the session-container
                    container.Store(new Person("Joe"));
                }
            }
            // #end example
        }


        private static void PrintAll(IEnumerable<Person> persons)
        {
            foreach (Person person in persons)
            {
                Console.WriteLine(person);
            }
        }
        private static void StoreAPerson(IObjectContainer session1)
        {
            session1.Store(new Person("Joe"));
            session1.Commit();
        }

        private static void CleanUp()
        {
            File.Delete(DatabaseFileName);
        }


        private class Person
        {
            private string name;

            public Person(string name)
            {
                this.name = name;
            }

            public string Name
            {
                get { return name; }
                set { name = value; }
            }

            public override string ToString()
            {
                return string.Format("Name: {0}", name);
            }
        }
    }
}