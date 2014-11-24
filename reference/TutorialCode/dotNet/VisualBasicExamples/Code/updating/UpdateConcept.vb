Imports System.Collections.Generic
Imports System.Linq
Imports Db4objects.Db4o
Imports Db4objects.Db4o.Config
Imports Db4objects.Db4o.Linq

Namespace Db4oTutorialCode.Code.Updating
    Public Class UpdateConcept
        Private Const DatabaseFile As String = "database.db4o"

        Public Shared Sub Main(args As String())
            StoreExampleObjects()
            UpdatingDriverDoesNotUpdateCar()
            DealWithUpdateDepth()
            IncreaseUpdateDepth()
        End Sub

        Private Shared Sub StoreExampleObjects()
            Using container As IObjectContainer = Db4oEmbedded.OpenFile(DatabaseFile)
                ' #example: Store a driver and his cars
                Dim beetle = New Car("VW Beetle")
                Dim ferrari = New Car("Ferrari")

                Dim driver = New Driver("John", ferrari)
                driver.AddOwnedCar(beetle)
                driver.AddOwnedCar(ferrari)
                
                container.Store(driver)
                ' #end example
            End Using
        End Sub

        Private Shared Sub UpdatingDriverDoesNotUpdateCar()
            Using container As IObjectContainer = Db4oEmbedded.OpenFile(DatabaseFile)
                ' #example: Update the driver and his cars
                Dim driver As Driver = QueryForDriver(container)
                driver.Name = "Johannes"
                driver.MostLovedCar.CarName = "Red Ferrari"
                driver.AddOwnedCar(New Car("Fiat Punto"))
                container.Store(driver)
                ' #end example
            End Using
            PrintOutContent()
        End Sub

        Private Shared Sub IncreaseUpdateDepth()
            ' #example: Increase the update depth to 2
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            configuration.Common.UpdateDepth = 2
            ' #end example

            Using container As IObjectContainer = Db4oEmbedded.OpenFile(configuration, DatabaseFile)
                Dim driver As Driver = QueryForDriver(container)
                driver.Name = "Joe"
                driver.MostLovedCar.CarName = "Red Ferrari"
                driver.AddOwnedCar(New Car("Fiat Punto"))
                container.Store(driver)
            End Using
            PrintOutContent()
        End Sub

        Private Shared Sub DealWithUpdateDepth()
            Using container As IObjectContainer = Db4oEmbedded.OpenFile(DatabaseFile)
                Dim driver As Driver = QueryForDriver(container)
                driver.Name = "Joe"
                driver.MostLovedCar.CarName = "Red Ferrari"
                driver.AddOwnedCar(New Car("Fiat Punto"))
                ' #example: Update everything explicitly
                container.Store(driver)
                container.Store(driver.MostLovedCar)
                container.Store(driver.OwnedCars)
                ' #end example
            End Using
            PrintOutContent()
        End Sub

        Private Shared Sub PrintOutContent()
            Using container As IObjectContainer = Db4oEmbedded.OpenFile(DatabaseFile)
                ' #example: Check the updated objects
                Dim driver As Driver = QueryForDriver(container)
                ' Is updated
                Console.WriteLine(driver.Name)
                ' Isn't updated at all
                Console.WriteLine(driver.MostLovedCar.CarName)
                ' Also the owned car list isn't updated
                For Each car As Car In driver.OwnedCars
                    Console.WriteLine(car)
                Next
                ' #end example
            End Using
        End Sub

        Private Shared Function MoreUpdateOptions() As IEmbeddedConfiguration
            ' #example: More update options
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            ' Update all referenced objects for the Driver-class
            configuration.Common.ObjectClass(GetType(Driver)).CascadeOnUpdate(True)
            ' #end example
            Return configuration
        End Function

        Private Shared Function QueryForDriver(container As IObjectContainer) As Driver
            Return (From d As Driver In container _
                    Select d).First()
        End Function
    End Class
End Namespace
