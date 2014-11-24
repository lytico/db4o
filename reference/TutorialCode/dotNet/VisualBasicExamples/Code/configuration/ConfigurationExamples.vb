Imports Db4objects.Db4o
Imports Db4objects.Db4o.Config

Namespace Db4oTutorialCode.Code.Configuration
    Public Class ConfigurationExamples
        Public Sub ConfigureDb4O()
            ' #example: Important configuration switches
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            ' #end example


            ' #example: A few examples
            ' Add index
            configuration.Common.ObjectClass(GetType(Driver)).Indexed(True)
            ' #end example

            ' #example: Finally pass the configuration container factory
            Dim container As IObjectContainer = Db4oEmbedded.OpenFile(configuration, "database.db4o")
            ' #end example
        End Sub

        Private Class Driver
        End Class
    End Class
End Namespace
