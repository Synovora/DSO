Imports System.Data.SqlClient

Public Class InternauteConnection

    Public Property Id As Long
    Public Property Internaute As Long
    Public Property Datetime As Date
    Public Property Ip As String

    Public Sub New()
    End Sub

    Public Sub New(reader As SqlDataReader)
        Me.Id = reader("id")
        Me.Internaute = reader("internaute")
        Me.Datetime = reader("datetime")
        Me.Ip = reader("ip")
    End Sub

End Class
