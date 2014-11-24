
Imports Db4objects.Db4o.Linq.Tests
Imports Db4objects.Db4o.Linq
Imports Db4oUnit

Public Class WhereTestCase

	Inherits AbstractDb4oLinqTestCase

	Public Class Item

		Public Name As String

		Sub New(ByVal Name As String)
			Me.Name = Name
		End Sub

		Public Overrides Function Equals(ByVal obj As Object) As Boolean
			Dim other = TryCast(obj, Item)
			If other Is Nothing Then
				Return False
			End If
			Return Name = other.Name
		End Function

		Public Overrides Function GetHashCode() As Integer
			Return Name.GetHashCode
		End Function

		Public Overrides Function ToString() As String
			Return String.Format("Item({0})", Name)
		End Function

	End Class

	Protected Overrides Sub Store()
		Store(New Item("jb"))
		Store(New Item("ana"))
	End Sub

'	Public Sub TestBuild()
'		Assert.IsFalse(True)
'	End Sub

	Public Sub TestWhereStringEqual()
		AssertQuery("(Item(Name == 'ana'))", AddressOf WhereStringEqual)
	End Sub

	Public Sub TestWhereStringNotEqual()
		AssertQuery("(Item(Name not 'jb'))", AddressOf WhereStringNotEqual)
	End Sub

	Sub WhereStringEqual()
		Dim query = From p As Item In Db() Where p.Name = "ana" Select p
		AssertSet(New Item() {New Item("ana")}, query)
	End Sub

	Sub WhereStringNotEqual()
		Dim query = From p As Item In Db() Where p.Name <> "jb" Select p
		AssertSet(New Item() {New Item("ana")}, query)
	End Sub
End Class
