﻿Imports System.Configuration
Imports System.Data.SqlClient

Public Class OrdonnanceDao
    Inherits StandardDao

    Private Function BuildBean(reader As SqlDataReader) As Ordonnance
        Dim ordonnance As New Ordonnance With {
            .Id = reader("oa_ordonnance_id"),
            .PatientId = Coalesce(reader("oa_ordonnance_patient_id"), 0),
            .EpisodeId = Coalesce(reader("oa_ordonnance_episode_id"), 0),
            .UtilisateurCreation = Coalesce(reader("oa_ordonnance_utilisateur_creation"), 0),
            .DateCreation = Coalesce(reader("oa_ordonnance_date_creation"), Nothing),
            .DateValidation = Coalesce(reader("oa_ordonnance_date_validation"), Nothing),
            .UserValidation = Coalesce(reader("oa_ordonnance_user_validation"), 0),
            .DateEdition = Coalesce(reader("oa_ordonnance_date_edition"), Nothing),
            .Commentaire = Coalesce(reader("oa_ordonnance_commentaire"), ""),
            .Renouvellement = Coalesce(reader("oa_ordonnance_renouvellement"), 0),
            .Inactif = Coalesce(reader("oa_ordonnance_inactif"), False),
            .Signature = Coalesce(reader("oa_ordonnance_signature"), "")
        }
        Return ordonnance
    End Function

    Public Function GetOrdonnaceById(OrdonnanceId As Long) As Ordonnance
        Dim ordonnance As Ordonnance
        Dim con As SqlConnection = GetConnection()
        Try
            Dim command As SqlCommand = con.CreateCommand()
            command.CommandText =
                "SELECT * FROM oasis.oa_patient_ordonnance WHERE oa_ordonnance_id = @ordonnanceId"
            command.Parameters.AddWithValue("@ordonnanceId", OrdonnanceId)
            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    ordonnance = BuildBean(reader)
                Else
                    Throw New ArgumentException("Ordonnance inexistante !")
                End If
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try
        Return ordonnance
    End Function

    Public Function GetOrdonnaceBySignature(OrdonnanceSignature As String) As Ordonnance
        Dim ordonnance As Ordonnance
        Dim con As SqlConnection = GetConnection()
        Try
            Dim command As SqlCommand = con.CreateCommand()
            command.CommandText = "SELECT * FROM oasis.oa_patient_ordonnance WHERE oa_ordonnance_signature = @signature"
            command.Parameters.AddWithValue("@signature", OrdonnanceSignature)
            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    ordonnance = BuildBean(reader)
                Else
                    Throw New ArgumentException("Ordonnance inexistante !")
                End If
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try
        Return ordonnance
    End Function

    Public Function GetAllOrdonnanceByPatient(patientId As Long) As List(Of Ordonnance)
        Dim con As SqlConnection = GetConnection()
        Dim ordonnances As List(Of Ordonnance) = New List(Of Ordonnance)
        Try
            Dim command As SqlCommand = con.CreateCommand()
            command.CommandText = "SELECT * FROM oasis.oa_patient_ordonnance" &
                    " WHERE oa_ordonnance_patient_id = @patienId" &
                    " ORDER BY oa_ordonnance_id DESC"
            command.Parameters.AddWithValue("@patienId", patientId)
            Using reader As SqlDataReader = command.ExecuteReader()
                While (reader.Read())
                    ordonnances.Add(BuildBean(reader))
                End While
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try
        Return ordonnances
    End Function

    Public Function GetOrdonnanceValideByPatient(patientId As Long, episodeId As Long) As List(Of Ordonnance)
        Dim con As SqlConnection = GetConnection()
        Dim ordonnances As List(Of Ordonnance) = New List(Of Ordonnance)
        Try
            Dim command As SqlCommand = con.CreateCommand()
            command.CommandText = "SELECT * FROM oasis.oa_patient_ordonnance" &
                    " WHERE oa_ordonnance_patient_id = @patienId" &
                    " AND oa_ordonnance_episode_id = @episodeId" &
                    " AND (oa_ordonnance_inactif = 'False' OR oa_ordonnance_inactif is Null)"
            command.Parameters.AddWithValue("@patienId", patientId)
            command.Parameters.AddWithValue("@episodeId", episodeId)
            Using reader As SqlDataReader = command.ExecuteReader()
                While (reader.Read())
                    ordonnances.Add(BuildBean(reader))
                End While
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try
        Return ordonnances
    End Function

    Public Function CreateOrdonnance(patientId As Long, episodeId As Long, userLog As Utilisateur) As Long
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim OrdonnanceId As Long = 0
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
        Finally
            con.Close()
        End Try
        Return OrdonnanceId
    End Function

    Public Sub ModificationOrdonnanceCommentaire(OrdonnanceId As Long, Commentaire As String)
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim con As SqlConnection = GetConnection()
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
            Throw ex
        Finally
            con.Close()
        End Try
    End Sub

    Public Sub ModificationOrdonnanceRenouvellement(OrdonnanceId As Long, Renouvellement As Long)
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim con As SqlConnection = GetConnection()
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
            Throw ex
        Finally
            con.Close()
        End Try
    End Sub

    Public Sub AnnulerOrdonnance(OrdonnanceId As Long)
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim con As SqlConnection = GetConnection()
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
            Throw ex
        Finally
            con.Close()
        End Try
    End Sub

    Public Sub ValidationOrdonnance(OrdonnanceId As Long, userLog As Utilisateur)
        Try
            Dim ordonnanceDao As OrdonnanceDao = New OrdonnanceDao
            Dim ordonnanceDetailDao As OrdonnanceDetailDao = New OrdonnanceDetailDao
            Dim ordonnance As Ordonnance = ordonnanceDao.GetOrdonnaceById(OrdonnanceId)
            Dim ordonnanceDetailA As List(Of OrdonnanceDetail) = ordonnanceDetailDao.GetOrdonnanceLigneByOrdonnanceId(OrdonnanceId)
            Dim ordonnanceDetail As List(Of OrdonnanceDetail) = TryCast(CObj(ordonnanceDetailA), List(Of OrdonnanceDetail))
            Dim ordonnanceFull As OrdonnanceFull = New OrdonnanceFull With {
                .Ordonnance = ordonnance,
                .Details = ordonnanceDetail
            }
            Dim da As SqlDataAdapter = New SqlDataAdapter()
            Dim codeRetour As Boolean = True
            Dim con As SqlConnection = GetConnection()

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
                Throw ex
            Finally
                con.Close()
            End Try
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub CreateNewOrdonnanceDetail(patientId As Long, ordonnanceId As Long, episode As Episode)
        Console.WriteLine("CreateNewOrdonnanceDetail: " & ordonnanceId)
        Try
            Dim alddao As New AldDao
            Dim TraitementDao As New TraitementDao
            Dim OrdonnanceDetailDao As New OrdonnanceDetailDao
            Dim TraitementDataTable As DataTable
            Dim PatientAld As Boolean
            Dim FenetreTherapeutiqueEnCours As Boolean
            Dim posologieMatin, posologieMidi, posologieApresMidi, posologieSoir As Long
            Dim Rythme As Long

            PatientAld = alddao.IsPatientALD(patientId)
            TraitementDataTable = TraitementDao.GetTraitementEnCoursbyPatient(patientId)

            Dim i As Long
            Dim rowCount As Long = TraitementDataTable.Rows.Count - 1

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

                Dim duree As Long = outils.CalculDureeTraitementEnJour(DateDebutCalcul, DateFinCalcul)

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

                Dim ordonnanceDetail As New OrdonnanceDetail With {
                .OrdonnanceId = ordonnanceId,
                .Traitement = True,
                .TraitementId = TraitementDataTable.Rows(i)("oa_traitement_id"),
                .OrdreAffichage = TraitementDataTable.Rows(i)("oa_traitement_ordre_affichage"),
                .MedicamentCis = TraitementDataTable.Rows(i)("oa_traitement_medicament_cis"),
                .MedicamentDci = Coalesce(TraitementDataTable.Rows(i)("oa_traitement_medicament_dci"), ""),
                .DateDebut = DateDebut,
                .DateFin = DateFin,
                .Duree = duree,
                .Posologie = Posologie,
                .PosologieBase = Coalesce(TraitementDataTable.Rows(i)("oa_traitement_posologie_base"), ""),
                .PosologieRythme = Coalesce(TraitementDataTable.Rows(i)("oa_traitement_posologie_rythme"), 0),
                .PosologieMatin = Coalesce(TraitementDataTable.Rows(i)("oa_traitement_posologie_matin"), 0),
                .PosologieMidi = Coalesce(TraitementDataTable.Rows(i)("oa_traitement_posologie_midi"), 0),
                .PosologieApresMidi = Coalesce(TraitementDataTable.Rows(i)("oa_traitement_posologie_apres_midi"), 0),
                .PosologieSoir = Coalesce(TraitementDataTable.Rows(i)("oa_traitement_posologie_soir"), 0),
                .FractionMatin = Coalesce(TraitementDataTable.Rows(i)("oa_traitement_fraction_matin"), ""),
                .FractionMidi = Coalesce(TraitementDataTable.Rows(i)("oa_traitement_fraction_midi"), ""),
                .FractionApresMidi = Coalesce(TraitementDataTable.Rows(i)("oa_traitement_fraction_apres_midi"), ""),
                .FractionSoir = Coalesce(TraitementDataTable.Rows(i)("oa_traitement_fraction_soir"), ""),
                .PosologieCommentaire = Coalesce(TraitementDataTable.Rows(i)("oa_traitement_posologie_commentaire"), ""),
                .Commentaire = Coalesce(TraitementDataTable.Rows(i)("oa_traitement_commentaire"), ""),
                .Fenetre = Coalesce(TraitementDataTable.Rows(i)("oa_traitement_fenetre"), False),
                .FenetreDateDebut = Coalesce(TraitementDataTable.Rows(i)("oa_traitement_fenetre_date_debut"), "31/12/2999"),
                .FenetreDateFin = Coalesce(TraitementDataTable.Rows(i)("oa_traitement_fenetre_date_fin"), "31/12/2999")
            }
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
                Dim PeriodeNonDelivrance As Long
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
        Catch ex As Exception
            MsgBox(ex.Message())
        End Try
    End Sub

End Class