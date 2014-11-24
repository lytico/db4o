Imports System.Linq
Imports Db4objects.Db4o
Imports Db4objects.Db4o.Config
Imports Db4objects.Db4o.Linq
Imports Db4objects.Db4o.TA

Namespace Db4oTutorialCode.Code.TransparentPersistence
    Public Class TransparentPersistence
        Private Const DatabaseFileName As String = "database.db4o"

        Public Shared Sub Main(args As String())
            CheckEnhancement()
            StoreExampleObjects()
            ActivationJustWorks()
            UpdatesJustWork()
        End Sub

        Private Shared Sub ActivationJustWorks()
            ' #example: Configure transparent persistence
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            configuration.Common.Add(New TransparentPersistenceSupport(New DeactivatingRollbackStrategy()))
            Using container As IObjectContainer = Db4oEmbedded.OpenFile(configuration, DatabaseFileName)
                ' #end example
                '#example: Transparent persistence manages activation
                Dim driver As Driver = QueryForDriver(container)
                ' Transparent persistence will activate objects as needed
                Console.WriteLine("Is activated? " & container.Ext().IsActive(driver))
                Dim nameOfDriver As String = driver.Name
                Console.WriteLine("The name is " & nameOfDriver)
                Console.WriteLine("Is activated? " & container.Ext().IsActive(driver))
                '#end example
            End Using
        End Sub

        Private Shared Sub UpdatesJustWork()
            Using container As IObjectContainer = OpenDatabase()
                ' #example: Just update and commit. Transparent persistence manages all updates
                Dim driver As Driver = QueryForDriver(container)
                driver.MostLovedCar.CarName = "New name"
                driver.Name = "John Turbo"
                driver.AddOwnedCar(New Car("Volvo Combi"))
                ' Just commit the transaction. All modified objects are stored
                container.Commit()
                ' #end example
            End Using
        End Sub

        Private Shared Sub CheckEnhancement()
            ' #example: Check for enhancement
            If Not GetType(IActivatable).IsAssignableFrom(GetType(Car)) Then
                Throw New InvalidOperationException(String.Format("Expect that the {0} implements {1}", GetType(Car), GetType(IActivatable)))
            End If
            ' #end example
            If Not GetType(IActivatable).IsAssignableFrom(GetType(Driver)) Then
                Throw New InvalidOperationException(String.Format("Expect that the {0} implements {1}", GetType(Driver), GetType(IActivatable)))
            End If
        End Sub

        Private Shared Sub StoreExampleObjects()
            Using container As IObjectContainer = Db4oEmbedded.OpenFile(DatabaseFileName)
                Dim beetle = New Car("VW Beetle")
                Dim ferrari = New Car("Ferrari")

                Dim driver = New Driver("John", ferrari)
                driver.AddOwnedCar(beetle)
                driver.AddOwnedCar(ferrari)

                container.Store(driver)
            End Using
        End Sub

        Private Shared Function OpenDatabase() As IObjectContainer
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            configuration.Common.Add(New TransparentPersistenceSupport(New DeactivatingRollbackStrategy()))
            Return Db4oEmbedded.OpenFile(configuration, DatabaseFileName)
        End Function

        Private Shared Function QueryForDriver(container As IObjectContainer) As Driver
            Return (From d As Driver In container _
                    Select d).First()
        End Function
    End Class
End Namespace
