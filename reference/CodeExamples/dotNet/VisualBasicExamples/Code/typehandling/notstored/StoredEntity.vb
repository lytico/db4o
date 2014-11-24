Imports Db4objects.Db4o

Namespace Db4oDoc.Code.TypeHandling.NotStored
    ' #example: Mark a field as transient
    <Serializable()>
    Public Class StoredEntity
        ' Fields marked with NonSerialized won't be stored
        <NonSerialized()> _
        Private someCachedValue As Integer

        ' Fields marked with Transient won't be stored
        <Transient()> _
        Private someOtherCachedValue As Integer

        ' ..
    End Class
    ' #end example
End Namespace
