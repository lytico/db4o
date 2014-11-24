Imports Db4objects.Db4o.Collections

Public Class Book

	Private _title As String
	Private _authors As ArrayList4(of Author)

	Sub New(ByVal title As String)
		_title = title
		_authors = New ArrayList4(Of Author)
	End Sub

	Public ReadOnly Property Title() As String
		Get
			Return _title
		End Get
	End Property

	Public Sub AddAuthor(ByVal author As Author)
		_authors.Add(author)
	End Sub

End Class
