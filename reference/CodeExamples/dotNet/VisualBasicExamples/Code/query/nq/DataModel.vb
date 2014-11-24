
Namespace Db4oDoc.Code.Query.NativeQueries

    Friend Class Pilot
        Private m_name As String
        Private m_age As Integer

        Public Sub New(ByVal name As String, ByVal age As Integer)
            Me.m_name = name
            Me.m_age = age
        End Sub

        Public Property Name() As String
            Get
                Return m_name
            End Get
            Set(ByVal value As String)
                m_name = value
            End Set
        End Property

        Public Property Age() As Integer
            Get
                Return m_age
            End Get
            Set(ByVal value As Integer)
                m_age = value
            End Set
        End Property

        Public Overrides Function ToString() As String
            Return String.Format("Name: {0}, Age: {1}", m_name, m_age)
        End Function
    End Class

    Friend Class Car
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

        Public Overrides Function ToString() As String
            Return String.Format("Pilot: {0}, Name: {1}", m_pilot, m_name)
        End Function
    End Class
End Namespace