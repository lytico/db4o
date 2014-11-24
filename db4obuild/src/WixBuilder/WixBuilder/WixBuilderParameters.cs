using System;
using System.IO;
using System.Xml.Serialization;

public class Shortcut
{
	public string Path;
	public string Name;
}

public class KnownId
{
	public string Id;
	public string Path;
}

public class Feature
{
	[XmlAttribute] public string Id;
	[XmlAttribute] public string Title;
	[XmlAttribute] public string Description;
	public Content Content;
	public Shortcut[] Shortcuts = new Shortcut[0];

	public void Validate()
	{
		if (string.IsNullOrEmpty(Id))
			throw new ArgumentException("Expecting Id for feature '" + Title + "'");
		if (null == Content)
			throw new ArgumentException("Expecting content definition for feature '" + Id + "'");
	
	}
}

public class Content
{
	[XmlAttribute] public string Include;
	[XmlAttribute] public string Exclude;
}

public class WixBuilderParameters
{
	public KnownId[] KnownIds = new KnownId[0];

	public Feature[] Features = new Feature[0];

	[XmlAttribute] public string InstallationFolder = string.Empty;

	public static WixBuilderParameters FromFile(string fname)
	{
		using (TextReader reader = File.OpenText(fname))
		{
			XmlSerializer serializer = new XmlSerializer(typeof(WixBuilderParameters));
			return (WixBuilderParameters)serializer.Deserialize(reader);
		}
	}

	public void Validate()
	{
		foreach (Feature feature in Features)
			feature.Validate();
	}
}