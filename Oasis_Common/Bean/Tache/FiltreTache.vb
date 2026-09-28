Public Class FiltreTache

    Property LstUniteSanitaire As New List(Of UniteSanitaire)

    Public Sub AddSiteToUniteSanitaire(us As UniteSanitaire, site As Site)
        us.AddSite(site)
    End Sub

    Public Sub AddUniteSanitaire(unite_s As UniteSanitaire)
        LstUniteSanitaire.Add(unite_s)
    End Sub

    Public Function ResumeFiltre() As String
        Dim resu As String = "", strSite As String
        Dim us As UniteSanitaire
        Dim firstSite As Boolean
        For Each us In LstUniteSanitaire
            If resu <> "" Then resu += vbCrLf
            resu += us.Oa_unite_sanitaire_description.ToUpper
            firstSite = True
            strSite = " : tous les sites"
            For Each sitelu In us.LstSite
                If firstSite Then
                    strSite = " : "
                Else
                    strSite += ", "
                End If
                firstSite = False
                strSite += sitelu.Oa_site_description
            Next
            resu += strSite
        Next
        Return resu
    End Function

    Public Function GetListAllSite() As List(Of Site)
        Dim lstAllSite As List(Of Site) = New List(Of Site)
        For Each us In LstUniteSanitaire
            For Each sitelu In us.LstSite
                lstAllSite.Add(sitelu)
            Next
        Next
        Return lstAllSite
    End Function

    ''' <summary>
    ''' Clause SQL (colonnes unite_sanitaire_id et site_id) du filtre de structure,
    ''' unité par unité : une unité avec des sites retenus ne garde que ces sites,
    ''' une unité sans site retenu garde tous les siens. Vide si aucune unité.
    ''' </summary>
    Public Function GetClauseSqlUniteSite() As String
        If LstUniteSanitaire.Count = 0 Then Return ""
        Dim conditions As New List(Of String)
        Dim lstUniteSansSite As New List(Of UniteSanitaire)
        For Each us In LstUniteSanitaire
            If us.LstSite Is Nothing OrElse us.LstSite.Count = 0 Then
                lstUniteSansSite.Add(us)
            Else
                conditions.Add("(unite_sanitaire_id = " & us.Oa_unite_sanitaire_id & " AND site_id " & Site.GetQueryInForIds(us.LstSite) & ")")
            End If
        Next
        If lstUniteSansSite.Count > 0 Then
            conditions.Insert(0, "unite_sanitaire_id " & UniteSanitaire.GetQueryInForIds(lstUniteSansSite))
        End If
        Return "AND (" & String.Join(" OR ", conditions) & ")" & vbCrLf
    End Function

    Public Sub Clear()
        LstUniteSanitaire.Clear()
    End Sub

End Class
