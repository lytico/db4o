using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using System.Linq;

namespace EnumHandling
{
	public partial class EnumHandling : Form
	{
		public EnumHandling()
		{
			InitializeComponent();

			InitializeDb4oVersions();
			
			InitializeEnumForQuery();
		}

		private void InitializeEnumForQuery()
		{
			comboEnumValues.DataSource = Enum.GetValues(typeof (Db4oTest.SimpleEnum));
		}

		private void InitializeDb4oVersions()
		{
			string[] directories = Directory.GetDirectories(".", "db4o-?.?").Select(dir => Path.GetFileName(dir)).ToArray();
			db4oVersion1.DataSource = directories;
			db4oVersion2.DataSource = directories.Clone();
			db4oVersion2.SelectedIndex = directories.Length - 1;
		}

		private void cmdRun_Click(object sender, EventArgs e)
		{
			txtResult.AppendText(RunTestWith(db4oVersion1.Text));
			txtResult2.AppendText(RunTestWith(db4oVersion2.Text));
		}

		private string RunTestWith(string assemblyPath)
		{
			AppDomain domain = null;
			try
			{
				domain = AppDomainFor(assemblyPath);
				var runner = NewTestRunnerForDomain(domain);
				return runner.Run(EnumValueFor(comboEnumValues.Text), true);
			}
			finally
			{
				if (domain != null)
				{
					AppDomain.Unload(domain);
				}
			}
		}

		private static IRunner NewTestRunnerForDomain(AppDomain domain)
		{
			return (IRunner)domain.CreateInstanceAndUnwrap(Assembly.GetExecutingAssembly().GetName().Name, "EnumHandling.Db4oTest");
		}

		private static Db4oTest.SimpleEnum EnumValueFor(string enumTextualRepresentation)
		{
			return (Db4oTest.SimpleEnum) Enum.Parse(typeof(Db4oTest.SimpleEnum), enumTextualRepresentation);
		}

		private static AppDomain AppDomainFor(string assemblyPath)
		{
			PrepareApplicationDomain(assemblyPath);
			return AppDomain.CreateDomain("Version 2", AppDomain.CurrentDomain.Evidence, AppDomainSetupFor(assemblyPath));
		}

		private static void PrepareApplicationDomain(string path)
		{
			SetConfigurationFile(path);
		}

		private static void SetConfigurationFile(string path)
		{
			var name = CurrentApplicationName();
			XmlDocument doc = LoadTemplate(TemplateNameFor(name));
			XmlNode newVersionNode = doc.SelectSingleNode("//@newVersion");
			newVersionNode.Value = Db4oVersionFor(path);

			doc.Save(Path.Combine(path, AppConfigNameFor(name)));
		}

		private static string CurrentApplicationName()
		{
			return Path.GetFileName(Assembly.GetExecutingAssembly().Location);
		}

		private static string AppConfigNameFor(string appName)
		{
			return appName + ".config";
		}

		private static string TemplateNameFor(string name)
		{
			return name + ".template";
		}

		private static string Db4oVersionFor(string path)
		{
			var db4oAssembly = Assembly.LoadFrom(Path.Combine(path, "Db4Objects.Db4o.dll"));
			FieldInfo db4oVersionNameField = FieldFor(db4oAssembly, "Db4objects.Db4o.Db4oVersion", "Name");

			return (string) db4oVersionNameField.GetValue(null);
		}

		private static FieldInfo FieldFor(Assembly assembly, string typeName, string fieldName)
		{
			var type = assembly.GetType(typeName);
			return type.GetField(fieldName, BindingFlags.Static | BindingFlags.Public);
		}

		private static XmlDocument LoadTemplate(string templatePath)
		{
			XmlDocument doc = new XmlDocument();
			doc.Load(templatePath);
			return doc;
		}

		private static AppDomainSetup AppDomainSetupFor(string assemblyPath)
		{
			AppDomainSetup setup = new AppDomainSetup();
			setup.ApplicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
			setup.PrivateBinPath = assemblyPath;
			setup.ConfigurationFile = Path.Combine(assemblyPath, "EnumHandling.exe.config");

			return setup;
		}

		private void cmdClose_Click(object sender, EventArgs e)
		{
			Close();
		}
	}
    
	public interface IRunner
	{
		string Run(Db4oTest.SimpleEnum enumValue, bool runIndexed);
	}
}
