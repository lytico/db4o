Imports System.Collections
Imports System.Collections.Generic
Imports Db4objects.Db4o
Imports Db4objects.Db4o.Query
' #example: Include LINQ namespaces
Imports System.Linq
Imports Db4objects.Db4o.Linq
' #end example

Namespace Db4oTutorialCode.Code.Queries
    Public Class Queries
        Private Const DatabaseFile As String = "databaseFile.db4o"

        Public Shared Sub Main(args As String())
            StoreExampleObjects()
            QueryForJoe()
            QueryPeopleWithPowerfulCar()
            QueryableInterface()
            UnoptimizableQuery()
            SodaQueryForJoe()
            SodaQueryForPowerfulCars()
        End Sub

        Private Shared Sub QueryForJoe()
            Using container As IObjectContainer = Db4oEmbedded.OpenFile(DatabaseFile)
                ' #example: Query for drivers named Joe
                Dim drivers = From d As Driver In container _
                              Where d.Name = "Joe" _
                              Select d
                ' #end example
                Console.WriteLine("Driver named Joe")
                For Each driver As Driver In drivers
                    Console.WriteLine(driver)
                Next
            End Using
        End Sub

        Private Shared Sub QueryPeopleWithPowerfulCar()
            Using container As IObjectContainer = Db4oEmbedded.OpenFile(DatabaseFile)
                ' #example: Query for people with powerful cars
                Dim drivers = From d As Driver In container _
                              Where d.MostLovedCar.HorsePower > 150 AndAlso d.Age >= 18 _
                              Select d
                ' #end example
                Console.WriteLine("People with powerful cars:")
                For Each driver As Driver In drivers
                    Console.WriteLine(driver)
                Next
            End Using
        End Sub

        Private Shared Sub QueryableInterface()
            Using container As IObjectContainer = Db4oEmbedded.OpenFile(DatabaseFile)
                ' #example: Get the IQueryable interface
                Dim querable As IQueryable(Of Driver) = container.AsQueryable(Of Driver)()

                Dim drivers = From d In querable _
                              Where d.Name = "Joe"
                              Select d
                ' #end example
                Console.WriteLine("Drivers named Joe")
                For Each driver As Driver In drivers
                    Console.WriteLine(driver)
                Next
            End Using
        End Sub

        Private Shared Sub UnoptimizableQuery()
            Using container As IObjectContainer = Db4oEmbedded.OpenFile(DatabaseFile)
                ' #example: Unoptimizable query
                ' This query will print a 'QueryOptimizationException' in the debugger console.
                ' That means it runs very slowly and you should find an alternative.
                ' This example query cannot be optimized because the hash code isn't a stored in database
                Dim drivers = From d As Driver In container _
                              Where d.GetHashCode() = 42 _
                              Select d
                ' #end example
                For Each driver As Driver In drivers
                    Console.WriteLine(driver)
                Next
            End Using
        End Sub

        Private Shared Sub SodaQueryForJoe()
            Using container As IObjectContainer = Db4oEmbedded.OpenFile(DatabaseFile)
                ' #example: Query for drivers named Joe with SODA
                Dim query As IQuery = container.Query()
                query.Constrain(GetType(Driver))
                query.Descend("name").Constrain("Joe")
                Dim queryResult As IList = query.Execute()
                Dim drivers As IList(Of Driver) = queryResult.Cast(Of Driver)().ToList()
                ' #end example
                Console.WriteLine("Driver named Joe")
                For Each driver As Driver In drivers
                    Console.WriteLine(driver)
                Next
            End Using
        End Sub

        Private Shared Sub SodaQueryForPowerfulCars()
            Using container As IObjectContainer = Db4oEmbedded.OpenFile(DatabaseFile)
                ' #example: Query for people with powerful cars with SODA
                Dim query As IQuery = container.Query()
                query.Constrain(GetType(Driver))
                query.Descend("mostLovedCar").Descend("horsePower").Constrain(150).Greater()
                query.Descend("age").Constrain(18).Greater().Equal()
                Dim queryResult As IList = query.Execute()
                Dim drivers As IList(Of Driver) = queryResult.Cast(Of Driver)().ToList()
                ' #end example
                Console.WriteLine("People with powerful cars:")
                For Each driver As Driver In drivers
                    Console.WriteLine(driver)
                Next
            End Using
        End Sub


        Private Shared Sub StoreExampleObjects()
            Using container As IObjectContainer = Db4oEmbedded.OpenFile(DatabaseFile)
                Dim vwBeetle = New Car("VW Beetle", 90)
                Dim audi = New Car("Audi A6", 175)
                Dim ferrari = New Car("Ferrari", 215)

                Dim joe = New Driver("Joe", 42, audi)
                Dim joanna = New Driver("Joanna", 24, vwBeetle)
                Dim jenny = New Driver("Jenny", 54)
                Dim john = New Driver("John", 17, ferrari)
                Dim jim = New Driver("Jim", 18, audi)

                container.Store(joe)
                container.Store(joanna)
                container.Store(jenny)
                container.Store(john)
                container.Store(jim)
            End Using
        End Sub
    End Class
End Namespace
