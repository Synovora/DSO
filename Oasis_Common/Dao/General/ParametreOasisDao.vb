Imports System.Configuration
Imports System.Data.SqlClient
Imports Oasis_Common
Public Class ParametreOasisDao
    Inherits StandardDao

    Public Sub TraitementContexte()
        Dim con As SqlConnection
        Dim CodeRetour As Boolean = False

        con = GetConnection()

        Try
            Dim command As SqlCommand = con.CreateCommand()

            'Recherche de la date du dernier traitement réalisé
            command.CommandText = "select * from oasis.oa_parametre_oasis where oa_parametre_oasis_id = 1"
            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    Dim DateTraitement As Date = Coalesce(reader("oa_parametre_oasis_date"), Date.Now().AddDays(-1))
                    Dim dateJouraComparer As New Date(Date.Now.Year, Date.Now.Month, Date.Now.Day, 0, 0, 0)
                    Dim dateTraitementaComparer As New Date(DateTraitement.Year, DateTraitement.Month, DateTraitement.Day, 0, 0, 0)
                    If dateTraitementaComparer < dateJouraComparer Then
                        'La date du dernier traitement est inférieure à date du jour impliquant de réaliser le traitement de nouveau
                        CodeRetour = True
                        'Mise à jour du paramètre avec la date du jour
                        ModificationParametre(1, Date.Now())
                    End If
                Else
                    CodeRetour = True
                    'Création du paramètre avec la date du jour
                    CreationParametre(1, "Date du dernier traitement des contextes obsolètes", Date.Now())
                End If
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try


        If CodeRetour = True Then
            Dim ContexteDao As ContexteDao = New ContexteDao
            Dim ContexteObsoleteDataTable As DataTable
            ContexteObsoleteDataTable = ContexteDao.getContexteObsolete()
            Dim i As Integer
            Dim rowCount As Integer = ContexteObsoleteDataTable.Rows.Count - 1
            For i = 0 To rowCount Step 1
                Dim AntecedentHistoaCreer As New AntecedentHisto
                AntecedentHistoaCreer.HistorisationDate = Date.Now()
                AntecedentHistoaCreer.UtilisateurId = 1
                AntecedentHistoaCreer.Etat = 0
                AntecedentHistoaCreer.AntecedentId = ContexteObsoleteDataTable.Rows(i)("oa_antecedent_id")
                AntecedentHistoaCreer.PatientId = ContexteObsoleteDataTable.Rows(i)("oa_antecedent_patient_id")
                AntecedentHistoaCreer.Type = ContexteObsoleteDataTable.Rows(i)("oa_antecedent_type")
                AntecedentHistoaCreer.DrcId = ContexteObsoleteDataTable.Rows(i)("oa_antecedent_drc_id")
                If ContexteObsoleteDataTable.Rows(i)("oa_antecedent_description") Is DBNull.Value Then
                    AntecedentHistoaCreer.Description = ""
                Else
                    AntecedentHistoaCreer.Description = ContexteObsoleteDataTable.Rows(i)("oa_antecedent_description")
                End If
                If ContexteObsoleteDataTable.Rows(i)("oa_antecedent_date_creation") Is DBNull.Value Then
                    AntecedentHistoaCreer.DateCreation = New Date(1, 1, 1, 0, 0, 0)
                Else
                    AntecedentHistoaCreer.DateCreation = ContexteObsoleteDataTable.Rows(i)("oa_antecedent_date_creation")
                End If
                If ContexteObsoleteDataTable.Rows(i)("oa_antecedent_utilisateur_creation") Is DBNull.Value Then
                    AntecedentHistoaCreer.UtilisateurCreation = 0
                Else
                    AntecedentHistoaCreer.UtilisateurCreation = ContexteObsoleteDataTable.Rows(i)("oa_antecedent_utilisateur_creation")
                End If
                If ContexteObsoleteDataTable.Rows(i)("oa_antecedent_date_modification") Is DBNull.Value Then
                    AntecedentHistoaCreer.DateModification = New Date(1, 1, 1, 0, 0, 0)
                Else
                    AntecedentHistoaCreer.DateModification = ContexteObsoleteDataTable.Rows(i)("oa_antecedent_date_modification")
                End If
                If ContexteObsoleteDataTable.Rows(i)("oa_antecedent_utilisateur_modification") Is DBNull.Value Then
                    AntecedentHistoaCreer.UtilisateurModification = 0
                Else
                    AntecedentHistoaCreer.UtilisateurModification = ContexteObsoleteDataTable.Rows(i)("oa_antecedent_utilisateur_modification")
                End If
                If ContexteObsoleteDataTable.Rows(i)("oa_antecedent_diagnostic") Is DBNull.Value Then
                    AntecedentHistoaCreer.Diagnostic = 0
                Else
                    AntecedentHistoaCreer.Diagnostic = ContexteObsoleteDataTable.Rows(i)("oa_antecedent_diagnostic")
                End If
                If ContexteObsoleteDataTable.Rows(i)("oa_antecedent_date_debut") Is DBNull.Value Then
                    AntecedentHistoaCreer.DateDebut = New Date(1, 1, 1, 0, 0, 0)
                Else
                    AntecedentHistoaCreer.DateDebut = ContexteObsoleteDataTable.Rows(i)("oa_antecedent_date_debut")
                End If
                If ContexteObsoleteDataTable.Rows(i)("oa_antecedent_date_fin") Is DBNull.Value Then
                    AntecedentHistoaCreer.DateFin = New Date(1, 1, 1, 0, 0, 0)
                Else
                    AntecedentHistoaCreer.DateFin = ContexteObsoleteDataTable.Rows(i)("oa_antecedent_date_fin")
                End If
                If ContexteObsoleteDataTable.Rows(i)("oa_antecedent_arret") Is DBNull.Value Then
                    AntecedentHistoaCreer.Arret = False
                Else
                    AntecedentHistoaCreer.Arret = ContexteObsoleteDataTable.Rows(i)("oa_antecedent_arret")
                End If
                If ContexteObsoleteDataTable.Rows(i)("oa_antecedent_arret_commentaire") Is DBNull.Value Then
                    AntecedentHistoaCreer.ArretCommentaire = ""
                Else
                    AntecedentHistoaCreer.ArretCommentaire = ContexteObsoleteDataTable.Rows(i)("oa_antecedent_arret_commentaire")
                End If
                If ContexteObsoleteDataTable.Rows(i)("oa_antecedent_nature") Is DBNull.Value Then
                    AntecedentHistoaCreer.Nature = ""
                Else
                    AntecedentHistoaCreer.Nature = ContexteObsoleteDataTable.Rows(i)("oa_antecedent_nature")
                End If
                If ContexteObsoleteDataTable.Rows(i)("oa_antecedent_niveau") Is DBNull.Value Then
                    AntecedentHistoaCreer.Niveau = 0
                Else
                    AntecedentHistoaCreer.Niveau = ContexteObsoleteDataTable.Rows(i)("oa_antecedent_niveau")
                End If
                If ContexteObsoleteDataTable.Rows(i)("oa_antecedent_id_niveau1") Is DBNull.Value Then
                    AntecedentHistoaCreer.Niveau1Id = 0
                Else
                    AntecedentHistoaCreer.Niveau1Id = ContexteObsoleteDataTable.Rows(i)("oa_antecedent_id_niveau1")
                End If
                If ContexteObsoleteDataTable.Rows(i)("oa_antecedent_id_niveau2") Is DBNull.Value Then
                    AntecedentHistoaCreer.Niveau2Id = 0
                Else
                    AntecedentHistoaCreer.Niveau2Id = ContexteObsoleteDataTable.Rows(i)("oa_antecedent_id_niveau2")
                End If
                If ContexteObsoleteDataTable.Rows(i)("oa_antecedent_ordre_affichage1") Is DBNull.Value Then
                    AntecedentHistoaCreer.Ordre1 = 0
                Else
                    AntecedentHistoaCreer.Ordre1 = ContexteObsoleteDataTable.Rows(i)("oa_antecedent_ordre_affichage1")
                End If
                If ContexteObsoleteDataTable.Rows(i)("oa_antecedent_ordre_affichage2") Is DBNull.Value Then
                    AntecedentHistoaCreer.Ordre2 = 0
                Else
                    AntecedentHistoaCreer.Ordre2 = ContexteObsoleteDataTable.Rows(i)("oa_antecedent_ordre_affichage2")
                End If
                If ContexteObsoleteDataTable.Rows(i)("oa_antecedent_ordre_affichage3") Is DBNull.Value Then
                    AntecedentHistoaCreer.Ordre3 = 0
                Else
                    AntecedentHistoaCreer.Ordre3 = ContexteObsoleteDataTable.Rows(i)("oa_antecedent_ordre_affichage3")
                End If
                If ContexteObsoleteDataTable.Rows(i)("oa_antecedent_statut_affichage") Is DBNull.Value Then
                    AntecedentHistoaCreer.StatutAffichage = ""
                Else
                    AntecedentHistoaCreer.StatutAffichage = ContexteObsoleteDataTable.Rows(i)("oa_antecedent_statut_affichage")
                End If
                If ContexteObsoleteDataTable.Rows(i)("oa_antecedent_categorie_contexte") Is DBNull.Value Then
                    AntecedentHistoaCreer.Categorie = ""
                Else
                    AntecedentHistoaCreer.Categorie = ContexteObsoleteDataTable.Rows(i)("oa_antecedent_categorie_contexte")
                End If
                If ContexteObsoleteDataTable.Rows(i)("oa_antecedent_inactif") Is DBNull.Value Then
                    AntecedentHistoaCreer.Inactif = False
                Else
                    AntecedentHistoaCreer.Inactif = ContexteObsoleteDataTable.Rows(i)("oa_antecedent_inactif")
                End If
                AntecedentHistoaCreer.AldId = Coalesce(ContexteObsoleteDataTable.Rows(i)("oa_antecedent_ald_id"), 0)
                AntecedentHistoaCreer.AldCim10Id = Coalesce(ContexteObsoleteDataTable.Rows(i)("oa_antecedent_ald_cim_10_id"), 0)
                AntecedentHistoaCreer.AldDateDebut = Coalesce(ContexteObsoleteDataTable.Rows(i)("oa_antecedent_ald_date_debut"), Date.MaxValue)
                AntecedentHistoaCreer.AldDateFin = Coalesce(ContexteObsoleteDataTable.Rows(i)("oa_antecedent_ald_date_fin"), Date.MaxValue)
                AntecedentHistoaCreer.AldDemandeEnCours = Coalesce(ContexteObsoleteDataTable.Rows(i)("oa_antecedent_ald_demande_en_cours"), False)
                AntecedentHistoaCreer.AldDateDemande = Coalesce(ContexteObsoleteDataTable.Rows(i)("oa_antecedent_ald_demande_date"), Date.MaxValue)
                Dim contexteId As Integer = ContexteObsoleteDataTable.Rows(i)("oa_antecedent_id")
                Dim Description As String = AntecedentHistoaCreer.Description & " (" & AntecedentHistoaCreer.DateDebut.ToString("MM.yyyy") & ")"

                'Récupération de l'utilisateur par défaut dans les paramètres de l'application
                Dim IdUserAutoString As String = ConfigurationManager.AppSettings("IdUserAuto")
                Dim IdUserAuto As Long
                If IsNumeric(IdUserAutoString) Then
                    IdUserAuto = CInt(IdUserAutoString)
                Else
                    CreateLog("Paramètre application 'IdUserAuto' non trouvé !", "ParametreOasisDao", LogDao.EnumTypeLog.ERREUR.ToString)
                    IdUserAuto = 1
                End If
                Dim userdao As New UserDao
                Dim user As Utilisateur = userdao.getUserById(IdUserAuto)

                ContexteDao.TransformationEnAntecedent(contexteId, AntecedentHistoaCreer, Description, Coalesce(ContexteObsoleteDataTable.Rows(i)("oa_antecedent_statut_affichage_transformation"), ""), user)
            Next
        End If

    End Sub



    Public Function CreationParametre(Id As Integer, Description As String, dateValeur As Date) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim con As SqlConnection
        con = GetConnection()

        Dim SQLstring As String = "insert into oasis.oa_parametre_oasis" &
        " (oa_parametre_oasis_id, oa_parametre_oasis_description, oa_parametre_oasis_date)" &
        " VALUES (@Id, @description, @date)"

        Dim cmd As New SqlCommand(SQLstring, con)
        With cmd.Parameters
            .AddWithValue("@Id", Id.ToString)
            .AddWithValue("@description", Description)
            .AddWithValue("@date", dateValeur.ToString)
        End With

        Try
            da.InsertCommand = cmd
            da.InsertCommand.ExecuteNonQuery()
        Catch ex As Exception
            Throw New Exception(ex.Message)
            codeRetour = False
        Finally
            con.Close()
        End Try

        Return codeRetour
    End Function

    Public Function ModificationParametre(Id As Integer, dateValeur As Date) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim con As SqlConnection
        con = GetConnection()

        Dim dateModification As Date = Date.Now.Date

        Dim SQLstring As String = "update oasis.oa_parametre_oasis set oa_parametre_oasis_date = @date where oa_parametre_oasis_id = @Id"

        Dim cmd As New SqlCommand(SQLstring, con)

        With cmd.Parameters
            .AddWithValue("@date", dateValeur.ToString("yyyy-MM-dd HH:mm:ss"))
            .AddWithValue("@Id", Id.ToString)
        End With
        Try
            'con.Open()
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
End Class
