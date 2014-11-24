Imports System.Collections.Generic
Imports System.IO
Imports Db4objects.Db4o
Imports Db4objects.Db4o.Config

#Region "LINQ-Imports"
'#example: Use the LINQ namespace
Imports System.Linq
Imports Db4objects.Db4o.Linq
' #end example
#End Region

Namespace Db4oDoc.Code.Query.Linq
    Public Class LinqExamples
        Private Const DatabaseFileName As String = "database.db4o"

        Public Shared Sub Main(ByVal args As String())
            CleanUp()
            PrepareData()
            Console.Out.WriteLine("SimpleQuery:---")
            SimpleQuery()
            Console.Out.WriteLine("QueryForName:---")
            QueryForName()
            Console.Out.WriteLine("QueryWithConstrain:---")
            QueryWithConstrain()
            Console.Out.WriteLine("QueryWithSortingConstrain:---")
            QueryWithSortingConstrain()
            Console.Out.WriteLine("AsQueriable:---")
            AsQueryable()
            Console.Out.WriteLine("OptimizableQuery:---")
            OptimizableQuery()
            Console.Out.WriteLine("UnOptimizableQuery:---")
            UnOptimizableQuery()
            Console.Out.WriteLine("MixingOptimizing:---")
            MixingOptimizing()

            Console.Read()
        End Sub

        Private Shared Sub SimpleQuery()
            Using container = OpenDB()
                ' #example: Simple query
                Dim allPersons = From p In container _
                 Select p
                ' #end example
                ListAll(allPersons)
            End Using
        End Sub

        Private Shared Sub QueryForName()
            Using container = OpenDB()
                ' #example: Query for name
                Dim allPersons = From p As Person In container _
                 Where p.Name.Equals("Joe") _
                 Select p
                ' #end example
                ListAll(allPersons)
            End Using
        End Sub

        Private Shared Sub QueryWithConstrain()
            Using container = OpenDB()
                ' #example: Query with a constraint
                Dim allPersons = From p As Person In container _
                 Where p.Age > 21 _
                 Select p
                ' #end example
                ListAll(allPersons)
            End Using
        End Sub

        Private Shared Sub QueryWithSortingConstrain()
            Using container = OpenDB()
                ' #example: Use sorting on the query
                Dim allPersons = From p As Person In container _
                 Where p.Age > 21 _
                 Order By p.Name _
                 Select p
                ' #end example
                ListAll(allPersons)
            End Using
        End Sub

        Private Shared Sub AsQueryable()
            Using container = OpenDB()
                ' #example: Get a IQueryable-instance
                Dim personQuerable As IQueryable(Of Person) = container.AsQueryable(Of Person)()
                Dim adults = From p In personQuerable _
                 Where p.Age > 18 _
                 Order By p.Name _
                 Select p
                ' #end example
                ListAll(adults)
            End Using
        End Sub

        Private Shared Sub OptimizableQuery()
            Using container = OpenDB()
                ' #example: A query which is optimizable
                Dim adults = From p As Person In container _
                 Where p.Age > 18 AndAlso p.Age < 70 _
                 Order By p.Name _
                 Select p
                ' #end example
                ListAll(adults)
            End Using
        End Sub

        Private Shared Sub UnOptimizableQuery()
            Using container = OpenDB()
                ' #example: Unoptimizable query, because of the 'operations' withing the query
                Dim adults = From p As Person In container _
                 Where p.Name.ToLowerInvariant().Equals("joe") _
                 Select p
                ' #end example
                ListAll(adults)
            End Using
        End Sub

        Private Shared Sub MixingOptimizing()
            Using container = OpenDB()
                ' #example: Unoptimizable query
                Dim adults = From p As Person In container _
                 Where p.Age > 18 AndAlso p.Age < 70 AndAlso p.Name.Substring(2).Contains("n") _
                 Select p
                ' #end example
                ListAll(adults)

                ' #example: Splitting into two parts
                Dim optimizedPart = From p As Person In container _
                 Where p.Age > 18 AndAlso p.Age < 70 _
                 Select p
                Dim endResult = From p In optimizedPart.AsEnumerable() _
                 Where p.Name.Substring(2).Contains("n") _
                 Select p
                ' #end example
                ListAll(endResult)
            End Using
        End Sub

        Private Shared Sub PrepareData()
            Using container = OpenDB()
                container.Store(New Person("Joanna", 34))
                container.Store(New Person("Joe", 42))
                container.Store(New Person("Julia", 62))
                container.Store(New Person("John", 23))
                container.Store(New Person("Jonathan", 19))
                container.Store(New Person("Amelia", 38))
                container.Store(New Person("Amanda", 17))
            End Using
        End Sub

        Private Shared Sub ListAll(Of T)(ByVal printOut As IEnumerable(Of T))
            For Each item As T In printOut
                Console.Out.WriteLine(item)
            Next
        End Sub


        Private Shared Function OpenDB() As IObjectContainer
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            Return Db4oEmbedded.OpenFile(configuration, DatabaseFileName)
        End Function

        Private Shared Sub CleanUp()
            File.Delete(DatabaseFileName)
        End Sub
    End Class


    Friend Class Person
        Private m_name As String
        Private m_age As Integer

        Public Sub New()
            m_name = ""
            m_age = 42
        End Sub

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

End Namespace