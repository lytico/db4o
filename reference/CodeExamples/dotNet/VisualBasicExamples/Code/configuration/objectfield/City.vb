Imports Db4objects.Db4o.Config.Attributes

Namespace Db4oDoc.Code.Configuration.Objectfield
    Public Class City
        ' #example: Index a field
        <Indexed()> _
        Private m_zipCode As String
        ' #end example
        Private m_name As String

        Public Sub New(ByVal zipCode As String)
            Me.m_zipCode = zipCode
        End Sub

        Public ReadOnly Property ZipCode() As String
            Get
                Return m_zipCode
            End Get
        End Property

        Public ReadOnly Property Name() As String
            Get
                Return m_name
            End Get
        End Property
    End Class
End NameSpace