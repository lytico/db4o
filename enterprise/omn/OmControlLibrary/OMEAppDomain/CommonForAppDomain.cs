using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace OMControlLibrary.OMEAppDomain
{
    class CommonForAppDomain
    {
        public static string GetPath()
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), @"Microsoft\MSEnvShared\Addins\OMAddin.addin");
            XmlDocument addinDoc = new XmlDocument();
            string nodePath = "/ns:Extensibility/ns:Addin/ns:Assembly";
            addinDoc.Load(path);
            XmlNode nodePath1 = addinDoc.SelectSingleNode(nodePath, NameSpaceManagerFor(addinDoc, ""));
            return Path.GetDirectoryName(nodePath1.FirstChild.Value);
        }

        private static XmlNamespaceManager NameSpaceManagerFor(XmlDocument addinDoc, string prefix)
        {
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(addinDoc.NameTable);
            nsmgr.AddNamespace("ns", addinDoc.DocumentElement.GetNamespaceOfPrefix(prefix));

            return nsmgr;
        }

    }
}
