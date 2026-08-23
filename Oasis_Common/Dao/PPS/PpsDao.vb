Imports System.Data.SqlClient

Public Class PpsDao
    Inherits StandardDao

    Dim patientDao As New PatientDao

    Public Function getAllPPSbyPatient(patientId As Integer, Optional filter As String = "") As DataTable
        Dim SQLString As String

        SQLString = "Select oa_r_pps_categorie_id, oa_r_pps_sous_categorie_id, oa_r_pps_sous_categorie_type," &
        " oa_pps_id, oa_pps_drc_id, oa_pps_commentaire, oa_pps_drc_id, oa_pps_date_debut, oa_pps_date_creation, oa_pps_date_modification," &
        " oa_pps_arret, oa_pps_date_fin, " &
        " oa_parcours_id, oa_parcours_specialite, oa_parcours_ror_id, oa_parcours_base, oa_parcours_rythme, oa_parcours_commentaire, oa_parcours_date_creation," &
        " oa_parcours_date_modification, oa_parcours_cacher" &
        " From oasis.oasis.oa_r_pps_sous_categorie" &
        " Left outer join oasis.oasis.oa_patient_pps On oa_r_pps_categorie_id = oa_pps_categorie And oa_r_pps_sous_categorie_id = oa_pps_sous_categorie" &
        " Left outer join oasis.oasis.oa_patient_parcours on oa_r_pps_categorie_id = oa_parcours_categorie_id And oa_r_pps_sous_categorie_id = oa_parcours_sous_categorie_id" &
        " Where (((oa_pps_inactif = 0 Or oa_pps_inactif Is NULL) And oa_pps_patient_id = " & patientId.ToString & ") Or" &
        " ((oa_parcours_inactif = 0 Or oa_parcours_inactif Is NULL) And oa_parcours_patient_id = " & patientId.ToString & "))" &
        filter &
        " order by oa_r_pps_sous_categorie_ordre_affichage, oa_pps_priorite"

        Using con As SqlConnection = GetConnection()
            Dim PPSDataAdapter As SqlDataAdapter = New SqlDataAdapter()
            Using PPSDataAdapter
                PPSDataAdapter.SelectCommand = New SqlCommand(SQLString, con)
                Dim PPSDataTable As DataTable = New DataTable()
                Using PPSDataTable
                    Try
                        PPSDataAdapter.Fill(PPSDataTable)
                        Dim command As SqlCommand = con.CreateCommand()
                    Catch ex As Exception
                        Throw ex
                    End Try
                    Return PPSDataTable
                End Using
            End Using
        End Using
    End Function

    Public Function getPpsById(ppsId As Integer) As Pps
        Dim pps As Pps
        Dim con As SqlConnection = GetConnection()
        Try
            Dim command As SqlCommand = con.CreateCommand()
            command.CommandText =
                "select * from oasis.oa_patient_pps where oa_pps_id = @id"
            command.Parameters.AddWithValue("@id", ppsId)
            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    pps = BuildBean(reader)
                Else
                    Throw New ArgumentException("PPS inexistant !")
                End If
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try

        Return pps
    End Function

    Public Function getPpsObjectifByPatientId(patientId As Integer) As Pps
        Dim pps As Pps
        Dim con As SqlConnection = GetConnection()
        Try
            Dim command As SqlCommand = con.CreateCommand()
            command.CommandText =
                "select * from oasis.oa_patient_pps where oa_pps_categorie = 1 and oa_pps_sous_categorie = 1 and oa_pps_patient_id = @id"
            command.Parameters.AddWithValue("@id", patientId)
            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    pps = BuildBean(reader)
                Else
                    Throw New ArgumentException("PPS inexistant !")
                End If
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try
        Return pps
    End Function

    Public Function ExistPPSObjectifByPatientId(patientId As Integer) As Boolean
        Dim con As SqlConnection = GetConnection()
        Try
            Dim command As SqlCommand = con.CreateCommand()
            command.CommandText =
                "SELECT oa_pps_id FROM oasis.oasis.oa_patient_pps" &
                " WHERE oa_pps_patient_id = @id and oa_pps_categorie = 1 and oa_pps_sous_categorie = 1 and (oa_pps_inactif is NULL or oa_pps_inactif = 0)"
            command.Parameters.AddWithValue("@id", patientId)
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

    Private Function BuildBean(reader As SqlDataReader) As Pps
        Dim pps As New Pps With {
            .Id = reader("oa_pps_id"),
            .PatientId = Coalesce(reader("oa_pps_patient_id"), 0),
            .CategorieId = Coalesce(reader("oa_pps_categorie"), 0),
            .SousCategorieId = Coalesce(reader("oa_pps_sous_categorie"), 0),
            .Priorite = Coalesce(reader("oa_pps_priorite"), 0),
            .DrcId = Coalesce(reader("oa_pps_drc_id"), 0),
            .AffichageSynthese = Coalesce(reader("oa_pps_affichage_synthese"), False),
            .Commentaire = Coalesce(reader("oa_pps_commentaire"), ""),
            .DateDebut = Coalesce(reader("oa_pps_date_debut"), Nothing),
            .DateFin = Coalesce(reader("oa_pps_date_fin"), Nothing),
            .Arret = Coalesce(reader("oa_pps_arret"), False),
            .ArretCommentaire = Coalesce(reader("oa_pps_commentaire_arret"), ""),
            .UserCreation = Coalesce(reader("oa_pps_utilisateur_creation"), 0),
            .DateCreation = Coalesce(reader("oa_pps_date_creation"), Nothing),
            .UserModification = Coalesce(reader("oa_pps_utilisateur_modification"), 0),
            .DateModification = Coalesce(reader("oa_pps_date_modification"), Nothing)
        }
        Return pps
    End Function

    Public Function getAllPPSStrategiePatient(patientId As Integer) As DataTable
        Dim SQLString As String = "select * from oasis.oa_patient_pps where (oa_pps_inactif is Null or oa_pps_inactif = 0) and" &
        " oa_pps_categorie = 4 And oa_pps_patient_id = " & patientId.ToString & " order by oa_pps_priorite;"

        Using con As SqlConnection = GetConnection()
            Dim PPSDataAdapter As SqlDataAdapter = New SqlDataAdapter()
            Using PPSDataAdapter
                PPSDataAdapter.SelectCommand = New SqlCommand(SQLString, con)
                Dim PPSDataTable As DataTable = New DataTable()
                Using PPSDataTable
                    Try
                        PPSDataAdapter.Fill(PPSDataTable)
                        Dim command As SqlCommand = con.CreateCommand()
                    Catch ex As Exception
                        Throw ex
                    End Try
                    Return PPSDataTable
                End Using
            End Using
        End Using
    End Function

    Public Function getAllPPSSuivibyPatient(patientId As Integer) As DataTable
        Dim SQLString As String = "select * from oasis.oa_patient_pps where (oa_pps_inactif is Null or oa_pps_inactif = 0)" &
            " And (oa_pps_affichage_synthese Is Null Or oa_pps_affichage_synthese = 1)" &
            " And oa_pps_categorie = 2 And oa_pps_sous_categorie <> 2" &
            " And oa_pps_patient_id = " & patientId.ToString & " order by oa_pps_sous_categorie"

        Using con As SqlConnection = GetConnection()
            Dim PPSDataAdapter As SqlDataAdapter = New SqlDataAdapter()
            Using PPSDataAdapter
                PPSDataAdapter.SelectCommand = New SqlCommand(SQLString, con)
                Dim PPSDataTable As DataTable = New DataTable()
                Using PPSDataTable
                    Try
                        PPSDataAdapter.Fill(PPSDataTable)
                        Dim command As SqlCommand = con.CreateCommand()
                    Catch ex As Exception
                        Throw ex
                    End Try
                    Return PPSDataTable
                End Using
            End Using
        End Using
    End Function

    Public Function getAllPPSPreventionbyPatient(patientId As Integer) As DataTable
        Dim SQLString As String = "select * from oasis.oa_patient_pps where (oa_pps_inactif is Null or oa_pps_inactif = 0)" &
        " and oa_pps_categorie = 2 and oa_pps_sous_categorie = 2 and oa_pps_patient_id = " & patientId.ToString &
        " order by oa_pps_priorite"

        Using con As SqlConnection = GetConnection()
            Dim PPSDataAdapter As SqlDataAdapter = New SqlDataAdapter()
            Using PPSDataAdapter
                PPSDataAdapter.SelectCommand = New SqlCommand(SQLString, con)
                Dim PPSDataTable As DataTable = New DataTable()
                Using PPSDataTable
                    Try
                        PPSDataAdapter.Fill(PPSDataTable)
                        Dim command As SqlCommand = con.CreateCommand()
                    Catch ex As Exception
                        Throw ex
                    End Try
                    Return PPSDataTable
                End Using
            End Using
        End Using
    End Function

    Public Function CreationPPS(pps As Pps, userLog As Utilisateur) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim ppsId As Long

        Dim SQLstring As String = "insert into oasis.oa_patient_pps" &
        " (oa_pps_patient_id, oa_pps_categorie, oa_pps_sous_categorie, oa_pps_priorite, oa_pps_drc_id, oa_pps_commentaire," &
        " oa_pps_utilisateur_creation, oa_pps_date_creation, oa_pps_affichage_synthese, oa_pps_date_fin)" &
        " VALUES (@patientId, @categorie, @sousCategorie, @priorite, @drcId, @commentaire, @utilisateurCreation, @dateCreation, @affichageSynthese, @dateFin); SELECT SCOPE_IDENTITY()"

        Dim con As SqlConnection = GetConnection()
        Dim cmd As New SqlCommand(SQLstring, con)

        With cmd.Parameters
            .AddWithValue("@patientId", pps.PatientId)
            .AddWithValue("@categorie", pps.CategorieId)
            .AddWithValue("@sousCategorie", pps.SousCategorieId)
            .AddWithValue("@priorite", pps.Priorite.ToString)
            .AddWithValue("@drcId", pps.DrcId)
            .AddWithValue("@commentaire", pps.Commentaire)
            .AddWithValue("@utilisateurCreation", userLog.UtilisateurId.ToString)
            .AddWithValue("@dateCreation", Date.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            .AddWithValue("@dateFin", pps.DateFin)
            .AddWithValue("@affichageSynthese", 1)
        End With

        Try
            da.InsertCommand = cmd
            ppsId = da.InsertCommand.ExecuteScalar()
        Catch ex As Exception
            Throw New Exception(ex.Message)
            codeRetour = False
        Finally
            con.Close()
        End Try

        If codeRetour = True Then
            'Mise à jour des données dans l'instance de la classe Historisation antecedent
            Dim PPSHistoACreer As New PpsHisto With {
                .PpsId = ppsId, 'Récupération de l'id de l'occurrence créée
                .HistorisationDate = DateTime.Now(),
                .HistorisationUtilisateurId = userLog.UtilisateurId,
                .HistorisationEtat = PpsHisto.EnumEtatPPSHisto.Creation,
                .PatientId = pps.PatientId,
                .Categorie = pps.CategorieId,
                .SousCategorie = pps.SousCategorieId,
                .Priorite = pps.Priorite,
                .DrcId = pps.DrcId,
                .Commentaire = pps.Commentaire,
                .Inactif = 0,
                .AffichageSynthese = 1
            }

            'Lecture de l'antecedent créé avec toutes ses données pour communiquer le DataReader à la fonction dédiée
            Dim PPSCreeDataReader As SqlDataReader
            SQLstring = "Select * from oasis.oa_patient_pps where oa_pps_id = " & PPSHistoACreer.PpsId & ";"
            Dim antecedentCreeCommand As New SqlCommand(SQLstring, con)
            con.Open()
            PPSCreeDataReader = antecedentCreeCommand.ExecuteReader()
            If PPSCreeDataReader.Read() Then
                'Initialisation classe Historisation PPS
                InitClassePPStHistorisation(PPSCreeDataReader, userLog, PPSHistoACreer)

                'Libération des ressources d'accès aux données
                con.Close()
                antecedentCreeCommand.Dispose()
            End If

            'Création dans l'historique des PPS du PPS créé
            CreationPPSHisto(PPSHistoACreer, userLog, PpsHisto.EnumEtatPPSHisto.Creation)

            'Mise à jour de la date de mise à jour de la synthèse (table patient)
            patientDao.ModificationDateMajSynthesePatient(pps.PatientId, userLog)
        End If

        Return codeRetour
    End Function

    Public Function ModificationPPS(pps As Pps, userLog As Utilisateur) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim dateModification As Date = Date.Now.Date
        Dim SQLstring As String = "update oasis.oa_patient_pps set oa_pps_sous_categorie = @sousCategorie, oa_pps_date_modification = @dateModification," &
        " oa_pps_utilisateur_modification = @utilisateurModification, oa_pps_priorite = @priorite, oa_pps_drc_id = @drcId," &
        " oa_pps_commentaire = @commentaire, oa_pps_date_fin = @dateFin where oa_pps_id = @ppsId"

        Dim con As SqlConnection = GetConnection()
        Dim cmd As New SqlCommand(SQLstring, con)

        With cmd.Parameters
            .AddWithValue("@utilisateurModification", userLog.UtilisateurId)
            .AddWithValue("@sousCategorie", pps.SousCategorieId)
            .AddWithValue("@dateModification", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            .AddWithValue("@priorite", pps.Priorite)
            .AddWithValue("@commentaire", pps.Commentaire)
            .AddWithValue("@drcId", pps.DrcId)
            .AddWithValue("@dateFin", If(pps.DateFin Is Nothing, DBNull.Value, pps.DateFin))
            .AddWithValue("@ppsId", pps.Id)
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
            Dim PPSHistoACreer As New PpsHisto
            'Mise à jour des données modifiées dans l'instance de la classe Historisation antecedent
            PPSHistoACreer.HistorisationDate = Date.Now()
            PPSHistoACreer.HistorisationUtilisateurId = userLog.UtilisateurId
            PPSHistoACreer.HistorisationEtat = PpsHisto.EnumEtatPPSHisto.Modification
            PPSHistoACreer.Categorie = pps.CategorieId
            PPSHistoACreer.SousCategorie = pps.SousCategorieId
            PPSHistoACreer.Priorite = pps.Priorite
            PPSHistoACreer.PatientId = pps.PatientId
            PPSHistoACreer.PpsId = pps.Id
            PPSHistoACreer.Commentaire = pps.Commentaire
            PPSHistoACreer.Inactif = False
            PPSHistoACreer.AffichageSynthese = True
            PPSHistoACreer.DrcId = pps.DrcId

            'Création dans l'historique des modifications de l'antecedent
            CreationPPSHisto(PPSHistoACreer, userLog, PpsHisto.EnumEtatPPSHisto.Modification)

            'Mise à jour de la date de mise à jour de la synthèse (table patient)
            patientDao.ModificationDateMajSynthesePatient(pps.PatientId, userLog)
        End If

        Return codeRetour
    End Function

    Public Function AnnulationPrevention(pps As Pps, userLog As Utilisateur) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim dateModification As Date = Date.Now.Date
        Dim SQLstring As String = "update oasis.oa_patient_pps set oa_pps_date_modification = @dateModification," &
        " oa_pps_utilisateur_modification = @utilisateurModification, oa_pps_inactif = @inactif, oa_pps_arret = @arret," &
        " oa_pps_commentaire_arret = @commentaireArret where oa_pps_id = @ppsId"

        Dim con As SqlConnection = GetConnection()
        Dim cmd As New SqlCommand(SQLstring, con)

        With cmd.Parameters
            .AddWithValue("@utilisateurModification", userLog.UtilisateurId.ToString)
            .AddWithValue("@dateModification", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            .AddWithValue("@inactif", True)
            .AddWithValue("@ppsId", pps.Id)
            .AddWithValue("@arret", True)
            .AddWithValue("@commentaireArret", pps.ArretCommentaire)
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
            'Mise à jour des données modifiées dans l'instance de la classe Historisation antecedent
            Dim PPSHistoACreer As New PpsHisto With {
                .HistorisationDate = Date.Now(),
                .HistorisationUtilisateurId = userLog.UtilisateurId,
                .HistorisationEtat = PpsHisto.EnumEtatPPSHisto.Annulation,
                .Categorie = pps.CategorieId,
                .SousCategorie = pps.SousCategorieId,
                .Priorite = pps.Priorite,
                .PatientId = pps.PatientId,
                .PpsId = pps.Id,
                .Commentaire = pps.Commentaire,
                .Arret = True,
                .ArretCommentaire = pps.ArretCommentaire,
                .Inactif = True,
                .DrcId = pps.DrcId
            }

            'Création dans l'historique des modifications de l'antecedent
            CreationPPSHisto(PPSHistoACreer, userLog, PpsHisto.EnumEtatPPSHisto.Annulation)

            'Mise à jour de la date de mise à jour de la synthèse (table patient)
            patientDao.ModificationDateMajSynthesePatient(pps.PatientId, userLog)
        End If

        Return codeRetour
    End Function

    Public Function Compare(source1 As Pps, source2 As Pps) As Boolean
        If source1.AffichageSynthese <> source2.AffichageSynthese Then
            Return False
        End If

        If source1.Arret <> source2.Arret Then
            Return False
        End If

        If source1.ArretCommentaire <> source2.ArretCommentaire Then
            Return False
        End If

        If source1.CategorieId <> source2.CategorieId Then
            Return False
        End If

        If source1.Commentaire <> source2.Commentaire Then
            Return False
        End If

        If source1.DateDebut <> source2.DateDebut Then
            Return False
        End If

        If source1.DateFin <> source2.DateFin Then
            Return False
        End If

        If source1.DrcId <> source2.DrcId Then
            Return False
        End If

        If source1.Inactif <> source2.Inactif Then
            Return False
        End If

        If source1.PatientId <> source2.PatientId Then
            Return False
        End If

        If source1.Priorite <> source2.Priorite Then
            Return False
        End If

        If source1.SousCategorieId <> source2.SousCategorieId Then
            Return False
        End If

        If source1.SpecialiteId <> source2.SpecialiteId Then
            Return False
        End If

        Return True
    End Function

    Public Function DeterminationTypeStrategie(TypeStrategieString As String) As Integer
        Dim typeStrategie As Integer

        Select Case TypeStrategieString
            Case "Prophylactique"
                typeStrategie = 7
            Case "Sociale"
                typeStrategie = 8
            Case "Symptomatique"
                typeStrategie = 9
            Case "Curative"
                typeStrategie = 10
            Case "Diagnostique"
                typeStrategie = 11
            Case "Palliative"
                typeStrategie = 12
            Case Else
                typeStrategie = 0
        End Select

        Return typeStrategie

    End Function

End Class
