Imports System.Collections.Generic
Imports Db4objects.Db4o

Namespace Db4oDoc.Code.Strategies.Refactoring.RemoveHierarchy
    Public Class RemoveClassFromHierarchy
        Private Const DatabaseFileName As String = "database.db4o"

        Public Shared Sub Main(ByVal args As String())
            StoreOldObjectLayout()
            ListItems()
            Console.WriteLine("--After refactoring--")
            CopyToNewType()
            ListItems()
        End Sub


        Private Shared Sub CopyToNewType()
            Using container As IObjectContainer = Db4oEmbedded.OpenFile(DatabaseFileName)
                ' #example: copy the data from the old type to the new one
                Dim allMammals As IList(Of Human) = container.Query(Of Human)()
                For Each oldHuman As Human In allMammals
                    Dim newHuman As New HumanNew("")
                    newHuman.BodyTemperature = oldHuman.BodyTemperature
                    newHuman.IQ = oldHuman.IQ
                    newHuman.Name = oldHuman.Name

                    container.Store(newHuman)
                    container.Delete(oldHuman)
                    ' #end example
                Next
            End Using
        End Sub


        Private Shared Sub ListItems()
            Using container As IObjectContainer = Db4oEmbedded.OpenFile(DatabaseFileName)
                Dim allMammals As IList(Of Mammal) = container.Query(Of Mammal)()
                For Each mammal As Mammal In allMammals
                    Console.WriteLine(mammal)
                Next
            End Using
        End Sub

        Private Shared Sub StoreOldObjectLayout()
            Using container As IObjectContainer = Db4oEmbedded.OpenFile(DatabaseFileName)
                container.Store(New Human("Joe"))
                container.Store(New Human("Joey"))
            End Using
        End Sub
    End Class


    Friend Class Mammal
        Private m_bodyTemperature As Integer

        Public Property BodyTemperature() As Integer
            Get
                Return m_bodyTemperature
            End Get
            Set(ByVal value As Integer)
                m_bodyTemperature = value
            End Set
        End Property


        Public Overrides Function ToString() As String
            Return "Mammal{" & "bodyTemperature=" & m_bodyTemperature & "}"c
        End Function
    End Class

    Friend Class Primate
        Inherits Mammal
        Private m_iq As Integer

        Public Property IQ() As Integer
            Get
                Return m_iq
            End Get
            Set(ByVal value As Integer)
                m_iq = value
            End Set
        End Property

        Public Overrides Function ToString() As String
            Return "Primate{" & "iq=" & m_iq & "} is a " & MyBase.ToString()
        End Function
    End Class

    Friend Class HumanNew
        Inherits Mammal
        Private m_name As String
        Private m_iq As Integer

        Public Sub New(ByVal name As String)
            Me.m_name = name
            BodyTemperature = 36
            IQ = 120
        End Sub

        Public Property Name() As String
            Get
                Return m_name
            End Get
            Set(ByVal value As String)
                m_name = value
            End Set
        End Property

        Public Property IQ() As Integer
            Get
                Return m_iq
            End Get
            Set(ByVal value As Integer)
                m_iq = value
            End Set
        End Property


        Public Overrides Function ToString() As String
            Return "Human{" & "name='" & m_name & "'"c & "} is a " & MyBase.ToString()
        End Function
    End Class

    Friend Class Human
        Inherits Primate
        Private m_name As String


        Public Sub New(ByVal name As String)
            Me.m_name = name
            BodyTemperature = 36
            IQ = 120
        End Sub

        Public Property Name() As String
            Get
                Return m_name
            End Get
            Set(ByVal value As String)
                m_name = value
            End Set
        End Property

        Public Overrides Function ToString() As String
            Return "Human{" & "name='" & m_name & "'"c & "} is a " & MyBase.ToString()
        End Function
    End Class
End Namespace