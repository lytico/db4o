Imports Library.Model
Imports JSON

Module Spike1

	Sub Main()

		Dim book = New Book("The God Delusion")
		book.AddAuthor(New Author("Richard Dawkins"))

		Console.WriteLine(new JSONSerializer().Serialize(book))
		Console.ReadKey()

	End Sub

End Module
