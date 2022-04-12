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

    Public Sub Clear()
        LstUniteSanitaire.Clear()
    End Sub

End Class
