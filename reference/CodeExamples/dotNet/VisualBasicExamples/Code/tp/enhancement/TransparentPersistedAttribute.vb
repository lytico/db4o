Namespace Db4oDoc.Code.Tp.Enhancement
    ' #example: Annotation to mark persisted classes
    <AttributeUsage(AttributeTargets.Class, AllowMultiple:=False, Inherited:=True)> _
    Public Class TransparentPersistedAttribute
        Inherits Attribute
    End Class
    ' #end example

End Namespace
