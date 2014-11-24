Module JSONSerialization

	Public Function ToJSONString(ByVal o As Object) As String

		Return New JSONSerializer().Serialize(o)

	End Function

End Module
