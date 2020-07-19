Imports System.Data.SqlClient
Imports Oasis_Common

Public Class ParcoursDao
    Inherits StandardDao


    Dim patientDao As New PatientDao

    Public Structure EnumParcoursBaseItem
        Const Quotidien = "Quotidien"
        Const Hebdomadaire = "Par semaine"
        Const ParMois = "Par mois"
        Const ParAn = "Par an"
        Const TousLes2Ans = "Tous les 2 ans"
        Const TousLes3Ans = "Tous les 3 ans"
        Const TousLes4Ans = "Tous les 4 ans"
        Const TousLes5Ans = "Tous les 5 ans"
    End Structure

    Public Structure EnumParcoursBaseCode
        Const Quotidien = "QUOTIDIEN"
        Const Hebdomadaire = "HEBDOMADAIRE"
        Const ParMois = "PAR_MOIS"
        Const ParAn = "PAR_AN"
        Const TousLes2Ans = "TOUS_LES_2_ANS"
        Const TousLes3Ans = "TOUS_LES_3_ANS"
        Const TousLes4Ans = "TOUS_LES_4_ANS"
        Const TousLes5Ans = "TOUS_LES_5_ANS"
    End Structure

    Public Function GetParcoursById(parcoursId As Integer) As Parcours
        Dim parcours As Parcours
        Dim con As SqlConnection

        con = GetConnection()

        Try
            Dim command As SqlCommand = con.CreateCommand()

            command.CommandText =
                "select * from oasis.oa_patient_parcours where oa_parcours_id = @id"
            command.Parameters.AddWithValue("@id", parcoursId)
            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    parcours = BuildBean(reader)
                Else
                    Throw New ArgumentException("parcours inexistant !")
                End If
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try

        Return parcours
    End Function

    Private Function BuildBean(reader As SqlDataReader) As Parcours
        Dim parcours As New Parcours With {
            .Id = reader("oa_parcours_id"),
            .PatientId = Coalesce(reader("oa_parcours_patient_id"), 0),
            .SpecialiteId = Coalesce(reader("oa_parcours_specialite"), 0),
            .CategorieId = Coalesce(reader("oa_parcours_categorie_id"), 0),
            .SousCategorieId = Coalesce(reader("oa_parcours_sous_categorie_id"), 0),
            .IntervenantOasis = Coalesce(reader("oa_parcours_intervenant_oasis"), False),
            .RorId = Coalesce(reader("oa_parcours_ror_id"), 0),
            .Commentaire = Coalesce(reader("oa_parcours_commentaire"), ""),
            .Base = Coalesce(reader("oa_parcours_base"), ""),
            .Rythme = Coalesce(reader("oa_parcours_rythme"), 0),
            .Cacher = Coalesce(reader("oa_parcours_cacher"), False),
            .Inactif = Coalesce(reader("oa_parcours_inactif"), False),
            .UserCreation = Coalesce(reader("oa_parcours_utilisateur_creation"), 0),
            .DateCreation = Coalesce(reader("oa_parcours_date_creation"), Nothing),
            .UserModification = Coalesce(reader("oa_parcours_utilisateur_modification"), 0),
            .DateModification = Coalesce(reader("oa_parcours_date_modification"), Nothing)
        }
        Return parcours
    End Function

    Public Function GetAllParcoursbyPatient(patientId As Integer) As DataTable
        Dim SQLString As String
        SQLString =
            "SELECT oa_parcours_id, oa_parcours_patient_id, oa_parcours_specialite, oa_parcours_categorie_id, oa_parcours_sous_categorie_id," &
            " oa_parcours_intervenant_oasis, oa_parcours_ror_id, oa_parcours_commentaire, oa_parcours_base, oa_parcours_rythme, oa_parcours_cacher," &
            " oa_parcours_date_creation, oa_ror_nom, oa_ror_structure_nom, rdv.date_rendez_vous As LastRendezVous," &
            " NextRdv.date_rendez_vous As NextRendezVous, DemandeRdv.date_rendez_vous As DateDemandeRdv, DemandeRdv.type_demande_rendez_vous As TypeDemandeRdv" &
            " FROM oasis.oa_patient_parcours" &
            " LEFT JOIN oasis.oa_ror ON oa_ror_id = oa_parcours_ror_id" &
            " OUTER APPLY (Select TOP (1) * FROM oasis.oasis.oa_tache" &
               " WHERE oa_tache.patient_Id = oa_parcours_patient_id And etat = 'TERMINEE' AND oa_tache.parcours_id = oa_parcours_id" &
               " AND categorie = 'SOIN' AND ([type] = 'RDV' OR [type] = 'RDV_SPECIALISTE') AND (nature = 'RDV' OR nature = 'RDV_SPECIALISTE')" &
               " ORDER BY date_rendez_vous DESC) As RDV" &
            " OUTER APPLY(SELECT TOP (1) * FROM oasis.oasis.oa_tache" &
               " WHERE oa_tache.patient_Id = oa_parcours_patient_id AND (etat = 'EN_ATTENTE' OR etat = 'EN_COURS') AND oa_tache.parcours_id = oa_parcours_id" &
               " AND categorie = 'SOIN' AND ([type] = 'RDV' OR [type] = 'RDV_SPECIALISTE') AND (nature = 'RDV' OR nature = 'RDV_SPECIALISTE')" &
               " ORDER BY date_rendez_vous) As NextRDV" &
            " OUTER APPLY(SELECT TOP (1) * FROM oasis.oasis.oa_tache" &
               " WHERE oa_tache.patient_Id = oa_parcours_patient_id AND (etat = 'EN_ATTENTE' OR etat = 'EN_COURS') AND oa_tache.parcours_id = oa_parcours_id" &
               " And categorie = 'SOIN' AND [type] = 'RDV_DEMANDE' AND nature = 'RDV_DEMANDE'" &
               " ORDER BY date_rendez_vous) As DemandeRdv" &
            " WHERE (oa_parcours_inactif = 'False' OR oa_parcours_inactif is Null)" &
            " AND oa_parcours_patient_id = " & patientId.ToString &
            " ORDER BY oa_parcours_sous_categorie_id, oa_parcours_specialite"

        Dim ParcoursDataTable As DataTable = New DataTable()

        Using con As SqlConnection = GetConnection()
            Dim ParcoursDataAdapter As SqlDataAdapter = New SqlDataAdapter()
            Using ParcoursDataAdapter
                ParcoursDataAdapter.SelectCommand = New SqlCommand(SQLString, con)
                'Using ParcoursDataTable
                Try
                    ParcoursDataAdapter.Fill(ParcoursDataTable)
                    Dim command As SqlCommand = con.CreateCommand()
                Catch ex As Exception
                    Throw ex
                Finally

                End Try
                'End Using
            End Using
        End Using

        Return ParcoursDataTable
    End Function

    Public Function GetParcoursIDEbyPatient(patientId As Integer) As Parcours
        Dim parcours As Parcours
        Dim con As SqlConnection

        con = GetConnection()

        Try
            Dim command As SqlCommand = con.CreateCommand()

            command.CommandText =
                "SELECT * FROM oasis.oa_patient_parcours" &
                    " WHERE (oa_parcours_inactif = 'False' or oa_parcours_inactif is Null)" &
                    " AND oa_parcours_categorie_id = 3" &           'Suivi
                    " AND oa_parcours_sous_categorie_id = 3" &      'Suivi IDE
                    " AND oa_parcours_intervenant_oasis = 'True'" & 'Intervenant Oasis
                    " AND oa_parcours_patient_id = @id"

            command.Parameters.AddWithValue("@id", patientId)

            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    parcours = buildBean(reader)
                Else
                    Throw New ArgumentException("parcours inexistant !")
                End If
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try

        Return parcours
    End Function

    Public Function GetListOfIntervenantNonOasisByPatient(patientId As Integer) As List(Of IntervenantParcours)
        Dim rorDao As New RorDao

        Dim ListIntervenant As New List(Of IntervenantParcours)
        Dim SQLString As String
        SQLString = "SELECT * FROM oasis.oa_patient_parcours" &
                    " WHERE (oa_parcours_inactif = 'False' OR oa_parcours_inactif IS Null)" &
                    " AND oa_parcours_patient_id = " & patientId.ToString &
                    " AND (oa_parcours_intervenant_oasis = 'False' OR oa_parcours_intervenant_oasis IS Null)" &
                    " ORDER BY oa_parcours_sous_categorie_id, oa_parcours_specialite"

        Dim dt As DataTable = New DataTable()

        Using con As SqlConnection = GetConnection()
            Dim da As SqlDataAdapter = New SqlDataAdapter()
            Using da
                da.SelectCommand = New SqlCommand(SQLString, con)
                Try
                    da.Fill(dt)
                    Dim command As SqlCommand = con.CreateCommand()
                Catch ex As Exception
                    Throw ex
                Finally
                    con.Close()
                End Try

                For Each row In dt.Rows
                    Dim intervenantParcours As New IntervenantParcours
                    intervenantParcours.IntervenantId = row("oa_parcours_ror_id")
                    intervenantParcours.PatientId = row("oa_parcours_patient_id")
                    Dim Intervenant As Ror
                    Intervenant = rorDao.getRorById(intervenantParcours.IntervenantId)
                    intervenantParcours.Nom = Intervenant.Nom
                    intervenantParcours.Structure = Intervenant.StructureNom

                    Dim SpecialiteId As Long = row("oa_parcours_specialite")
                    intervenantParcours.Specialite = Environnement.Table_specialite.GetSpecialiteDescription(SpecialiteId)

                    ListIntervenant.Add(intervenantParcours)
                Next
            End Using
        End Using

        Return ListIntervenant
    End Function

    Public Function GetAllIntervenantOasisByPatient(patientId As Integer) As DataTable
        Dim SQLString As String
        SQLString = "SELECT * FROM oasis.oa_patient_parcours" &
                    " WHERE (oa_parcours_inactif = 'False' or oa_parcours_inactif is Null)" &
                    " AND oa_parcours_intervenant_oasis = 'True'" &
                    " AND oa_parcours_patient_id = " & patientId.ToString &
                    " ORDER BY oa_parcours_sous_categorie_id, oa_parcours_specialite"

        Dim ParcoursDataTable As DataTable = New DataTable()

        Using con As SqlConnection = GetConnection()
            Dim ParcoursDataAdapter As SqlDataAdapter = New SqlDataAdapter()
            Using ParcoursDataAdapter
                ParcoursDataAdapter.SelectCommand = New SqlCommand(SQLString, con)
                Try
                    ParcoursDataAdapter.Fill(ParcoursDataTable)
                    Dim command As SqlCommand = con.CreateCommand()
                Catch ex As Exception
                    Throw ex
                Finally
                    con.Close()
                End Try
            End Using
        End Using

        Return ParcoursDataTable
    End Function

    Public Function ExistIntervenantByPatientId(patientId As Integer, categorieId As Integer, sousCategorieId As Integer, specialiteId As Integer) As Boolean
        Dim con As SqlConnection
        con = GetConnection()

        Try
            Dim command As SqlCommand = con.CreateCommand()

            command.CommandText =
                "SELECT oa_parcours_id FROM oasis.oasis.oa_patient_parcours" &
                " WHERE oa_parcours_patient_id = @patientId and oa_parcours_categorie_id = @categorieId and oa_parcours_sous_categorie_id = @sousCategorieId" &
                " And oa_parcours_specialite = @specialiteId And (oa_parcours_inactif Is NULL Or oa_parcours_inactif = 0)"

            command.Parameters.AddWithValue("@patientId", patientId)
            command.Parameters.AddWithValue("@sousCategorieId", sousCategorieId)
            command.Parameters.AddWithValue("@categorieId", categorieId)
            command.Parameters.AddWithValue("@specialiteId", specialiteId)
            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    Return True
                End If
            End Using

        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try

        Return False
    End Function

    Public Function CreateIntervenantOasisByPatient(PatientId As Integer, Optional afficheMessage As Boolean = True) As Boolean
        Dim parcours As New Parcours
        'Dim rorDao As New RorDao

        'Création médecin référent
        If ExistIntervenantByPatientId(PatientId, EnumCategoriePPS.Suivi, EnumSousCategoriePPS.medecinReferent, EnumSpecialiteOasis.medecinReferent) = False Then
            parcours.PatientId = PatientId
            parcours.SpecialiteId = EnumSpecialiteOasis.medecinReferent
            parcours.CategorieId = EnumCategoriePPS.Suivi
            parcours.SousCategorieId = EnumSousCategoriePPS.medecinReferent
            parcours.IntervenantOasis = True
            parcours.RorId = 1      'ROR
            parcours.Commentaire = ""
            parcours.Base = ParcoursDao.EnumParcoursBaseCode.TousLes3Ans
            parcours.Rythme = 1
            parcours.Cacher = False
            parcours.Inactif = False
            parcours.UserCreation = userLog.UtilisateurId
            parcours.DateCreation = Date.Now()

            parcours.Id = CreateIntervenantParcours(parcours)
            If parcours.Id <> 0 Then
                If afficheMessage = True Then
                    Throw New Exception("Intervenant médecin référent du parcours de soin créé")
                End If
                'Création automatique de la première demande de rendez-vous
                Dim patient As PatientBase = patientDao.GetPatientById(PatientId)
                Dim tacheDao As New TacheDao
                tacheDao.CreationAutomatiqueDeDemandeRendezVous(patient, parcours, Date.Now(), True)
            End If
        End If

        'Création IDE
        If ExistIntervenantByPatientId(PatientId, EnumCategoriePPS.Suivi, EnumSousCategoriePPS.IDE, EnumSpecialiteOasis.IDE) = False Then
            parcours.PatientId = PatientId
            parcours.SpecialiteId = EnumSpecialiteOasis.IDE
            parcours.CategorieId = EnumCategoriePPS.Suivi
            parcours.SousCategorieId = EnumSousCategoriePPS.IDE
            parcours.IntervenantOasis = True
            parcours.RorId = 2      'ROR
            parcours.Commentaire = ""
            parcours.Base = ParcoursDao.EnumParcoursBaseCode.ParAn
            parcours.Rythme = 1
            parcours.Cacher = False
            parcours.Inactif = False
            parcours.UserCreation = userLog.UtilisateurId
            parcours.DateCreation = Date.Now()

            parcours.Id = CreateIntervenantParcours(parcours)
            If parcours.Id <> 0 Then
                If afficheMessage = True Then
                    Throw New Exception("Intervenant IDE sur site du parcours de soin créé")
                End If
                'Création automatique de la première demande de rendez-vous
                Dim patient As PatientBase = patientDao.GetPatientById(PatientId)
                Dim tacheDao As New TacheDao
                tacheDao.CreationAutomatiqueDeDemandeRendezVous(patient, parcours, Date.Now(), True)
            End If
        End If

        Return True
    End Function

    Public Function CreateIntervenantParcours(parcours As Parcours) As Long
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim parcoursId As Long = 0
        Dim con As SqlConnection
        con = GetConnection()

        Dim dateCreation As Date = Date.Now.Date

        Dim SQLstring As String = "insert into oasis.oa_patient_parcours" &
        " (oa_parcours_patient_id, oa_parcours_specialite, oa_parcours_categorie_id, oa_parcours_sous_categorie_id, oa_parcours_intervenant_oasis," &
        " oa_parcours_ror_id, oa_parcours_commentaire, oa_parcours_base, oa_parcours_rythme," &
        " oa_parcours_cacher, oa_parcours_inactif, oa_parcours_utilisateur_creation, oa_parcours_date_creation)" &
        " VALUES (@patientId, @specialite, @categorieId, @sousCategorieId, @intervenantOasis," &
        " @intervenantId, @commentaire, @base, @rythme," &
        " @cacher, @inactif, @userCreation, @dateCreation); SELECT SCOPE_IDENTITY()"

        Dim cmd As New SqlCommand(SQLstring, con)
        With cmd.Parameters
            .AddWithValue("@patientId", parcours.PatientId)
            .AddWithValue("@specialite", parcours.SpecialiteId)
            .AddWithValue("@categorieId", parcours.CategorieId)
            .AddWithValue("@sousCategorieId", parcours.SousCategorieId)
            .AddWithValue("@intervenantOasis", parcours.IntervenantOasis)
            .AddWithValue("@intervenantId", parcours.RorId)
            .AddWithValue("@commentaire", parcours.Commentaire)
            .AddWithValue("@base", parcours.Base)
            .AddWithValue("@rythme", parcours.Rythme)
            .AddWithValue("@cacher", parcours.Cacher)
            .AddWithValue("@inactif", parcours.Inactif)
            .AddWithValue("@userCreation", parcours.UserCreation)
            .AddWithValue("@dateCreation", parcours.DateCreation)
        End With

        Try
            da.InsertCommand = cmd
            parcoursId = da.InsertCommand.ExecuteScalar()
        Catch ex As Exception
            Throw New Exception(ex.Message)
            Return 0
        Finally
            con.Close()
        End Try

        If parcoursId <> 0 Then
            Dim parcoursHistoDao As New ParcoursHistoDao
            Dim ParcoursHistoACreer As New ParcoursHisto
            'Mise à jour des données dans l'instance de la classe d'historisation du parcours
            ParcoursHistoACreer.Id = parcoursId  'Récupération de l'id du parcours créé
            ParcoursHistoACreer.HistorisationDate = parcours.DateCreation
            ParcoursHistoACreer.HistorisationUtilisateurId = parcours.UserCreation
            ParcoursHistoACreer.HistorisationEtat = ParcoursHistoDao.EnumEtatParcoursHisto.Creation
            ParcoursHistoACreer.PatientId = parcours.PatientId
            ParcoursHistoACreer.SpecialiteId = parcours.SpecialiteId
            ParcoursHistoACreer.CategorieId = parcours.CategorieId
            ParcoursHistoACreer.SousCategorieId = parcours.SousCategorieId
            ParcoursHistoACreer.IntervenantOasis = parcours.IntervenantOasis
            ParcoursHistoACreer.RorId = parcours.RorId
            ParcoursHistoACreer.Commentaire = parcours.Commentaire
            ParcoursHistoACreer.Base = parcours.Base
            ParcoursHistoACreer.Rythme = parcours.Rythme
            ParcoursHistoACreer.Cacher = parcours.Cacher
            ParcoursHistoACreer.Inactif = parcours.Inactif

            'Création dans l'historique des PPS du PPS créé
            parcoursHistoDao.CreationParcoursHisto(ParcoursHistoACreer, userLog, ParcoursHistoDao.EnumEtatParcoursHisto.Creation)

            'Mise à jour de la date de mise à jour de la synthèse (table patient)
            patientDao.ModificationDateMajSynthesePatient(parcours.PatientId)
        End If


        Return parcoursId
    End Function

    Public Function ModificationIntervenantParcours(parcours As Parcours) As Boolean
        Dim patientDao As New PatientDao
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim con As SqlConnection = GetConnection()

        Dim dateModification As Date = Date.Now.Date

        Dim SQLstring As String = "update oasis.oa_patient_parcours set" &
        " oa_parcours_patient_id = @patientId, oa_parcours_specialite = @specialite, oa_parcours_categorie_id = @categorieId, oa_parcours_sous_categorie_Id = @sousCategorieId," &
        " oa_parcours_intervenant_oasis = @intervenantOasis, oa_parcours_ror_id = @intervenantId," &
        " oa_parcours_commentaire = @commentaire, oa_parcours_base = @base, oa_parcours_rythme = @rythme," &
        " oa_parcours_cacher = @cacher, oa_parcours_inactif = @inactif," &
        " oa_parcours_utilisateur_modification = @userModification, oa_parcours_date_modification = @dateModification" &
        " where oa_parcours_id = @Id"

        Dim cmd As New SqlCommand(SQLstring, con)

        With cmd.Parameters
            .AddWithValue("@Id", parcours.Id)
            .AddWithValue("@patientId", parcours.PatientId)
            .AddWithValue("@specialite", parcours.SpecialiteId)
            .AddWithValue("@categorieId", parcours.CategorieId)
            .AddWithValue("@sousCategorieId", parcours.SousCategorieId)
            .AddWithValue("@intervenantOasis", parcours.IntervenantOasis)
            .AddWithValue("@intervenantId", parcours.RorId)
            .AddWithValue("@commentaire", parcours.Commentaire)
            .AddWithValue("@base", parcours.Base)
            .AddWithValue("@rythme", parcours.Rythme)
            .AddWithValue("@cacher", parcours.Cacher)
            .AddWithValue("@inactif", parcours.Inactif)
            .AddWithValue("@userModification", userLog.UtilisateurId.ToString)
            .AddWithValue("@dateModification", Date.Now)
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

        If codeRetour = True Then
            Dim parcoursHistoDao As New ParcoursHistoDao
            Dim ParcoursHistoACreer As New ParcoursHisto
            'Mise à jour des données dans l'instance de la classe d'historisation du parcours
            ParcoursHistoACreer.HistorisationDate = parcours.DateCreation
            ParcoursHistoACreer.HistorisationUtilisateurId = parcours.UserCreation
            ParcoursHistoACreer.HistorisationEtat = ParcoursHistoDao.EnumEtatParcoursHisto.Modification
            ParcoursHistoACreer.Id = parcours.Id
            ParcoursHistoACreer.PatientId = parcours.PatientId
            ParcoursHistoACreer.SpecialiteId = parcours.SpecialiteId
            ParcoursHistoACreer.CategorieId = parcours.CategorieId
            ParcoursHistoACreer.SousCategorieId = parcours.SousCategorieId
            ParcoursHistoACreer.IntervenantOasis = parcours.IntervenantOasis
            ParcoursHistoACreer.RorId = parcours.RorId
            ParcoursHistoACreer.Commentaire = parcours.Commentaire
            ParcoursHistoACreer.Base = parcours.Base
            ParcoursHistoACreer.Rythme = parcours.Rythme
            ParcoursHistoACreer.Cacher = parcours.Cacher
            ParcoursHistoACreer.Inactif = parcours.Inactif

            'Création dans l'historique des PPS du PPS créé
            parcoursHistoDao.CreationParcoursHisto(ParcoursHistoACreer, userLog, ParcoursHistoDao.EnumEtatParcoursHisto.Modification)

            'Mise à jour de la date de mise à jour de la synthèse (table patient)
            patientDao.ModificationDateMajSynthesePatient(parcours.PatientId)
        End If

        Return codeRetour
    End Function

    Public Function AnnulationIntervenantParcours(parcours As Parcours) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim con As SqlConnection = GetConnection()
        Dim patientDao As New PatientDao

        Dim dateModification As Date = Date.Now.Date

        Dim SQLstring As String = "update oasis.oa_patient_parcours set oa_parcours_inactif = @inactif," &
        " oa_parcours_utilisateur_modification = @userModification, oa_parcours_date_modification = @dateModification" &
        " where oa_parcours_id = @Id"

        Dim cmd As New SqlCommand(SQLstring, con)

        With cmd.Parameters
            .AddWithValue("@Id", parcours.Id)
            .AddWithValue("@inactif", True)
            .AddWithValue("@userModification", userLog.UtilisateurId.ToString)
            .AddWithValue("@dateModification", Date.Now)
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

        If codeRetour = True Then
            Dim parcoursHistoDao As New ParcoursHistoDao
            Dim ParcoursHistoACreer As New ParcoursHisto
            'Mise à jour des données dans l'instance de la classe d'historisation du parcours
            ParcoursHistoACreer.HistorisationDate = parcours.DateCreation
            ParcoursHistoACreer.HistorisationUtilisateurId = parcours.UserCreation
            ParcoursHistoACreer.HistorisationEtat = ParcoursHistoDao.EnumEtatParcoursHisto.Annulation
            ParcoursHistoACreer.Id = parcours.Id
            ParcoursHistoACreer.PatientId = parcours.PatientId
            ParcoursHistoACreer.SpecialiteId = parcours.SpecialiteId
            ParcoursHistoACreer.CategorieId = parcours.CategorieId
            ParcoursHistoACreer.SousCategorieId = parcours.SousCategorieId
            ParcoursHistoACreer.IntervenantOasis = parcours.IntervenantOasis
            ParcoursHistoACreer.RorId = parcours.RorId
            ParcoursHistoACreer.Commentaire = parcours.Commentaire
            ParcoursHistoACreer.Base = parcours.Base
            ParcoursHistoACreer.Rythme = parcours.Rythme
            ParcoursHistoACreer.Cacher = parcours.Cacher
            ParcoursHistoACreer.Inactif = True

            'Création dans l'historique des PPS du PPS créé
            parcoursHistoDao.CreationParcoursHisto(ParcoursHistoACreer, userLog, ParcoursHistoDao.EnumEtatParcoursHisto.Annulation)

            'Mise à jour de la date de mise à jour de la synthèse (table patient)
            patientDao.ModificationDateMajSynthesePatient(parcours.PatientId)
        End If

        Return codeRetour
    End Function

    Public Function GetAllIntervenantSansRendezVous() As DataTable
        Dim SQLString As String
        SQLString = "SELECT * FROM oasis.v_intervenant_sans_rdv" &
                    " WHERE DATE_RDV IS NULL" &
                    " ORDER BY [oa_parcours_patient_id], [oa_parcours_ror_id]"

        Dim dt As DataTable = New DataTable()

        Using con As SqlConnection = GetConnection()
            Dim da As SqlDataAdapter = New SqlDataAdapter()
            Using da
                da.SelectCommand = New SqlCommand(SQLString, con)
                Try
                    da.Fill(dt)
                    Dim command As SqlCommand = con.CreateCommand()
                Catch ex As Exception
                    Throw ex
                Finally
                    con.Close()
                End Try
            End Using
        End Using

        Return dt
    End Function

    Public Function CloneParcours(Source As Parcours) As Parcours
        Dim Cible As New Parcours
        Cible.Id = Source.Id
        Cible.PatientId = Source.PatientId
        Cible.SpecialiteId = Source.SpecialiteId
        Cible.CategorieId = Source.CategorieId
        Cible.SousCategorieId = Source.SousCategorieId
        Cible.IntervenantOasis = Source.IntervenantOasis
        Cible.RorId = Source.RorId
        Cible.Commentaire = Source.Commentaire
        Cible.Base = Source.Base
        Cible.Rythme = Source.Rythme
        Cible.Cacher = Source.Cacher
        Cible.Inactif = Source.Inactif
        Cible.UserCreation = Source.UserCreation
        Cible.DateCreation = Source.DateCreation
        Cible.UserModification = Source.UserModification
        Cible.DateModification = Source.DateModification
        Return Cible
    End Function

    Public Function Compare(source1 As Parcours, source2 As Parcours) As Boolean
        If source1.Id <> source2.Id Then
            Return False
        End If
        If source1.PatientId <> source2.PatientId Then
            Return False
        End If
        If source1.SpecialiteId <> source2.SpecialiteId Then
            Return False
        End If
        If source1.CategorieId <> source2.CategorieId Then
            Return False
        End If
        If source1.SousCategorieId <> source2.SousCategorieId Then
            Return False
        End If
        If source1.IntervenantOasis <> source2.IntervenantOasis Then
            Return False
        End If
        If source1.RorId <> source2.RorId Then
            Return False
        End If
        If source1.Commentaire <> source2.Commentaire Then
            Return False
        End If
        If source1.Base <> source2.Base Then
            Return False
        End If
        If source1.Rythme <> source2.Rythme Then
            Return False
        End If
        If source1.Cacher <> source2.Cacher Then
            Return False
        End If
        If source1.Inactif <> source2.Inactif Then
            Return False
        End If
        If source1.UserCreation <> source2.UserCreation Then
            Return False
        End If
        If source1.DateCreation.Date <> source2.DateCreation.Date Then
            Return False
        End If
        If source1.UserModification <> source2.UserModification Then
            Return False
        End If
        If source1.DateModification.Date <> source2.DateModification.Date Then
            Return False
        End If
        Return True
    End Function

End Class
