﻿Imports System.Configuration
Imports System.Data.SqlClient
Imports Oasis_Common

Public Class OrdonnanceDao
    Inherits OrdonnanceDaoBase

    Public Function CreateOrdonnance(patientId As Integer, episodeId As Integer, userLog As Utilisateur) As Integer
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim OrdonnanceId As Integer = 0
        Dim con As SqlConnection = GetConnection()
        Dim dateCreation As Date = Date.Now.Date
        Dim SQLstring As String = "INSERT into oasis.oa_patient_ordonnance" &
                                " (oa_ordonnance_patient_id, oa_ordonnance_utilisateur_creation, oa_ordonnance_date_creation," &
                                " oa_ordonnance_episode_id, oa_ordonnance_commentaire, oa_ordonnance_renouvellement)" &
                                " VALUES (@patientId, @userCreation, @dateCreation," &
                                " @episodeid, @commentaire, @renouvellement); SELECT SCOPE_IDENTITY()"

        Dim cmd As New SqlCommand(SQLstring, con)
        With cmd.Parameters
            .AddWithValue("@patientId", patientId)
            .AddWithValue("@userCreation", userLog.UtilisateurId.ToString)
            .AddWithValue("@dateCreation", Date.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            .AddWithValue("@episodeId", episodeId.ToString)
            .AddWithValue("@commentaire", "")
            .AddWithValue("@renouvellement", 0)
        End With

        Try
            da.InsertCommand = cmd
            OrdonnanceId = da.InsertCommand.ExecuteScalar()
        Catch ex As Exception
            Throw ex
            OrdonnanceId = 0
        Finally
            con.Close()
        End Try

        Return OrdonnanceId
    End Function

    Public Function ModificationOrdonnanceCommentaire(OrdonnanceId As Integer, Commentaire As String) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim con As SqlConnection
        con = GetConnection()

        Dim SQLstring As String = "UPDATE oasis.oa_patient_ordonnance SET" &
        " oa_ordonnance_commentaire = @commentaire" &
        " WHERE oa_ordonnance_id = @ordonnanceId"

        Dim cmd As New SqlCommand(SQLstring, con)

        With cmd.Parameters
            .AddWithValue("@ordonnanceId", OrdonnanceId)
            .AddWithValue("@commentaire", Commentaire.Trim())
        End With

        Try
            da.UpdateCommand = cmd
            da.UpdateCommand.ExecuteNonQuery()
        Catch ex As Exception
            Throw New Exception(ex.Message)
            codeRetour = False
        Finally
            con.Close()
        End Try

        Return codeRetour
    End Function

    Public Function ModificationOrdonnanceRenouvellement(OrdonnanceId As Integer, Renouvellement As Integer) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim con As SqlConnection
        con = GetConnection()

        Dim SQLstring As String = "UPDATE oasis.oa_patient_ordonnance SET" &
        " oa_ordonnance_renouvellement = @renouvellement" &
        " WHERE oa_ordonnance_id = @ordonnanceId"

        Dim cmd As New SqlCommand(SQLstring, con)

        With cmd.Parameters
            .AddWithValue("@ordonnanceId", OrdonnanceId)
            .AddWithValue("@renouvellement", Renouvellement)
        End With

        Try
            da.UpdateCommand = cmd
            da.UpdateCommand.ExecuteNonQuery()
        Catch ex As Exception
            Throw New Exception(ex.Message)
            codeRetour = False
        Finally
            con.Close()
        End Try

        Return codeRetour
    End Function

    Public Function AnnulerOrdonnance(OrdonnanceId As Integer) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim con As SqlConnection
        con = GetConnection()

        Dim SQLstring As String = "UPDATE oasis.oa_patient_ordonnance SET" &
        " oa_ordonnance_inactif = @inactif" &
        " WHERE oa_ordonnance_id = @ordonnanceId"

        Dim cmd As New SqlCommand(SQLstring, con)

        With cmd.Parameters
            .AddWithValue("@ordonnanceId", OrdonnanceId)
            .AddWithValue("@inactif", True)
        End With

        Try
            da.UpdateCommand = cmd
            da.UpdateCommand.ExecuteNonQuery()
        Catch ex As Exception
            Throw New Exception(ex.Message)
            codeRetour = False
        Finally
            con.Close()
        End Try

        Return codeRetour
    End Function

    Public Function ValidationOrdonnance(OrdonnanceId As Integer, userLog As Utilisateur) As Boolean
        Dim ordonnanceDao As OrdonnanceDao = New OrdonnanceDao 'Why instanciate ?
        Dim ordonnanceDetailDao As OrdonnanceDetailDao = New OrdonnanceDetailDao 'Why instanciate ?

        Dim ordonnance As Ordonnance = ordonnanceDao.GetOrdonnaceById(OrdonnanceId)
        Dim ordonnanceDetailA As List(Of OrdonnanceDetail) = ordonnanceDetailDao.GetOrdonnanceLigneByOrdonnanceId(OrdonnanceId)
        Dim ordonnanceDetail As List(Of OrdonnanceDetail) = TryCast(CObj(ordonnanceDetailA), List(Of OrdonnanceDetail))

        Dim ordonnanceFull As OrdonnanceFull = New OrdonnanceFull With {
            .Ordonnance = ordonnance,
            .Details = ordonnanceDetail
        }

        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim con As SqlConnection
        con = GetConnection()

        'Update ordonnance before sign
        ordonnanceFull.Ordonnance.DateValidation = Date.Now
        ordonnanceFull.Ordonnance.DateEdition = ordonnanceFull.Ordonnance.DateValidation
        ordonnanceFull.Ordonnance.UserValidation = userLog.UtilisateurId

        Dim SQLstring As String = "UPDATE oasis.oa_patient_ordonnance SET" &
        " oa_ordonnance_date_validation = @dateValidation," &
        " oa_ordonnance_user_validation = @userValidation," &
                " oa_ordonnance_date_edition = @dateEdition," &
        " oa_ordonnance_signature = @signature" &
        " WHERE oa_ordonnance_id = @ordonnanceId"

        Dim cmd As New SqlCommand(SQLstring, con)
        With cmd.Parameters
            .AddWithValue("@ordonnanceId", OrdonnanceId)
            .AddWithValue("@dateValidation", ordonnanceFull.Ordonnance.DateValidation)
            .AddWithValue("@userValidation", ordonnanceFull.Ordonnance.UserValidation)
            .AddWithValue("@dateEdition", ordonnanceFull.Ordonnance.DateEdition)
            .AddWithValue("@signature", userLog.Sign(ordonnanceFull.Serialize()))
        End With

        Try
            da.UpdateCommand = cmd
            da.UpdateCommand.ExecuteNonQuery()
        Catch ex As Exception
            Throw New Exception(ex.Message)
            codeRetour = False
        Finally
            con.Close()
        End Try

        Return codeRetour
    End Function

    Public Function CreateNewOrdonnanceDetail(patientId As Integer, ordonnanceId As Integer, episode As Episode) As Boolean
        Dim Coderetour As Boolean = True
        Dim alddao As New AldDao
        Dim TraitementDao As New TraitementDao
        Dim OrdonnanceDetailDao As New OrdonnanceDetailDao

        Dim TraitementDataTable As DataTable
        Dim PatientAld As Boolean
        Dim FenetreTherapeutiqueEnCours As Boolean

        Dim posologieMatin, posologieMidi, posologieApresMidi, posologieSoir As Integer
        Dim Rythme As Integer

        PatientAld = alddao.IsPatientALD(patientId)

        TraitementDataTable = TraitementDao.GetTraitementEnCoursbyPatient(patientId)

        Dim i As Integer
        Dim rowCount As Integer = TraitementDataTable.Rows.Count - 1

        Dim DateFin, DateDebut As Date

        For i = 0 To rowCount Step 1
            'Exclusion des médicaments déclarés 'allergique'
            If TraitementDataTable.Rows(i)("oa_traitement_allergie") IsNot DBNull.Value Then
                If TraitementDataTable.Rows(i)("oa_traitement_allergie") = "1" Then
                    Continue For
                End If
            End If

            'Exclusion des médicaments déclarés 'contre-indiqué'
            If TraitementDataTable.Rows(i)("oa_traitement_contre_indication") IsNot DBNull.Value Then
                If TraitementDataTable.Rows(i)("oa_traitement_contre_indication") = "1" Then
                    Continue For
                End If
            End If

            'Exclusion des traitements déclarés 'arrêté'
            'Cette condition est traitée en exclusion (et non dans la requête SQL) pour stocker les allergies et les contre-indications arrêtés dans la StringCollection
            If TraitementDataTable.Rows(i)("oa_traitement_arret") IsNot DBNull.Value Then
                If TraitementDataTable.Rows(i)("oa_traitement_arret") = "A" Then
                    Continue For
                End If
            End If

            'Date début
            DateDebut = Coalesce(TraitementDataTable.Rows(i)("oa_traitement_date_debut"), "01/01/1900")

            'Exclusion des traitements dont la date de fin est < à la date du jour
            DateFin = Coalesce(TraitementDataTable.Rows(i)("oa_traitement_date_fin"), "31/12/2999")
            If DateFin.Date < Date.Now.Date Then
                Continue For
            End If

            Dim DateFinCalcul As Date
            If DateFin > Date.Now.AddDays(29) Then
                DateFinCalcul = Date.Now.AddDays(29)
            Else
                DateFinCalcul = DateFin
            End If

            Dim DateDebutCalcul As Date
            If DateDebut.Date < Date.Now.Date Then
                DateDebutCalcul = Date.Now
            Else
                DateDebutCalcul = DateDebut
            End If

            Dim duree As Integer = outils.CalculDureeTraitementEnJour(DateDebutCalcul, DateFinCalcul)

            'Formatage de la posologie
            Dim PosologieMatinString, PosologieMidiString, PosologieApresMidiString, PosologieSoirString As String
            Dim FractionMatin, FractionMidi, FractionApresMidi, FractionSoir As String
            Dim PosologieBase As String
            Dim Base As String
            Dim Posologie As String = ""

            FractionMatin = Coalesce(TraitementDataTable.Rows(i)("oa_traitement_fraction_matin"), Traitement.EnumFraction.Non)
            FractionMidi = Coalesce(TraitementDataTable.Rows(i)("oa_traitement_fraction_midi"), Traitement.EnumFraction.Non)
            FractionApresMidi = Coalesce(TraitementDataTable.Rows(i)("oa_traitement_fraction_apres_midi"), Traitement.EnumFraction.Non)
            FractionSoir = Coalesce(TraitementDataTable.Rows(i)("oa_traitement_fraction_soir"), Traitement.EnumFraction.Non)

            posologieMatin = Coalesce(TraitementDataTable.Rows(i)("oa_traitement_Posologie_matin"), 0)
            posologieMidi = Coalesce(TraitementDataTable.Rows(i)("oa_traitement_Posologie_midi"), 0)
            posologieApresMidi = Coalesce(TraitementDataTable.Rows(i)("oa_traitement_Posologie_apres_midi"), 0)
            posologieSoir = Coalesce(TraitementDataTable.Rows(i)("oa_traitement_Posologie_soir"), 0)

            PosologieBase = Coalesce(TraitementDataTable.Rows(i)("oa_traitement_Posologie_base"), "")

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
            If TraitementDataTable.Rows(i)("oa_traitement_posologie_base") IsNot DBNull.Value Then
                Rythme = TraitementDataTable.Rows(i)("oa_traitement_posologie_rythme")
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

                        Base = TraitementDao.GetBaseDescription(TraitementDataTable.Rows(i)("oa_traitement_posologie_base"))
                        If Base = "Conditionnel : " Then
                            Base = ""
                        End If
                        Posologie = Base + RythmeString
                End Select
            End If

            Dim ordonnanceDetail As New OrdonnanceDetail
            ordonnanceDetail.OrdonnanceId = ordonnanceId
            ordonnanceDetail.Traitement = True
            ordonnanceDetail.TraitementId = TraitementDataTable.Rows(i)("oa_traitement_id")
            ordonnanceDetail.OrdreAffichage = TraitementDataTable.Rows(i)("oa_traitement_ordre_affichage")
            ordonnanceDetail.MedicamentCis = TraitementDataTable.Rows(i)("oa_traitement_medicament_cis")
            ordonnanceDetail.MedicamentDci = Coalesce(TraitementDataTable.Rows(i)("oa_traitement_medicament_dci"), "")
            ordonnanceDetail.DateDebut = DateDebut
            ordonnanceDetail.DateFin = DateFin
            ordonnanceDetail.Duree = duree
            ordonnanceDetail.Posologie = Posologie
            ordonnanceDetail.PosologieBase = Coalesce(TraitementDataTable.Rows(i)("oa_traitement_posologie_base"), "")
            ordonnanceDetail.PosologieRythme = Coalesce(TraitementDataTable.Rows(i)("oa_traitement_posologie_rythme"), 0)
            ordonnanceDetail.PosologieMatin = Coalesce(TraitementDataTable.Rows(i)("oa_traitement_posologie_matin"), 0)
            ordonnanceDetail.PosologieMidi = Coalesce(TraitementDataTable.Rows(i)("oa_traitement_posologie_midi"), 0)
            ordonnanceDetail.PosologieApresMidi = Coalesce(TraitementDataTable.Rows(i)("oa_traitement_posologie_apres_midi"), 0)
            ordonnanceDetail.PosologieSoir = Coalesce(TraitementDataTable.Rows(i)("oa_traitement_posologie_soir"), 0)
            ordonnanceDetail.FractionMatin = Coalesce(TraitementDataTable.Rows(i)("oa_traitement_fraction_matin"), "")
            ordonnanceDetail.FractionMidi = Coalesce(TraitementDataTable.Rows(i)("oa_traitement_fraction_midi"), "")
            ordonnanceDetail.FractionApresMidi = Coalesce(TraitementDataTable.Rows(i)("oa_traitement_fraction_apres_midi"), "")
            ordonnanceDetail.FractionSoir = Coalesce(TraitementDataTable.Rows(i)("oa_traitement_fraction_soir"), "")
            ordonnanceDetail.PosologieCommentaire = Coalesce(TraitementDataTable.Rows(i)("oa_traitement_posologie_commentaire"), "")
            ordonnanceDetail.Commentaire = Coalesce(TraitementDataTable.Rows(i)("oa_traitement_commentaire"), "")
            ordonnanceDetail.Fenetre = Coalesce(TraitementDataTable.Rows(i)("oa_traitement_fenetre"), False)
            ordonnanceDetail.FenetreDateDebut = Coalesce(TraitementDataTable.Rows(i)("oa_traitement_fenetre_date_debut"), "31/12/2999")
            ordonnanceDetail.FenetreDateFin = Coalesce(TraitementDataTable.Rows(i)("oa_traitement_fenetre_date_fin"), "31/12/2999")
            ordonnanceDetail.Fenetre = Coalesce(TraitementDataTable.Rows(i)("oa_traitement_fenetre"), False)

            'Existence d'une fenêtre thérapeutique
            Dim FenetreTherapeutiqueExiste As Boolean = Coalesce(TraitementDataTable.Rows(i)("oa_traitement_fenetre"), False)
            FenetreTherapeutiqueEnCours = False
            If FenetreTherapeutiqueExiste = True Then
                'Fenêtre thérapeutique en cours, à venir ou obsolète
                If ordonnanceDetail.FenetreDateDebut.Date <= Date.Now.Date And ordonnanceDetail.FenetreDateFin.Date >= Date.Now.Date Then
                    'Fenêtre thérapeutique en cours
                    FenetreTherapeutiqueEnCours = True
                End If
            End If

            'Récupération de la période de non délivrance dans les paramètres de l'application
            Dim PeriodeNonDelivranceStringAllergieString As String = ConfigurationManager.AppSettings("PeriodeNonDelivrance")
            Dim PeriodeNonDelivrance As Integer
            If IsNumeric(PeriodeNonDelivranceStringAllergieString) Then
                PeriodeNonDelivrance = CInt(PeriodeNonDelivranceStringAllergieString)
            Else
                'TODO: CreateLog("Paramètre application 'PeriodeNonDelivrance' non trouvé !", "OrdonnanceDao", LogDao.EnumTypeLog.ERREUR.ToString)
                PeriodeNonDelivrance = 15
            End If
            PeriodeNonDelivrance *= -1

            'Détermination de la délivrance des traitements prescrits
            ordonnanceDetail.ADelivrer = True
            Select Case episode.TypeActivite
                Case Episode.EnumTypeActiviteEpisodeCode.PATHOLOGIE_AIGUE,
                     Episode.EnumTypeActiviteEpisodeCode.SOCIAL
                    If DateDebut.Date < Date.Now.Date Then
                        ordonnanceDetail.ADelivrer = False
                    End If
                Case Episode.EnumTypeActiviteEpisodeCode.SUIVI_CHRONIQUE,
                         Episode.EnumTypeActiviteEpisodeCode.PREVENTION_SUIVI_GROSSESSE,
                         Episode.EnumTypeActiviteEpisodeCode.PREVENTION_SUIVI_GYNECOLOGIQUE,
                         Episode.EnumTypeActiviteEpisodeCode.PREVENTION_ENFANT_SCOLAIRE,
                         Episode.EnumTypeActiviteEpisodeCode.PREVENTION_ENFANT_PRE_SCOLAIRE,
                         Episode.EnumTypeActiviteEpisodeCode.PREVENTION_AUTRE
                    If DateDebut.Date <> Date.Now.Date Then
                        If DateDebut >= Date.Now.AddDays(PeriodeNonDelivrance) Then
                            ordonnanceDetail.ADelivrer = False
                        End If
                    End If
                Case Else
                    ordonnanceDetail.ADelivrer = False
            End Select

            '====> Enlevé sur demande de Francis le 29/04/2020
            'A ne pas délivrer si traitement conditionnel quel que soit le type d'épisode
            'If TraitementDataTable.Rows(i)("oa_traitement_posologie_base") = TraitementDao.EnumBaseCode.CONDITIONNEL Then
            'ordonnanceDetail.ADelivrer = False
            'End If

            'A ne pas délivrer si fenêtre thérapeutique en cours quel que soit le type d'épisode
            If FenetreTherapeutiqueEnCours = True Then
                ordonnanceDetail.ADelivrer = False
            End If

            'Détermination si traitement ALD / Non ALD
            If PatientAld = True Then
                ordonnanceDetail.Ald = True
                If episode.TypeActivite = Episode.EnumTypeActiviteEpisodeCode.SUIVI_CHRONIQUE Then
                    If TraitementDataTable.Rows(i)("oa_traitement_posologie_base") = Traitement.EnumBaseCode.CONDITIONNEL Then
                        ordonnanceDetail.Ald = False
                    Else
                        If DateDebut.Date <> Date.Now.Date Then
                            If DateDebut >= Date.Now.AddDays(PeriodeNonDelivrance) Then
                                ordonnanceDetail.Ald = False
                            End If
                        End If
                    End If
                Else
                    If DateDebut >= Date.Now.AddDays(PeriodeNonDelivrance) Then
                        ordonnanceDetail.Ald = False
                    Else
                        If TraitementDataTable.Rows(i)("oa_traitement_posologie_base") = Traitement.EnumBaseCode.CONDITIONNEL Then
                            ordonnanceDetail.Ald = False
                        End If
                    End If
                End If
            Else
                ordonnanceDetail.Ald = False
            End If

            OrdonnanceDetailDao.CreationOrdonnanceDetail(ordonnanceDetail)
        Next

        Return Coderetour
    End Function

End Class
