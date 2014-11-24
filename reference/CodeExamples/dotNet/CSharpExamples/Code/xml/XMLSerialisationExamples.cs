using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Db4objects.Db4o;
using Db4objects.Db4o.Linq;

namespace Db4oDoc.Code.xml
{
    public class XMLSerialisationExamples
    {
        public static void Main(string[] args)
        {
            File.Delete("database.db4o");
            FillData();
            WriteToXML();
            ReadFromXML();
        }

        private static void WriteToXML()
        {
            // #example: Serialize to XML
            using (IObjectContainer container = Db4oEmbedded.OpenFile("database.db4o"))
            {
                XmlSerializer pilotSerializer = new XmlSerializer(typeof(Pilot[]));
                using(FileStream file  = new FileStream("pilots.xml",FileMode.CreateNew))
                {
                    var pilots = (from Pilot c in container
                                  select c).ToArray();
                    pilotSerializer.Serialize(file, pilots);   
                }
            }
            // #end example
        }

        private static void ReadFromXML()
        {
            // #example: Read objects from XML
            using (IObjectContainer container = Db4oEmbedded.OpenFile("database.db4o"))
            {
                XmlSerializer pilotSerializer = new XmlSerializer(typeof(Pilot[]));
                using (FileStream file = new FileStream("pilots.xml", FileMode.Open))
                {
                    Pilot[] pilots = (Pilot[])pilotSerializer.Deserialize(file);
                    foreach (var pilot in pilots)
                    {
                        container.Store(pilot);
                    }
                }
            }
            // #end example
        }

        private static void FillData()
        {
            using (IObjectContainer container = Db4oEmbedded.OpenFile("database.db4o"))
            {
                container.Store(new Pilot("Joe", 42));
                container.Store(new Pilot("Joanna", 24));
            }
        }

        public class Pilot
        {
            private string name;
            private int point;

            public Pilot()
            {
            }

            public Pilot(string name, int point)
            {
                this.name = name;
                this.point = point;
            }

            public string Name
            {
                get { return name; }
                set { name = value; }
            }

            public int Point
            {
                get { return point; }
                set { point = value; }
            }
        }
    }
}