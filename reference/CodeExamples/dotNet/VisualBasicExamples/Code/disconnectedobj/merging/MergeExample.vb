Imports System
Imports System.Collections.Generic
Imports System.IO
Imports Db4objects.Db4o
Imports Db4objects.Db4o.Config
Imports Db4objects.Db4o.Linq

Namespace Db4oDoc.Code.DisconnectedObj.Merging
    Public Class MergeExample
        Private Const DatabaseFileName As String = "database.db4o"


        Public Shared Sub Main(ByVal args As String())
            RunMergeExample()
        End Sub

        Private Shared Sub RunMergeExample()
            CleanUp()

            StoreCar()
            PrintCars()

            Dim car As Car = GetCarByName("Slow Car")
            UpdateCar(car)

            UpdateWithMerging(car)


            PrintCars()

            CleanUp()
        End Sub

        Private Shared Sub UpdateWithMerging(ByVal disconnectedCar As Car)
            ' #example: merging
            Using container As IObjectContainer = OpenDatabase()
                ' first get the object from the database
                Dim carInDb As Car = GetCarById(container, disconnectedCar.ObjectId)

                ' copy the value-objects (int, long, double, string etc)
                carInDb.Name = disconnectedCar.Name

                ' traverse into the references
                Dim pilotInDB As Pilot = carInDb.Pilot
                Dim disconnectedPilot As Pilot = disconnectedCar.Pilot

                ' check if the object is still the same
                If pilotInDB.ObjectId.Equals(disconnectedPilot.ObjectId) Then
                    ' if it is, copy the value-objects
                    pilotInDB.Name = disconnectedPilot.Name
                    pilotInDB.Points = disconnectedPilot.Points
                Else
                    ' otherwise replace the object
                    carInDb.Pilot = disconnectedPilot
                End If

                ' finally store the changes
                container.Store(pilotInDB)
                ' #end example
                container.Store(carInDb)
            End Using
        End Sub

        Private Shared Sub UpdateCar(ByVal car As Car)
            car.Name = "Fast Car"
            car.Pilot.Points = 300
        End Sub

        Private Shared Sub PrintCars()
            Using container As IObjectContainer = OpenDatabase()
                Dim cars As IList(Of Car) = container.Query(Of Car)()
                For Each car As Car In cars
                    Console.WriteLine(car)
                Next
            End Using
        End Sub

        Private Shared Function GetCarById(ByVal container As IObjectContainer, ByVal id As Guid) As Car
            Return (From car As Car In container _
                Where car.ObjectId.Equals(id) _
                Select car).Single()
        End Function

        Private Shared Function GetCarByName(ByVal carName As String) As Car
            Using container As IObjectContainer = OpenDatabase()
                Return (From car As Car In container _
                        Where car.Name.Equals(carName) _
                        Select car).First()
            End Using
        End Function


        Private Shared Sub StoreCar()
            Using container As IObjectContainer = OpenDatabase()
                container.Store(New Car(New Pilot("Joe", 200), "Slow Car"))
            End Using
        End Sub


        Private Shared Sub CleanUp()
            File.Delete(DatabaseFileName)
        End Sub


        Private Shared Function OpenDatabase() As IObjectContainer
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            configuration.Common.ObjectClass(GetType(IDHolder)).ObjectField("guid").Indexed(True)
            Return Db4oEmbedded.OpenFile(configuration, DatabaseFileName)
        End Function
    End Class
End Namespace
