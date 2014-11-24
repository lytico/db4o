Imports Db4objects.Db4o.Activation
Imports Db4objects.Db4o.Collections

Namespace Db4oDoc.Code.TA.Collections


    ' #example: Using the activation aware collections
    Public Class Team
        Inherits ActivatableBase
        Private ReadOnly m_pilots As IList(Of Pilot) = New ActivatableList(Of Pilot)()

        Public Sub Add(ByVal pilot As Pilot)
            Activate(ActivationPurpose.Write)
            m_pilots.Add(pilot)
        End Sub

        Public ReadOnly Property Pilots() As ICollection(Of Pilot)
            Get
                Activate(ActivationPurpose.Read)
                Return m_pilots
            End Get
        End Property
    End Class
    ' #end example
End Namespace