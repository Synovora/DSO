Imports System
Imports System.Collections.Generic
Imports System.ComponentModel.DataAnnotations
Imports System.Linq
Imports System.Web

Namespace Models
    Public Class UserRecover
        <DataType(DataType.Password)>
        <Required(AllowEmptyStrings:=False, ErrorMessage:="Password required")>
        Public Property Password As String
        <DataType(DataType.Password)>
        <Required(AllowEmptyStrings:=False, ErrorMessage:="Password confirmation required")>
        Public Property PasswordBis As String
        Public Property Recovery As String
        Public Property Code As String
    End Class
End Namespace