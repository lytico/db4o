using System.Collections.Generic;

namespace Db4oDoc.Code.Query.QueryByExample
{

    internal class Pilot
    {
        private string name;
        private int age;

        public Pilot()
        {
        }

        public Pilot(string name, int age)
        {
            this.name = name;
            this.age = age;
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public int Age
        {
            get { return age; }
            set { age = value; }
        }

        public override string ToString()
        {
            return string.Format("Name: {0}, Age: {1}", name, age);
        }
    }

    internal class Car
    {
        private Pilot pilot;
        private string name;

        public Car()
        {
        }

        public Car(Pilot pilot, string name)
        {
            this.pilot = pilot;
            this.name = name;
        }

        public Pilot Pilot
        {
            get { return pilot; }
            set { pilot = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public override string ToString()
        {
            return string.Format("Pilot: {0}, Name: {1}", pilot, name);
        }
    }

    internal class Author
    {
        private string name;

        public Author(string name)
        {
            this.name = name;
        }
    }

    internal class BlogPost
    {
        private string title;
        private string content;
        private readonly IList<string> tags = new List<string>();
        private readonly IList<Author> authors = new List<Author>();
        private readonly IDictionary<string, object> metaData = new Dictionary<string, object>();

        public BlogPost()
        {
        }

        public BlogPost(string title, string content)
        {
            this.title = title;
            this.content = content;
        }

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        public string Content
        {
            get { return content; }
            set { content = value; }
        }

        public IList<string> Tags
        {
            get { return tags; }
        }

        public IDictionary<string, object> MetaData
        {
            get { return metaData; }
        }

        public void AddTags(params string[] tags)
        {
            foreach (string tag in tags)
            {
                this.tags.Add(tag);
            }
        }

        public void AddAuthors(params Author[] authors)
        {
            foreach (Author author in authors)
            {
                this.authors.Add(author);
            }
        }


        public void AddMetaData(string key, object value)
        {
            metaData.Add(key, value);
        }
    }
    
}