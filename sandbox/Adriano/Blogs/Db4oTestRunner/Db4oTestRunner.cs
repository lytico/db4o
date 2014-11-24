using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Linq;

namespace Db4oTestRunner
{
	public partial class Db4oTestRunner : Form, INotifyPropertyChanged
	{
		public const string TEST_ASSEMBLY_PATH_DATA_NAME = "TestAssemblyPath";

		private static string NameOf(Expression<Func<Db4oTestRunner, bool>> expression)
		{
			MemberExpression me = (MemberExpression) expression.Body;
			return me.Member.Name;
		}

		public Db4oTestRunner()
		{
			InitializeComponent();

			InitializeDb4oVersions();
			
			InitializeDataBindings();
		}

		private void InitializeDataBindings()
		{
			cmdRun.DataBindings.Add("Enabled", this, IS_OK_TO_RUN_PROPERTY_NAME, true, DataSourceUpdateMode.Never);
			db4oVersion1.DataBindings.Add("Enabled", cbRunVersion1, "Checked");
			db4oVersion2.DataBindings.Add("Enabled", cbRunVersion2, "Checked");
		}

		public bool IsOkToRun
		{
			get { return txtTestAssembly.Text.Length > 0 && File.Exists(txtTestAssembly.Text) && IsValidDb4oVersionsSelection(); }
		}

		private bool IsValidDb4oVersionsSelection()
		{
			return db4oVersion1.SelectedIndex != db4oVersion2.SelectedIndex;
		}

		private void InitializeDb4oVersions()
		{
			string[] directories = Directory.GetDirectories(".", "db4o-?.??").Select(dir => Path.GetFileName(dir)).OrderBy(dirName => dirName, new DirectoryNameComparer()).ToArray();
			db4oVersion1.DataSource = directories;
			db4oVersion2.DataSource = directories.Clone();
			db4oVersion2.SelectedIndex = directories.Length - 1;
		}

		private void cmdRun_Click(object sender, EventArgs e)
		{
			txtResult.Clear();
			txtResult2.Clear();

			RunTestWith(db4oVersion1.Text, cbRunVersion1.Checked, RichTextBoxLogger.For(txtResult));
			RunTestWith(db4oVersion2.Text, cbRunVersion2.Checked, RichTextBoxLogger.For(txtResult2));
		}

		private void RunTestWith(string versionFolder, bool enabled, ILogger logger)
		{
			ThreadPool.QueueUserWorkItem(
				state =>
					{
						logger.LogMessageFormated(Color.Blue, Verdata10Bold(), "======= Db4o Version {0} ======",
						                          Db4oVersionFor(versionFolder));

						if (!enabled)
						{
							logger.LogMessage("Test disable by the user.");
							return;
						}

						AppDomain domain = null;
						try
						{
							domain = AppDomainFor(versionFolder, txtTestAssembly.Text);
							foreach (var testClass in EnabledTest())
							{
								var runner = NewTestRunnerFor(domain, txtTestAssembly.Text, testClass);
								logger.LogMessageFormated(Color.DarkGreen, Verdata10Bold(), "Running test {0}", testClass);
								runner.Run(logger);
							}
						}
						catch (Exception ex)
						{
							logger.LogException(ex);
						}
						finally
						{
							if (domain != null)
							{
								AppDomain.Unload(domain);
							}
						}
					});
		}

		private Font Verdata10Bold()
		{
			return new Font("Verdana", 10, FontStyle.Bold);
		}

		private IEnumerable<string> EnabledTest()
		{
			foreach (var item in lbTestClasses.CheckedItems)
			{
				yield return (string) item;
			}
		}

		private static ITestRunner NewTestRunnerFor(AppDomain domain, string testAssemblyPath, string testClass)
		{
			var assembly = Assembly.LoadFrom(testAssemblyPath);
			return (ITestRunner)domain.CreateInstanceAndUnwrap(assembly.FullName, testClass);
		}

		private static AppDomain AppDomainFor(string db4oAssemblyPath, string testAssemblyPath)
		{
			SetConfigurationFile(db4oAssemblyPath);
			AppDomain domain = CreateTestAppDomain(db4oAssemblyPath);
			WireAssemblyResolverHandler(domain, testAssemblyPath);

			return domain;
		}

		private static AppDomain CreateTestAppDomain(string db4oAssemblyPath)
		{
			return AppDomain.CreateDomain("Version 2", AppDomain.CurrentDomain.Evidence, AppDomainSetupFor(db4oAssemblyPath));
		}

		private static void WireAssemblyResolverHandler(AppDomain domain, string testAssemblyPath)
		{
			domain.SetData(TEST_ASSEMBLY_PATH_DATA_NAME, testAssemblyPath);
			domain.AssemblyResolve += (sender, args) =>
			                          	{
			                          		string assemblyPath = (string)AppDomain.CurrentDomain.GetData(TEST_ASSEMBLY_PATH_DATA_NAME);
											TraceInformation("Trying to resolve '{0}' to '{1}'", args.Name, assemblyPath);
			                          		return assemblyPath.Contains(UnqualifiedAssemblyName(args)) ? Assembly.LoadFrom(assemblyPath) : null;
			                          	};
		}

		private static string UnqualifiedAssemblyName(ResolveEventArgs args)
		{
			int assemblyNameSeparator = args.Name.IndexOf(",");
			return assemblyNameSeparator > 0 ? args.Name.Substring(0, assemblyNameSeparator - 1) : args.Name;
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

		private static string Db4oVersionFor(string basePath)
		{
			AssemblyName assemblyName = AssemblyName.GetAssemblyName(Path.Combine(basePath, "Db4Objects.Db4o.dll"));
			return assemblyName.Version.ToString();
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
			setup.ConfigurationFile = Path.Combine(assemblyPath, "Db4oTestRunner.exe.config");
			TraceInformation("[{0}] Configuration File: '{1}'", assemblyPath, setup.ConfigurationFile);

			return setup;
		}

		private static void TraceInformation(string msg, params string[] args)
		{
			Trace.TraceInformation(msg, args);
		}

		private void TryFillTestList(string assemblyPath)
		{
			if (File.Exists(assemblyPath))
			{
				foreach (var testClass in TestListFor(assemblyPath))
				{
					lbTestClasses.Items.Add(testClass, true);
				}
			}
		}

		private static IEnumerable<string> TestListFor(string assemblyPath)
		{
			var assembly = Assembly.LoadFrom(assemblyPath);

			IEnumerable<Type> testClasses = assembly.GetTypes().Where(candidateType => typeof(ITestRunner).IsAssignableFrom(candidateType));
			if (testClasses.All(IsIntraAppDomainFriendly))
			{
				return testClasses.Select(type => type.FullName);
			}

			throw new ArgumentException("All types that implemenst " + typeof(ITestRunner).Name + "interface in assembly " + assembly.FullName + " must either be serializable or extend " + typeof(MarshalByRefObject).FullName + ".", "assemblyPath");
		}

		private static bool IsIntraAppDomainFriendly(Type type)
		{
			return type != null && (typeof(MarshalByRefObject).IsAssignableFrom(type) || type.IsSerializable);
		}

		private void txtTestAssembly_DragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
				e.Effect = DragDropEffects.Copy;
		}

		private void txtTestAssembly_DragDrop(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				txtTestAssembly.Text = ((string[]) e.Data.GetData(DataFormats.FileDrop))[0];
			}
		}

		private void cmdClose_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void cmdSelectTestAssembly_Click(object sender, EventArgs e)
		{
			using(OpenFileDialog assemblySelector = new OpenFileDialog())
			{
				assemblySelector.Filter = "(*.dll, *.exe) Assembly Files|*.dll;*.exe";
				if (assemblySelector.ShowDialog() == DialogResult.OK)
				{
					txtTestAssembly.Text = assemblySelector.FileName;
				}
			}
		}

		private void txtTestAssembly_TextChanged(object sender, EventArgs e)
		{
			RaiseIsOkToRunPropertyChangeEvent();

			TryFillTestList(txtTestAssembly.Text);
		}

		private void VersionStatusToogle(object sender, EventArgs e)
		{
			RaiseIsOkToRunPropertyChangeEvent();
		}

		private void RaiseIsOkToRunPropertyChangeEvent()
		{
			RaisePropertyChangedEvent(IS_OK_TO_RUN_PROPERTY_NAME);
		}

		private void RaisePropertyChangedEvent(string propertyName)
		{
			if (null != PropertyChanged)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
		private static readonly string IS_OK_TO_RUN_PROPERTY_NAME = NameOf(f => f.IsOkToRun);
	}

	internal class DirectoryNameComparer : IComparer<string>
	{
		public int Compare(string lhs, string rhs)
		{
			Version lhsVersion = new Version(VersionStringFrom(lhs));
			Version rhsVersion = new Version(VersionStringFrom(rhs));

			return lhsVersion.CompareTo(rhsVersion);
		}

		private string VersionStringFrom(string namedVersion)
		{
			int dashIndex = namedVersion.IndexOf('-');
			return dashIndex >= 0 ? namedVersion.Substring(dashIndex + 1) : namedVersion;
		}
	}
}
