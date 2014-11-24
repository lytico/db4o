Imports Db4objects.Db4o.Config
Imports Db4objects.Db4o
Imports System.IO
Imports Library.Model
Imports JSON
Imports Db4objects.Db4o.TA

Module Module1

	Sub Main()
		PopulateDatabaseIfNeeded()

		Using db = OpenDatabase()
			For Each book In db.Query(Of Book)()
				' Comment out the next line to fix it
				'				db.Activate(book, Integer.MaxValue)
				Console.WriteLine(New JSONSerializer().Serialize(book))
			Next
		End Using

		Console.ReadKey()
	End Sub

	Private Function OpenDatabase() As IObjectContainer

		Dim configuration As IConfiguration = Db4oFactory.NewConfiguration()
		configuration.Add(New TransparentActivationSupport())
		Return Db4oFactory.OpenFile(configuration, "library.db4o")

	End Function

	Private Sub PopulateDatabaseIfNeeded()
		If File.Exists("library.db4o") Then
			Return
		End If

		Using db = OpenDatabase()

			Dim book As Book = New Book("The God Delusion")
			book.AddAuthor(New Author("Richard Dawkins"))

			db.Store(book)
		End Using

	End Sub
End Module
