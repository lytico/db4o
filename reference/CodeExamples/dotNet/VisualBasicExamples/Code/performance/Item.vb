Imports Db4objects.Db4o.Config.Attributes

Namespace Db4oDoc.Code.Performance
    Public Class Item
        <Indexed()> _
        Private ReadOnly m_indexedString As String
        <Indexed()> _
        Private ReadOnly m_indexNumber As Integer
        <Indexed()> _
        Private ReadOnly m_indexDate As DateTime

        Public Event AnEvent As Action

        Public Sub New(number As Integer)
            Me.m_indexedString = DataString(number)
            Me.m_indexNumber = number
            Me.m_indexDate = New DateTime(number)
        End Sub


        Public Shared Function DataString(number As Integer) As String
            Return "data for " & number
        End Function

        Public Function ComplexMethod() As Boolean
            Return m_indexedString.Contains((m_indexNumber Mod 42).ToString())
        End Function

        Public ReadOnly Property IndexedString() As String
            Get
                Return m_indexedString
            End Get
        End Property

        Public ReadOnly Property PropertyWhichFiresEvent() As String
            Get
                RaiseEvent AnEvent()
                Return m_indexedString
            End Get
        End Property

        Public ReadOnly Property IndexNumber() As Integer
            Get
                Return m_indexNumber
            End Get
        End Property

        Public ReadOnly Property IndexDate() As DateTime
            Get
                Return m_indexDate
            End Get
        End Property
    End Class
End Namespace
