Imports System.Collections
Imports System.Collections.Generic
Imports System.IO
Imports Db4objects.Db4o
Imports Db4objects.Db4o.Query

Namespace Db4oDoc.Code.Query.Soda
    Public Class SodaQueryExamples
        Private Const DatabaseFile As String = "database.db4o"

        Public Shared Sub Main(ByVal args As String())
            CleanUp()

            Using container As IObjectContainer = Db4oEmbedded.OpenFile(DatabaseFile)
                StoreData(container)

                SimplestPossibleQuery(container)
                EqualsConstrain(container)
                GreaterThanConstrain(container)
                GreaterThanOrEqualConstrain(container)
                NotConstrain(container)
                CombiningConstrains(container)
                StringConstrains(container)
                CompareWithStoredObject(container)
                DescentDeeper(container)

                ContainsOnCollection(container)
                DescendIntoCollectionMembers(container)
                ContainsOnMaps(container)
                FieldObject(container)

                GenericConstrains(container)
                DescendIntoNotExistingField(container)
                MixWithQueryByExample(container)
            End Using
        End Sub

        Private Shared Sub SimplestPossibleQuery(ByVal container As IObjectContainer)
            Console.WriteLine("Type constrain for the objects")
            ' #example: Type constrain for the objects
            Dim query As IQuery = container.Query()
            query.Constrain(GetType(Pilot))

            Dim result As IObjectSet = query.Execute()
            ' #end example
            ListResult(result)
        End Sub

        Private Shared Sub EqualsConstrain(ByVal container As IObjectContainer)
            Console.WriteLine("A simple constrain on a field")
            ' #example: A simple constrain on a field
            Dim query As IQuery = container.Query()
            query.Constrain(GetType(Pilot))
            query.Descend("name").Constrain("John")

            Dim result As IObjectSet = query.Execute()
            ' #end example
            ListResult(result)
        End Sub

        Private Shared Sub GreaterThanConstrain(ByVal container As IObjectContainer)
            Console.WriteLine("A greater than constrain")
            ' #example: A greater than constrain
            Dim query As IQuery = container.Query()
            query.Constrain(GetType(Pilot))
            query.Descend("age").Constrain(42).Greater()

            Dim result As IObjectSet = query.Execute()
            ' #end example
            ListResult(result)
        End Sub

        Private Shared Sub GreaterThanOrEqualConstrain(ByVal container As IObjectContainer)
            Console.WriteLine("A greater than or equals constrain")
            ' #example: A greater than or equals constrain
            Dim query As IQuery = container.Query()
            query.Constrain(GetType(Pilot))
            query.Descend("age").Constrain(42).Greater().Equal()

            Dim result As IObjectSet = query.Execute()
            ' #end example
            ListResult(result)
        End Sub

        Private Shared Sub NotConstrain(ByVal container As IObjectContainer)
            Console.WriteLine("Not constrain")
            ' #example: Not constrain
            Dim query As IQuery = container.Query()
            query.Constrain(GetType(Pilot))
            query.Descend("age").Constrain(42).Not()

            Dim result As IObjectSet = query.Execute()
            ' #end example
            ListResult(result)
        End Sub

        Private Shared Sub CombiningConstrains(ByVal container As IObjectContainer)
            Console.WriteLine("Logical combination of constrains")
            ' #example: Logical combination of constrains
            Dim query As IQuery = container.Query()
            query.Constrain(GetType(Pilot))
            query.Descend("age").Constrain(42).Greater().Or(query.Descend("age").Constrain(30).Smaller())

            Dim result As IObjectSet = query.Execute()
            ' #end example
            ListResult(result)
        End Sub

        Private Shared Sub StringConstrains(ByVal container As IObjectContainer)
            Console.WriteLine("String comparison")
            ' #example: String comparison
            Dim query As IQuery = container.Query()
            query.Constrain(GetType(Pilot))
            ' First strings, you can use the contains operator
            ' Or like, which is like .contains(), but case insensitive
            ' The .endsWith and .startWith constrains are also there,
            ' the true for case-sensitive, false for case-insensitive
            query.Descend("name").Constrain("oh").Contains().Or(query.Descend("name").Constrain("AnN").Like()).Or(query.Descend("name").Constrain("NY").EndsWith(False))

            Dim result As IObjectSet = query.Execute()
            ' #end example
            ListResult(result)
        End Sub

        Private Shared Sub CompareWithStoredObject(ByVal container As IObjectContainer)
            Console.WriteLine("Compare with existing object")
            ' #example: Compare with existing object
            Dim pilot As Pilot = container.Query(Of Pilot)()(0)

            Dim query As IQuery = container.Query()
            query.Constrain(GetType(Car))
            ' if the given object is stored, its compared by identity
            query.Descend("pilot").Constrain(pilot)

            Dim carsOfPilot As IObjectSet = query.Execute()
            ' #end example
            ListResult(carsOfPilot)
        End Sub

        Private Shared Sub DescentDeeper(ByVal container As IObjectContainer)
            Console.WriteLine("Descend over multiple fields")

            ' #example: Descend over multiple fields
            Dim query As IQuery = container.Query()
            query.Constrain(GetType(Car))
            query.Descend("pilot").Descend("name").Constrain("John")

            Dim result As IObjectSet = query.Execute()
            ' #end example
            ListResult(result)
        End Sub

        Private Shared Sub ContainsOnCollection(ByVal container As IObjectContainer)
            Console.WriteLine("Collection contains constrain")
            ' #example: Collection contains constrain
            Dim query As IQuery = container.Query()
            query.Constrain(GetType(BlogPost))
            query.Descend("tags").Constrain("db4o")

            Dim result As IObjectSet = query.Execute()
            ' #end example
            ListResult(result)
        End Sub

        Private Shared Sub DescendIntoCollectionMembers(ByVal container As IObjectContainer)
            Console.WriteLine("Descend into collection members")
            ' #example: Descend into collection members
            Dim query As IQuery = container.Query()
            query.Constrain(GetType(BlogPost))
            query.Descend("authors").Descend("name").Constrain("Jenny")

            Dim result As IObjectSet = query.Execute()
            ' #end example
            ListResult(result)
        End Sub

        Private Shared Sub ContainsOnMaps(ByVal container As IObjectContainer)
            Console.WriteLine("Dictionary contains a key constrain")
            ' #example: Dictionary contains a key constrain
            Dim query As IQuery = container.Query()
            query.Constrain(GetType(BlogPost))
            query.Descend("metaData").Constrain("source")

            Dim result As IObjectSet = query.Execute()
            ' #end example
            ListResult(result)
        End Sub

        Private Shared Sub FieldObject(ByVal container As IObjectContainer)
            Console.WriteLine("Return the object of a field")
            ' #example: Return the object of a field
            Dim query As IQuery = container.Query()
            query.Constrain(GetType(Car))
            query.Descend("name").Constrain("Mercedes")

            ' returns the pilot of these cars
            Dim result As IObjectSet = query.Descend("pilot").Execute()
            ' #end example
            ListResult(result)
        End Sub

        Private Shared Sub GenericConstrains(ByVal container As IObjectContainer)
            Console.WriteLine("Pure field constrains")
            ' #example: Pure field constrains
            Dim query As IQuery = container.Query()
            ' You can simple filter objects which have a certain field
            query.Descend("name").Constrain(Nothing).Not()

            Dim result As IObjectSet = query.Execute()
            ' #end example
            ListResult(result)
        End Sub

        Private Shared Sub DescendIntoNotExistingField(ByVal container As IObjectContainer)
            Console.WriteLine("Using not existing fields excludes objects")
            ' #example: Using not existing fields excludes objects
            Dim query As IQuery = container.Query()
            query.Constrain(GetType(Pilot))
            ' using not existing fields doesn't throw an exception
            ' but rather exclude all object which don't use this field
            query.Descend("notExisting").Constrain(Nothing).Not()

            Dim result As IObjectSet = query.Execute()
            ' #end example
            ListResult(result)
        End Sub

        Private Shared Sub MixWithQueryByExample(ByVal container As IObjectContainer)
            Console.WriteLine("Mix with query by example")
            ' #example: Mix with query by example
            Dim query As IQuery = container.Query()
            query.Constrain(GetType(Car))
            ' if the given object is not stored,
            ' it will behave like query by example for the given object
            Dim examplePilot As New Pilot(Nothing, 42)
            query.Descend("pilot").Constrain(examplePilot)

            Dim carsOfPilot As IObjectSet = query.Execute()
            ' #end example
            ListResult(carsOfPilot)
        End Sub


        Private Shared Sub CleanUp()
            File.Delete(DatabaseFile)
        End Sub


        Private Shared Sub ListResult(ByVal result As IEnumerable)
            For Each obj As Object In result
                Console.WriteLine(obj)
            Next
        End Sub

        Private Shared Sub StoreData(ByVal container As IObjectContainer)
            Dim john As New Pilot("John", 42)
            Dim joanna As New Pilot("Joanna", 45)
            Dim jenny As New Pilot("Jenny", 21)
            Dim rick As New Pilot("Rick", 33)

            container.Store(New Car(john, "Ferrari"))
            container.Store(New Car(joanna, "Mercedes"))
            container.Store(New Car(jenny, "Volvo"))
            container.Store(New Car(rick, "Fiat"))

            Dim firstPost As New BlogPost("db4o", "Content about db4o")
            firstPost.AddTags("db4o", ".net", "java", "database")
            firstPost.AddMetaData("comment-feed-link", "localhost/rss")
            firstPost.AddAuthors(New Author("John"), New Author("Jenny"), New Author("Joanna"))

            container.Store(firstPost)

            Dim secondPost As New BlogPost("cars", "Speedy cars")
            secondPost.AddTags("cars", "fast")
            secondPost.AddMetaData("comment-feed-link", "localhost/rss")
            secondPost.AddMetaData("source", "www.wikipedia.org")
            secondPost.AddAuthors(New Author("Joanna"), New Author("Jenny"))

            container.Store(secondPost)
        End Sub
    End Class
End Namespace