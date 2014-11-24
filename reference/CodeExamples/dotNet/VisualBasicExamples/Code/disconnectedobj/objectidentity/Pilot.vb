Imports System

Namespace Db4oDoc.Code.DisconnectedObj.ObjectIdentity
    Public Class Pilot
        Private m_name As String

        Public Sub New(ByVal name As String)
            Me.m_name = name
        End Sub

        Public Property Name() As String
            Get
                Return m_name
            End Get
            Set(ByVal value As String)
                m_name = value
            End Set
        End Property

        Public Overloads Overrides Function ToString() As String
            Return m_name
        End Function
    End Class

End Namespace
