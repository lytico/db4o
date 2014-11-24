Imports System.Collections.Generic
Imports System.IO
Imports System.Linq
Imports Db4objects.Db4o
Imports Db4objects.Db4o.Config
Imports Db4objects.Db4o.Linq

Namespace Db4oDoc.Code.Pitfalls.UpdateDepth
    Public Class UpdateDepthPitfall
        Public Const DatabaseFile As String = "database.db4o"

        Public Shared Sub Main(ByVal args As String())
            CleanUpAndPrepare()

            ToLowUpdateDepthOnObject()
            ToLowUpdateDepthOnCollection()

            ExplicitlyUpdateObjects()
            ExplicitlyStateUpdateDepth()
            UpdateDepthForCollection()
        End Sub

        Private Shared Sub ToLowUpdateDepthOnObject()
            ' #example: Update depth limits what is store when updating objects
            Using container As IObjectContainer = Db4oEmbedded.OpenFile(DatabaseFile)
                Dim car As Car = QueryForCar(container)
                car.CarName = "New Mercedes"
                car.Driver.Name = "New Driver Name"

                ' With the default-update depth of one, only the changes
                ' on the car-object are stored, but not the changes on
                ' the person
                container.Store(car)
            End Using
            Using container As IObjectContainer = Db4oEmbedded.OpenFile(DatabaseFile)
                Dim car As Car = QueryForCar(container)
                Console.WriteLine("Car-Name:" & car.CarName)
                Console.WriteLine("Driver-Name:" & car.Driver.Name)
            End Using
            ' #end example
        End Sub

        Private Shared Sub ToLowUpdateDepthOnCollection()
            ' #example: Update doesn't work on collection
            Using container As IObjectContainer = Db4oEmbedded.OpenFile(DatabaseFile)
                Dim jodie As Person = QueryForJodie(container)
                jodie.Add(New Person("Jamie"))
                ' Remember that a collection is also a regular object
                ' so with the default-update depth of one, only the changes
                ' on the person-object are stored, but not the changes on
                ' the friend-list.
                container.Store(jodie)
            End Using
            Using container As IObjectContainer = Db4oEmbedded.OpenFile(DatabaseFile)
                Dim jodie As Person = QueryForJodie(container)
                For Each person As Person In jodie.Friends
                    ' the added friend is gone, because the update-depth is to low
                    Console.WriteLine("Friend=" & person.Name)
                Next
            End Using
            ' #end example
        End Sub

        Private Shared Sub ExplicitlyUpdateObjects()
            CleanUp()
            Using container As IObjectContainer = Db4oEmbedded.OpenFile(DatabaseFile)
                ' #example: Explicitly store changes on the driver
                Dim car As Car = QueryForCar(container)
                car.CarName = "New Mercedes"
                car.Driver.Name = "New Driver Name"

                ' Explicitly store the driver to ensure that those changes are also in the database
                container.Store(car)
                ' #end example
                container.Store(car.Driver)
            End Using
            PrintCar()
        End Sub

        Private Shared Sub ExplicitlyStateUpdateDepth()
            CleanUp()
            Using container As IObjectContainer = Db4oEmbedded.OpenFile(DatabaseFile)
                ' #example: Explicitly use the update depth
                Dim car As Car = QueryForCar(container)
                car.CarName = "New Mercedes"
                car.Driver.Name = "New Driver Name"

                ' Explicitly state the update depth
                ' #end example
                container.Ext().Store(car, 2)
            End Using
            PrintCar()
        End Sub

        Private Shared Sub UpdateDepthForCollection()
            ' #example: A higher update depth fixes the issue
            Dim config As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            config.Common.UpdateDepth = 2
            Using container As IObjectContainer = Db4oEmbedded.OpenFile(DatabaseFile)
                ' #end example
                Dim jodie As Person = QueryForJodie(container)
                jodie.Add(New Person("Jamie"))
                container.Store(jodie)
            End Using
            config = Db4oEmbedded.NewConfiguration()
            config.Common.UpdateDepth = 2
            Using container As IObjectContainer = Db4oEmbedded.OpenFile(DatabaseFile)
                Dim jodie As Person = QueryForJodie(container)
                For Each person As Person In jodie.Friends
                    ' the added friend is gone, because the update-depth is to low
                    Console.WriteLine("Friend=" & person.Name)
                Next
            End Using
        End Sub

        Private Shared Sub PrintCar()
            Using container As IObjectContainer = Db4oEmbedded.OpenFile(DatabaseFile)
                Dim car As Car = QueryForCar(container)
                Console.WriteLine("Car-Name:" & car.CarName)
                Console.WriteLine("Driver-Name:" & car.Driver.Name)
            End Using
        End Sub


        Private Shared Function QueryForCar(ByVal container As IObjectContainer) As Car
            Return container.Query(Of Car)()(0)
        End Function

        Private Shared Sub CleanUpAndPrepare()
            CleanUp()
            PrepareDeepObjGraph()
        End Sub


        Private Shared Sub CleanUp()
            File.Delete(DatabaseFile)
        End Sub

        Private Shared Function QueryForJodie(ByVal container As IObjectContainer) As Person
            Return (From p As Person In container Where p.Name = "Jodie").First()
        End Function

        Private Shared Sub PrepareDeepObjGraph()
            Using container As IObjectContainer = Db4oEmbedded.OpenFile(DatabaseFile)
                Dim jodie As New Person("Jodie")

                jodie.Add(New Person("Joanna"))
                jodie.Add(New Person("Julia"))

                container.Store(jodie)

                container.Store(New Car(New Person("Janette"), "Mercedes"))
            End Using
        End Sub
    End Class


    Friend Class Car
        Private m_driver As Person
        Private m_carName As String

        Friend Sub New(ByVal driver As Person, ByVal carName As String)
            Me.m_driver = driver
            Me.m_carName = carName
        End Sub

        Public Property Driver() As Person
            Get
                Return m_driver
            End Get
            Set(ByVal value As Person)
                m_driver = value
            End Set
        End Property

        Public Property CarName() As String
            Get
                Return m_carName
            End Get
            Set(ByVal value As String)
                m_carName = value
            End Set
        End Property
    End Class

    Friend Class Person
        Private m_friends As IList(Of Person) = New List(Of Person)()

        Private m_name As String

        Friend Sub New(ByVal name As String)
            Me.m_name = name
        End Sub


        Public ReadOnly Property Friends() As IList(Of Person)
            Get
                Return m_friends
            End Get
        End Property

        Public Property Name() As String
            Get
                Return m_name
            End Get
            Set(ByVal value As String)
                m_name = value
            End Set
        End Property

        Public Sub Add(ByVal item As Person)
            m_friends.Add(item)
        End Sub
    End Class
End Namespace
