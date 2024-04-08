Namespace Entities

    Public Class Container
        Public Property Id As Integer
        Public Property Node As Node

        Public Overrides Function ToString() As String
            Return $"Id: {Id}, Node: {Node}"
        End Function
    End Class
End NameSpace