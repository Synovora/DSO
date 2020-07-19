Imports System.Data.SqlClient
Imports Oasis_Common
Public Class AntecedentDao
    Inherits StandardDao

    Dim patientDao As New PatientDao

    Public Structure EnumTypeAntecedentContexte
        Const ANTECEDENT = "A"
        Const CONTEXTE = "C"
    End Structure

    Public Structure EnumStatutAffichage
        Const PUBLIE = "P"
        Const CACHE = "C"
        Const OCCULTE = "O"
    End Structure

    Public Function GetAntecedentById(antecedentId As Integer) As Antecedent
        Dim antecedent As Antecedent
        Dim con As SqlConnection
        con = GetConnection()

        Try
            Dim command As SqlCommand = con.CreateCommand()

            command.CommandText =
                "select * from oasis.oa_antecedent where oa_antecedent_id = @id"
            command.Parameters.AddWithValue("@id", antecedentId)
            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    antecedent = BuildBean(reader)
                Else
                    Throw New ArgumentException("Antécédent inexistant !")
                End If
            End Using

        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try

        Return antecedent
    End Function

    Private Function BuildBean(reader As SqlDataReader) As Antecedent
        Dim antecedent As New Antecedent With {
            .Id = reader("oa_antecedent_id"),
            .PatientId = Coalesce(reader("oa_antecedent_patient_id"), 0),
            .Type = Coalesce(reader("oa_antecedent_type"), ""),
            .DrcId = Coalesce(reader("oa_antecedent_drc_id"), 0),
            .Description = Coalesce(reader("oa_antecedent_description"), ""),
            .UserCreation = Coalesce(reader("oa_antecedent_utilisateur_creation"), 0),
            .DateCreation = Coalesce(reader("oa_antecedent_date_creation"), Nothing),
            .UserModification = Coalesce(reader("oa_antecedent_utilisateur_modification"), 0),
            .DateModification = Coalesce(reader("oa_antecedent_date_modification"), Nothing),
            .Diagnostic = Coalesce(reader("oa_antecedent_diagnostic"), 0),
            .DateDebut = Coalesce(reader("oa_antecedent_date_debut"), Nothing),
            .DateFin = Coalesce(reader("oa_antecedent_date_fin"), Nothing),
            .AldId = Coalesce(reader("oa_antecedent_ald_id"), 0),
            .AldCim10Id = Coalesce(reader("oa_antecedent_ald_cim_10_id"), 0),
            .AldValide = Coalesce(reader("oa_antecedent_ald_valide"), False),
            .AldDateDebut = Coalesce(reader("oa_antecedent_ald_date_debut"), Nothing),
            .AldDateFin = Coalesce(reader("oa_antecedent_ald_date_fin"), Nothing),
            .AldDemandeEnCours = Coalesce(reader("oa_antecedent_ald_demande_en_cours"), False),
            .AldDateDemande = Coalesce(reader("oa_antecedent_ald_demande_date"), Nothing),
            .Arret = Coalesce(reader("oa_antecedent_arret"), False),
            .ArretCommentaire = Coalesce(reader("oa_antecedent_arret_commentaire"), ""),
            .Nature = Coalesce(reader("oa_antecedent_nature"), ""),
            .Priorite = Coalesce(reader("oa_antecedent_priorite"), 0),
            .Niveau = Coalesce(reader("oa_antecedent_niveau"), 0),
            .Niveau1Id = Coalesce(reader("oa_antecedent_id_niveau1"), 0),
            .Niveau2Id = Coalesce(reader("oa_antecedent_id_niveau2"), 0),
            .Ordre1 = Coalesce(reader("oa_antecedent_ordre_affichage1"), 0),
            .Ordre2 = Coalesce(reader("oa_antecedent_ordre_affichage2"), 0),
            .Ordre3 = Coalesce(reader("oa_antecedent_ordre_affichage3"), 0),
            .StatutAffichage = Coalesce(reader("oa_antecedent_statut_affichage"), ""),
            .StatutAffichageTransformation = Coalesce(reader("oa_antecedent_statut_affichage_transformation"), ""),
            .CategorieContexte = Coalesce(reader("oa_antecedent_categorie_contexte"), ""),
            .EpisodeId = Coalesce(reader("oa_episode_id"), 0),
            .Inactif = Coalesce(reader("oa_antecedent_inactif"), False)
        }
        Return antecedent
    End Function

    Public Function CloneAntecedent(Source As Antecedent) As Antecedent
        Dim Cible As New Antecedent
        Cible.Id = Source.Id
        Cible.PatientId = Source.PatientId
        Cible.Type = Source.Type
        Cible.DrcId = Source.DrcId
        Cible.Description = Source.Description
        Cible.UserCreation = Source.UserCreation
        Cible.DateCreation = Source.DateCreation
        Cible.UserModification = Source.UserModification
        Cible.DateModification = Source.DateModification
        Cible.Diagnostic = Source.Diagnostic
        Cible.DateDebut = Source.DateDebut
        Cible.DateFin = Source.DateFin
        Cible.AldId = Source.AldId
        Cible.AldCim10Id = Source.AldCim10Id
        Cible.AldValide = Source.AldValide
        Cible.AldDateDebut = Source.AldDateDebut
        Cible.AldDateFin = Source.AldDateFin
        Cible.AldDemandeEnCours = Source.AldDemandeEnCours
        Cible.AldDateDemande = Source.AldDateDemande
        Cible.Arret = Source.Arret
        Cible.ArretCommentaire = Source.ArretCommentaire
        Cible.Nature = Source.Nature
        Cible.Priorite = Source.Priorite
        Cible.Niveau = Source.Niveau
        Cible.Niveau1Id = Source.Niveau1Id
        Cible.Niveau2Id = Source.Niveau2Id
        Cible.Ordre1 = Source.Ordre1
        Cible.Ordre2 = Source.Ordre2
        Cible.Ordre3 = Source.Ordre3
        Cible.StatutAffichage = Source.StatutAffichage
        Cible.StatutAffichageTransformation = Source.StatutAffichageTransformation
        Cible.CategorieContexte = Source.CategorieContexte
        Cible.EpisodeId = Source.EpisodeId
        Cible.Inactif = Source.Inactif
        Return Cible
    End Function

    Public Function Compare(source1 As Antecedent, source2 As Antecedent) As Boolean
        If source1.Id <> source2.Id Then
            Return False
        End If
        If source1.PatientId <> source2.PatientId Then
            Return False
        End If
        If source1.Type <> source2.Type Then
            Return False
        End If
        If source1.DrcId <> source2.DrcId Then
            Return False
        End If
        If source1.Description <> source2.Description Then
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
        If source1.Diagnostic <> source2.Diagnostic Then
            Return False
        End If
        If source1.DateDebut.Date <> source2.DateDebut.Date Then
            Return False
        End If
        If source1.DateFin.Date <> source2.DateFin.Date Then
            Return False
        End If
        If source1.AldId <> source2.AldId Then
            Return False
        End If
        If source1.AldCim10Id <> source2.AldCim10Id Then
            Return False
        End If
        If source1.AldValide <> source2.AldValide Then
            Return False
        End If
        If source1.AldDateDebut.Date <> source2.AldDateDebut.Date Then
            Return False
        End If
        If source1.AldDateFin.Date <> source2.AldDateFin.Date Then
            Return False
        End If
        If source1.AldDemandeEnCours <> source2.AldDemandeEnCours Then
            Return False
        End If
        If source1.AldDateDemande.Date <> source2.AldDateDemande.Date Then
            Return False
        End If
        If source1.Arret <> source2.Arret Then
            Return False
        End If
        If source1.ArretCommentaire <> source2.ArretCommentaire Then
            Return False
        End If
        If source1.Nature <> source2.Nature Then
            Return False
        End If
        If source1.Priorite <> source2.Priorite Then
            Return False
        End If
        If source1.Niveau <> source2.Niveau Then
            Return False
        End If
        If source1.Niveau1Id <> source2.Niveau1Id Then
            Return False
        End If
        If source1.Niveau2Id <> source2.Niveau2Id Then
            Return False
        End If
        If source1.Ordre1 <> source2.Ordre1 Then
            Return False
        End If
        If source1.Ordre2 <> source2.Ordre2 Then
            Return False
        End If
        If source1.Ordre3 <> source2.Ordre3 Then
            Return False
        End If
        If source1.StatutAffichage <> source2.StatutAffichage Then
            Return False
        End If
        If source1.StatutAffichageTransformation <> source2.StatutAffichageTransformation Then
            Return False
        End If
        If source1.CategorieContexte <> source2.CategorieContexte Then
            Return False
        End If
        If source1.EpisodeId <> source2.EpisodeId Then
            Return False
        End If
        If source1.Inactif <> source2.Inactif Then
            Return False
        End If

        Return True
    End Function

    Public Function GetAllAntecedentbyPatient(patientId As Integer, publication As Boolean, parPriorite As Boolean) As DataTable
        Dim SQLString As String = "SELECT oa_antecedent_date_modification, oa_antecedent_date_creation, oa_antecedent_statut_affichage," &
                    " oa_antecedent_ald_valide, oa_antecedent_ald_date_fin, oa_antecedent_ald_demande_en_cours, oa_antecedent_diagnostic, oa_antecedent_drc_id," &
                    " oa_antecedent_description, oa_antecedent_date_debut, A.oa_ald_cim10_description, oa_antecedent_id, oa_antecedent_niveau," &
                    " oa_antecedent_id_niveau1, oa_antecedent_id_niveau2, oa_antecedent_ordre_affichage1, oa_antecedent_ordre_affichage2, oa_antecedent_ordre_affichage3, D.oa_drc_libelle" &
                    " FROM oasis.oa_antecedent" &
                    " LEFT JOIN oasis.oa_drc D ON D.oa_drc_id = oa_antecedent_drc_id" &
                    " LEFT JOIN oasis.oa_ald_cim10 A ON A.oa_ald_cim10_id = oa_antecedent_ald_cim_10_id" &
                    " WHERE oa_antecedent_type = 'A'" &
                    " AND (oa_antecedent_inactif = '0' OR oa_antecedent_inactif is Null)" &
                    " AND oa_antecedent_patient_id = " + patientId.ToString

        If publication = True Then
            If parPriorite = True Then
                SQLString += " AND oa_antecedent_statut_affichage = 'P'" &
                    " ORDER BY oa_antecedent_ordre_affichage1, oa_antecedent_ordre_affichage2, oa_antecedent_ordre_affichage3;"
            Else
                SQLString += " AND oa_antecedent_statut_affichage = 'P'" &
                    " ORDER BY oa_antecedent_date_debut"
            End If
        Else
            If parPriorite = True Then
                SQLString += " AND (oa_antecedent_statut_affichage = 'P' OR oa_antecedent_statut_affichage = 'C')" &
                    " ORDER BY oa_antecedent_ordre_affichage1, oa_antecedent_ordre_affichage2, oa_antecedent_ordre_affichage3;"
            Else
                SQLString += " AND (oa_antecedent_statut_affichage = 'P' OR oa_antecedent_statut_affichage = 'C')" &
                    " ORDER BY oa_antecedent_date_debut"
            End If
        End If

        Using con As SqlConnection = GetConnection()
            Dim AntecedentDataAdapter As SqlDataAdapter = New SqlDataAdapter()
            Using AntecedentDataAdapter
                AntecedentDataAdapter.SelectCommand = New SqlCommand(SQLString, con)
                Dim AntecedentDataTable As DataTable = New DataTable()
                Using AntecedentDataTable
                    Try
                        AntecedentDataAdapter.Fill(AntecedentDataTable)
                        Dim command As SqlCommand = con.CreateCommand()
                    Catch ex As Exception
                        Throw ex
                    End Try
                    Return AntecedentDataTable
                End Using
            End Using
        End Using
    End Function

    Public Function GetContextebyPatient(patientId As Integer, publication As Boolean) As DataTable
        Dim SQLString As String = "SELECT A.oa_antecedent_id, A.oa_antecedent_drc_id, A.oa_antecedent_description, A.oa_antecedent_diagnostic," &
            " A.oa_antecedent_statut_affichage, A.oa_antecedent_categorie_contexte, A.oa_antecedent_niveau, A.oa_antecedent_date_creation," &
            " A.oa_antecedent_date_modification, A.oa_antecedent_date_debut, A.oa_antecedent_date_fin, A.oa_antecedent_ordre_affichage1," &
            " D.oa_drc_libelle" &
            " FROM oasis.oa_antecedent A" &
            " LEFT JOIN oasis.oa_drc D ON D.oa_drc_id = A.oa_antecedent_drc_id" &
            " WHERE A.oa_antecedent_type = 'C'" &
            " AND (A.oa_antecedent_inactif = '0' OR A.oa_antecedent_inactif is Null)" &
            " AND (A.oa_antecedent_arret = '0' OR A.oa_antecedent_arret is Null)" &
            " AND A.oa_antecedent_patient_id = " + patientId.ToString

        If publication = True Then
            SQLString += " AND A.oa_antecedent_statut_affichage = 'P'" &
            " ORDER BY A.oa_antecedent_categorie_contexte DESC, A.oa_antecedent_date_modification DESC, A.oa_antecedent_id DESC;"
        Else
            SQLString += " AND (A.oa_antecedent_statut_affichage = 'P' OR A.oa_antecedent_statut_affichage = 'C')" &
            " ORDER BY A.oa_antecedent_categorie_contexte DESC, A.oa_antecedent_date_modification DESC, A.oa_antecedent_id DESC;"
        End If

        Using con As SqlConnection = GetConnection()
            Dim ContexteDataAdapter As SqlDataAdapter = New SqlDataAdapter()
            Using ContexteDataAdapter
                ContexteDataAdapter.SelectCommand = New SqlCommand(SQLString, con)
                Dim ContexteDataTable As DataTable = New DataTable()
                Using ContexteDataTable
                    Try
                        ContexteDataAdapter.Fill(ContexteDataTable)
                        Dim command As SqlCommand = con.CreateCommand()
                    Catch ex As Exception
                        Throw ex
                    End Try
                    Return ContexteDataTable
                End Using
            End Using
        End Using
    End Function

    Public Function GetListOfAntecedentPatient(patientId As Integer) As List(Of AntecedentCourrier)
        Dim ListAntecedent As New List(Of AntecedentCourrier)
        Dim antecedentDao As New AntecedentDao
        Dim dt As DataTable
        dt = antecedentDao.GetAllAntecedentbyPatient(patientId, False, True)

        Dim indentation As String
        Dim diagnostic As String

        Dim rowCount As Integer = dt.Rows.Count - 1
        For i = 0 To rowCount Step 1
            Dim antecedentCourrier As New AntecedentCourrier
            antecedentCourrier.PatientId = patientId
            antecedentCourrier.Id = dt.Rows(i)("oa_antecedent_id")

            Select Case dt.Rows(i)("oa_antecedent_niveau")
                Case 1
                    indentation = ""
                Case 2
                    indentation = "           > "
                Case 3
                    indentation = "                        >> "
                Case Else
                    indentation = ""
            End Select

            diagnostic = ""
            If dt.Rows(i)("oa_antecedent_diagnostic") IsNot DBNull.Value Then
                If CInt(dt.Rows(i)("oa_antecedent_diagnostic")) = ContexteDao.EnumDiagnostic.SUSPICION_DE Then
                    diagnostic = "Suspicion de : "
                Else
                    If CInt(dt.Rows(i)("oa_antecedent_diagnostic")) = ContexteDao.EnumDiagnostic.NOTION_DE Then
                        diagnostic = "Notion de : "
                    End If
                End If
            End If

            Dim longueurString As Integer
            Dim longueurMax As Integer = 100
            Dim antecedentDescription As String

            If dt.Rows(i)("oa_antecedent_description") Is DBNull.Value Or dt.Rows(i)("oa_antecedent_description") = "" Then
                antecedentDescription = ""
            Else
                antecedentDescription = dt.Rows(i)("oa_antecedent_description")
                antecedentDescription = Replace(antecedentDescription, vbCrLf, " ")
                longueurString = antecedentDescription.Length
                If longueurString > longueurMax Then
                    longueurString = longueurMax
                End If
                antecedentDescription = antecedentDescription.Substring(0, longueurString)
            End If

            antecedentCourrier.Description = indentation & diagnostic & " " & antecedentDescription

            ListAntecedent.Add(antecedentCourrier)
        Next

        Return ListAntecedent
    End Function

    Public Function ModificationAntecedent(antecedentUpdate As Antecedent, antecedentRead As Antecedent, userLog As Utilisateur) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim patientDao As New PatientDao
        Dim codeRetour As Boolean = True

        Dim dateModification As Date = Date.Now.Date

        Dim SQLstring As String = "update oasis.oa_antecedent set oa_antecedent_date_modification = @dateModification," &
        " oa_antecedent_utilisateur_modification = @utilisateurModification, oa_antecedent_drc_id = @drcId, oa_antecedent_description = @description," &
        " oa_antecedent_date_debut = @dateDebut, oa_antecedent_diagnostic = @diagnostic, oa_antecedent_statut_affichage = @publication," &
        " oa_antecedent_ald_id = @aldId, oa_antecedent_ald_cim_10_id = @aldCim10Id, oa_antecedent_ald_valide = @aldValide, oa_antecedent_ald_date_debut = @aldDateDebut," &
        " oa_antecedent_ald_date_fin = @aldDateFin, oa_antecedent_ald_demande_en_cours = @aldDemandeEnCours, oa_antecedent_ald_demande_date = @aldDateDemande" &
        " where oa_antecedent_id = @antecedentId"

        Dim con As SqlConnection = GetConnection()
        Dim cmd As New SqlCommand(SQLstring, con)

        If antecedentUpdate.AldId = 0 Then
            antecedentUpdate.AldCim10Id = 0
            antecedentUpdate.AldValide = False
            antecedentUpdate.AldDemandeEnCours = False
        End If

        If antecedentUpdate.AldValide = False Then
            antecedentUpdate.AldDateDebut = Date.MaxValue
            antecedentUpdate.AldDateFin = Date.MaxValue
        End If

        If antecedentUpdate.AldDemandeEnCours = False Then
            antecedentUpdate.AldDateDemande = Date.MaxValue
        End If

        With cmd.Parameters
            .AddWithValue("@utilisateurModification", userLog.UtilisateurId.ToString)
            .AddWithValue("@dateModification", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            .AddWithValue("@drcId", antecedentUpdate.DrcId)
            .AddWithValue("@description", antecedentUpdate.Description)
            .AddWithValue("@dateDebut", antecedentUpdate.DateDebut)
            .AddWithValue("@diagnostic", antecedentUpdate.Diagnostic)
            .AddWithValue("@publication", antecedentUpdate.StatutAffichage)
            .AddWithValue("@antecedentId", antecedentUpdate.Id)
            .AddWithValue("@aldId", antecedentUpdate.AldId)
            .AddWithValue("@aldCim10Id", antecedentUpdate.AldCim10Id)
            .AddWithValue("@aldValide", antecedentUpdate.AldValide)
            .AddWithValue("@aldDateDebut", antecedentUpdate.AldDateDebut)
            .AddWithValue("@aldDateFin", antecedentUpdate.AldDateFin)
            .AddWithValue("@alddemandeEnCours", antecedentUpdate.AldDemandeEnCours)
            .AddWithValue("@aldDateDemande", antecedentUpdate.AldDateDemande)
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
            Dim AntecedentHistoACreer As New AntecedentHisto
            'Initialisation classe Historisation antecedent 
            AntecedentHistoCreationDao.InitAntecedentHistorisation(antecedentRead, userLog, AntecedentHistoACreer)

            'Mise à jour des données modifiées dans l'instance de la classe Historisation antecedent
            AntecedentHistoACreer.HistorisationDate = Date.Now()
            AntecedentHistoACreer.UtilisateurId = userLog.UtilisateurId
            AntecedentHistoACreer.Etat = AntecedentHistoCreationDao.EnumEtatAntecedentHisto.ModificationAntecedent
            AntecedentHistoACreer.DrcId = antecedentUpdate.DrcId
            AntecedentHistoACreer.Description = antecedentUpdate.Description
            AntecedentHistoACreer.DateDebut = antecedentUpdate.DateDebut
            AntecedentHistoACreer.StatutAffichage = antecedentUpdate.StatutAffichage
            AntecedentHistoACreer.Diagnostic = antecedentUpdate.Diagnostic
            AntecedentHistoACreer.AldId = antecedentUpdate.AldId
            AntecedentHistoACreer.AldCim10Id = antecedentUpdate.AldCim10Id
            AntecedentHistoACreer.AldValide = antecedentUpdate.AldValide
            AntecedentHistoACreer.AldDateDebut = antecedentUpdate.AldDateDebut
            AntecedentHistoACreer.AldDateFin = antecedentUpdate.AldDateFin
            AntecedentHistoACreer.AldDemandeEnCours = antecedentUpdate.AldDemandeEnCours
            AntecedentHistoACreer.AldDateDemande = antecedentUpdate.AldDateDemande

            'Création dans l'historique des modifications de l'antecedent
            AntecedentHistoCreationDao.CreationAntecedentHisto(AntecedentHistoACreer, userLog, AntecedentHistoCreationDao.EnumEtatAntecedentHisto.ModificationAntecedent)

            'Mise à jour de la date de mise à jour de la synthèse (table patient)
            patientDao.ModificationDateMajSynthesePatient(antecedentUpdate.PatientId, userLog)
        End If

        Return codeRetour
    End Function

    Public Function AnnulationAntecedent(antecedentUpdate As Antecedent, antecedentRead As Antecedent, userLog As Utilisateur) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True

        Dim dateModification As Date = Date.Now.Date

        Dim SQLstring As String = "update oasis.oa_antecedent set oa_antecedent_date_modification = @dateModification," &
        " oa_antecedent_utilisateur_modification = @utilisateurModification, oa_antecedent_inactif = @inactif where oa_antecedent_id = @antecedentId"

        Dim con As SqlConnection = GetConnection()
        Dim cmd As New SqlCommand(SQLstring, con)

        With cmd.Parameters
            .AddWithValue("@utilisateurModification", userLog.UtilisateurId.ToString)
            .AddWithValue("@dateModification", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            .AddWithValue("@inactif", True)
            .AddWithValue("@antecedentId", antecedentUpdate.Id)
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
            Dim AntecedentHistoACreer As New AntecedentHisto
            'Initialisation classe Historisation antecedent 
            AntecedentHistoCreationDao.InitAntecedentHistorisation(antecedentRead, userLog, AntecedentHistoACreer)

            'Mise à jour des données modifiées dans l'instance de la classe Historisation antecedent
            AntecedentHistoACreer.HistorisationDate = Date.Now()
            AntecedentHistoACreer.UtilisateurId = userLog.UtilisateurId
            AntecedentHistoACreer.Etat = AntecedentHistoCreationDao.EnumEtatAntecedentHisto.AnnulationAntecedent
            AntecedentHistoACreer.Inactif = True

            'Création dans l'historique des modifications de l'antecedent
            AntecedentHistoCreationDao.CreationAntecedentHisto(AntecedentHistoACreer, userLog, AntecedentHistoCreationDao.EnumEtatAntecedentHisto.AnnulationAntecedent)

            'Mise à jour de la date de mise à jour de la synthèse (table patient)
            patientDao.ModificationDateMajSynthesePatient(antecedentUpdate.PatientId, userLog)
        End If

        Return codeRetour
    End Function

    Public Function CreationAntecedent(antecedentUpdate As Antecedent, userLog As Utilisateur) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim antecedentId As Long

        Dim dateCreation As Date = Date.Now.Date

        Dim SQLstring As String = "insert into oasis.oa_antecedent (oa_antecedent_patient_id, oa_antecedent_type, oa_antecedent_drc_id, oa_antecedent_description," &
        " oa_antecedent_date_creation, oa_antecedent_utilisateur_creation, oa_antecedent_utilisateur_modification, oa_antecedent_date_debut, oa_antecedent_niveau," &
        " oa_antecedent_nature, oa_antecedent_statut_affichage, oa_antecedent_inactif, oa_antecedent_ordre_affichage1, oa_antecedent_ordre_affichage2," &
        " oa_antecedent_ordre_affichage3, oa_antecedent_diagnostic," &
        " oa_antecedent_ald_id, oa_antecedent_ald_cim_10_id, oa_antecedent_ald_valide, oa_antecedent_ald_date_debut," &
        " oa_antecedent_ald_date_fin, oa_antecedent_ald_demande_en_cours, oa_antecedent_ald_demande_date)" &
        " VALUES (@patientId, @type, @drcId, @description, @dateCreation, @utilisateurCreation," &
        " @utilisateurModification, @dateDebut, @niveau, @nature, @publication, @inactif, @ordreAffichage1, @ordreAffichage2, @ordreAffichage3, @diagnostic," &
        " @aldId, @aldCim10Id, @aldValide, @aldDateDebut, @aldDateFin, @aldDemandeEnCours, @aldDateDemande); SELECT SCOPE_IDENTITY()"

        Dim con As SqlConnection = GetConnection()
        Dim cmd As New SqlCommand(SQLstring, con)

        If antecedentUpdate.AldId = 0 Then
            antecedentUpdate.AldCim10Id = 0
            antecedentUpdate.AldValide = False
            antecedentUpdate.AldDemandeEnCours = False
        End If

        If antecedentUpdate.AldValide = False Then
            antecedentUpdate.AldDateDebut = Date.MaxValue
            antecedentUpdate.AldDateFin = Date.MaxValue
        End If

        If antecedentUpdate.AldDemandeEnCours = False Then
            antecedentUpdate.AldDateDemande = Date.MaxValue
        End If

        With cmd.Parameters
            .AddWithValue("@patientId", antecedentUpdate.PatientId)
            .AddWithValue("@type", "A")
            .AddWithValue("@drcId", antecedentUpdate.DrcId)
            .AddWithValue("@description", antecedentUpdate.Description)
            .AddWithValue("@dateCreation", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            .AddWithValue("@utilisateurCreation", userLog.UtilisateurId.ToString)
            .AddWithValue("@utilisateurModification", 0)
            .AddWithValue("@dateDebut", antecedentUpdate.DateDebut)
            .AddWithValue("@niveau", 1)
            .AddWithValue("@nature", "Patient")
            .AddWithValue("@publication", antecedentUpdate.StatutAffichage)
            .AddWithValue("@inactif", 0)
            .AddWithValue("@ordreAffichage1", 980)
            .AddWithValue("@ordreAffichage2", 0)
            .AddWithValue("@ordreAffichage3", 0)
            .AddWithValue("@diagnostic", antecedentUpdate.Diagnostic)
            .AddWithValue("@aldId", antecedentUpdate.AldId)
            .AddWithValue("@aldCim10Id", antecedentUpdate.AldCim10Id)
            .AddWithValue("@aldValide", antecedentUpdate.AldValide)
            .AddWithValue("@aldDateDebut", antecedentUpdate.AldDateDebut)
            .AddWithValue("@aldDateFin", antecedentUpdate.AldDateFin)
            .AddWithValue("@aldDemandeEnCours", antecedentUpdate.AldDemandeEnCours)
            .AddWithValue("@aldDateDemande", antecedentUpdate.AldDateDemande)
        End With

        Try
            da.InsertCommand = cmd
            antecedentId = da.InsertCommand.ExecuteScalar()
        Catch ex As Exception
            Throw New Exception(ex.Message)
            codeRetour = False
        Finally
            con.Close()
        End Try

        If codeRetour = True Then
            Dim AntecedentHistoACreer As New AntecedentHisto

            'Mise à jour des données dans l'instance de la classe Historisation antecedent
            AntecedentHistoACreer.AntecedentId = antecedentId 'Récupération de l'id créé
            AntecedentHistoACreer.HistorisationDate = DateTime.Now()
            AntecedentHistoACreer.UtilisateurId = userLog.UtilisateurId
            AntecedentHistoACreer.Etat = AntecedentHistoCreationDao.EnumEtatAntecedentHisto.CreationAntecedent
            AntecedentHistoACreer.PatientId = antecedentUpdate.PatientId
            AntecedentHistoACreer.DrcId = antecedentUpdate.DrcId
            AntecedentHistoACreer.Type = "A"
            AntecedentHistoACreer.Description = antecedentUpdate.Description
            AntecedentHistoACreer.DateDebut = antecedentUpdate.DateDebut
            AntecedentHistoACreer.Niveau = 1
            AntecedentHistoACreer.Nature = "Patient"
            AntecedentHistoACreer.StatutAffichage = antecedentUpdate.StatutAffichage
            AntecedentHistoACreer.Inactif = 0
            AntecedentHistoACreer.Ordre1 = 980
            AntecedentHistoACreer.Ordre2 = 0
            AntecedentHistoACreer.Ordre3 = 0
            AntecedentHistoACreer.Diagnostic = antecedentUpdate.Diagnostic
            AntecedentHistoACreer.AldId = antecedentUpdate.AldId
            AntecedentHistoACreer.AldCim10Id = antecedentUpdate.AldCim10Id
            AntecedentHistoACreer.AldValide = antecedentUpdate.AldValide
            AntecedentHistoACreer.AldDateDebut = antecedentUpdate.AldDateDebut
            AntecedentHistoACreer.AldDateFin = antecedentUpdate.AldDateFin
            AntecedentHistoACreer.AldDemandeEnCours = antecedentUpdate.AldDemandeEnCours
            AntecedentHistoACreer.AldDateDemande = antecedentUpdate.AldDateDemande

            'Création dans l'historique des antecedents du antecedent créé
            AntecedentHistoCreationDao.CreationAntecedentHisto(AntecedentHistoACreer, userLog, AntecedentHistoCreationDao.EnumEtatAntecedentHisto.CreationAntecedent)

            'Mise à jour de la date de mise à jour de la synthèse (table patient)
            patientDao.ModificationDateMajSynthesePatient(antecedentUpdate.PatientId, userLog)
        End If

        Return codeRetour

    End Function






    '================================================================================================================================================================
    'Fonctions obsolètes ================================>
    '================================================================================================================================================================

    'Réactivation des antécédents liés à un antécédent réactivé
    Private Function TraitementReactivationAntecedentLies(AntecedentId As Integer, userLog As Utilisateur) As Boolean
        Dim codeRetour As Boolean = True
        'Déclaration des données de connexion
        Dim conxn As New SqlConnection(GetConnectionString())
        Dim antecedentDataAdapter As SqlDataAdapter = New SqlDataAdapter()
        Dim antecedentDataTable As DataTable = New DataTable()
        Dim SQLString As String

        SQLString = "select oasis.oa_antecedent_id from oasis.oa_antecedent where oa_antecedent_type = 'A' and" &
        " (oa_antecedent_id_niveau1 = " + AntecedentId.ToString + " Or oa_antecedent_id_niveau2 = " + AntecedentId.ToString + ");"

        'Lecture des données en base
        antecedentDataAdapter.SelectCommand = New SqlCommand(SQLString, conxn)
        antecedentDataAdapter.Fill(antecedentDataTable)
        conxn.Open()

        'Déclaration des variables pour réaliser le parcours du DataTable pour alimenter le DataGridView
        Dim i As Integer
        Dim rowCount As Integer = antecedentDataTable.Rows.Count - 1

        'Parcours du DataTable pour réactiver les antécédents liés
        For i = 0 To rowCount Step 1
            'Traitement de réactivation des antécédents liés
            ReactivationAntecedent(antecedentDataTable.Rows(i)("oa_antecedent_id"), userLog)
        Next

        'Libération des ressources
        conxn.Close()
        antecedentDataAdapter.Dispose()

        Return codeRetour
    End Function

    'Réactivation d'un antécédent en contexte médical
    Private Function ReactivationAntecedent(antecedentId As Integer, userLog As Utilisateur) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True

        Dim dateModification As Date = Date.Now.Date

        Dim SQLstring As String = "update oasis.oa_antecedent set oa_antecedent_type = 'C', oa_antecedent_date_modification = @dateModification," &
        " oa_antecedent_utilisateur_modification = @utilisateurModification, oa_antecedent_date_fin = @dateFin, oa_antecedent_nature = @nature," &
        " oa_antecedent_priorite = @priorite, oa_antecedent_niveau = @niveau, oa_antecedent_id_niveau1 = @idNiveau1, oa_antecedent_id_niveau2 = @idNiveau2," &
        " oa_antecedent_ordre_affichage1 = @ordreAffichage1, oa_antecedent_ordre_affichage2 = @ordreAffichage2, oa_antecedent_ordre_affichage3 = @ordreAffichage3" &
        " where oa_antecedent_id = @antecedentId"

        Dim con As SqlConnection = GetConnection()
        Dim cmd As New SqlCommand(SQLstring, con)

        With cmd.Parameters
            .AddWithValue("@utilisateurModification", userLog.UtilisateurId.ToString)
            .AddWithValue("@dateModification", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            .AddWithValue("@dateFin", New Date(2999, 12, 31, 0, 0, 0).ToString("yyyy-MM-dd HH:mm:ss"))
            .AddWithValue("@nature", "")
            .AddWithValue("@priorite", 0)
            .AddWithValue("@niveau", 1)
            .AddWithValue("@idNiveau1", 0)
            .AddWithValue("@idNiveau2", 0)
            .AddWithValue("@ordreAffichage1", 990)
            .AddWithValue("@ordreAffichage2", 0)
            .AddWithValue("@ordreAffichage3", 0)
            .AddWithValue("@antecedentId", antecedentId)
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
            Dim AntecedentHistoACreer As New AntecedentHisto
            'Initialisation classe Historisation antecedent 
            Dim antecedentdao As New AntecedentDao
            Dim antecedentRead As Antecedent = antecedentdao.GetAntecedentById(antecedentId)
            AntecedentHistoCreationDao.InitAntecedentHistorisation(antecedentRead, userLog, AntecedentHistoACreer)

            'Mise à jour des données modifiées dans l'instance de la classe Historisation antecedent
            AntecedentHistoACreer.HistorisationDate = Date.Now()
            AntecedentHistoACreer.Type = "C"
            AntecedentHistoACreer.UtilisateurId = userLog.UtilisateurId
            AntecedentHistoACreer.Etat = AntecedentHistoCreationDao.EnumEtatAntecedentHisto.ReactivationAntecedent
            AntecedentHistoACreer.Nature = ""
            AntecedentHistoACreer.Niveau = 1
            AntecedentHistoACreer.Niveau1Id = 0
            AntecedentHistoACreer.Niveau2Id = 0
            AntecedentHistoACreer.Ordre1 = 980
            AntecedentHistoACreer.Ordre2 = 0
            AntecedentHistoACreer.Ordre3 = 0

            'Création dans l'historique des modifications de l'antecedent
            AntecedentHistoCreationDao.CreationAntecedentHisto(AntecedentHistoACreer, userLog, AntecedentHistoCreationDao.EnumEtatAntecedentHisto.ReactivationAntecedent)

            'Mise à jour de la date de mise à jour de la synthèse (table patient)
            patientDao.ModificationDateMajSynthesePatient(antecedentRead.PatientId, userLog)
        End If

        Return codeRetour
    End Function

    'Modification de la publication d'un antécédent en base de données
    Private Function ModificationPublicationAntecedent(antecedentId As Integer, publication As String, userLog As Utilisateur) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True

        Dim dateModification As Date = Date.Now.Date

        Dim SQLstring As String = "update oasis.oa_antecedent set oa_antecedent_date_modification = @dateModification," &
        " oa_antecedent_utilisateur_modification = @utilisateurModification, oa_antecedent_statut_affichage = @publication" &
        " where oa_antecedent_id = @antecedentId"

        Dim con As SqlConnection = GetConnection()
        Dim cmd As New SqlCommand(SQLstring, con)

        With cmd.Parameters
            .AddWithValue("@utilisateurModification", userLog.UtilisateurId.ToString)
            .AddWithValue("@dateModification", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            .AddWithValue("@publication", publication)
            .AddWithValue("@antecedentId", antecedentId.ToString)
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
            Dim AntecedentHistoACreer As New AntecedentHisto
            'Initialisation classe Historisation antecedent 
            Dim antecedentdao As New AntecedentDao
            Dim antecedentRead As Antecedent = antecedentdao.GetAntecedentById(antecedentId)

            'Mise à jour des données modifiées dans l'instance de la classe Historisation antecedent
            AntecedentHistoACreer.HistorisationDate = Date.Now()
            AntecedentHistoACreer.UtilisateurId = userLog.UtilisateurId
            AntecedentHistoACreer.Etat = AntecedentHistoCreationDao.EnumEtatAntecedentHisto.ModificationAntecedent
            AntecedentHistoACreer.StatutAffichage = publication

            'Création dans l'historique des modifications de l'antecedent
            AntecedentHistoCreationDao.CreationAntecedentHisto(AntecedentHistoACreer, userLog, AntecedentHistoCreationDao.EnumEtatAntecedentHisto.ModificationAntecedent)

            'Mise à jour de la date de mise à jour de la synthèse (table patient)
            patientDao.ModificationDateMajSynthesePatient(antecedentRead.PatientId, userLog)
        End If

        Return codeRetour
    End Function


    'Occultation des antécédents liés à un antécédent réactivé 
    Private Function TraitementOccultationAntecedentLies(AntecedentId As Integer, userLog As Utilisateur) As Boolean
        Dim codeRetour As Boolean = True
        'Déclaration des données de connexion
        Dim conxn As New SqlConnection(GetConnectionString())
        Dim antecedentDataAdapter As SqlDataAdapter = New SqlDataAdapter()
        Dim antecedentDataTable As DataTable = New DataTable()
        Dim SQLString As String

        SQLString = "Select oa_antecedent_id from oasis.oa_antecedent where oa_antecedent_type = 'A' and (oa_antecedent_id_niveau1 = " & AntecedentId.ToString &
            " or oa_antecedent_id_niveau2 = " + AntecedentId.ToString + ");"

        'Lecture des données en base
        antecedentDataAdapter.SelectCommand = New SqlCommand(SQLString, conxn)
        antecedentDataAdapter.Fill(antecedentDataTable)
        conxn.Open()

        'Déclaration des variables pour réaliser le parcours du DataTable pour alimenter le DataGridView
        Dim i As Integer
        Dim rowCount As Integer = antecedentDataTable.Rows.Count - 1

        'Parcours du DataTable pour réactiver les antécédents liés
        For i = 0 To rowCount Step 1
            'Traitement de réactivation des antécédents liés
            ModificationPublicationAntecedent(antecedentDataTable.Rows(i)("oa_antecedent_id"), "O", userLog)
        Next

        'Libération des ressources
        conxn.Close()
        antecedentDataAdapter.Dispose()

        Return codeRetour
    End Function


End Class
