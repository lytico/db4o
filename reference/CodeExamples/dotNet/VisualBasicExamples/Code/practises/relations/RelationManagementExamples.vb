Imports System.Linq
Imports Db4objects.Db4o
Imports Db4objects.Db4o.Linq

Namespace Db4oDoc.Code.Practises.Relations
    Public Class RelationManagementExamples
        Public Shared Sub Main(args As String())
            Using container As IObjectContainer = Db4oEmbedded.OpenFile("database.db4o")
                StoreTestData(container)

                LoadPersonsOfACountry(container)
            End Using
        End Sub

        Private Shared Sub StoreTestData(container As IObjectContainer)
            Dim switzerland = New Country("Switzerland")
            Dim china = New Country("China")
            Dim japan = New Country("Japan")
            Dim usa = New Country("USA")
            Dim germany = New Country("Germany")

            container.Store(New Person("Berni", "Gian-Reto", switzerland))
            container.Store(New Person("Wang", "Long", china))
            container.Store(New Person("Tekashi", "Amuro", japan))
            container.Store(New Person("Miller", "John", usa))
            container.Store(New Person("Smith", "Paul", usa))
            container.Store(New Person("Müller", "Hans", germany))
        End Sub

        Private Shared Sub LoadPersonsOfACountry(container As IObjectContainer)
            ' #example: Query for people burn in a country
            Dim country = LoadCountry(container, "USA")
            Dim peopleBurnInTheUs = From p As Person In container _
                    Where p.BornIn Is country _
                    Select p
            ' #end example
            Console.Out.WriteLine(peopleBurnInTheUs.Count())
        End Sub

        Private Shared Function LoadCountry(container As IObjectContainer, countryName As String) As Country
            Return (From c As Country In container _
                Where c.Name = countryName _
                Select c).Single()
        End Function
    End Class
End Namespace
