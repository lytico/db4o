Imports System.Collections
Imports System.IO
Imports Db4objects.Db4o

Namespace Db4oDoc.Code.Query.QueryByExample
    Public Class QueryByExamples
        Private Const DatabaseFile As String = "database.db4o"

        Public Shared Sub Main(ByVal args As String())
            CleanUp()
            Using container As IObjectContainer = Db4oEmbedded.OpenFile(DatabaseFile)
                StoreData(container)

                QueryForName(container)
                QueryForAge(container)
                QueryForNameAndAge(container)

                NestedObjects(container)
                AllObjects(container)
                AllObjectsOfAType(container)
                AllObjectsOfATypeWithEmptyObject(container)

                ContainsQuery(container)
                StructuredContains(container)
            End Using
        End Sub

        Private Shared Sub QueryForName(ByVal container As IObjectContainer)
            ' #example: Query for John by example
            Dim theExample As New Pilot()
            theExample.Name = "John"
            Dim result As IList = container.QueryByExample(theExample)
            ' #end example

            ListResult(result)
        End Sub

        Private Shared Sub QueryForAge(ByVal container As IObjectContainer)
            ' #example: Query for 33 year old pilots
            Dim theExample As New Pilot()
            theExample.Age = 33
            Dim result As IList = container.QueryByExample(theExample)
            ' #end example

            ListResult(result)
        End Sub

        Private Shared Sub QueryForNameAndAge(ByVal container As IObjectContainer)
            ' #example: Query a 29 years old Jo
            Dim theExample As New Pilot()
            theExample.Name = "Jo"
            theExample.Age = 29
            Dim result As IList = container.QueryByExample(theExample)
            ' #end example

            ListResult(result)
        End Sub

        Private Shared Sub AllObjects(ByVal container As IObjectContainer)
            ' #example: All objects
            Dim result As IList = container.QueryByExample(Nothing)
            ' #end example

            ListResult(result)
        End Sub

        Private Shared Sub AllObjectsOfAType(ByVal container As IObjectContainer)
            ' #example: All objects of a type by passing the type
            Dim result As IList = container.QueryByExample(GetType(Pilot))
            ' #end example

            ListResult(result)
        End Sub

        Private Shared Sub AllObjectsOfATypeWithEmptyObject(ByVal container As IObjectContainer)
            ' #example: All objects of a type by passing a empty example
            Dim example As New Pilot()
            Dim result As IList = container.QueryByExample(example)
            ' #end example

            ListResult(result)
        End Sub

        Private Shared Sub NestedObjects(ByVal container As IObjectContainer)
            ' #example: Nested objects example
            Dim pilotExample As New Pilot()
            pilotExample.Name = "Jenny"

            Dim carExample As New Car()
            carExample.Pilot = pilotExample
            Dim result As IList = container.QueryByExample(carExample)
            ' #end example

            ListResult(result)
        End Sub

        Private Shared Sub ContainsQuery(ByVal container As IObjectContainer)
            ' #example: Contains in collections
            Dim pilotExample As New BlogPost()
            pilotExample.AddTags("db4o")
            Dim result As IList = container.QueryByExample(pilotExample)
            ' #end example

            ListResult(result)
        End Sub

        Private Shared Sub StructuredContains(ByVal container As IObjectContainer)
            ' #example: Structured contains
            Dim pilotExample As New BlogPost()
            pilotExample.AddAuthors(New Author("John"))
            Dim result As IList = container.QueryByExample(pilotExample)
            ' #end example

            ListResult(result)
        End Sub


        Private Shared Sub ListResult(ByVal result As IEnumerable)
            For Each obj As Object In result
                Console.WriteLine(obj)
            Next
        End Sub

        Private Shared Sub CleanUp()
            File.Delete(DatabaseFile)
        End Sub

        Private Shared Sub StoreData(ByVal container As IObjectContainer)
            Dim john As New Pilot("John", 42)
            Dim joanna As New Pilot("Joanna", 45)
            Dim jenny As New Pilot("Jenny", 21)
            Dim rick As New Pilot("Rick", 33)
            Dim juliette As New Pilot("Juliette", 33)
            container.Store(New Pilot("Jo", 34))
            container.Store(New Pilot("Jo", 29))
            container.Store(New Pilot("Jimmy", 33))


            container.Store(New Car(john, "Ferrari"))
            container.Store(New Car(joanna, "Mercedes"))
            container.Store(New Car(jenny, "Volvo"))
            container.Store(New Car(rick, "Fiat"))
            container.Store(New Car(juliette, "Suzuki"))


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
