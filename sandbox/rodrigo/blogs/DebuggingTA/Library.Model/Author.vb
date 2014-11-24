Public Class Author
	Private _name As String

	Sub New(ByVal name As String)
		_name = name
	End Sub

	Public ReadOnly Property Name() As String
		Get
			Return _name
		End Get
	End Property
End Class
