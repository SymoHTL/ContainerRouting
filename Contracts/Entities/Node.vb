Namespace Entities
    Public Class Node
        Public Property CurrentCity As String
        Public Property NextCity As String
        Public Property EndCity As String


        Public Sub New(currentCity As String, nextCity As String, endCity As String)
            Me.CurrentCity = currentCity
            Me.NextCity = nextCity
            Me.EndCity = endCity
        End Sub


        Public Overrides Function ToString() As String
            Return  $"{CurrentCity} -> {NextCity} -> {EndCity}"
        End Function
    End Class
End NameSpace