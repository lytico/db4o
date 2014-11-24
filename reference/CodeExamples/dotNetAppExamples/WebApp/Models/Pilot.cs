using System.ComponentModel.DataAnnotations;

namespace Db4oDoc.WebApp.Models
{
    public class Pilot :IDHolder
    {
        private string name;
        private int points;

        public Pilot()
        {
        }

        public Pilot(string name, int points)
        {
            this.name = name;
            this.points = points;
        }

        [Required]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        [Required]
        public int Points
        {
            get { return points; }
            set { points = value; }
        }
    }
}