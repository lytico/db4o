Imports Db4objects.Db4o.Config.Attributes

Namespace Db4oTutorialCode.Code.Indexes
    ' #example: Indexing the car name
    Public Class Car
        <Indexed()> _
        Private m_carName As String
        Private m_horsePower As Integer

        Public Sub New(carName As String, horsePower As Integer)
            Me.m_carName = carName
            Me.m_horsePower = horsePower
        End Sub

        Public Property CarName() As String
            Get
                Return m_carName
            End Get
            Set(value As String)
                m_carName = value
            End Set
        End Property

        Public Property HorsePower() As Integer
            Get
                Return m_horsePower
            End Get
            Set(value As Integer)
                m_horsePower = value
            End Set
        End Property
    End Class
    ' #end example
End Namespace
