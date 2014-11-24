

Namespace Db4oDoc.Code.Query.Soda
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

    Class Author
        Private name As String

        Public Sub New(ByVal name As String)
            Me.name = name
        End Sub
    End Class

    Friend Class BlogPost
        Private m_title As String
        Private m_content As String
        Private ReadOnly m_tags As IList(Of String) = New List(Of String)()
        Private ReadOnly authors As IList(Of Author) = New List(Of Author)()
        Private ReadOnly m_metaData As IDictionary(Of String, Object) = New Dictionary(Of String, Object)()

        Public Sub New(ByVal title As String, ByVal content As String)
            Me.m_title = title
            Me.m_content = content
        End Sub

        Public Property Title() As String
            Get
                Return m_title
            End Get
            Set(ByVal value As String)
                m_title = value
            End Set
        End Property

        Public Property Content() As String
            Get
                Return m_content
            End Get
            Set(ByVal value As String)
                m_content = value
            End Set
        End Property

        Public ReadOnly Property Tags() As IList(Of String)
            Get
                Return m_tags
            End Get
        End Property

        Public ReadOnly Property MetaData() As IDictionary(Of String, Object)
            Get
                Return m_metaData
            End Get
        End Property

        Public Sub AddTags(ByVal ParamArray tags As String())
            For Each tag As String In tags
                Me.m_tags.Add(tag)
            Next
        End Sub
        Public Sub AddAuthors(ByVal ParamArray authors As Author())
            For Each author As Author In authors
                Me.authors.Add(author)
            Next
        End Sub


        Public Sub AddMetaData(ByVal key As String, ByVal value As Object)
            m_metaData.Add(key, value)
        End Sub
    End Class
End Namespace
