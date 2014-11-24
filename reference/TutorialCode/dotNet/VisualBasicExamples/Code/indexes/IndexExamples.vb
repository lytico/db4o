Imports Db4objects.Db4o
Imports Db4objects.Db4o.Config

Namespace Db4oTutorialCode.Code.Indexes
    Public Class IndexExamples
        Private Shared Sub ConfigureIndexes()
            ' #example: Configure index externally
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            configuration.Common.ObjectClass(GetType(Car)).ObjectField("m_carName").Indexed(True)

            Dim container As IObjectContainer = Db4oEmbedded.OpenFile(configuration, "database.db4o")
            ' #end example
        End Sub
    End Class
End Namespace
