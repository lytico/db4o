using System;
using System.IO;
using System.Linq;
using System.Xml;
using NUnit.Framework;
using WixBuilder.Tests.Wix;

namespace WixBuilder.Tests
{
	[TestFixture]
	public class WixScriptBuilderTestCase
	{
		private IFolder root; 

		[SetUp]
		public void SetUpFolderLayout()
		{
			root = CreateFolderBuilder()
				.EnterFolder("Bin")
					.AddFiles("foo.exe", "bar.dll")
				.LeaveFolder()
				.EnterFolder("Doc")
					.AddFiles("foo.chm", "README.TXT")
				.LeaveFolder()
				.EnterFolder("Src")
					.AddFiles("foo.boo", "foo.booproj")
				.LeaveFolder()
                .EnterFolder("native")
                    .EnterFolder("Db4objects.Db4o")
                        .EnterFolder("Config")
                            .AddFiles("JavaSupport.cs")
                        .LeaveFolder()
                    .LeaveFolder()
                .LeaveFolder()
				.GetFolder();
		}

		protected virtual IFolderBuilder CreateFolderBuilder()
		{
			return new FolderMock("root");
		}

		private WixDocument WixDocumentFor(WixBuilderParameters parameters)
		{
			return RunScriptBuilderWith(parameters).DocumentElement.ToWix<WixDocument>();
		}

		private XmlDocument RunScriptBuilderWith(WixBuilderParameters parameters)
		{
			var resultingDocument = new StringWriter();
			new WixScriptBuilder(resultingDocument, root, parameters).Build();
			return LoadXml(resultingDocument.ToString());
		}

		private static XmlDocument LoadXml(string xmlString)
		{
			var document = new XmlDocument();
			document.LoadXml(xmlString);
			return document;
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void TestEmptyFeatureId()
		{
			RunScriptBuilderWith(new WixBuilderParameters { Features = new[] { new Feature { Id = "", Content = new Content() } } });
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void TestNullFeatureId()
		{
			RunScriptBuilderWith(new WixBuilderParameters { Features = new[] { new Feature { Id = null, Content = new Content() } } });
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void TestNullFeatureContent()
		{
			RunScriptBuilderWith(new WixBuilderParameters { Features = new[] { new Feature { Id = "id", Content = null } } });
		}

		[Test]
		public void TestKnownId()
		{
			var parameters = new WixBuilderParameters
			{
				Features = new[]
				{
					new Feature
					{
						Id = "ApplicationFiles",
						Title = "All Files",
						Description = "All Files",
						Content = new Content
						{
							Include = @"**/*"
						}
					}
				},
				KnownIds = new[]
				{
					new KnownId { Id = "foo", Path = "Bin/foo.exe" }
				}
			};

			var document = WixDocumentFor(parameters);
			var fileWithKnownId = (from file in document.Files where file.Id == "foo" select file).AssertSingle();
			WixAssert.AssertFile(DirectoryFromRoot("Bin")["foo.exe"], fileWithKnownId);
		}

		[Test]
		public void TestSingleFeatureComponents()
		{
			var parameters = new WixBuilderParameters
							 {
								 Features = new[]
			                 	           {
			                 	           	new Feature
			                 	           	{
			                 	           		Id = "ApplicationFiles",
			                 	           		Title = "Documentation",
			                 	           		Description = "all the docs",
			                 	           		Content = new Content
			                 	           		          {
			                 	           		          	Include = @"Doc\*.*"
			                 	           		          }
			                 	           	}
			                 	           }
							 };

			WixDocument wix = WixDocumentFor(parameters);

			WixFeature featureElement = wix.Features.AssertSingle();
			Feature expectedFeature = parameters.Features[0];
			WixAssert.AssertFeature(expectedFeature, featureElement);

			string componentRef = featureElement.ComponentReferences.AssertSingle();
			WixComponent docComponent = wix.ResolveComponentReference(componentRef);
			WixAssert.AssertDirectoryComponent(DirectoryFromRoot("Doc"), docComponent);
		}

        [Test]
        public void TestMultipleEmptyFolders()
        {
            var parameters = new WixBuilderParameters
            {
                Features = new[]
			                 	           {
			                 	           	new Feature
			                 	           	{
			                 	           		Id = "ApplicationFiles",
			                 	           		Title = "Documentation",
			                 	           		Description = "all the docs",
			                 	           		Content = new Content
			                 	           		          {
			                 	           		          	Include = @"native\**\*.*"
			                 	           		          }
			                 	           	}
			                 	           }
            };

            WixDocument wix = WixDocumentFor(parameters);

            WixFeature featureElement = wix.Features.AssertSingle();
            Feature expectedFeature = parameters.Features[0];
            WixAssert.AssertFeature(expectedFeature, featureElement);

            string componentRef = featureElement.ComponentReferences.AssertSingle();
            WixComponent configComponent = wix.ResolveComponentReference(componentRef);
            WixAssert.AssertDirectoryComponent(DirectoryFromRoot("native/Db4objects.Db4o/Config"), configComponent);

            WixDirectory configDirectory = configComponent.ParentElement.ToWix<WixDirectory>();
            Assert.AreEqual(DirectoryFromRoot("native/Db4objects.Db4o/Config").Name, configDirectory.Name);
            Assert.AreEqual(DirectoryFromRoot("native/Db4objects.Db4o").Name, configDirectory.ParentElement.ToWix<WixDirectory>().Name);
            Assert.AreEqual(DirectoryFromRoot("native").Name, configDirectory.ParentElement.ToWix<WixDirectory>().ParentElement.ToWix<WixDirectory>().Name);

        }


		[Test]
		[Ignore("Still considering the feature")]
		public void TestMultipleFeaturesReferencesFilesInCommonFolder()
		{
			var parameters = new WixBuilderParameters
			{
				Features = new[]
				{
					new Feature
					{
						Id = "CHM_FILES",
						Title = "Documentation",
						Description = "Windows Help Files",
						Content = new Content
						{
							Include = @"Doc\*.chm"
						}
					},

					new Feature
					{
						Id = "TXT_Files",
						Title = "Text Files",
						Description = "Text Files",
						Content = new Content
						{
							Include= @"Doc\*.TXT"
						}
					}
				}
			};

			WixDocument wix = WixDocumentFor(parameters);
			wix.ResolveDirectoryByName("Doc").AssertSingle();
		}

		[Test]
		public void TestMultipleFeaturesSingleTARGETDIR()
		{
			WixDocument wix = WixDocumentFor(ParametersForMultipleFeaturesTest());
			Assert.AreEqual(2, wix.Features.Count());
			wix.ResolveDirectoryById("TARGETDIR");
		}

		[Test]
		public void TestMultipleFeaturesFileAddedOnlyOnce()
		{
			WixDocument wix = WixDocumentFor(ParametersForMultipleFeaturesTest());
			Assert.AreEqual(2, wix.Features.Count());

			Assert.AreEqual(1, wix.Files.Where(file => file.Id == "foo_exe").Count());
		}

		[Test]
		public void TestMultipleFeaturesDirectoriesAppearOnlyOnce()
		{
			WixDocument wix = WixDocumentFor(ParametersForMultipleFeaturesTest());
			Assert.AreEqual(2, wix.Features.Count());

			wix.ResolveDirectoryByName("Doc").AssertSingle();
		}

		[Test]
		public void TestFeaturesReferencesCorrectComponents()
		{
			WixDocument wix = WixDocumentFor(ParametersForMultipleFeaturesTest());
			Assert.AreEqual(2, wix.Features.Count());

			WixFeature wixFeature = wix.Features.Where(feature => feature.Id == "Documentation").AssertSingle();
			string componentRef = wixFeature.ComponentReferences.AssertSingle();
			WixComponent component = wix.ResolveComponentReference(componentRef);

			WixAssert.AssertDirectoryComponent(DirectoryFromRoot("Doc"), component);
		}

		[Test]
		public void MissingFolderForShortcut()
		{
			var parameters = new WixBuilderParameters
			{
				Features = new[]
					{
             	           	new Feature
             	           	{
             	           		Id = "Documentation",
             	           		Title = "Documentation",
             	           		Description = "all the docs",
             	           		Content = new Content
             	           		          {
             	           		          	Include = @"Doc\*.*"
             	           		          },

             	           		Shortcuts = new[]
             	           		            {
             	           		            	new Shortcut
             	           		            	{
             	           		            		Name = "Foo documentation",
             	           		            		Path = @"Doc/Bar/bar.chm"
             	           		            	}
             	           		            }
             	           	},
             	           },
			};

			try
			{
				RunScriptBuilderWith(parameters);
			}
			catch (ArgumentException x)
			{
				Assert.AreEqual("Could not find file 'Doc/Bar/bar.chm' required by shortcut 'Foo documentation'", x.Message);
			}
		}

		[Test]
		public void MissingFileForShortcut()
		{
			var parameters = new WixBuilderParameters
			 {
				 Features = new[]
					{
             	           	new Feature
             	           	{
             	           		Id = "Documentation",
             	           		Title = "Documentation",
             	           		Description = "all the docs",
             	           		Content = new Content
             	           		          {
             	           		          	Include = @"Doc\*.*"
             	           		          },

             	           		Shortcuts = new[]
             	           		            {
             	           		            	new Shortcut
             	           		            	{
             	           		            		Name = "Foo documentation",
             	           		            		Path = @"Doc/bar.chm"
             	           		            	}
             	           		            }
             	           	},
             	           },
			 };

			try
			{
				RunScriptBuilderWith(parameters);
			}
			catch (ArgumentException x)
			{
				Assert.AreEqual("Could not find file 'Doc/bar.chm' required by shortcut 'Foo documentation'", x.Message);
			}
		}

		[Test]
		public void TestShortcuts()
		{
			var parameters = new WixBuilderParameters
			{
				Features = new[]
				{
					new Feature
					{
						Id = "Documentation",
						Title = "Documentation",
						Description = "all the docs",
						Content = new Content
						{
							Include = @"Doc\*.*"
						},
						
						Shortcuts = new[]
						{
							new Shortcut
							{
                                Name = "Foo documentation",
								Path = @"Doc/foo.chm"
							}
						}
					},

					new Feature
					{
						Id = "SourceFiles",
						Title = "Source Files",
						Description = "all the source files",
						Content = new Content
						{
							Include = @"Src\*.*"
						},
						
						Shortcuts = new[]
						{
							new Shortcut
							{
                                Name = "Foo Project File",
								Path = @"Src/foo.booproj"
							}
						}
					},

					new Feature
					{
						Id="ApplicationFiles",
						Title = "Visual Studio 2005 Plugin",
						Description = "Visual Studio 2005 Plugin",
						Content = new Content
						{
							Include=@"Bin\*.*"
						}
					}
				},

				KnownIds = new[]
				{
					new KnownId
					{
						Id = "foo_help",
						Path = @"Doc/foo.chm"
					},
					new KnownId
					{
						Id = "foo_project",
						Path = @"Src/foo.booproj"
					}
				}
			};

			var wixDocument = WixDocumentFor(parameters);
			var targetMenuFolder = wixDocument.ResolveDirectoryRef("TargetMenuFolder");

			var shortcutsByTarget = targetMenuFolder.Shortcuts.ToLookup(s => s.Target);
			var knownIdsByPath = parameters.KnownIds.ToLookup(ki => ki.Path);
			foreach (var feature in parameters.Features)
			{
				var wixFeature = wixDocument.FeatureById(feature.Id);
				foreach (var shortcut in feature.Shortcuts)
				{
					var shortcutId = knownIdsByPath[shortcut.Path].AssertSingle();
					var shortcutTarget = "[#" + shortcutId.Id + "]";
					var wixShortcut = shortcutsByTarget[shortcutTarget].AssertSingle();

					var parentComponentId = wixShortcut.ParentElement.ToWix<WixComponent>().Id;
					Assert.IsTrue(
						wixFeature.ComponentReferences.Contains(parentComponentId),
						"Shortcut '" + shortcut.Name + "' is not referenced from feature '" + feature.Id + "'.");
				}
			}
		}

		[Test]
		public void TestCreateFolderIsInjectedInFoldersStoringOnlyShortcuts()
		{
			var parameters = new WixBuilderParameters
			{
				Features = new[]
				{
					new Feature
					{
						Id = "Documentation",
						Title = "Documentation",
						Description = "all the docs",
						Content = new Content
						{
							Include = @"Doc\*.*"
						},
						
						Shortcuts = new[]
						{
							new Shortcut
							{
                                Name = "Foo documentation",
								Path = @"Doc/foo.chm"
							}
						}
					},
				},

				KnownIds = new[]
				{
					new KnownId
					{
						Id = "foo_help",
						Path = @"Doc/foo.chm"
					},
				}
			};

			var wixDocument = WixDocumentFor(parameters);
			var targetMenuFolder = wixDocument.ResolveDirectoryRef("TargetMenuFolder");

			foreach (var shortcut in targetMenuFolder.Shortcuts)
			{
				WixComponent component = shortcut.ParentElement.ToWix<WixComponent>();
				Assert.AreEqual(0, component.Files.Count());
				Assert.AreEqual(1, component.SelectNodes("wix:CreateFolder").Count);
		
				AssertPerUserProfileComponentUsesRegistryKeyAsPath(component);
			}
		}

		private static void AssertPerUserProfileComponentUsesRegistryKeyAsPath(WixComponent component)
		{
			Assert.GreaterOrEqual(component.SelectNodes("wix:RegistryValue").Count, 1);
		}

		private IFolder DirectoryFromRoot(string name)
		{
			return (IFolder)root.GetItem(name);
		}

		private static WixBuilderParameters ParametersForMultipleFeaturesTest()
		{
			return new WixBuilderParameters
					{
						Features = new[]
			       		           	{
			       		           		new Feature
			       		           			{
			       		           				Id = "Documentation",
			       		           				Title = "Documentation",
			       		           				Description = "all the docs",
			       		           				Content = new Content
			       		           				          	{
			       		           				          		Include = @"Doc\*.*"
			       		           				          	}
			       		           			},

			       		           		new Feature
			       		           			{
			       		           				Id="ApplicationFiles",
			       		           				Title = "Visual Studio 2005 Plugin",
			       		           				Description = "Visual Studio 2005 Plugin",
			       		           				Content = new Content
			       		           				          	{
			       		           				          		Include=@"Bin\*.*"
			       		           				          	}
			       		           			}
			       		           	},

						KnownIds = new[]
			       		           	{
			       		           		new KnownId
			       		           			{
			       		           				Id = "foo_exe",
			       		           				Path = @"Bin\foo.exe"
			       		           			}
			       		           	}
					};
		}

		[Test]
		public void TestWixNamespace()
		{
			XmlDocument script = RunScriptBuilderWith(new WixBuilderParameters());
			Assert.AreEqual(WixScriptBuilder.WixNamespace, script.DocumentElement.NamespaceURI);
		}
	}
}