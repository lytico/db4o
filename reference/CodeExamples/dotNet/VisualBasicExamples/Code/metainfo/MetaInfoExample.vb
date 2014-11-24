Imports System.Collections.Generic
Imports Db4objects.Db4o
Imports Db4objects.Db4o.Ext

Namespace Db4oDoc.Code.MetaInfo
    Public Class MetaInfoExample
        Public Shared Sub Main(ByVal args As String())
            Using container As IObjectContainer = Db4oEmbedded.OpenFile("database.db4o")
                container.Store(New Person("Johnson", "Roman", 42))
                container.Store(New Person("Miller", "John", 21))

                ' #example: All stored classes
                ' Get the information about all stored classes.
                Dim classesInDB As IStoredClass() = container.Ext().StoredClasses()
                For Each storedClass As IStoredClass In classesInDB
                    Console.WriteLine(storedClass.GetName())
                Next

                ' Information for a certain class
                Dim metaInfo As IStoredClass = container.Ext().StoredClass(GetType(Person))
                ' #end example

                ' #example: Accessing stored fields
                Dim metaInfoForPerson As IStoredClass = container.Ext().StoredClass(GetType(Person))
                ' Access all existing fields
                For Each field As IStoredField In metaInfoForPerson.GetStoredFields()
                    Console.WriteLine("Field: " & field.GetName())
                Next
                ' Accessing the field 'name' of any type.
                Dim nameField As IStoredField = metaInfoForPerson.StoredField("name", Nothing)
                ' Accessing the string field 'name'. Important if this field had another time in previous
                ' versions of the class model
                Dim ageField As IStoredField = metaInfoForPerson.StoredField("age", GetType(Integer))

                ' Check if the field is indexed
                Dim isAgeFieldIndexed As Boolean = ageField.HasIndex()

                ' Get the type of the field
                Dim fieldType As String = ageField.GetStoredType().GetName()
                ' #end example

                ' #example: Access via meta data
                Dim metaForPerson As IStoredClass = container.Ext().StoredClass(GetType(Person))
                Dim metaNameField As IStoredField = metaForPerson.StoredField("name", Nothing)

                Dim persons As IList(Of Person) = container.Query(Of Person)()
                For Each person As Person In persons
                    Dim name As String = DirectCast(metaNameField.Get(person), String)
                    Console.WriteLine("Name is " & name)
                    ' #end example
                Next
            End Using
        End Sub
    End Class

    Friend Class Person
        Private sirname As String
        Private firstname As String
        Private age As Integer

        Public Sub New(ByVal sirname As String, ByVal firstname As String, ByVal age As Integer)
            Me.sirname = sirname
            Me.firstname = firstname
            Me.age = age
        End Sub
    End Class
End Namespace
