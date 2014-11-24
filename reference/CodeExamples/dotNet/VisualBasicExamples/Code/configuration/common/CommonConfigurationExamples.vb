Imports Db4objects.Db4o
Imports Db4objects.Db4o.Config
Imports Db4objects.Db4o.Diagnostic
Imports Db4objects.Db4o.Internal.Reflect
Imports Db4objects.Db4o.Config.Encoding

Namespace Db4oDoc.Code.Configuration.Common
    Public Class CommonConfigurationExamples
        Private Const DatabaseFile As String = "database.db4o"

        Private Shared Sub ChangeGlobalActivationDepth()
            ' #example: Change activation depth
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            configuration.Common.ActivationDepth = 2
            ' #end example

            Dim container As IObjectContainer = Db4oEmbedded.OpenFile(configuration, DatabaseFile)
            container.Close()
        End Sub

        Private Shared Sub UpdateFileFormat()
            ' #example: Update the database-format
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            configuration.Common.AllowVersionUpdates = True

            ' reopen and close the database to do the update
            Dim container As IObjectContainer = Db4oEmbedded.OpenFile(configuration, DatabaseFile)
            container.Close()
            ' #end example
        End Sub

        Private Shared Sub DisableAutomaticShutdown()
            ' #example: Disable automatic shutdown
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            configuration.Common.AutomaticShutDown = False
            ' #end example

            Dim container As IObjectContainer = Db4oEmbedded.OpenFile(configuration, DatabaseFile)
            container.Close()
        End Sub


        Private Shared Sub ChangeBTreeNodeSize()
            ' #example: Change B-tree node size
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            configuration.Common.BTreeNodeSize = 256
            ' #end example

            Dim container As IObjectContainer = Db4oEmbedded.OpenFile(Configuration, DatabaseFile)
            container.Close()
        End Sub

        Private Shared Sub DisableCallbacks()
            ' #example: Disable callbacks
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            configuration.Common.Callbacks = False
            ' #end example

            Dim container As IObjectContainer = Db4oEmbedded.OpenFile(configuration, DatabaseFile)
            container.Close()
        End Sub

        Private Shared Sub CallConstructors()
            ' #example: Call constructors
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            configuration.Common.CallConstructors = True
            ' #end example

            Dim container As IObjectContainer = Db4oEmbedded.OpenFile(configuration, DatabaseFile)
            container.Close()
        End Sub

        Private Shared Sub DisableSchemaEvolution()
            ' #example: Disable schema evolution
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            configuration.Common.DetectSchemaChanges = False
            ' #end example

            Dim container As IObjectContainer = Db4oEmbedded.OpenFile(configuration, DatabaseFile)
            container.Close()
        End Sub

        Private Shared Sub AddDiagnosticListener()
            ' #example: Add a diagnostic listener
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            configuration.Common.Diagnostic.AddListener(New DiagnosticToConsole())
            ' #end example

            Dim container As IObjectContainer = Db4oEmbedded.OpenFile(configuration, DatabaseFile)
            container.Close()
        End Sub

        Private Shared Sub DisableExceptionsOnNotStorableObjects()
            ' #example: Disable exceptions on not storable objects
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            configuration.Common.ExceptionsOnNotStorable = False
            ' #end example

            Dim container As IObjectContainer = Db4oEmbedded.OpenFile(configuration, DatabaseFile)

            container.Close()
        End Sub

        Private Shared Sub ChangeMessageLevel()
            ' #example: Change the message-level
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            configuration.Common.MessageLevel = 4
            ' #end example

            Dim container As IObjectContainer = Db4oEmbedded.OpenFile(configuration, DatabaseFile)

            container.Close()
        End Sub

        Private Shared Sub ChangeOutputStream()
            ' #example: Change the output stream
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            configuration.Common.OutStream = Console.Out
            ' #end example

            Dim container As IObjectContainer = Db4oEmbedded.OpenFile(configuration, DatabaseFile)

            container.Close()
        End Sub

        Private Shared Sub ChangeQueryMode()
            ' #example: Change the query mode
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            configuration.Common.Queries.EvaluationMode(QueryEvaluationMode.Immediate)
            ' #end example

            Dim container As IObjectContainer = Db4oEmbedded.OpenFile(configuration, DatabaseFile)

            container.Close()
        End Sub

        Private Shared Sub InternStrings()
            ' #example: intern strings
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            configuration.Common.InternStrings = True
            ' #end example

            Dim container As IObjectContainer = Db4oEmbedded.OpenFile(configuration, DatabaseFile)

            container.Close()
        End Sub

        Private Shared Sub ChangeReflector()
            ' #example: Change the reflector
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            configuration.Common.ReflectWith(New FastNetReflector())
            ' #end example

            Dim container As IObjectContainer = Db4oEmbedded.OpenFile(configuration, DatabaseFile)
            container.Close()
        End Sub

        Private Shared Sub MaxStackSize()
            ' #example: Set the stack size
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            configuration.Common.MaxStackDepth = 16
            ' #end example

            Dim container As IObjectContainer = Db4oEmbedded.OpenFile(configuration, DatabaseFile)

            container.Close()
        End Sub

        Private Shared Sub NameProvider()
            ' #example: set a name-provider
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            configuration.Common.NameProvider(New SimpleNameProvider("Database"))
            ' #end example

            Dim container As IObjectContainer = Db4oEmbedded.OpenFile(configuration, DatabaseFile)

            container.Close()
        End Sub


        Private Shared Sub ChangeWeakReferenceCollectionIntervall()
            ' #example: change weak reference collection interval
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            configuration.Common.WeakReferenceCollectionInterval = (10 * 1000)
            ' #end example

            Dim container As IObjectContainer = Db4oEmbedded.OpenFile(configuration, DatabaseFile)

            container.Close()
        End Sub

        Private Shared Sub DisableWeakReferences()
            ' #example: Disable weak references
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            configuration.Common.WeakReferences = False
            ' #end example

            Dim container As IObjectContainer = Db4oEmbedded.OpenFile(configuration, DatabaseFile)
            container.Close()
        End Sub

        Private Shared Sub MarkTransient()
            CleanUp()

            ' #example: add an transient marker annotatin
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            configuration.Common.MarkTransient(GetType(TransientMarkerAttribute).FullName)
            ' #end example

            Dim container As IObjectContainer = Db4oEmbedded.OpenFile(configuration, DatabaseFile)
            container.Store(New WithTransient())
            container.Close()

            ReadWithTransientMarker()

            CleanUp()
        End Sub

        Private Shared Sub ChangeStringEncoding()
            ' #example: Use the utf8 encoding
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            configuration.Common.StringEncoding = StringEncodings.Utf8()
            ' #end example

            Dim container As IObjectContainer = Db4oEmbedded.OpenFile(configuration, DatabaseFile)
            container.Close()
        End Sub

        Private Shared Sub DisableTestingConstructors()
            ' #example: Disable testing for callable constructors
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            configuration.Common.TestConstructors = False
            ' #end example

            Dim container As IObjectContainer = Db4oEmbedded.OpenFile(configuration, DatabaseFile)
            container.Close()
        End Sub

        Private Shared Sub IncreasingUpdateDepth()
            ' #example: Increasing the update-depth
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            configuration.Common.UpdateDepth = 2
            ' #end example

            Dim container As IObjectContainer = Db4oEmbedded.OpenFile(configuration, DatabaseFile)
            container.Close()
        End Sub

        Private Shared Sub DisableRuntimeNQOptimizer()
            ' #example: Disable the runtime native query optimizer
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            configuration.Common.OptimizeNativeQueries = False
            ' #end example

            Dim container As IObjectContainer = Db4oEmbedded.OpenFile(configuration, DatabaseFile)
            container.Close()
        End Sub

        Private Shared Sub CleanUp()
            System.IO.File.Delete(DatabaseFile)
        End Sub

        Private Shared Sub ReadWithTransientMarker()
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            configuration.Common.MarkTransient(GetType(TransientMarkerAttribute).FullName)
            Dim container As IObjectContainer = Db4oEmbedded.OpenFile(configuration, DatabaseFile)
            Dim instance As WithTransient = container.Query(Of WithTransient)()(0)

            AssertTransientNotStored(instance)

            container.Close()
        End Sub

        Private Shared Sub AssertTransientNotStored(ByVal instance As WithTransient)
            If instance.TransientString IsNot Nothing Then
                Throw New Exception("Transient was stored!")
            End If
        End Sub
    End Class

    Friend Class WithTransient
        <TransientMarker()> _
        Private m_transientString As String = "New"

        Public Property TransientString() As String
            Get
                Return m_transientString
            End Get
            Set(ByVal value As String)
                m_transientString = value
            End Set
        End Property
    End Class

    <AttributeUsage(AttributeTargets.Field)> _
    Friend Class TransientMarkerAttribute
        Inherits Attribute
    End Class
End Namespace