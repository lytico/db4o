Imports Db4objects.Db4o
Imports Db4objects.Db4o.Config
Imports Db4objects.Db4o.TA

Namespace Db4oDoc.Code.Tp
    Public Class TransparentActivationExample
        Public Shared Sub Main(ByVal args As String())
            ' #example: Add transparent persistence support
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            configuration.Common.Add(New TransparentPersistenceSupport())
            ' #end example
            Using container As IObjectContainer = Db4oEmbedded.OpenFile(configuration, "database.db4o")
                container.Store(New Pilot("Joe"))
            End Using

            ' #example: Verify that pilot support transparent activation
            Dim supportsTransparentPersistence As Boolean = TypeOf GetType(Pilot) Is IActivatable
            If supportsTransparentPersistence Then
                Console.Out.WriteLine("The Pilot-class supports transparent persistence." & " The enhancement worked")
            Else
                Console.Out.WriteLine("Oups, not transperent persistence support." & " Something went wrong with the enhancement")
            End If
            ' #end example
        End Sub
    End Class

    ' #example: Our domain model
    Class Pilot
        Private m_name As String

        Public Sub New(ByVal name As String)
            Me.m_name = name
        End Sub

        Public Property Name() As String
            Get
                Return m_name
            End Get
            Set(ByVal value As String)
                m_name = value
            End Set
        End Property
    End Class
    ' #end example
End Namespace
