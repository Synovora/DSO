Public Class Site

    Public Property Oa_site_id As Long
    Public Property Oa_site_description As String
    Public Property Oa_site_territoire_id As Object
    Public Property Oa_site_unite_sanitaire_id As Long
    Public Property Oa_site_adresse1 As String
    Public Property Oa_site_adresse2 As String
    Public Property Oa_site_ville As String
    Public Property Oa_site_code_postal As String
    Public Property Oa_site_inactif As Boolean
    Public Property Telephone As String
    Public Property Mail As String
    Public Property Fax As String

    ''' <summary>
    ''' return in "(id1,id2, ... , idn)"
    ''' </summary>
    ''' <param name="lstOrigine"></param>
    ''' <returns></returns>
    Public Shared Function getQueryInForIds(lstOrigine As List(Of Site)) As String
        Dim lstId As List(Of Long) = New List(Of Long)

        If lstOrigine Is Nothing Then Return ""
        For Each sitelu In lstOrigine
            lstId.Add(sitelu.Oa_site_id)
        Next

        Return " in ( " + String.Join(",", lstId) + ") "

    End Function

End Class
