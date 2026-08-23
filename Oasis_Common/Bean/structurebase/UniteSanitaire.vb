Public Class UniteSanitaire

    Property Oa_unite_sanitaire_id As Integer
    Property Oa_unite_sanitaire_description As String
    Property Oa_unite_sanitaire_siege_id As Integer
    Property Oa_unite_sanitaire_adresse1 As String
    Property Oa_unite_sanitaire_adresse2 As String
    Property Oa_unite_sanitaire_ville As String
    Property Oa_unite_sanitaire_code_postal As String
    Property Oa_unite_sanitaire_inactif As Boolean
    Property Telephone As String
    Property Mail As String
    Property Fax As String
    Property NumeroStructure As Long
    Property LstSite As List(Of Site)

    Public Sub AddSite(site As Site)
        If _LstSite Is Nothing Then
            _LstSite = New List(Of Site)
        End If
        _LstSite.Add(site)
    End Sub

    Public Shared Function GetQueryInForIds(lstUS As List(Of UniteSanitaire)) As String
        Dim lstId As List(Of Long) = New List(Of Long)
        If lstUS Is Nothing Then Return ""
        For Each uniteSanitaire In lstUS
            lstId.Add(uniteSanitaire.Oa_unite_sanitaire_id)
        Next
        Return " in ( " + String.Join(",", lstId) + ") "
    End Function

    Public Function Clone() As Object
        Dim newInstance As UniteSanitaire = DirectCast(Me.MemberwiseClone(), UniteSanitaire)
        Return newInstance
    End Function

End Class
