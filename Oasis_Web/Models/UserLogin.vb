Imports System
Imports System.Collections.Generic
Imports System.ComponentModel.DataAnnotations
Imports System.Linq
Imports System.Web

Namespace Models
    Public Class UserLogin
        <Display(Name:="Email")>
        <Required(AllowEmptyStrings:=False, ErrorMessage:="Email required")>
        Public Property Email As String
        <DataType(DataType.Password)>
        <Required(AllowEmptyStrings:=False, ErrorMessage:="Password required")>
        Public Property Password As String
        <Display(Name:="Prenom")>
        <Required(AllowEmptyStrings:=False, ErrorMessage:="Prenom required")>
        Public Property Prenom As String

        <Display(Name:="Nom")>
        <Required(AllowEmptyStrings:=False, ErrorMessage:="Nom required")>
        Public Property Nom As String

        <Display(Name:="NIR")>
        <Required(AllowEmptyStrings:=False, ErrorMessage:="NIR required")>
        Public Property NIR As String
        <Display(Name:="Remember Me")>
        Public Property RememberMe As Boolean
    End Class
End Namespace