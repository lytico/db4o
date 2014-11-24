Imports System.Collections.Generic
Imports Db4objects.Db4o.Collections

Namespace Db4oDoc.Code.TA.CollectionEnhanceIssues
    ' #example: Can be enhanced by the db4o-tools
    Public Class CanBeEnhanced
        Private _names As IList(Of String) = New List(Of String)()

        Public Function ContainsName(ByVal item As String) As Boolean
            Return _names.Contains(item)
        End Function

        Public Sub AddName(ByVal item As String)
            _names.Add(item)
        End Sub
    End Class
    ' #end example
    Namespace AfterEnhancement
        ' #example: Is enhanced to
        Public Class CanBeEnhanced
            Private _names As IList(Of String) = New ActivatableList(Of String)()

            Public Function ContainsName(ByVal item As String) As Boolean
                Return _names.Contains(item)
            End Function

            Public Sub AddName(ByVal item As String)
                _names.Add(item)
            End Sub
        End Class
        ' #end example

    End Namespace


    ' #example: Cannot be enhanced by the db4o-tools
    Public Class CannotBeEnhanced
        ' cannot be enhanced, because it uses the concrete type
        Private _names As New List(Of String)()

        Public Function ContainsName(ByVal item As String) As Boolean
            Return _names.Contains(item)
        End Function

        Public Sub AddName(ByVal item As String)
            _names.Add(item)
        End Sub
    End Class
    ' #end example
End Namespace
