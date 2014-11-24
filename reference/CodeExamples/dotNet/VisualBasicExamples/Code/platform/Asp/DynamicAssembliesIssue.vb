Imports Db4objects.Db4o
Imports Db4objects.Db4o.Config

Namespace Db4oDoc.Code.Platform.Asp
    ' #example: Replace the dynamic assembly name with a fixed one
    Class AspAssemblyNamingFix
        Implements IAlias
        Private Const FixedName As String = "AspFixedAssemblyName"
        Private ReadOnly DynamicName As String _
                    = GetType(AspAssemblyNamingFix).Assembly.GetName().Name

        Public Function ResolveRuntimeName(runtimeTypeName As String) As String _
            Implements IAlias.ResolveRuntimeName
            Return runtimeTypeName.Replace(DynamicName, FixedName)
        End Function

        Public Function ResolveStoredName(storedTypeName As String) As String _
            Implements IAlias.ResolveStoredName
            Return storedTypeName.Replace(FixedName, DynamicName)
        End Function
    End Class
    ' #end example

    Public Class DynamicAssembliesIssue
        Public Shared Sub Main(args As String())
            ' #example: Fix ASP.NET assembly names
            Dim config = Db4oEmbedded.NewConfiguration()
            config.Common.AddAlias(New AspAssemblyNamingFix())
            ' #end example

        End Sub
    End Class
End Namespace
