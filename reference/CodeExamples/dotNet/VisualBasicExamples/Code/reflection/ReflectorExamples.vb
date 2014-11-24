Imports Db4objects.Db4o
Imports Db4objects.Db4o.Config
Imports Db4objects.Db4o.Reflect
Imports Db4objects.Db4o.Reflect.Net

Namespace Db4oDoc.Code.Reflection
    Public Class ReflectorExamples
        Public Shared Sub Main(ByVal args As String())
            UseLoggerReflector()
        End Sub

        Private Shared Sub UseLoggerReflector()
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            configuration.Common.ReflectWith(New LoggerReflector())
            Using container As IObjectContainer = Db4oEmbedded.OpenFile("database.db4o")
            End Using
        End Sub
    End Class

    ' #example: Logging reflector
    Friend Class LoggerReflector
        Implements IReflector
        Private ReadOnly readReflector As IReflector

        Public Sub New()
            Me.readReflector = New NetReflector()
        End Sub

        Public Sub New(ByVal readReflector As IReflector) 
            Me.readReflector = readReflector
        End Sub

        Public Sub Configuration(ByVal reflectorConfiguration As IReflectorConfiguration) _
            Implements IReflector.Configuration
            readReflector.Configuration(reflectorConfiguration)
        End Sub

        Public Function Array() As IReflectArray _
            Implements IReflector.Array
            Return readReflector.Array()
        End Function

        Public Function ForClass(ByVal type As Type) As IReflectClass _
            Implements IReflector.ForClass
            Console.WriteLine("Reflector.forClass({0})", type)
            Return readReflector.ForClass(type)
        End Function

        Public Function ForName(ByVal className As String) As IReflectClass _
            Implements IReflector.ForName
            Console.WriteLine("Reflector.forName({0})", className)
            Return readReflector.ForName(className)
        End Function

        Public Function ForObject(ByVal o As Object) As IReflectClass _
            Implements IReflector.ForObject
            Console.WriteLine("Reflector.forObject(" & Convert.ToString(o) & ")")
            Return readReflector.ForObject(o)
        End Function

        Public Function IsCollection(ByVal reflectClass As IReflectClass) As Boolean _
            Implements IReflector.IsCollection
            Return readReflector.IsCollection(reflectClass)
        End Function

        Public Sub SetParent(ByVal reflector As IReflector) _
            Implements IReflector.SetParent
            readReflector.SetParent(reflector)
        End Sub

        Public Function DeepClone(ByVal o As Object) As Object _
            Implements IReflector.DeepClone
            Return New LoggerReflector(DirectCast(readReflector.DeepClone(o), IReflector))
        End Function
    End Class
    ' #end example
End Namespace
