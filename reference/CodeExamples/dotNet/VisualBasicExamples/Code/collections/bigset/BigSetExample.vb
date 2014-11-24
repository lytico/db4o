Imports System
Imports System.Collections.Generic
Imports System.IO
Imports Db4objects.Db4o
Imports Db4objects.Db4o.Collections

Namespace Db4oDoc.Code.Collections.BigSet
    Public Class BigSetExample
        Private Const DatabaseFileName As String = "database.db4o"
        Private Const PopulationSize As Integer = 10000

        Public Shared Sub Main(ByVal args As String())
            CleanUp()

            StoreBigSet()
            CheckInBigSet()
            BigSetIsByIdentity()

            CleanUp()
        End Sub


        Private Shared Sub StoreBigSet()
            Using container As IObjectContainer = OpenDatabase()
                Dim city As City = CreateCity(container)
                container.Store(city)
                StoreOtherPeople(container)
                container.Commit()
            End Using
        End Sub

        Private Shared Sub CheckInBigSet()
            Using container As IObjectContainer = OpenDatabase()
                Dim city As City = container.Query(Of City)()(0)
                Console.WriteLine("City's population " & city.Population)

                CheckAFewPersons(container, city)
            End Using
        End Sub

        Private Shared Sub BigSetIsByIdentity()
            Using container As IObjectContainer = OpenDatabase()
                Dim city As City = container.Query(Of City)()(0)

                ' #example: Note that the big-set compares by identity, not by equality
                Dim aCitizen As Person
                Using aCitizenEnumerator As IEnumerator(Of Person) = city.Citizen.GetEnumerator()
                    aCitizenEnumerator.MoveNext()
                    aCitizen = aCitizenEnumerator.Current
                End Using
                Console.WriteLine("The big-set uses the identity, not equality of an object")
                Console.WriteLine("Therefore it .contains() on the same person-object is " & city.Citizen.Contains(aCitizen))
                Dim equalPerson As New Person(aCitizen.Name)
                ' #end example
                Console.WriteLine("Therefore it .contains() on a equal person-object is " & city.Citizen.Contains(equalPerson))
            End Using
        End Sub

        Private Shared Function CreateCity(ByVal container As IObjectContainer) As City
            ' #example: Crate a big-set instance
            Dim citizen As ICollection(Of Person) = _
                CollectionFactory.ForObjectContainer(container).NewBigSet(Of Person)()
            ' now you can use the big-set like a normal set:
            citizen.Add(New Person("Citizen Kane"))
            ' #end example
            For i As Integer = 0 To PopulationSize - 1
                citizen.Add(New Person("Citizen No " & i))
            Next
            Return New City(citizen)
        End Function

        Private Shared Sub StoreOtherPeople(ByVal container As IObjectContainer)
            For i As Integer = 0 To PopulationSize - 1
                container.Store(New Person("Citizen No " & i))
            Next
        End Sub

        Private Shared Sub CheckAFewPersons(ByVal container As IObjectContainer, ByVal city As City)
            Dim random As New Random()
            Dim persons As IList(Of Person) = container.Query(Of Person)()
            Dim personCount As Integer = persons.Count
            For i As Integer = 0 To 9
                Dim aPerson As Person = persons(random.Next(personCount))
                PrintCitizenStatus(city, aPerson)
            Next
        End Sub

        Private Shared Sub PrintCitizenStatus(ByVal city As City, ByVal aPerson As Person)
            If city.IsCitizen(aPerson) Then
                Console.WriteLine(aPerson.ToString() & " is a citizen")
            Else
                Console.WriteLine(aPerson.ToString() & " isn't a citizen")
            End If
        End Sub


        Private Shared Function OpenDatabase() As IObjectContainer
            Return Db4oEmbedded.OpenFile(DatabaseFileName)
        End Function

        Private Shared Sub CleanUp()
            File.Delete(DatabaseFileName)
        End Sub
    End Class

    Friend Class City
        Private ReadOnly m_citizen As ICollection(Of Person)

        Public Sub New(ByVal citizen As ICollection(Of Person))
            Me.m_citizen = citizen
        End Sub

        Public Function IsCitizen(ByVal person As Person) As Boolean
            Return m_citizen.Contains(person)
        End Function

        Public ReadOnly Property Citizen() As ICollection(Of Person)
            Get
                Return m_citizen
            End Get
        End Property

        Public ReadOnly Property Population() As Integer
            Get
                Return m_citizen.Count
            End Get
        End Property
    End Class

    Friend Class Person
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

        Public Overloads Function Equals (ByVal other As Person) as Boolean
            If ReferenceEquals (Nothing, other) Then Return False
            if ReferenceEquals (Me, other) Then Return True
            Return Equals (other.m_name, m_name)

        End Function

        Public Overloads Overrides Function Equals (ByVal obj As Object) as Boolean
            If ReferenceEquals (Nothing, obj) Then Return False
            if ReferenceEquals (Me, obj) Then Return True
            If Not Equals (obj.GetType(), GetType (Person)) Then Return False
            Return Equals (DirectCast (obj, Person))

        End Function

        Public Overrides Function GetHashCode() as Integer
            If m_name IsNot Nothing Then Return m_name.GetHashCode()
            Return 0
        End Function

        Public Overloads Overrides Function ToString() As String
            Return String.Format("Name: {0}", m_name)
        End Function
    End Class
End Namespace
