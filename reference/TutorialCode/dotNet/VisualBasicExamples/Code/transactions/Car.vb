Imports Db4oTutorialCode.Db4oTutorialCode.Code.TransparentPersistence

Namespace Db4oTutorialCode.Code.Transactions
    ' #example: Domain model for cars
    <TransparentPersisted()> _
    Public Class Car
        Private m_carName As String

        Public Sub New(carName As String)
            Me.m_carName = carName
        End Sub

        Public ReadOnly Property CarName() As String
            Get
                Return m_carName
            End Get
        End Property
    End Class
    ' #end example
End Namespace
