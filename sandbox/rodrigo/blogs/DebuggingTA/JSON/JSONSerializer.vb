Imports System.Collections
Imports System.Reflection


Public Class JSONSerializer

	Dim _buffer As New System.Text.StringBuilder

	Public Function Serialize(ByVal o As Object)
		SerializeElement(o)
		Return _buffer.ToString
	End Function

	Private Sub SerializeElement(ByVal o As Object)

		If o Is Nothing Then
			Append("null")
			Return
		End If

		Dim s As String = TryCast(o, String)
		If Not s Is Nothing Then
			SerializeString(s)
		Else
			Dim l As IList = TryCast(o, IList)
			If Not l Is Nothing Then
				SerializeList(l)
			Else
				SerializeObject(o)
			End If
		End If
	End Sub

	Private Sub SerializeString(ByVal s As String)
		Append(" """)
		Append(s)
		Append(""" ")
	End Sub

	Private Sub SerializeList(ByVal l As IList)
		Append("[ ")
		For Each item In l
			SerializeElement(item)
		Next
		Append("]")
	End Sub

	Private Sub SerializeObject(ByVal o As Object)
		Dim appendComma = False
		Append("{ ")
		For Each field In o.GetType().GetFields(BindingFlags.Public + BindingFlags.NonPublic + BindingFlags.Instance)
			If field.IsNotSerialized Then
				Continue For
			End If
			If appendComma Then
				Append(", ")
			Else
				appendComma = True
			End If
			Append(field.Name)
			Append(" : ")
			SerializeElement(field.GetValue(o))
		Next
		Append("} ")
	End Sub

	Private Sub Append(ByVal s As String)
		_buffer.Append(s)
	End Sub

End Class
