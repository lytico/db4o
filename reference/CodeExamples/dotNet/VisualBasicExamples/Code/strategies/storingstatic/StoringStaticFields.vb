Imports System.Collections.Generic
Imports Db4objects.Db4o
Imports Db4objects.Db4o.Config

Namespace Db4oDoc.Code.Strategies.StoringStatic
    Public Class StoringStaticFields
        Private Const DatabaseFile As String = "database.db4o"

        Public Shared Sub Main(ByVal args As String())
            StoreCars()
            LoadCars()
        End Sub

        Private Shared Sub LoadCars()
            Using container As IObjectContainer = OpenDatabase()
                Dim cars As IList(Of Car) = container.Query(Of Car)()

                For Each car As Car In cars
                    ' #example: Compare by reference
                    ' When you enable persist static field values, you can compare by reference
                    ' because db4o stores the static field
                    If car.Color Is Color.Black Then
                        Console.WriteLine("Black cars are boring")
                    ElseIf car.Color Is Color.Red Then
                        Console.WriteLine("Fire engine?")
                        ' #end example
                    End If
                Next
            End Using
        End Sub

        Private Shared Sub StoreCars()
            Using container As IObjectContainer = OpenDatabase()
                container.Store(New Car(Color.Black))
                container.Store(New Car(Color.White))
                container.Store(New Car(Color.Green))
                container.Store(New Car(Color.Red))
            End Using
        End Sub

        Private Shared Function OpenDatabase() As IObjectContainer
            '#example: Enable storing static fields for our color class
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            configuration.Common.ObjectClass(GetType(Color)).PersistStaticFieldValues()
            ' #end example
            Return Db4oEmbedded.OpenFile(configuration, DatabaseFile)
        End Function
    End Class



    Public Class Car
        Private m_color As Color = Color.Black

        Public Sub New()
        End Sub

        Public Sub New(ByVal color As Color)
            Me.m_color = color
        End Sub

        Public Property Color() As Color
            Get
                Return m_color
            End Get
            Set(ByVal value As Color)
                m_color = value
            End Set
        End Property
    End Class
End Namespace