Imports System
Imports Db4objects.Db4o
Imports Db4objects.Db4o.Activation
Imports Db4objects.Db4o.TA

Namespace Db4oDoc.Ta.Example
    ' #example: Implement the required activatable interface and add activator
    Public Class Person
        Implements IActivatable
        <Transient()> _
        Private m_activator As IActivator
        ' #end example 

        Private m_name As String
        Private m_mother As Person

        Public Sub New(ByVal name As String)
            Me.m_name = name
        End Sub

        Public Sub New(ByVal name As String, ByVal mother As Person)
            Me.m_name = name
            Me.m_mother = mother
        End Sub

        Public Property Mother() As Person
            Get
                Activate(ActivationPurpose.Read)
                Return m_mother
            End Get
            Set(ByVal value As Person)
                Activate(ActivationPurpose.Write)
                m_mother = value
            End Set
        End Property

        ' #example: Call the activate method on every field access
        Public Property Name() As String
            Get
                Activate(ActivationPurpose.Read)
                Return m_name
            End Get
            Set(ByVal value As String)
                Activate(ActivationPurpose.Write)
                m_name = value
            End Set
        End Property


        Public Overloads Overrides Function ToString() As String
            ' use the getter/setter withing the class,
            ' to ensure the activate-method is called
            Return Name
        End Function
        ' #end example

        ' #end example

        ' #example: Implement the activatable interface methods
        Public Sub Bind(ByVal activator As IActivator) _
                Implements IActivatable.Bind
            If m_activator Is activator Then
                Exit Sub
            End If
            If activator IsNot Nothing AndAlso m_activator IsNot Nothing Then
                Throw New InvalidOperationException("Object can only be bound to one activator")
            End If
            m_activator = activator
        End Sub

        Public Sub Activate(ByVal activationPurpose As ActivationPurpose) _
                Implements IActivatable.Activate
            If m_activator IsNot Nothing Then
                m_activator.Activate(activationPurpose)
            End If
        End Sub

        ' #end example

        Public Shared Function PersonWithHistory() As Person
            Return CreateFamily(10)
        End Function

        Private Shared Function CreateFamily(ByVal generation As Integer) As Person
            If 0 < generation Then
                Dim previousGeneration As Integer = generation - 1
                Return New Person("Joanna the " & generation, CreateFamily(previousGeneration))
            Else
                Return Nothing
            End If
        End Function
    End Class
End Namespace
