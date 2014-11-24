Imports System.Text

Namespace Db4oDoc.Code.Tuning.Monitoring
    Class DataObject
        Private label As String

        Friend Sub New(ByVal rnd As Random)
            Me.label = NewString(rnd)
        End Sub

        Private Shared Function NewString(ByVal rnd As Random) As String
            Dim buffer As New StringBuilder()
            For i As Integer = 0 To rnd.Next(4096) - 1
                Dim charNr As Integer = 65 + rnd.Next(26)
                buffer.Append(ChrW(charNr))
            Next
            Return buffer.ToString()
        End Function
    End Class
End Namespace