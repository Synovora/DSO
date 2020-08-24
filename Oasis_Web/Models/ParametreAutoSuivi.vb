Imports System
Imports System.Collections.Generic
Imports System.ComponentModel.DataAnnotations
Imports System.Linq
Imports System.Web
Imports Oasis_Common

Namespace Models
    Public Class ParametreAutoSuivi

        Public Property Id As Integer
        Public Property Parametres As List(Of Parametre)

    End Class
End Namespace