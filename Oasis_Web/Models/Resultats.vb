Imports System
Imports System.Collections.Generic
Imports System.ComponentModel.DataAnnotations
Imports System.Linq
Imports System.Web
Imports Oasis_Common

Public Class ResultatsModel
        Public Property SelectedItems As IEnumerable(Of String)
        Public Property SelectedItem As String
        Public Property Items As IEnumerable(Of SelectListItem)
    End Class
