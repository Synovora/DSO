Public Class Fonction

    Property Id As Long
    Property Designation As String
    Property Libelle As String
    Property Type As String
    Property RorId As Long
    Property IsInactif As Boolean

    Public Shared Function GetQueryInForIds(lstOrigine As List(Of Fonction)) As String
        Dim lstId As List(Of Long) = New List(Of Long)
        If lstOrigine Is Nothing Then Return ""
        For Each bean In lstOrigine
            lstId.Add(bean.Id)
        Next
        Return " in ( " + String.Join(",", lstId) + ") "
    End Function

End Class
