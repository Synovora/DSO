Imports Oasis_WF

Public Class FiltreTache

    Private _lstUniteSanitaire As New List(Of UniteSanitaire)

    Public Property LstUniteSanitaire As List(Of UniteSanitaire)
        Get
            Return _lstUniteSanitaire
        End Get
        Set(value As List(Of UniteSanitaire))
            _lstUniteSanitaire = value
        End Set
    End Property

    Public Sub addSiteToUniteSanitaire(us As UniteSanitaire, site As Site)
        us.AddSite(site)
    End Sub

    Public Sub addUniteSanitaire(unite_s As UniteSanitaire)
        LstUniteSanitaire.Add(unite_s)
    End Sub

    ''' <summary>
    ''' Résumé du filtre
    ''' </summary>
    ''' <returns></returns>
    Public Function resumeFiltre() As String
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

    ''' <summary>
    ''' retourne une liste plate de tous les sites selectionnés
    ''' </summary>
    ''' <returns></returns>
    Public Function getListAllSite() As List(Of Site)
        Dim lstAllSite As List(Of Site) = New List(Of Site)
        For Each us In LstUniteSanitaire
            For Each sitelu In us.LstSite
                lstAllSite.Add(sitelu)
            Next
        Next
        Return lstAllSite
    End Function

End Class
