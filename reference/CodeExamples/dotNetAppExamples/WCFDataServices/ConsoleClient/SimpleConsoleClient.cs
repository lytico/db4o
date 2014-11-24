using System;
using System.Linq;
using ConsoleClient.Db4oDoc.TeamService;

namespace ConsoleClient
{
    class SimpleConsoleClient
    {
        static void Main(string[] args)
        {
            // #example: Now the service can be used
            var serviceURL = new Uri("http://localhost:8080/TeamDataService.svc");    
            var serviceContext = new TeamDataContext(serviceURL);

            var teams = serviceContext.Teams;
            foreach (var team in teams)
            {
                Console.Out.WriteLine(team.TeamName);
            }
            // #end example

            var joe = Person.CreatePerson("joe@localhost");
            joe.Name = "Joe";
            serviceContext.AddToPersons(joe);


            var sarah = Person.CreatePerson("sarah@localhost");
            sarah.Name = "Sarah";
            serviceContext.AddToPersons(sarah);

            var newTeam = Team.CreateTeam("db4o Team");
            newTeam.Motivation = "database for developer";
            serviceContext.AddToTeams(newTeam);

            serviceContext.AddLink(newTeam, "Members", sarah);
            serviceContext.SaveChanges();

            foreach (var t in serviceContext.Teams)
            {
                Console.Out.WriteLine(t.TeamName);
                serviceContext.LoadProperty(t, "Members");
                foreach (var member in t.Members)
                {
                    Console.Out.WriteLine(member.Name);
                }
            }
            Console.Read();
        }
    }
}
