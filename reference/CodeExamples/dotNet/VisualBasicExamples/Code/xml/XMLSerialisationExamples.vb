Imports System.IO
Imports System.Linq
Imports System.Xml.Serialization
Imports Db4objects.Db4o
Imports Db4objects.Db4o.Linq

Namespace Db4oDoc.Code.xml
    Public Class XMLSerialisationExamples
        Public Shared Sub Main(ByVal args As String())
            File.Delete("database.db4o")
            FillData()
            WriteToXML()
            ReadFromXML()
        End Sub

        Private Shared Sub WriteToXML()
            ' #example: Serialize to XML
            Using container As IObjectContainer = Db4oEmbedded.OpenFile("database.db4o")
                Dim pilotSerializer As New XmlSerializer(GetType(Pilot()))
                Using file As New FileStream("pilots.xml", FileMode.CreateNew)
                    Dim pilots = (From c In container Select c).ToArray()
                    pilotSerializer.Serialize(file, pilots)
                End Using
            End Using
            ' #end example
        End Sub

        Private Shared Sub ReadFromXML()
            ' #example: Read objects from XML
            Using container As IObjectContainer = Db4oEmbedded.OpenFile("database.db4o")
                Dim pilotSerializer As New XmlSerializer(GetType(Pilot()))
                Using file As New FileStream("pilots.xml", FileMode.Open)
                    Dim pilots As Pilot() = DirectCast(pilotSerializer.Deserialize(file), Pilot())
                    For Each pilot As Pilot In pilots
                        container.Store(pilot)
                    Next
                End Using
            End Using
            ' #end example
        End Sub

        Private Shared Sub FillData()
            Using container As IObjectContainer = Db4oEmbedded.OpenFile("database.db4o")
                container.Store(New Pilot("Joe", 42))
                container.Store(New Pilot("Joanna", 24))
            End Using
        End Sub

        Public Class Pilot
            Private m_name As String
            Private m_point As Integer

            Public Sub New()
            End Sub

            Public Sub New(ByVal name As String, ByVal point As Integer)
                Me.m_name = name
                Me.m_point = point
            End Sub

            Public Property Name() As String
                Get
                    Return m_name
                End Get
                Set(ByVal value As String)
                    m_name = value
                End Set
            End Property

            Public Property Point() As Integer
                Get
                    Return m_point
                End Get
                Set(ByVal value As Integer)
                    m_point = value
                End Set
            End Property
        End Class
    End Class
End Namespace
