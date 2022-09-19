Imports System.Configuration
Imports Telerik.WinControls.UI
Imports Telerik.WinControls.UI.Localization
Imports Oasis_Common

Public Class RadFPPSObsoletes

    Property SelectedPatient As Patient
    Property UtilisateurConnecte As Utilisateur

    Dim Horizon As Integer
    Dim PremierAffichage As Boolean = True


    Private Sub RadFTraitementObsoletes_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        RadGridLocalizationProvider.CurrentProvider = New FrenchRadGridViewLocalizationProvider()
        ChargementParametreApplication()
        DteHorizonAffichage.CustomFormat = "MMMM-yyyy"
        DteHorizonAffichage.Value = Date.Now.AddYears(-1 * Horizon)
        DteHorizonAffichage.MaxDate = Date.Now
        DTPDateFin.CustomFormat = "MMMM-yyyy"
        DTPDateFin.Value = Date.Now
        DTPDateFin.MaxDate = Date.Now
        InitZones()
        ChargementEtatCivil()
        ChargementPPS()
    End Sub


    Private Sub ChargementParametreApplication()
        'Récupération du nom de l'organisation dans les paramètres de l'application
        Dim HorizonString As String = ConfigurationManager.AppSettings("horizonTraitementObsolete")
        If IsNumeric(HorizonString) Then
            Horizon = CInt(HorizonString)
        Else
            Horizon = 3
        End If
    End Sub

    '==========================================================
    '======================= Etat civil =======================
    '==========================================================

    'Chargement des données dans les labels dédiés
    Private Sub ChargementEtatCivil()
        LblPatientNIR.Text = SelectedPatient.PatientNir
        LblPatientPrenom.Text = SelectedPatient.PatientPrenom
        LblPatientNom.Text = SelectedPatient.PatientNom
        LblPatientAge.Text = SelectedPatient.PatientAge
        LblPatientGenre.Text = SelectedPatient.PatientGenre
        LblPatientAdresse1.Text = SelectedPatient.PatientAdresse1
        LblPatientAdresse2.Text = SelectedPatient.PatientAdresse2
        LblPatientCodePostal.Text = SelectedPatient.PatientCodePostal
        LblPatientVille.Text = SelectedPatient.PatientVille
        LblPatientTel1.Text = SelectedPatient.PatientTel1
        LblPatientTel2.Text = SelectedPatient.PatientTel2
        LblPatientSite.Text = Environnement.Table_site.GetSiteDescription(SelectedPatient.PatientSiteId)
        LblPatientUniteSanitaire.Text = Environnement.Table_unite_sanitaire.GetUniteSanitaireDescription(SelectedPatient.PatientUniteSanitaireId)
        LblPatientDateMaj.Text = SelectedPatient.PatientSyntheseDateMaj.ToString("dd/MM/yyyy")

        'Vérification de l'existence d'ALD
        LblALD.Hide()
        Dim StringTooltip As String
        Dim aldDao As New AldDao
        StringTooltip = aldDao.DateFinALD(Me.SelectedPatient.PatientId)
        If StringTooltip <> "" Then
            LblALD.Show()
            ToolTip.SetToolTip(LblALD, StringTooltip)
        End If

    End Sub

    '==========================================================
    '======================= Traitements obsolètes ============
    '==========================================================

    'Chargement de la Grid
    Private Sub ChargementPPS()
        RadPPSDataGridView.Rows.Clear()
        Dim filtreDateDebut As Date
        filtreDateDebut = DteHorizonAffichage.Value

        Dim filtreDateFin As Date
        filtreDateFin = DTPDateFin.Value

        Dim PPSDataTable As DataTable
        Dim PPSDao As PpsDao = New PpsDao
        PPSDataTable = PPSDao.getAllPPSbyPatient(SelectedPatient.PatientId, " And ((oa_pps_date_fin IS NOT NULL) AND (oa_pps_date_fin >= '" & filtreDateDebut.ToString("yyyy-MM-dd") & "' AND oa_pps_date_fin <= '" & filtreDateFin.ToString("yyyy-MM-dd") & "'))")

        'Déclaration des variables pour réaliser le parcours du DataTable pour alimenter le DataGridView
        Dim i, mesureCount As Integer
        Dim iGrid As Integer = -1 'Indice pour alimenter la Grid qui peut comporter moins d'occurrences que le DataTable
        Dim dateDebut, dateModification As Date
        Dim rowCount As Integer = PPSDataTable.Rows.Count - 1
        Dim categoriePPS, sousCategoriePPS, Rythme, SpecialiteId As Integer
        Dim ppsArret As Boolean
        Dim NaturePPS, CommentairePPS, commentaireParcours, AffichePPS, AfficheDateModificationPPS, AfficheDateModificationParcours, Base, BaseItem, SpecialiteDescription As String

        'Parcours du DataTable pour alimenter le DataGridView
        For i = 0 To rowCount Step 1
            categoriePPS = 0
            If PPSDataTable.Rows(i)("oa_r_pps_categorie_id") IsNot DBNull.Value Then
                categoriePPS = PPSDataTable.Rows(i)("oa_r_pps_categorie_id")
            End If

            sousCategoriePPS = 0
            If PPSDataTable.Rows(i)("oa_r_pps_sous_categorie_id") IsNot DBNull.Value Then
                sousCategoriePPS = PPSDataTable.Rows(i)("oa_r_pps_sous_categorie_id")
            End If

            'Date de début
            If PPSDataTable.Rows(i)("oa_pps_date_debut") IsNot DBNull.Value Then
                dateDebut = PPSDataTable.Rows(i)("oa_pps_date_debut")
            Else
                dateDebut = "01/01/1900"
            End If

            'Rythme
            Rythme = Coalesce(PPSDataTable.Rows(i)("oa_parcours_rythme"), 0)
            Base = Coalesce(PPSDataTable.Rows(i)("oa_parcours_base"), "")
            Select Case Base
                Case ParcoursDao.EnumParcoursBaseCode.Quotidien
                    BaseItem = ParcoursDao.EnumParcoursBaseItem.Quotidien
                Case ParcoursDao.EnumParcoursBaseCode.Hebdomadaire
                    BaseItem = ParcoursDao.EnumParcoursBaseItem.Hebdomadaire
                Case ParcoursDao.EnumParcoursBaseCode.ParMois
                    BaseItem = ParcoursDao.EnumParcoursBaseItem.ParMois
                Case ParcoursDao.EnumParcoursBaseCode.ParAn
                    BaseItem = ParcoursDao.EnumParcoursBaseItem.ParAn
                Case ParcoursDao.EnumParcoursBaseCode.TousLes2Ans
                    BaseItem = ParcoursDao.EnumParcoursBaseItem.TousLes2Ans
                Case ParcoursDao.EnumParcoursBaseCode.TousLes3Ans
                    BaseItem = ParcoursDao.EnumParcoursBaseItem.TousLes3Ans
                Case ParcoursDao.EnumParcoursBaseCode.TousLes4Ans
                    BaseItem = ParcoursDao.EnumParcoursBaseItem.TousLes4Ans
                Case ParcoursDao.EnumParcoursBaseCode.TousLes5Ans
                    BaseItem = ParcoursDao.EnumParcoursBaseItem.TousLes5Ans
                Case Else
                    BaseItem = ""
            End Select

            CommentairePPS = Coalesce(PPSDataTable.Rows(i)("oa_pps_commentaire"), "")
            commentaireParcours = Coalesce(PPSDataTable.Rows(i)("oa_parcours_commentaire"), "")

            AfficheDateModificationPPS = ""
            If PPSDataTable.Rows(i)("oa_pps_date_modification") IsNot DBNull.Value Then
                dateModification = PPSDataTable.Rows(i)("oa_pps_date_modification")
                AfficheDateModificationPPS = FormatageDateAffichage(dateModification) + " : "
            Else
                If PPSDataTable.Rows(i)("oa_pps_date_creation") IsNot DBNull.Value Then
                    dateModification = PPSDataTable.Rows(i)("oa_pps_date_creation")
                    AfficheDateModificationPPS = FormatageDateAffichage(dateModification) + " : "
                End If
            End If

            'Recherche si le parcours a été modifié
            AfficheDateModificationParcours = ""
            If PPSDataTable.Rows(i)("oa_parcours_date_modification") IsNot DBNull.Value Then
                dateModification = PPSDataTable.Rows(i)("oa_parcours_date_modification")
                AfficheDateModificationParcours = FormatageDateAffichage(dateModification) + " : "
            Else
                If PPSDataTable.Rows(i)("oa_parcours_date_creation") IsNot DBNull.Value Then
                    dateModification = PPSDataTable.Rows(i)("oa_parcours_date_creation")
                    AfficheDateModificationParcours = FormatageDateAffichage(dateModification) + " : "
                End If
            End If

            'PPS caché
            ppsArret = False
            If PPSDataTable.Rows(i)("oa_pps_arret") IsNot DBNull.Value Then
                If PPSDataTable.Rows(i)("oa_pps_arret") = "1" Then
                    ppsArret = True
                End If
            End If

            'NaturePPS = ""
            AffichePPS = ""
            'Présentation PPS : Cible/Objectif de santé (commentaire)
            If categoriePPS = EnumCategoriePPS.Objectif Then
                NaturePPS = "Objectif santé : "
                AffichePPS = NaturePPS + " " + CommentairePPS
            End If

            If categoriePPS = EnumCategoriePPS.MesurePreventive Then
                mesureCount += 1
                NaturePPS = "Mesures préventives : "
                AffichePPS = NaturePPS & " " & CommentairePPS
            End If

            SpecialiteDescription = ""
            'Présentation PPS : Suivi
            If categoriePPS = EnumCategoriePPS.Suivi Then
                'Un parcours caché ne doit être affiché
                Dim parcoursCache As Boolean = Coalesce(PPSDataTable.Rows(i)("oa_parcours_cacher"), False)
                If parcoursCache = True Then
                    'Continue For
                End If
                'Un suivi intervenant sans rythme ne doit pas être affiché dans le PPS
                If Rythme = 0 Then
                    Continue For
                End If

                'Suivi IDE, Médecin référent, Sage-femme et Spécialiste (Base, Rythme, Commentaire)
                Select Case sousCategoriePPS
                    Case EnumSousCategoriePPS.IDE
                        NaturePPS = "Suivi IDE : "
                    Case EnumSousCategoriePPS.medecinReferent
                        NaturePPS = "Suivi médecin télémédecine : "
                    Case EnumSousCategoriePPS.sageFemme
                        NaturePPS = "Suivi sage-femme : "
                    Case EnumSousCategoriePPS.specialiste
                        'Récupération spécialité
                        If PPSDataTable.Rows(i)("oa_parcours_specialite") IsNot DBNull.Value Then
                            SpecialiteId = PPSDataTable.Rows(i)("oa_parcours_specialite")
                            SpecialiteDescription = Table_specialite.GetSpecialiteDescription(SpecialiteId)
                        End If
                        NaturePPS = "Suivi " + SpecialiteDescription + " : "
                    Case Else
                        NaturePPS = "Inconnue "
                End Select
                If Base = ParcoursDao.EnumParcoursBaseCode.Hebdomadaire _
                    Or Base = ParcoursDao.EnumParcoursBaseCode.ParMois _
                    Or Base = ParcoursDao.EnumParcoursBaseCode.ParAn Then
                    AffichePPS = NaturePPS + Rythme.ToString + " / " + BaseItem + " " + CommentairePPS
                Else
                    AffichePPS = NaturePPS + BaseItem + " " + CommentairePPS
                End If
            End If

            'Présentation PPS : Stratégie contextuelle (Base, Rythme, Commentaire)
            If categoriePPS = EnumCategoriePPS.Strategie Then
                Select Case sousCategoriePPS
                    'TODO: Synthese -> Déclarer ces sous-catégories PPS dans une Enum
                    Case 7
                        NaturePPS = "Démarche prophylactique "
                    Case 8
                        NaturePPS = "Démarche sociale "
                    Case 9
                        NaturePPS = "Démarche symptomatique "
                    Case 10
                        NaturePPS = "Démarche curative "
                    Case 11
                        NaturePPS = "Démarche diagnostique "
                    Case 12
                        NaturePPS = "Démarche palliative "
                    Case Else
                        NaturePPS = "Inconnue "
                End Select
                AffichePPS = AfficheDateModificationPPS + NaturePPS + " " + CommentairePPS
            End If

            'Transformation des "Tab" et "Return" en espace pour afficher les éléments correctement
            AffichePPS = Replace(AffichePPS, vbTab, " ")
            AffichePPS = Replace(AffichePPS, vbCrLf, " ")

            'Ajout d'une ligne au DataGridView
            iGrid += 1
            RadPPSDataGridView.Rows.Add(iGrid)
            'Alimentation du DataGridView
            If ppsArret = True Then
                RadPPSDataGridView.Rows(iGrid).Cells("pps").Style.ForeColor = Color.Red
            End If

            'Affichage du PPS
            RadPPSDataGridView.Rows(iGrid).Cells("pps").Value = AffichePPS

            'Identifiant pps
            RadPPSDataGridView.Rows(iGrid).Cells("ppsId").Value = PPSDataTable.Rows(i)("oa_pps_id")
            RadPPSDataGridView.Rows(iGrid).Cells("parcoursId").Value = PPSDataTable.Rows(i)("oa_parcours_id")

            RadPPSDataGridView.Rows(iGrid).Cells("categorieId").Value = categoriePPS
            RadPPSDataGridView.Rows(iGrid).Cells("sousCategorieId").Value = sousCategoriePPS
            RadPPSDataGridView.Rows(iGrid).Cells("specialiteId").Value = SpecialiteId
        Next

        'Positionnement du grid sur la première occurrence
        If RadPPSDataGridView.Rows.Count > 0 Then
            RadPPSDataGridView.CurrentRow = RadPPSDataGridView.Rows(0)
            RadPPSDataGridView.TableElement.VScrollBar.Value = 0
        End If
    End Sub

    '===========================================================
    '======================= Généralités =======================
    '===========================================================

    'Initialisation de l'écran
    Private Sub InitZones()
        'Etat civil
        LblPatientNIR.Text = ""
        LblPatientPrenom.Text = ""
        LblPatientNom.Text = ""
        LblPatientAge.Text = ""
        LblPatientGenre.Text = ""
        LblPatientAdresse1.Text = ""
        LblPatientAdresse2.Text = ""
        LblPatientCodePostal.Text = ""
        LblPatientVille.Text = ""
        LblPatientTel1.Text = ""
        LblPatientTel2.Text = ""
        LblPatientSite.Text = ""
        LblPatientUniteSanitaire.Text = ""
        LblPatientDateMaj.Text = ""
        'Traitements
        RadPPSDataGridView.Rows.Clear()
    End Sub

    'Private Sub RadPPSDataGridView_CellDoubleClick(sender As Object, e As Telerik.WinControls.UI.GridViewCellEventArgs) Handles RadPPSDataGridView.CellDoubleClick
    '    'Appeler selon la nature du PPS le DetailEdit correspondant
    '    If RadPPSDataGridView.CurrentRow IsNot Nothing Then
    '        Dim PPSId, ParcoursId, categoriePPS, sousCategoriePPS, SpecialiteId As Integer
    '        Dim aRow As Integer = Me.RadPPSDataGridView.Rows.IndexOf(Me.RadPPSDataGridView.CurrentRow)
    '        If aRow >= 0 Then
    '            PPSId = RadPPSDataGridView.Rows(aRow).Cells("ppsId").Value
    '            ParcoursId = RadPPSDataGridView.Rows(aRow).Cells("parcoursId").Value
    '            categoriePPS = RadPPSDataGridView.Rows(aRow).Cells("categorieId").Value
    '            sousCategoriePPS = RadPPSDataGridView.Rows(aRow).Cells("sousCategorieId").Value
    '            SpecialiteId = RadPPSDataGridView.Rows(aRow).Cells("specialiteId").Value
    '            Select Case categoriePPS
    '                Case Pps.EnumCategoriePPS.OBJECTIF_SANTE
    '                    Cursor.Current = Cursors.WaitCursor
    '                    Me.Enabled = False

    '                    Try
    '                        Using vRadFPPSObjectifSanteDetail As New RadFPPSDetailEdit
    '                            vRadFPPSObjectifSanteDetail.PPSId = PPSId
    '                            vRadFPPSObjectifSanteDetail.CategoriePPS = EnumCategoriePPS.Objectif
    '                            vRadFPPSObjectifSanteDetail.SelectedPatient = Me.SelectedPatient
    '                            vRadFPPSObjectifSanteDetail.UtilisateurConnecte = Me.UtilisateurConnecte
    '                            vRadFPPSObjectifSanteDetail.PositionGaucheDroite = EnumPosition.Droite
    '                            vRadFPPSObjectifSanteDetail.ShowDialog() 'Modal
    '                            If vRadFPPSObjectifSanteDetail.CodeRetour = True Then
    '                                ChargementPPS()
    '                            End If
    '                        End Using
    '                    Catch ex As Exception
    '                        MsgBox(ex.Message())
    '                    End Try

    '                    Me.Enabled = True
    '                Case Pps.EnumCategoriePPS.MESURE_PREVENTIVE
    '                    Cursor.Current = Cursors.WaitCursor
    '                    Me.Enabled = False

    '                    Try
    '                        Using vFFPPSMesurePreventive As New RadFPPSDetailEdit
    '                            vFFPPSMesurePreventive.PPSId = PPSId
    '                            vFFPPSMesurePreventive.CategoriePPS = EnumCategoriePPS.MesurePreventive
    '                            vFFPPSMesurePreventive.SelectedPatient = Me.SelectedPatient
    '                            vFFPPSMesurePreventive.UtilisateurConnecte = Me.UtilisateurConnecte
    '                            vFFPPSMesurePreventive.PositionGaucheDroite = EnumPosition.Droite
    '                            vFFPPSMesurePreventive.ShowDialog() 'Modal
    '                            If vFFPPSMesurePreventive.CodeRetour = True Then
    '                                ChargementPPS()
    '                            End If
    '                        End Using
    '                    Catch ex As Exception
    '                        MsgBox(ex.Message())
    '                    End Try

    '                    Me.Enabled = True
    '                Case Pps.EnumCategoriePPS.SUIVI_INTERVENANT
    '                    Cursor.Current = Cursors.WaitCursor
    '                    Me.Enabled = False

    '                    Try
    '                        Using vFParcoursDetailEdit As New RadFParcoursDetailEdit
    '                            vFParcoursDetailEdit.SelectedParcoursId = ParcoursId
    '                            vFParcoursDetailEdit.SelectedPatient = Me.SelectedPatient
    '                            'vFParcoursDetailEdit.UtilisateurConnecte = Me.UtilisateurConnecte
    '                            vFParcoursDetailEdit.PositionGaucheDroite = EnumPosition.Droite
    '                            vFParcoursDetailEdit.ShowDialog() 'Modal
    '                            If vFParcoursDetailEdit.CodeRetour = True Then
    '                                ChargementPPS()
    '                            End If
    '                        End Using
    '                    Catch ex As Exception
    '                        MsgBox(ex.Message())
    '                    End Try

    '                    Me.Enabled = True
    '                Case Pps.EnumCategoriePPS.STRATEGIE
    '                    Cursor.Current = Cursors.WaitCursor
    '                    Me.Enabled = False

    '                    Try
    '                        Using vFPPSStrategie As New RadFPPSDetailEdit
    '                            vFPPSStrategie.PPSId = PPSId
    '                            vFPPSStrategie.CategoriePPS = EnumCategoriePPS.Strategie
    '                            vFPPSStrategie.SelectedPatient = Me.SelectedPatient
    '                            vFPPSStrategie.UtilisateurConnecte = Me.UtilisateurConnecte
    '                            vFPPSStrategie.PositionGaucheDroite = EnumPosition.Droite
    '                            vFPPSStrategie.ShowDialog() 'Modal
    '                            If vFPPSStrategie.CodeRetour = True Then
    '                                ChargementPPS()
    '                            End If
    '                        End Using
    '                    Catch ex As Exception
    '                        MsgBox(ex.Message())
    '                    End Try

    '                    Me.Enabled = True
    '            End Select
    '        End If
    '    End If
    'End Sub

    Private Sub RadBtnAbandon_Click(sender As Object, e As EventArgs) Handles RadBtnAbandon.Click
        Close()
    End Sub

    Private Sub DteHorizonAffichage_ValueChanged(sender As Object, e As EventArgs) Handles DteHorizonAffichage.ValueChanged, DTPDateFin.ValueChanged
        ChargementPPS()
    End Sub

End Class
