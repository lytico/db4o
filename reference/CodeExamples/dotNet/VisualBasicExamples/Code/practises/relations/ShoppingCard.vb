Imports System.Collections.Generic

Namespace Db4oDoc.Code.Practises.Relations
    Friend Class ShoppingCard
        ' #example: Simple 1-n relation. Navigating from the card to the items
        Private ReadOnly items As ICollection(Of Item) = New HashSet(Of Item)()

        Public Sub Add(terrain As Item)
            items.Add(terrain)
        End Sub

        Public Sub Remove(o As Item)
            items.Remove(o)
        End Sub
        ' #end example
    End Class
End Namespace
