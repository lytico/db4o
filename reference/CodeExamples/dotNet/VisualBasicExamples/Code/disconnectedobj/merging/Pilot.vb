Namespace Db4oDoc.Code.DisconnectedObj.Merging
    Public Class Pilot
        Inherits IDHolder
        Private m_name As String
        Private m_points As Integer

        Public Sub New(ByVal name As String)
            Me.m_name = name
        End Sub

        Public Sub New(ByVal name As String, ByVal points As Integer)
            Me.m_name = name
            Me.m_points = points
        End Sub

        Public Property Name() As String
            Get
                Return m_name
            End Get
            Set(ByVal value As String)
                m_name = value
            End Set
        End Property

        Public Property Points() As Integer
            Get
                Return m_points
            End Get
            Set(ByVal value As Integer)
                m_points = value
            End Set
        End Property

        Public Overloads Overrides Function ToString() As String
            Return String.Format("{0} with {1}", m_name, m_points)
        End Function
    End Class
End Namespace
