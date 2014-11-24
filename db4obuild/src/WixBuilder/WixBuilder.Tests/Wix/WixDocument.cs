using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace WixBuilder.Tests.Wix
{
	public class WixDocument : WixElement
	{
		public IEnumerable<WixFeature> Features
		{
			get { return SelectElements("//wix:Feature").Select(element => element.ToWix<WixFeature>()); }
		}

		public IEnumerable<WixComponent> Components
		{
			get { return SelectElements("//wix:Component").Select(element => element.ToWix<WixComponent>()); }
		}

		public IEnumerable<WixFile> Files
		{
			get { return Components.SelectMany(c => c.Files); }
		}

		public WixComponent ResolveComponentReference(string componentRef)
		{
			return SelectElements("//wix:Component[@Id = '" + componentRef + "']").AssertSingle().ToWix<WixComponent>();
		}

		public WixDirectory ResolveDirectoryById(string id)
		{
			return SelectElements("//wix:Directory[@Id='" + id + "']").AssertSingle().ToWix<WixDirectory>();
		}

		public IEnumerable<WixDirectory> ResolveDirectoryByName(string name)
		{
			return SelectElements("//wix:Directory[@LongName='" + name + "' or @Name='" + name + "']").Select(element => element.ToWix<WixDirectory>());
		}

		public WixDirectory ResolveDirectoryRef(string id)
		{
			return SelectElements("//wix:DirectoryRef[@Id='" + id + "']").AssertSingle().ToWix<WixDirectory>();
		}

		public WixFeature FeatureById(string id)
		{
			return Features.Where(f => f.Id == id).AssertSingle();
		}
	}

	static class XmlElementExtensions
	{
		public static T ToWix<T>(this XmlElement source) where T : WixElement, new()
		{
			return new T {XmlElement = source};
		}
	}

	public class WixElement
	{
		private XmlElement _element;
		private XmlNamespaceManager _namespaces;

		internal XmlElement XmlElement
		{
			set
			{
				_element = value;
				_namespaces = new XmlNamespaceManager(value.OwnerDocument.NameTable);
				_namespaces.AddNamespace("wix", WixScriptBuilder.WixNamespace);
			}
		}

		public XmlElement ParentElement
		{
			get { return (XmlElement)_element.ParentNode; }
		}

		public IEnumerable<XmlElement> SelectElements(string xpath)
		{
			return SelectNodes(xpath).Cast<XmlElement>();
		}

		public XmlNodeList SelectNodes(string xpath)
		{
			return _element.SelectNodes(xpath, _namespaces);
		}

		public string GetAttribute(string attributeName)
		{
			return _element.GetAttribute(attributeName);
		}
	}

	public class WixReferenceableElement : WixElement
	{
		public string Id
		{
			get { return GetAttribute("Id"); }
		}
	}

	public class WixFile : WixReferenceableElement
	{
		public string Name
		{
			get { return GetAttribute("Name"); }
		}
	}

	public class WixDirectory : WixReferenceableElement
	{
		public IEnumerable<WixShortcut> Shortcuts
		{
			get
			{
				return SelectElements("wix:Component/wix:Shortcut").Select(element => element.ToWix<WixShortcut>());
			}
		}

        public string Name
        {
            get { return GetAttribute("Name"); }
        }

	}

	public class WixShortcut : WixReferenceableElement
	{
		public string Target
		{
			get { return GetAttribute("Target"); }
		}

		public string WorkingDirectory
		{
			get { return GetAttribute("WorkingDirectory"); }
		}
	}

	public class WixFeature : WixReferenceableElement
	{
		public IEnumerable<string> ComponentReferences
		{
			get { return SelectNodes("wix:ComponentRef/@Id").Cast<XmlAttribute>().Select(attr => attr.Value);  }
		}

		public string Title
		{
			get { return GetAttribute("Title"); }
		}

		public string Description
		{
			get { return GetAttribute("Description"); }
		}
	}

	public class WixComponent : WixReferenceableElement
	{
		public IEnumerable<WixFile> Files
		{
			get { return SelectElements("wix:File").Select(e => e.ToWix<WixFile>()); }
		}

		public override string ToString()
		{
			return "WixComponent(" + Id + ", Files={ " + Files.MakeString(", ") + " }, ParentDirectory=" + ParentElement.GetAttribute("LongName")  + ")";
		}
	}
}