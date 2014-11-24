Namespace Db4oDoc.Code.Strategies.StoringStatic
    ' #example: Class as enumeration
    Public NotInheritable Class Color
        Public Shared ReadOnly Black As New Color(0, 0, 0)
        Public Shared ReadOnly White As New Color(255, 255, 255)
        Public Shared ReadOnly Red As New Color(255, 0, 0)
        Public Shared ReadOnly Green As New Color(0, 255, 0)
        Public Shared ReadOnly Blue As New Color(0, 0, 255)

        Private ReadOnly m_red As Integer
        Private ReadOnly m_green As Integer
        Private ReadOnly m_blue As Integer

        Private Sub New(ByVal red As Integer, ByVal green As Integer, ByVal blue As Integer)
            Me.m_red = red
            Me.m_green = green
            Me.m_blue = blue
        End Sub

        Public ReadOnly Property RedValue() As Integer
            Get
                Return m_red
            End Get
        End Property

        Public ReadOnly Property GreenValue() As Integer
            Get
                Return m_green
            End Get
        End Property

        Public ReadOnly Property BlueValue() As Integer
            Get
                Return m_blue
            End Get
        End Property

        Public Overloads Function Equals(ByVal other As Color) As Boolean
            If ReferenceEquals(Nothing, other) Then Return False
            If ReferenceEquals(Me, other) Then Return True
            Return other.m_red = m_red AndAlso other.m_green = m_green AndAlso other.m_blue = m_blue

        End Function

        Public Overloads Overrides Function Equals(ByVal obj As Object) As Boolean
            If ReferenceEquals(Nothing, obj) Then Return False
            If ReferenceEquals(Me, obj) Then Return True
            If Not Equals(obj.GetType(), GetType(Color)) Then Return False
            Return Equals(DirectCast(obj, Color))

        End Function

        Public Overrides Function GetHashCode() As Integer
            Dim hashCode As Integer = m_red
            hashCode = (hashCode * 397) Xor m_green
            hashCode = (hashCode * 397) Xor m_blue
            Return hashCode
        End Function

        Public Overrides Function ToString() As String
            Return String.Format("Red: {0}, Green: {1}, Blue: {2}", m_red, m_green, m_blue)
        End Function
    End Class
    ' #end example
End Namespace