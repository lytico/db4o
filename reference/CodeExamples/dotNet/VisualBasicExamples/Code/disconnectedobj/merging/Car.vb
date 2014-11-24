Namespace Db4oDoc.Code.DisconnectedObj.Merging
    Public Class Car
        Inherits IDHolder
        Private m_pilot As Pilot
        Private m_name As String

        Public Sub New(ByVal pilot As Pilot, ByVal name As String)
            Me.m_pilot = pilot
            Me.m_name = name
        End Sub

        Public Property Pilot() As Pilot
            Get
                Return m_pilot
            End Get
            Set(ByVal value As Pilot)
                m_pilot = value
            End Set
        End Property

        Public Property Name() As String
            Get
                Return m_name
            End Get
            Set(ByVal value As String)
                m_name = value
            End Set
        End Property


        Public Overloads Overrides Function ToString() As String
            Return String.Format("{0} pilot: {1}", m_name, m_pilot)
        End Function
    End Class
End Namespace
