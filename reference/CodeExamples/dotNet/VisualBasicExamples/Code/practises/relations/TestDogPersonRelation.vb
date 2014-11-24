Imports System.Linq
Imports NUnit.Framework

Namespace Db4oDoc.Code.Practises.Relations
    Public Class TestDogPersonRelation
        Private Shared ReadOnly USA As New Country("USA")

        <Test()> _
        Public Sub AddDogToPerson()
            Dim p = New Person("test", "test", USA)
            Dim dog = New Dog()
            p.AddOwnerShipOf(dog)

            assertIsConsistent(p, dog)
        End Sub

        <Test()> _
        Public Sub SetOwner()
            Dim p = New Person("test", "test", USA)
            Dim dog = New Dog()
            dog.Owner = p

            assertIsConsistent(p, dog)
        End Sub

        <Test()> _
        Public Sub SettingOwnerRemovesOldOwner()
            Dim dog = New Dog()
            Dim oldOwner = New Person("old-owner", "old-owner", USA)
            dog.Owner = oldOwner
            Dim p = New Person("test", "test", USA)
            dog.Owner = p

            assertIsConsistent(p, dog)
            Assert.AreEqual(0, oldOwner.OwnedDogs.Count())
        End Sub

        <Test()> _
        Public Sub RemoveOwner()
            Dim dog = New Dog()
            Dim oldOwner = New Person("old-owner", "old-owner", USA)
            dog.Owner = oldOwner
            dog.Owner = Nothing

            Assert.IsNull(dog.Owner)
            Assert.AreEqual(0, oldOwner.OwnedDogs.Count())
        End Sub

        <Test()> _
        Public Sub RemoveOwnedDog()
            Dim dog = New Dog()
            Dim oldOwner = New Person("old-owner", "old-owner", USA)
            dog.Owner = (oldOwner)
            oldOwner.RemoveOwnerShipOf(dog)

            Assert.IsNull(dog.Owner)
            Assert.AreEqual(0, oldOwner.OwnedDogs.Count())
        End Sub

        <Test()> _
        Public Sub ChangeOwnerShip()
            Dim dog = New Dog()
            Dim oldOwner = New Person("old-owner", "old-owner", USA)
            oldOwner.AddOwnerShipOf(dog)
            Dim p = New Person("test", "test", USA)
            p.AddOwnerShipOf(dog)

            assertIsConsistent(p, dog)
            Assert.AreEqual(0, oldOwner.OwnedDogs.Count())
        End Sub


        Private Sub assertIsConsistent(p As Person, dog As Dog)
            Assert.AreSame(p, dog.Owner)
            Assert.IsTrue(p.OwnedDogs.Contains(dog))
        End Sub
    End Class
End Namespace
