Imports Db4objects.Db4o
Imports Db4objects.Db4o.Config

Namespace Db4oDoc.Code.Platform.AutoProperties

    '#example: Auto property
    Public Class Person
        Public Property Name() As String
    End Class
    '#end example


    Public Class AutoPropertyConfiguration

        Private Sub ConfigureAutoProperty()
            ' #example: Configure auto properties
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            configuration.Common.ObjectClass(GetType(Person)).ObjectField("_Name").Indexed(True)
            ' #end example
        End Sub
    End Class

    Namespace HowItsImplemented

        '#example: Auto property behind the scenes
        Public Class Person
            Private _Name As String

            Public Property Name() As String
                Get
                    Return _Name
                End Get
                Set(ByVal value As String)
                    _Name = value
                End Set
            End Property
        End Class
        '#end example

    End Namespace
End Namespace
