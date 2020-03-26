Imports System.Data.SqlClient
Imports Oasis_Common
Public Class AntecedentDao
    Inherits StandardDao

    Public Structure EnumTypeAntecedentContexte
        Const ANTECEDENT = "A"
        Const CONTEXTE = "C"
    End Structure

    Public Structure EnumStatutAffichage
        Const PUBLIE = "P"
        Const CACHE = "C"
        Const OCCULTE = "O"
    End Structure

    Friend Function GetAntecedentById(antecedentId As Integer) As Antecedent
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
        Dim antecedent As New Antecedent

        antecedent.Id = reader("oa_antecedent_id")
        antecedent.PatientId = Coalesce(reader("oa_antecedent_patient_id"), 0)
        antecedent.Type = Coalesce(reader("oa_antecedent_type"), "")
        antecedent.DrcId = Coalesce(reader("oa_antecedent_drc_id"), 0)
        antecedent.Description = Coalesce(reader("oa_antecedent_description"), "")
        antecedent.UserCreation = Coalesce(reader("oa_antecedent_utilisateur_creation"), 0)
        antecedent.DateCreation = Coalesce(reader("oa_antecedent_date_creation"), Nothing)
        antecedent.UserModification = Coalesce(reader("oa_antecedent_utilisateur_modification"), 0)
        antecedent.DateModification = Coalesce(reader("oa_antecedent_date_modification"), Nothing)
        antecedent.Diagnostic = Coalesce(reader("oa_antecedent_diagnostic"), 0)
        antecedent.DateDebut = Coalesce(reader("oa_antecedent_date_debut"), Nothing)
        antecedent.DateFin = Coalesce(reader("oa_antecedent_date_fin"), Nothing)
        antecedent.AldId = Coalesce(reader("oa_antecedent_ald_id"), 0)
        antecedent.AldCim10Id = Coalesce(reader("oa_antecedent_ald_cim_10_id"), 0)
        antecedent.AldValide = Coalesce(reader("oa_antecedent_ald_valide"), False)
        antecedent.AldDateDebut = Coalesce(reader("oa_antecedent_ald_date_debut"), Nothing)
        antecedent.AldDateFin = Coalesce(reader("oa_antecedent_ald_date_fin"), Nothing)
        antecedent.AldDemandeEnCours = Coalesce(reader("oa_antecedent_ald_demande_en_cours"), False)
        antecedent.AldDateDemande = Coalesce(reader("oa_antecedent_ald_demande_date"), Nothing)
        antecedent.Arret = Coalesce(reader("oa_antecedent_arret"), False)
        antecedent.ArretCommentaire = Coalesce(reader("oa_antecedent_arret_commentaire"), "")
        antecedent.Nature = Coalesce(reader("oa_antecedent_nature"), "")
        antecedent.Priorite = Coalesce(reader("oa_antecedent_priorite"), 0)
        antecedent.Niveau = Coalesce(reader("oa_antecedent_niveau"), 0)
        antecedent.Niveau1Id = Coalesce(reader("oa_antecedent_id_niveau1"), 0)
        antecedent.Niveau2Id = Coalesce(reader("oa_antecedent_id_niveau2"), 0)
        antecedent.Ordre1 = Coalesce(reader("oa_antecedent_ordre_affichage1"), 0)
        antecedent.Ordre2 = Coalesce(reader("oa_antecedent_ordre_affichage2"), 0)
        antecedent.Ordre3 = Coalesce(reader("oa_antecedent_ordre_affichage3"), 0)
        antecedent.StatutAffichage = Coalesce(reader("oa_antecedent_statut_affichage"), "")
        antecedent.StatutAffichageTransformation = Coalesce(reader("oa_antecedent_statut_affichage_transformation"), "")
        antecedent.CategorieContexte = Coalesce(reader("oa_antecedent_categorie_contexte"), "")
        antecedent.EpisodeId = Coalesce(reader("oa_episode_id"), 0)
        antecedent.Inactif = Coalesce(reader("oa_antecedent_inactif"), False)
        Return antecedent
    End Function

    Friend Function CloneAntecedent(Source As Antecedent) As Antecedent
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

    Friend Function Compare(source1 As Antecedent, source2 As Antecedent) As Boolean
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

End Class
