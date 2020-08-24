﻿Imports System
Imports System.Collections.Generic
Imports System.ComponentModel.DataAnnotations
Imports System.Linq
Imports System.Web

Namespace Models
    Public Class UserLogin
        <Display(Name:="Username")>
        <Required(AllowEmptyStrings:=False, ErrorMessage:="Username required")>
        Public Property Username As String
        <DataType(DataType.Password)>
        <Required(AllowEmptyStrings:=False, ErrorMessage:="Password required")>
        Public Property Password As String
        <Display(Name:="Remember Me")>
        Public Property RememberMe As Boolean
    End Class
End Namespace