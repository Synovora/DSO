Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Net
Imports System.Net.Http.Formatting
Imports System.Web
Imports System.Web.Http
Imports System.Web.Mvc
Imports Oasis_Web
Imports Oasis_Common
Imports System.Globalization

Namespace Oasis_Web.Controllers
    Public Class SyntheseController
        Inherits Controller

        ReadOnly parametreDao As New ParametreDao
        ReadOnly episodeProtocoleCollaboratifDao As New EpisodeProtocoleCollaboratifDao
        ReadOnly episodeDao As New EpisodeDao
        ReadOnly patientDao As New PatientDao
        Dim episodeParametreDao As New EpisodeParametreDao

        <System.Web.Mvc.Authorize>
        Public Function Index() As ActionResult

            Dim strName As String = Constants.LAYOUT_VERTICAL
            Dim strWelcomeText As String = "Dashboard"
            If TempData("ModeName") IsNot Nothing Then strName = TempData("ModeName").ToString()
            If TempData("WelcomeText") IsNot Nothing Then strWelcomeText = TempData("WelcomeText").ToString()
            ViewBag.ModeName = strName
            ViewBag.WelcomeText = strWelcomeText

            If Request.Cookies("patientId") Is Nothing Then
                Return View("~/Views/Pages/pages-500.cshtml")
            End If

            Dim patient = patientDao.GetPatient(Request.Cookies("patientId").Value)
            ViewBag.Patient = patient
            ViewBag.Contexts = BuildContexte(patient.PatientId)
            ViewBag.Antecedents = BuildAntecedent(patient.PatientId)
            ViewBag.Traitements = BuildTraitement(patient.PatientId)
            ViewBag.PPS = BuildPPS(patient.PatientId)
            ViewBag.PS = BuildPS(patient.PatientId)
            ViewBag.Vaccins = BuildVaccin(patient.PatientId)
            ViewBag.Allergies = BuildAllergie(patient.PatientId)
            ViewBag.ContreIndication = BuildContreIndication(patient.PatientId)
            Return View()
        End Function

        Private Function BuildAllergie(patientId As Integer)
            Dim StringAllergieToolTip As String = patientDao.GetStringAllergieByPatient(patientId)
            Return StringAllergieToolTip
        End Function

        Private Function BuildContreIndication(patientId As Integer)
            Dim StringContreIndicationToolTip As String = patientDao.GetStringContreIndicationByPatient(patientId)
            Return StringContreIndicationToolTip
        End Function

        Private Function BuildAntecedent(patientId As Integer) As List(Of List(Of String))

            Dim result As List(Of List(Of String)) = New List(Of List(Of String))

            Dim antecedentDao As AntecedentDao = New AntecedentDao
            Dim antecedentDataTable As DataTable = antecedentDao.GetAllAntecedentbyPatient(patientId, True, True)


            Dim i As Integer
            Dim rowCount As Integer = antecedentDataTable.Rows.Count - 1
            Dim iGrid As Integer = -1
            Dim indentation As String
            Dim dateDateModification, AldDateFin As Date
            Dim AfficheDateModification As String
            Dim diagnostic As String
            Dim antecedentCache, AldValide, AldValideOK, AldDemandeEnCours As Boolean
            Dim antecedentIdPrecedent1, antecedentIdPrecedent2 As Long
            antecedentIdPrecedent1 = 0
            antecedentIdPrecedent2 = 0

            For i = 0 To rowCount Step 1
                Dim tmp As List(Of String) = New List(Of String)
                'If RadChkMajeurSeul.Checked = True Then
                '    If antecedentDataTable.Rows(i)("oa_antecedent_niveau") <> 1 Then
                '        Continue For
                '    End If
                'End If


                Select Case antecedentDataTable.Rows(i)("oa_antecedent_niveau")
                    Case 1
                        indentation = ""
                    Case 2
                        indentation = "----> "
                    Case 3
                        indentation = "-------->> "
                    Case Else
                        indentation = ""
                End Select


                'Recherche si le contexte a été modifié
                AfficheDateModification = ""
                If antecedentDataTable.Rows(i)("oa_antecedent_date_modification") IsNot DBNull.Value Then
                    dateDateModification = antecedentDataTable.Rows(i)("oa_antecedent_date_modification")
                    AfficheDateModification = " (" + FormatageDateAffichage(dateDateModification) + ")"
                Else
                    If antecedentDataTable.Rows(i)("oa_antecedent_date_creation") IsNot DBNull.Value Then
                        dateDateModification = antecedentDataTable.Rows(i)("oa_antecedent_date_creation")
                        AfficheDateModification = " (" + FormatageDateAffichage(dateDateModification) + ")"
                    End If
                End If

                'Identification si l'antécédent est caché
                antecedentCache = False
                If antecedentDataTable.Rows(i)("oa_antecedent_statut_affichage") IsNot DBNull.Value Then
                    If antecedentDataTable.Rows(i)("oa_antecedent_statut_affichage") = "C" Then
                        antecedentCache = True
                    End If
                End If

                AldValide = Coalesce(antecedentDataTable.Rows(i)("oa_antecedent_ald_valide"), False)
                AldDateFin = Coalesce(antecedentDataTable.Rows(i)("oa_antecedent_ald_date_fin"), Nothing)
                AldValideOK = False
                If AldValide = True Then
                    If AldDateFin > Date.Now() Then
                        AldValideOK = True
                    End If
                End If
                AldDemandeEnCours = Coalesce(antecedentDataTable.Rows(i)("oa_antecedent_ald_demande_en_cours"), False)

                If antecedentCache = True Then
                    tmp.Add("color: blue")
                Else
                    If AldValideOK = True Then
                        tmp.Add("color: red;")
                    Else
                        If AldDemandeEnCours = True Then
                            tmp.Add("color: orange;")
                        Else
                            tmp.Add("color: inherit;")
                        End If
                    End If
                End If

                'Alimentation du DataGridView
                diagnostic = ""
                If antecedentDataTable.Rows(i)("oa_antecedent_diagnostic") IsNot DBNull.Value Then
                    If CInt(antecedentDataTable.Rows(i)("oa_antecedent_diagnostic")) = 2 Then
                        diagnostic = "Suspicion de : "
                    Else
                        If CInt(antecedentDataTable.Rows(i)("oa_antecedent_diagnostic")) = 3 Then
                            diagnostic = "Notion de : "
                        End If
                    End If
                End If

                Dim antecedentDescription As String

                If antecedentDataTable.Rows(i)("oa_antecedent_description") Is DBNull.Value Or antecedentDataTable.Rows(i)("oa_antecedent_description") = "" Then
                    antecedentDescription = ""
                Else
                    antecedentDescription = antecedentDataTable.Rows(i)("oa_antecedent_description")
                    antecedentDescription = Replace(antecedentDescription, vbCrLf, " ")
                    tmp.Add(antecedentDataTable.Rows(i)("oa_antecedent_description"))
                    ' RadAntecedentDataGridView.Rows(iGrid).Cells("antecedentDescription").Value = antecedentDataTable.Rows(i)("oa_antecedent_description")
                End If

                Dim DescriptionDrcAld As String = ""
                If AldValideOK Or AldDemandeEnCours Then
                    'DescriptionDrcAld = Coalesce(antecedentDataTable.Rows(i)("oa_ald_cim10_description"), "")
                End If
                tmp.Add(indentation & diagnostic & DescriptionDrcAld & " " & antecedentDescription)
                'RadAntecedentDataGridView.Rows(iGrid).Cells("antecedent").Value = indentation & diagnostic & DescriptionDrcAld & " " & antecedentDescription
                '==========

                If AldValideOK = True Or AldDemandeEnCours = True Then
                    tmp.Add("X")
                Else
                    tmp.Add("")
                End If

                'Id antécédent
                tmp.Add(antecedentDataTable.Rows(i)("oa_antecedent_id"))
                tmp.Add(antecedentDataTable.Rows(i)("oa_antecedent_drc_id"))
                tmp.Add(antecedentDataTable.Rows(i)("oa_antecedent_niveau"))
                tmp.Add(Coalesce(antecedentDataTable.Rows(i)("oa_antecedent_ordre_affichage1"), 0))
                tmp.Add(Coalesce(antecedentDataTable.Rows(i)("oa_antecedent_ordre_affichage2"), 0))
                tmp.Add(Coalesce(antecedentDataTable.Rows(i)("oa_antecedent_ordre_affichage3"), 0))
                tmp.Add(Coalesce(antecedentDataTable.Rows(i)("oa_antecedent_id_niveau1"), 0))
                tmp.Add(Coalesce(antecedentDataTable.Rows(i)("oa_antecedent_id_niveau2"), 0))

                'Déplacement horizontal, détermination de l'antécédent précédent
                Select Case antecedentDataTable.Rows(i)("oa_antecedent_niveau")
                    Case 1
                        tmp.Add(antecedentIdPrecedent1)
                        antecedentIdPrecedent1 = antecedentDataTable.Rows(i)("oa_antecedent_id")
                        antecedentIdPrecedent2 = 0
                    Case 2
                        tmp.Add(antecedentIdPrecedent2)
                        antecedentIdPrecedent2 = antecedentDataTable.Rows(i)("oa_antecedent_id")
                    Case 3
                        'Non concerné
                End Select

                'Récupération de l'index du dernier antécédent déplacé pour lui remettre le focus lors du réaffichage de la grid
                'If antecedentIdADeplacer <> 0 AndAlso antecedentIdADeplacer = antecedentDataTable.Rows(i)("oa_antecedent_id") Then
                '    IndexAntecedentADeplacer = iGrid
                '    antecedentIdADeplacer = 0
                'End If

                'Déplacement vertical, détermination de l'antécédent pere si niveau 2 et 3
                Select Case CInt(antecedentDataTable.Rows(i)("oa_antecedent_niveau"))
                    Case 2
                        tmp.Add(antecedentDataTable.Rows(i)("oa_antecedent_id_niveau1"))
                    Case 3
                        tmp.Add(antecedentDataTable.Rows(i)("oa_antecedent_id_niveau2"))
                    Case Else
                        tmp.Add(0)
                End Select
                result.Add(tmp)
            Next
            Return result
        End Function

        Private Function BuildTraitement(patientId As Integer) As List(Of List(Of String))
            Dim result As List(Of List(Of String)) = New List(Of List(Of String))

            Dim traitementDataTable As DataTable
            Dim traitementDao As TraitementDao = New TraitementDao
            traitementDataTable = traitementDao.GetTraitementEnCoursbyPatient(patientId)

            'Ajout d'une colonne 'oa_traitement_posologie' dans le DataTable de traitement
            'traitementDataTable.Columns.Add("oa_traitement_posologie", Type.GetType("System.String"))

            Dim i As Integer
            Dim iGrid As Integer = -1 'Indice pour alimenter la Grid qui peut comporter moins d'occurrences que le DataTable
            Dim rowCount As Integer = traitementDataTable.Rows.Count - 1
            Dim Base As String
            Dim Posologie As String
            Dim dateFin, dateDebut, dateModification, dateCreation As Date
            Dim posologieMatin, posologieMidi, posologieApresMidi, posologieSoir As Integer
            Dim Rythme As Integer
            Dim FenetreTherapeutiqueEnCours As Boolean
            Dim FenetreTherapeutiqueAVenir As Boolean

            'Dim Allergie As Boolean = False
            Dim FenetreDateDebut, FenetreDateFin As Date

            'Parcours du DataTable pour alimenter les colonnes du DataGridView
            For i = 0 To rowCount Step 1
                Dim tmp As List(Of String) = New List(Of String)
                'Date de fin
                If traitementDataTable.Rows(i)("oa_traitement_date_fin") IsNot DBNull.Value Then
                    dateFin = traitementDataTable.Rows(i)("oa_traitement_date_fin")
                Else
                    dateFin = New Date(2999, 12, 31, 0, 0, 0)
                End If

                'Date début
                If traitementDataTable.Rows(i)("oa_traitement_date_debut") IsNot DBNull.Value Then
                    dateDebut = traitementDataTable.Rows(i)("oa_traitement_date_debut")
                Else
                    dateDebut = New Date(1900, 1, 1, 0, 0, 0)
                End If

                'Date création
                If traitementDataTable.Rows(i)("oa_traitement_date_creation") IsNot DBNull.Value Then
                    dateCreation = traitementDataTable.Rows(i)("oa_traitement_date_creation")
                Else
                    dateCreation = New Date(1900, 1, 1, 0, 0, 0)
                End If

                'Date modification
                If traitementDataTable.Rows(i)("oa_traitement_date_modification") IsNot DBNull.Value Then
                    dateModification = traitementDataTable.Rows(i)("oa_traitement_date_modification")
                Else
                    dateModification = dateCreation
                End If

                '===========================================================================> Obsolète début
                'Exclusion de l'affichage des traitements dont la date de fin est < à la date du jour
                'Cette condition est traitée en exclusion (et non dans la requête SQL) pour stocker les allergies et les contre-indications dans la StringCollection quel que soit leur date de fin
                Dim dateJouraComparer As New Date(Date.Now.Year, Date.Now.Month, Date.Now.Day, 0, 0, 0)
                Dim dateFinaComparer As New Date(dateFin.Year, dateFin.Month, dateFin.Day, 0, 0, 0)
                If (dateFinaComparer < dateJouraComparer) Then
                    Continue For
                End If
                '===========================================================================> Obsolète fin

                'Vérification de l'existence d'une fenêtre thérapeutique active et à venir
                FenetreTherapeutiqueEnCours = False
                FenetreTherapeutiqueAVenir = False

                If traitementDataTable.Rows(i)("oa_traitement_fenetre_date_debut") IsNot DBNull.Value Then
                    FenetreDateDebut = traitementDataTable.Rows(i)("oa_traitement_fenetre_date_debut")
                Else
                    FenetreDateDebut = Date.ParseExact("31/12/2999", "dd/MM/yyyy", CultureInfo.InvariantCulture)
                End If

                If traitementDataTable.Rows(i)("oa_traitement_fenetre_date_fin") IsNot DBNull.Value Then
                    FenetreDateFin = traitementDataTable.Rows(i)("oa_traitement_fenetre_date_fin")
                Else
                    FenetreDateFin = New Date(1900, 1, 1, 0, 0, 0)
                End If

                Posologie = ""

                'Existence d'une fenêtre thérapeutique
                Dim FenetreTherapeutiqueExiste As Boolean = False
                Dim dateDebutFenetreaComparer As New Date(FenetreDateDebut.Year, FenetreDateDebut.Month, FenetreDateDebut.Day, 0, 0, 0)
                Dim dateFinFenetreaComparer As New Date(FenetreDateFin.Year, FenetreDateFin.Month, FenetreDateFin.Day, 0, 0, 0)
                If traitementDataTable.Rows(i)("oa_traitement_fenetre") IsNot DBNull.Value Then
                    If traitementDataTable.Rows(i)("oa_traitement_fenetre") = "1" Then
                        'Fenêtre thérapeutique en cours, à venir ou obsolète
                        FenetreTherapeutiqueExiste = True
                        If FenetreDateDebut <= dateJouraComparer And FenetreDateFin >= dateJouraComparer Then
                            'Fenêtre thérapeutique en cours
                            FenetreTherapeutiqueEnCours = True
                            Posologie = "Fenêtre Th."
                        Else
                            If FenetreDateDebut > dateJouraComparer Then
                                FenetreTherapeutiqueAVenir = True
                            End If
                        End If
                    End If
                End If

                'Formatage de la posologie
                If FenetreTherapeutiqueEnCours = False Then
                    Dim PosologieMatinString, PosologieMidiString, PosologieApresMidiString, PosologieSoirString As String
                    Dim FractionMatin, FractionMidi, FractionApresMidi, FractionSoir As String
                    Dim PosologieBase As String

                    FractionMatin = Coalesce(traitementDataTable.Rows(i)("oa_traitement_fraction_matin"), Traitement.EnumFraction.Non)
                    FractionMidi = Coalesce(traitementDataTable.Rows(i)("oa_traitement_fraction_midi"), Traitement.EnumFraction.Non)
                    FractionApresMidi = Coalesce(traitementDataTable.Rows(i)("oa_traitement_fraction_apres_midi"), Traitement.EnumFraction.Non)
                    FractionSoir = Coalesce(traitementDataTable.Rows(i)("oa_traitement_fraction_soir"), Traitement.EnumFraction.Non)

                    posologieMatin = Coalesce(traitementDataTable.Rows(i)("oa_traitement_Posologie_matin"), 0)
                    posologieMidi = Coalesce(traitementDataTable.Rows(i)("oa_traitement_Posologie_midi"), 0)
                    posologieApresMidi = Coalesce(traitementDataTable.Rows(i)("oa_traitement_Posologie_apres_midi"), 0)
                    posologieSoir = Coalesce(traitementDataTable.Rows(i)("oa_traitement_Posologie_soir"), 0)

                    PosologieBase = Coalesce(traitementDataTable.Rows(i)("oa_traitement_Posologie_base"), "")

                    If FractionMatin <> "" AndAlso FractionMatin <> Traitement.EnumFraction.Non Then
                        If posologieMatin <> 0 Then
                            PosologieMatinString = posologieMatin.ToString & "+" & FractionMatin
                        Else
                            PosologieMatinString = FractionMatin
                        End If
                    Else
                        If posologieMatin <> 0 Then
                            PosologieMatinString = posologieMatin.ToString
                        Else
                            PosologieMatinString = "0"
                        End If
                    End If

                    If FractionMidi <> "" AndAlso FractionMidi <> Traitement.EnumFraction.Non Then
                        If posologieMidi <> 0 Then
                            PosologieMidiString = posologieMidi.ToString & "+" & FractionMidi
                        Else
                            PosologieMidiString = FractionMidi
                        End If
                    Else
                        If posologieMidi <> 0 Then
                            PosologieMidiString = posologieMidi.ToString
                        Else
                            PosologieMidiString = "0"
                        End If
                    End If

                    PosologieApresMidiString = ""
                    If FractionApresMidi <> "" AndAlso FractionApresMidi <> Traitement.EnumFraction.Non Then
                        If posologieApresMidi <> 0 Then
                            PosologieApresMidiString = posologieApresMidi.ToString & "+" & FractionApresMidi
                        Else
                            PosologieApresMidiString = FractionApresMidi
                        End If
                    Else
                        If posologieApresMidi <> 0 Then
                            PosologieApresMidiString = posologieApresMidi.ToString
                        End If
                    End If

                    If FractionSoir <> "" AndAlso FractionSoir <> Traitement.EnumFraction.Non Then
                        If posologieSoir <> 0 Then
                            PosologieSoirString = posologieSoir.ToString & "+" & FractionSoir
                        Else
                            PosologieSoirString = FractionSoir
                        End If
                    Else
                        If posologieSoir <> 0 Then
                            PosologieSoirString = posologieSoir.ToString
                        Else
                            PosologieSoirString = "0"
                        End If
                    End If
                    If traitementDataTable.Rows(i)("oa_traitement_posologie_base") IsNot DBNull.Value Then
                        Rythme = traitementDataTable.Rows(i)("oa_traitement_posologie_rythme")
                        Select Case PosologieBase
                            Case Traitement.EnumBaseCode.JOURNALIER
                                Base = ""
                                If posologieApresMidi <> 0 OrElse FractionApresMidi <> Traitement.EnumFraction.Non Then
                                    Posologie = Base + PosologieMatinString + ". " + PosologieMidiString + ". " + PosologieApresMidiString + ". " + PosologieSoirString
                                Else
                                    Posologie = Base + " " + PosologieMatinString + ". " + PosologieMidiString + ". " + PosologieSoirString
                                End If
                            Case Else
                                Dim RythmeString As String = ""
                                If FractionMatin <> "" AndAlso FractionMatin <> Traitement.EnumFraction.Non Then
                                    If Rythme <> 0 Then
                                        RythmeString = Rythme.ToString & "+" & FractionMatin
                                    Else
                                        RythmeString = FractionMatin
                                    End If
                                Else
                                    If Rythme <> 0 Then
                                        RythmeString = Rythme.ToString
                                    End If
                                End If
                                Base = traitementDao.GetBaseDescription(traitementDataTable.Rows(i)("oa_traitement_posologie_base"))
                                Posologie = Base + RythmeString
                        End Select
                    End If
                End If

                Dim commentaire As String = Coalesce(traitementDataTable.Rows(i)("oa_traitement_commentaire"), "")
                Dim commentairePosologie As String = Coalesce(traitementDataTable.Rows(i)("oa_traitement_posologie_commentaire"), "")

                'Stockage des médicaments prescrits (pour contrôle lors de la selection d'un médicament dans le cadre d'un nouveau traitement
                'SelectedPatient.PatientMedicamentsPrescritsCis.Add(traitementDataTable.Rows(i)("oa_traitement_medicament_cis"))

                iGrid += 1

                'Alimentation du DataGridView
                'DCI
                tmp.Add(traitementDataTable.Rows(i)("oa_traitement_medicament_dci"))
                'Posologie
                tmp.Add(Posologie)
                tmp.Add(commentaire)
                tmp.Add(commentairePosologie)

                'If Posologie = "Fenêtre Th." Then
                '    RadTraitementDataGridView.Rows(iGrid).Cells("posologie").Style.ForeColor = Color.Red
                'End If

                'Fenêtre thérapeutique existe (en cours ou à venir ou obsolète)
                If FenetreTherapeutiqueExiste = True Then
                    tmp.Add("O")
                Else
                    tmp.Add("")
                End If

                'Traitement du format d'affichage de la fin du traitement
                If dateDebut = DateTime.Parse("31/12/2999") Then
                    tmp.Add("Date non définie")
                Else
                    tmp.Add(FormatageDateAffichage(dateDebut, True))
                End If

                'Traitement du format d'affichage de modification du traitement
                If dateModification = DateTime.Parse("01/01/1900") Then
                    tmp.Add("Date non définie")
                Else
                    tmp.Add(FormatageDateAffichage(dateModification, True))
                End If

                'Identifiant du traitement
                tmp.Add(traitementDataTable.Rows(i)("oa_traitement_id"))

                'CIS du médicament
                tmp.Add(traitementDataTable.Rows(i)("oa_traitement_medicament_cis"))

                'Bouton gérer fenêtre thérapeutique
                'If FenetreTherapeutiqueAVenir = True Or FenetreTherapeutiqueEnCours = True Then
                '    RadTraitementDataGridView.Rows(iGrid).Cells("posologie").Style.ForeColor = Color.Red
                'End If
                result.Add(tmp)
            Next

            'Traitements arrêtés
            'Dim isTraitementArret As Boolean = False
            'Dim TraitementArretTooltip As String = ""
            'Dim DateArretString, TraitementArretString, TraitementArretMedicament, TraitementArretCommentaire As String
            'Dim DateArret As Date
            'Dim traitementArretDatatable As DataTable
            'traitementArretDatatable = traitementDao.GetAllTraitementArreteByPatient(patientId)
            'rowCount = traitementArretDatatable.Rows.Count - 1
            'For i = 0 To rowCount Step 1
            '    Dim tmp As List(Of String) = New List(Of String)
            '    isTraitementArret = True
            '    TraitementArretMedicament = Coalesce(traitementArretDatatable.Rows(i)("oa_traitement_medicament_dci"), "")
            '    TraitementArretCommentaire = Coalesce(traitementArretDatatable.Rows(i)("oa_traitement_arret_commentaire"), "")
            '    DateArret = Coalesce(traitementArretDatatable.Rows(i)("oa_traitement_date_modification"), Nothing)
            '    DateArretString = outils.FormatageDateAffichage(DateArret, True)
            '    TraitementArretString = TraitementArretMedicament & " (" & DateArretString & ")  " & TraitementArretCommentaire & vbCrLf
            '    TraitementArretTooltip += TraitementArretString
            'Next
            'If isTraitementArret Then
            '    ToolTip.SetToolTip(LblTraitementArret, TraitementArretTooltip)
            '    LblTraitementArret.Show()
            'Else
            '    LblTraitementArret.Hide()
            'End If

            'Positionnement du grid sur la première occurrence
            'If RadTraitementDataGridView.Rows.Count > 0 Then
            '    RadTraitementDataGridView.CurrentRow = RadTraitementDataGridView.Rows(0)
            '    RadTraitementDataGridView.TableElement.VScrollBar.Value = 0
            'End If
            Return result
        End Function

        Private Function BuildPPS(patientId As Integer) As List(Of List(Of String))
            Dim PPSDataTable As DataTable
            Dim PPSDao As PpsDao = New PpsDao
            PPSDataTable = PPSDao.getAllPPSbyPatient(patientId)

            Dim result As List(Of List(Of String)) = New List(Of List(Of String))

            'Déclaration des variables pour réaliser le parcours du DataTable pour alimenter le DataGridView
            Dim i, mesureCount As Integer
            Dim iGrid As Integer = -1 'Indice pour alimenter la Grid qui peut comporter moins d'occurrences que le DataTable
            Dim dateDebut, dateModification As Date
            Dim rowCount As Integer = PPSDataTable.Rows.Count - 1
            Dim categoriePPS, sousCategoriePPS, Rythme, SpecialiteId As Integer
            Dim ppsArret As Boolean
            Dim mesureMax As Boolean = False
            Dim NaturePPS, CommentairePPS, commentaireParcours, AffichePPS, AfficheDateModificationPPS, AfficheDateModificationParcours, Base, BaseItem, SpecialiteDescription As String

            'PPSSuiviIdeExiste = False
            'PPSSuiviMedecinExiste = False
            'PPSSuiviSageFemmeExiste = False

            'RadChkMesureMax.Hide()

            'Parcours du DataTable pour alimenter le DataGridView
            For i = 0 To rowCount Step 1
                Dim tmp As List(Of String) = New List(Of String)
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
                    dateDebut = New Date(1900, 1, 1, 0, 0, 0)
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

                'Détecter si les occurrences qui doivent être uniques existent pour ce patient
                'If categoriePPS = Pps.EnumCategoriePPS.SUIVI_INTERVENANT Then
                '    Select Case sousCategoriePPS
                '        Case EnumSousCategoriePPS.IDE
                '            PPSSuiviIdeExiste = True
                '        Case EnumSousCategoriePPS.medecinReferent
                '            PPSSuiviMedecinExiste = True
                '        Case EnumSousCategoriePPS.sageFemme
                '            PPSSuiviSageFemmeExiste = True
                '    End Select
                'End If

                'Recherche si le pps a été modifié
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

                NaturePPS = ""
                AffichePPS = ""
                'Présentation PPS : Cible/Objectif de santé (commentaire)
                If categoriePPS = EnumCategoriePPS.Objectif Then
                    NaturePPS = "Objectif santé : "
                    AffichePPS = NaturePPS + " " + CommentairePPS
                End If

                If categoriePPS = EnumCategoriePPS.MesurePreventive Then
                    mesureCount += 1
                    'If mesureCount > 2 Then
                    '    RadChkMesureMax.Show()
                    '    mesureMax = True
                    '    If RadChkMesureMax.CheckState = False Then
                    '        Continue For
                    '    End If
                    'End If
                    'Suivi mesures préventives (Code DRC, libellé DRC, commentaire)
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
                'Alimentation du DataGridView
                'If ppsArret = True Then
                '    RadPPSDataGridView.Rows(iGrid).Cells("pps").Style.ForeColor = Color.Red
                'End If

                'Affichage du PPS
                tmp.Add(AffichePPS)

                'Identifiant pps
                tmp.Add(Coalesce(PPSDataTable.Rows(i)("oa_pps_id"), 0))
                tmp.Add(Coalesce(PPSDataTable.Rows(i)("oa_parcours_id"), 0))

                tmp.Add(categoriePPS)
                tmp.Add(sousCategoriePPS)
                tmp.Add(SpecialiteId)
                result.Add(tmp)
            Next

            ''Positionnement du grid sur la première occurrence
            'If RadPPSDataGridView.Rows.Count > 0 Then
            '    RadPPSDataGridView.CurrentRow = RadPPSDataGridView.Rows(0)
            '    RadPPSDataGridView.TableElement.VScrollBar.Value = 0
            'End If
            Return result
        End Function

        Private Function BuildPS(patientId As Integer) As List(Of List(Of String))

            Dim ParcoursDataTable As DataTable
            Dim parcoursDao As New ParcoursDao
            Dim tacheDao As New TacheDao
            Dim SousCategorie, SpecialiteId As Integer
            Dim IntervenantOasis As Boolean

            ParcoursDataTable = parcoursDao.GetAllParcoursbyPatient(patientId)

            Dim iGrid As Integer = -1 'Indice pour alimenter la Grid qui peut comporter moins d'occurrences que le DataTable
            Dim rowCount As Integer = ParcoursDataTable.Rows.Count - 1
            Dim SpecialiteDescription As String
            Dim ParcoursCacher, ParcoursConsigneEnRouge As Boolean
            Dim result As List(Of List(Of String)) = New List(Of List(Of String))



            'Parcours du DataTable pour alimenter les colonnes du DataGridView
            For i = 0 To rowCount Step 1
                Dim tmp As List(Of String) = New List(Of String)
                Dim rorId As Integer = Coalesce(ParcoursDataTable.Rows(i)("oa_parcours_ror_id"), 0)
                ParcoursCacher = Coalesce(ParcoursDataTable.Rows(i)("oa_parcours_cacher"), False)
                'If RadChkParcoursNonCache.Checked = True Then
                '    If ParcoursCacher = True Then
                '        Continue For
                '    End If
                'End If

                iGrid += 1
                'Ajout d'une ligne au DataGridView
                'RadParcoursDataGridView.Rows.Add(iGrid)
                'Alimentation du DataGridView
                tmp.Add(ParcoursDataTable.Rows(i)("oa_parcours_id"))

                SpecialiteId = ParcoursDataTable.Rows(i)("oa_parcours_specialite")
                SpecialiteDescription = Table_specialite.GetSpecialiteDescription(SpecialiteId)
                tmp.Add(SpecialiteDescription)

                'Nom intervenant et Structure
                IntervenantOasis = False
                ParcoursConsigneEnRouge = False
                tmp.Add(ParcoursDataTable.Rows(i)("oa_parcours_sous_categorie_id"))
                'Select Case SousCategorie
                '    Case EnumSousCategoriePPS.medecinReferent
                '        IntervenantOasis = True
                '        ParcoursListProfilsOasis.Add(EnumSpecialiteOasis.medecinReferent)
                '    Case EnumSousCategoriePPS.IDE
                '        IntervenantOasis = True
                '        ParcoursListProfilsOasis.Add(EnumSpecialiteOasis.IDE)
                '        Dim pacoursConsigneDao As New ParcoursConsigneDao
                '        If pacoursConsigneDao.IsExistParcoursConsigne(ParcoursDataTable.Rows(i)("oa_parcours_id")) = False Then
                '            ParcoursConsigneEnRouge = True
                '        End If
                '    Case EnumSousCategoriePPS.sageFemme
                '        If ParcoursDataTable.Rows(i)("oa_parcours_intervenant_oasis") = True Then
                '            IntervenantOasis = True
                '            ParcoursListProfilsOasis.Add(EnumSpecialiteOasis.sageFemmeOasis)
                '        End If
                '    Case EnumSousCategoriePPS.specialiste
                'End Select

                If IntervenantOasis = True Then
                    tmp.Add("Oasis")
                    tmp.Add("Oasis")
                Else
                    tmp.Add(Coalesce(ParcoursDataTable.Rows(i)("oa_ror_nom"), ""))
                    tmp.Add(Coalesce(ParcoursDataTable.Rows(i)("oa_ror_structure_nom"), ""))
                End If

                'Recherche de la dernière consultation
                Dim dateLast, dateNext As Date
                Dim TypeDemandeRdv As String
                'Dim tache As Tache

                'RadParcoursDataGridView.Rows(iGrid).Cells("consultationLast").Value = "-"
                dateLast = Coalesce(ParcoursDataTable.Rows(i)("LastRendezVous"), Nothing)
                If dateLast <> Nothing Then
                    tmp.Add(outils.FormatageDateAffichage(dateLast, True))
                Else
                    tmp.Add("--")
                End If

                'RadParcoursDataGridView.Rows(iGrid).Cells("consultationNext").Value = "-"
                dateNext = Coalesce(ParcoursDataTable.Rows(i)("NextRendezVous"), Nothing)
                If dateNext <> Nothing Then
                    'Rendez-vous planifiée
                    tmp.Add(dateNext.ToString("dd.MM.yyyy"))
                    'RadParcoursDataGridView.Rows(iGrid).Cells("consultationNextHeure").Value = dateNext.ToString("HH:mm")
                Else
                    tmp.Add("--")
                    ''Recherche si existe demande de rendez-vous
                    'dateNext = Coalesce(ParcoursDataTable.Rows(i)("DateDemandeRdv"), Nothing)
                    'If dateNext <> Nothing Then
                    '    'Rendez-vous prévisionnel, demande en cours
                    '    TypeDemandeRdv = Coalesce(ParcoursDataTable.Rows(i)("TypeDemandeRdv"), "")
                    '    Select Case TypeDemandeRdv
                    '        Case Tache.EnumDemandeRendezVous.ANNEE.ToString
                    '            RadParcoursDataGridView.Rows(iGrid).Cells("consultationNext").Value = dateNext.ToString("yyyy")
                    '        Case Tache.EnumDemandeRendezVous.ANNEEMOIS.ToString
                    '            RadParcoursDataGridView.Rows(iGrid).Cells("consultationNext").Value = dateNext.ToString("MM.yyyy")
                    '        Case Else
                    '            RadParcoursDataGridView.Rows(iGrid).Cells("consultationNext").Value = outils.FormatageDateAffichage(dateNext, True)
                    '    End Select
                    'Else
                    '    Dim Rythme As Integer = Coalesce(ParcoursDataTable.Rows(i)("oa_parcours_rythme"), 0)
                    '    Dim Base As String = Coalesce(ParcoursDataTable.Rows(i)("oa_parcours_base"), 0)
                    '    If Rythme <> 0 And Base <> "" Then
                    '        If dateLast <> Nothing Then
                    '            'Rendez-vous prévisionnel, calculé selon le rythme saisi et le dernier rendez-vous passé
                    '            dateNext = CalculProchainRendezVous(dateLast, Rythme, Base)
                    '            RadParcoursDataGridView.Rows(iGrid).Cells("consultationNext").Value = outils.FormatageDateAffichage(dateNext, False)
                    '        Else
                    '            Dim DateCreation As Date = Coalesce(ParcoursDataTable.Rows(i)("oa_parcours_date_creation"), Nothing)
                    '            If DateCreation <> Nothing Then
                    '                'Rendez-vous prévisionnel, calculé selon le rythme saisi et la date de création de l'intervenant dans le parcours de soin du patient
                    '                dateNext = CalculProchainRendezVous(DateCreation, Rythme, Base)
                    '                RadParcoursDataGridView.Rows(iGrid).Cells("consultationNext").Value = outils.FormatageDateAffichage(dateNext, False)
                    '            Else
                    '                'Rendez-vous à venir non calculable
                    '            End If
                    '        End If
                    '    Else
                    '        'Pas de rendez-vous à venir pour cet intervenant
                    '    End If
                    'End If
                End If

                tmp.Add(Coalesce(ParcoursDataTable.Rows(i)("oa_parcours_commentaire"), ""))

                'If ParcoursCacher = True Then
                '    RadParcoursDataGridView.Rows(iGrid).Cells("specialite").Style.ForeColor = Color.CornflowerBlue
                '    RadParcoursDataGridView.Rows(iGrid).Cells("nomIntervenant").Style.ForeColor = Color.CornflowerBlue
                '    RadParcoursDataGridView.Rows(iGrid).Cells("nomStructure").Style.ForeColor = Color.CornflowerBlue
                '    RadParcoursDataGridView.Rows(iGrid).Cells("consultationLast").Style.ForeColor = Color.CornflowerBlue
                '    RadParcoursDataGridView.Rows(iGrid).Cells("consultationnext").Style.ForeColor = Color.CornflowerBlue
                '    RadParcoursDataGridView.Rows(iGrid).Cells("commentaire").Style.ForeColor = Color.CornflowerBlue
                'End If

                'If ParcoursConsigneEnRouge = True Then
                '    RadParcoursDataGridView.Rows(iGrid).Cells("specialite").Style.ForeColor = Color.Red
                '    RadParcoursDataGridView.Rows(iGrid).Cells("nomIntervenant").Style.ForeColor = Color.Red
                '    RadParcoursDataGridView.Rows(iGrid).Cells("nomStructure").Style.ForeColor = Color.Red
                '    RadParcoursDataGridView.Rows(iGrid).Cells("consultationLast").Style.ForeColor = Color.Red
                '    RadParcoursDataGridView.Rows(iGrid).Cells("consultationnext").Style.ForeColor = Color.Red
                '    RadParcoursDataGridView.Rows(iGrid).Cells("commentaire").Style.ForeColor = Color.Red
                'End If
                result.Add(tmp)
            Next

            'Positionnement du grid sur la première occurrence
            'If RadParcoursDataGridView.Rows.Count > 0 Then
            '    RadParcoursDataGridView.CurrentRow = RadParcoursDataGridView.ChildRows(0)
            '    RadParcoursDataGridView.TableElement.VScrollBar.Value = 0
            'End If
            Return result
        End Function

        Public Function BuildVaccin(patientId As Integer) As List(Of List(Of String))
            Dim NotePatientDataTable As New DataTable
            Dim result As List(Of List(Of String)) = New List(Of List(Of String))

            Dim patientNoteVaccinDao As PatientNoteVaccinDao = New PatientNoteVaccinDao()
            NotePatientDataTable = patientNoteVaccinDao.getAllNoteVaccinbyPatient(patientId)


            'Déclaration des variables pour réaliser le parcours du DataTable pour alimenter le DataGridView
            Dim i As Integer
            Dim iGrid As Integer = -1 'Indice pour alimenter la Grid qui peut comporter moins d'occurrences que le DataTable
            Dim dateCreation As Date
            Dim AfficheDateCreation, NotePatient, Auteur As String
            Dim AuteurId As Integer
            Dim rowCount As Integer = NotePatientDataTable.Rows.Count - 1

            'Parcours du DataTable pour alimenter le DataGridView
            For i = 0 To rowCount Step 1
                Dim tmp As List(Of String) = New List(Of String)
                If NotePatientDataTable.Rows(i)("oa_patient_note") IsNot DBNull.Value Then
                    NotePatient = NotePatientDataTable.Rows(i)("oa_patient_note")
                Else
                    NotePatient = ""
                End If

                'Utilisateur creation
                Auteur = ""
                If NotePatientDataTable.Rows(i)("oa_patient_note_utilisateur_creation") IsNot DBNull.Value Then
                    If NotePatientDataTable.Rows(i)("oa_patient_note_utilisateur_creation") <> 0 Then
                        Dim UtilisateurCreation As Utilisateur
                        Dim userDao As New UserDao
                        UtilisateurCreation = userDao.getUserById(NotePatientDataTable.Rows(i)("oa_patient_note_utilisateur_creation"))
                        'SetUtilisateur(utilisateurHisto, NotePatientDataTable.Rows(i)("oa_patient_note_utilisateur_creation"))
                        Auteur = UtilisateurCreation.UtilisateurPrenom & " " & UtilisateurCreation.UtilisateurNom
                    End If
                End If

                'Date création
                AfficheDateCreation = ""
                If NotePatientDataTable.Rows(i)("oa_patient_note_date_creation") IsNot DBNull.Value Then
                    dateCreation = NotePatientDataTable.Rows(i)("oa_patient_note_date_creation")
                    AfficheDateCreation = outils.FormatageDateAffichage(dateCreation)
                Else
                    If NotePatientDataTable.Rows(i)("oa_patient_note_date_creation") IsNot DBNull.Value Then
                        dateCreation = NotePatientDataTable.Rows(i)("oa_patient_note_date_creation")
                        AfficheDateCreation = outils.FormatageDateAffichage(dateCreation)
                    End If
                End If

                AuteurId = 0
                If NotePatientDataTable.Rows(i)("oa_patient_note_utilisateur_creation") IsNot DBNull.Value Then
                    AuteurId = NotePatientDataTable.Rows(i)("oa_patient_note_utilisateur_creation")
                End If



                'Alimentation du DataGridView
                tmp.Add(NotePatient)

                'Identifiant notePatient
                tmp.Add(NotePatientDataTable.Rows(i)("oa_patient_note_id"))

                'Auteur de la note
                tmp.Add(Auteur & vbCrLf & AfficheDateCreation)
                result.Add(tmp)
            Next

            Return result
        End Function

        Private Function BuildContexte(patientId As Integer) As List(Of List(Of String))
            Dim antecedentDao As AntecedentDao = New AntecedentDao
            Dim contexteDataTable As DataTable = antecedentDao.GetContextebyPatient(patientId, True)

            Dim i As Integer
            Dim dateFin, dateModification As Date
            Dim AfficheDateModification, diagnostic As String
            Dim rowCount As Integer = contexteDataTable.Rows.Count - 1
            Dim categorieContexte, categorieContexteString As String
            Dim contexteCache As Boolean
            Dim result As List(Of List(Of String)) = New List(Of List(Of String))


            For i = 0 To rowCount Step 1
                Dim tmp As List(Of String) = New List(Of String)
                categorieContexte = ""
                If contexteDataTable.Rows(i)("oa_antecedent_categorie_contexte") IsNot DBNull.Value Then
                    categorieContexte = contexteDataTable.Rows(i)("oa_antecedent_categorie_contexte")
                End If
                Select Case categorieContexte
                    Case ContexteCourrier.EnumParcoursBaseCode.Medical
                        categorieContexteString = ContexteCourrier.EnumParcoursBaseItem.Medical
                    Case ContexteCourrier.EnumParcoursBaseCode.BioEnvironnemental
                        categorieContexteString = ContexteCourrier.EnumParcoursBaseItem.BioEnvironnemental
                    Case Else
                        categorieContexteString = ""
                End Select

                'DateFin
                If contexteDataTable.Rows(i)("oa_antecedent_date_fin") IsNot DBNull.Value Then
                    dateFin = contexteDataTable.Rows(i)("oa_antecedent_date_fin")
                Else
                    dateFin = "31/12/9999"
                End If

                'Recherche si le contexte a été modifié (médical uniquement)
                AfficheDateModification = ""
                If categorieContexte = "M" Then
                    If contexteDataTable.Rows(i)("oa_antecedent_date_modification") IsNot DBNull.Value Then
                        dateModification = contexteDataTable.Rows(i)("oa_antecedent_date_modification")
                        AfficheDateModification = FormatageDateAffichage(dateModification) + " : "
                    Else
                        If contexteDataTable.Rows(i)("oa_antecedent_date_creation") IsNot DBNull.Value Then
                            dateModification = contexteDataTable.Rows(i)("oa_antecedent_date_creation")
                            AfficheDateModification = FormatageDateAffichage(dateModification) + " : "
                        End If
                    End If
                End If

                'Contexte caché
                contexteCache = False
                If contexteDataTable.Rows(i)("oa_antecedent_statut_affichage") IsNot DBNull.Value Then
                    If contexteDataTable.Rows(i)("oa_antecedent_statut_affichage") = "C" Then
                        contexteCache = True
                    End If
                End If

                'Alimentation du DataGridView
                tmp.Add(categorieContexteString)
                'RadContexteDataGridView.Rows(iGrid).Cells("categorieContexte").Value = categorieContexteString

                diagnostic = ""
                If contexteDataTable.Rows(i)("oa_antecedent_diagnostic") IsNot DBNull.Value Then
                    If CInt(contexteDataTable.Rows(i)("oa_antecedent_diagnostic")) = 2 Then
                        diagnostic = "Suspicion de "
                    Else
                        If CInt(contexteDataTable.Rows(i)("oa_antecedent_diagnostic")) = 3 Then
                            diagnostic = "Notion de "
                        End If
                    End If
                End If

                Dim contexteDescription As String
                contexteDescription = Coalesce(contexteDataTable.Rows(i)("oa_antecedent_description"), "")
                If contexteDescription <> "" Then
                    contexteDescription = Replace(contexteDescription, vbCrLf, " ")
                End If

                tmp.Add(AfficheDateModification & diagnostic & " " & contexteDescription)
                tmp.Add(contexteDataTable.Rows(i)("oa_antecedent_id"))
                result.Add(tmp)
            Next
            Return result
        End Function

    End Class
End Namespace
