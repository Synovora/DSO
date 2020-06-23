Imports System.Data.SqlClient

Public Class UserDao
    Inherits StandardDao

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="utilisateur"></param>
    ''' <returns></returns>
    Public Function Create(utilisateur As Utilisateur) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim con As SqlConnection

        con = GetConnection()
        Dim transaction As SqlClient.SqlTransaction = con.BeginTransaction

        Try
            Dim SQLstring As String = "INSERT INTO oasis.oa_utilisateur (" & vbCrLf &
                                     " oa_utilisateur_prenom, oa_utilisateur_nom, oa_utilisateur_profil_id, oa_utilisateur_login,oa_utilisateur_siege_id " &
                                     ",oa_utilisateur_unite_sanitaire_id, oa_utilisateur_site_id, oa_utilisateur_date_entree, oa_utilisateur_date_sortie " &
                                     ",oa_utilisateur_etat, oa_password, oa_utilisateur_admin, oa_utilisateur_telephone, oa_utilisateur_fax " &
                                     ",oa_utilisateur_mail, oa_utilisateur_rpps, oa_utilisateur_password_is_unique_usage)" & vbCrLf &
                                     " VALUES (" & vbCrLf &
                                     " @oa_utilisateur_prenom, @oa_utilisateur_nom, @oa_utilisateur_profil_id, @oa_utilisateur_login,@oa_utilisateur_siege_id " &
                                     ",@oa_utilisateur_unite_sanitaire_id, @oa_utilisateur_site_id, @oa_utilisateur_date_entree, @oa_utilisateur_date_sortie " &
                                     ",@oa_utilisateur_etat, @oa_password, @oa_utilisateur_admin, @oa_utilisateur_telephone, @oa_utilisateur_fax " &
                                     ",@oa_utilisateur_mail, @oa_utilisateur_rpps, @oa_utilisateur_password_is_unique_usage);" & vbCrLf &
                                     "SELECT SCOPE_IDENTITY()"

            Dim cmd As New SqlCommand(SQLstring, con, transaction)
            With cmd.Parameters
                .AddWithValue("@oa_utilisateur_prenom", utilisateur.UtilisateurPrenom)
                .AddWithValue("@oa_utilisateur_nom", utilisateur.UtilisateurNom)
                .AddWithValue("@oa_utilisateur_profil_id", utilisateur.UtilisateurProfilId)
                .AddWithValue("@oa_utilisateur_login", utilisateur.UtilisateurLogin)
                .AddWithValue("@oa_utilisateur_siege_id", If(utilisateur.UtilisateurSiegeId = 0, DBNull.Value, utilisateur.UtilisateurSiegeId))
                .AddWithValue("@oa_utilisateur_unite_sanitaire_id", If(utilisateur.UtilisateurUniteSanitaireId = 0, DBNull.Value, utilisateur.UtilisateurUniteSanitaireId))
                .AddWithValue("@oa_utilisateur_site_id", If(utilisateur.UtilisateurSiteId = 0, DBNull.Value, utilisateur.UtilisateurSiteId))
                .AddWithValue("@oa_utilisateur_date_entree", Date.Now)
                .AddWithValue("@oa_utilisateur_date_sortie", New Date(2999, 12, 31, 0, 0, 0))
                .AddWithValue("@oa_utilisateur_etat", "A")
                .AddWithValue("@oa_password", utilisateur.Password)
                .AddWithValue("@oa_utilisateur_admin", utilisateur.UtilisateurAdmin)
                .AddWithValue("@oa_utilisateur_telephone", utilisateur.UtilisateurTelephone)
                .AddWithValue("@oa_utilisateur_fax", utilisateur.UtilisateurFax)
                .AddWithValue("@oa_utilisateur_mail", utilisateur.UtilisateurMail)
                .AddWithValue("@oa_utilisateur_rpps", utilisateur.UtilisateurRPPS)
                .AddWithValue("@oa_utilisateur_password_is_unique_usage", utilisateur.IsPasswordUniqueUsage)
                .AddWithValue("@oa_utilisateur_cle_privee", utilisateur.UtilisateurClePrivee)
                .AddWithValue("@oa_utilisateur_cle_publique", utilisateur.UtilisateurAddress)
            End With

            da.InsertCommand = cmd
            utilisateur.UtilisateurId = da.InsertCommand.ExecuteScalar()

            transaction.Commit()

        Catch ex As Exception
            transaction.Rollback()
            Windows.Forms.MessageBox.Show(ex.Message)
            codeRetour = False
        Finally
            transaction.Dispose()
            con.Close()
        End Try

        Return codeRetour
    End Function

    Public Function UpdateSansChangerEtatEtDates(utilisateur As Utilisateur) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim con As SqlConnection

        con = GetConnection()
        Dim transaction As SqlClient.SqlTransaction = con.BeginTransaction

        Try
            Dim SQLstring As String = "UPDATE oasis.oa_utilisateur SET " & vbCrLf &
                                     " oa_utilisateur_prenom=@oa_utilisateur_prenom, oa_utilisateur_nom=@oa_utilisateur_nom " & vbCrLf &
                                     ",oa_utilisateur_profil_id=@oa_utilisateur_profil_id, oa_utilisateur_login=@oa_utilisateur_login" & vbCrLf &
                                     ",oa_utilisateur_siege_id=@oa_utilisateur_siege_id, oa_utilisateur_unite_sanitaire_id=@oa_utilisateur_unite_sanitaire_id" & vbCrLf &
                                     ", oa_utilisateur_site_id=@oa_utilisateur_site_id" & vbCrLf &
                                     ", oa_utilisateur_admin=@oa_utilisateur_admin, oa_utilisateur_telephone=@oa_utilisateur_telephone" & vbCrLf &
                                     ", oa_utilisateur_fax=oa_utilisateur_fax, oa_utilisateur_mail=@oa_utilisateur_mail, oa_utilisateur_rpps=@oa_utilisateur_rpps" & vbCrLf &
                                     ", oa_password=@oa_password, oa_utilisateur_password_is_unique_usage=@oa_utilisateur_password_is_unique_usage" & vbCrLf &
                                     "WHERE oa_utilisateur_id = @oa_utilisateur_id "

            Dim cmd As New SqlCommand(SQLstring, con, transaction)
            With cmd.Parameters
                .AddWithValue("@oa_utilisateur_prenom", utilisateur.UtilisateurPrenom)
                .AddWithValue("@oa_utilisateur_nom", utilisateur.UtilisateurNom)
                .AddWithValue("@oa_utilisateur_profil_id", utilisateur.UtilisateurProfilId)
                .AddWithValue("@oa_utilisateur_login", utilisateur.UtilisateurLogin)
                .AddWithValue("@oa_utilisateur_siege_id", If(utilisateur.UtilisateurSiegeId = 0, DBNull.Value, utilisateur.UtilisateurSiegeId))
                .AddWithValue("@oa_utilisateur_unite_sanitaire_id", If(utilisateur.UtilisateurUniteSanitaireId = 0, DBNull.Value, utilisateur.UtilisateurUniteSanitaireId))
                .AddWithValue("@oa_utilisateur_site_id", If(utilisateur.UtilisateurSiteId = 0, DBNull.Value, utilisateur.UtilisateurSiteId))
                .AddWithValue("@oa_utilisateur_admin", utilisateur.UtilisateurAdmin)
                .AddWithValue("@oa_utilisateur_telephone", utilisateur.UtilisateurTelephone)
                .AddWithValue("@oa_utilisateur_fax", utilisateur.UtilisateurFax)
                .AddWithValue("@oa_utilisateur_mail", utilisateur.UtilisateurMail)
                .AddWithValue("@oa_utilisateur_rpps", utilisateur.UtilisateurRPPS)
                .AddWithValue("@oa_password", utilisateur.Password)
                .AddWithValue("@oa_utilisateur_password_is_unique_usage", utilisateur.IsPasswordUniqueUsage)
                ' -- pour le where
                .AddWithValue("@oa_utilisateur_id", utilisateur.UtilisateurId)
            End With

            da.InsertCommand = cmd
            Dim nb As Integer = da.InsertCommand.ExecuteNonQuery()
            If (nb <> 1) Then
                Throw New Exception("Validation échouée (" & nb & ")")
            End If

            transaction.Commit()

        Catch ex As Exception
            transaction.Rollback()
            Windows.Forms.MessageBox.Show(ex.Message)
            codeRetour = False
        Finally
            transaction.Dispose()
            con.Close()
        End Try

        Return codeRetour
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="login"></param>
    ''' <param name="password"></param>
    ''' <returns></returns>
    Public Function getUserByLoginPassword(login As String, password As String) As Utilisateur
        Dim user As Utilisateur

        Using con As SqlConnection = GetConnection()
            Dim command As SqlCommand = con.CreateCommand()
            Try
                command.CommandText =
                   "select U.*, p.* " &
                   "from oasis.oa_utilisateur u " &
                   "inner join oasis.oa_r_profil p on p.oa_r_profil_id = oa_utilisateur_profil_id And COALESCE(oa_r_profil_inactif,'false')='false' " &
                   "where oa_utilisateur_login = @login AND oa_utilisateur_etat='A'"
                command.Parameters.AddWithValue("@login", login)
                Using reader As SqlDataReader = command.ExecuteReader()
                    If reader.Read() Then
                        user = buildBean(reader)
                        controlPassword(user, password)
                    Else
                        Throw New ArgumentException("Identifiant et/ou mot de passe erroné !")
                    End If
                End Using
            Catch ex As Exception
                Throw ex
            End Try
        End Using

        Return user
    End Function

    ''' <summary>
    ''' retrouve un utilisateur par son id
    ''' </summary>
    ''' <param name="userId"></param>
    ''' <returns></returns>
    Public Function getUserById(userId As Integer) As Utilisateur
        Dim user As Utilisateur
        Dim con As SqlConnection

        con = GetConnection()

        Try

            Dim command As SqlCommand = con.CreateCommand()

            command.CommandText =
               "select U.*, p.* " &
               "from oasis.oa_utilisateur u " &
               "left join oasis.oa_r_profil p on p.oa_r_profil_id = oa_utilisateur_profil_id " &
               "where oa_utilisateur_id = @id"
            command.Parameters.AddWithValue("@id", userId)
            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    user = buildBean(reader)
                Else
                    Throw New ArgumentException("Utilisateur non retrouvé !")
                End If
            End Using

        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try


        Return user
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="reader"></param>
    ''' <returns></returns>
    Public Function buildBean(reader As SqlDataReader) As Utilisateur
        Dim user As New Utilisateur

        user.UtilisateurId = reader("oa_utilisateur_id")
        user.UtilisateurNom = Coalesce(reader("oa_utilisateur_nom"), "")
        user.UtilisateurPrenom = Coalesce(reader("oa_utilisateur_prenom"), "")
        user.UtilisateurTelephone = Coalesce(reader("oa_utilisateur_telephone"), "")
        user.UtilisateurFax = Coalesce(reader("oa_utilisateur_fax"), "")
        user.UtilisateurMail = Coalesce(reader("oa_utilisateur_mail"), "")
        user.UtilisateurProfilId = Coalesce(reader("oa_utilisateur_profil_id"))
        user.UtilisateurAdmin = Coalesce(reader("oa_utilisateur_admin"), False)
        user.UtilisateurLogin = Coalesce(reader("oa_utilisateur_login"), "")
        user.UtilisateurSiteId = Coalesce(reader("oa_utilisateur_site_id"), 0)
        user.UtilisateurUniteSanitaireId = Coalesce(reader("oa_utilisateur_unite_sanitaire_id"), 0)
        user.UtilisateurSiegeId = Coalesce(reader("oa_utilisateur_siege_id"), 0)
        user.Password = Trim(Coalesce(reader("oa_password"), ""))
        user.UtilisateurProfilId = Coalesce(reader("oa_r_profil_id"), "ADMINISTRATIF")
        user.FonctionParDefautId = Coalesce(reader("oa_r_profil_fonction_id_defaut"), 0)
        user.UtilisateurNiveauAcces = Coalesce(reader("oa_r_profil_niveau_acces"), 3)
        user.TypeProfil = Coalesce(reader("oa_r_profil_type"), "")
        user.UtilisateurRPPS = Coalesce(reader("oa_utilisateur_rpps"), "")
        user.IsPasswordUniqueUsage = Coalesce(reader("oa_utilisateur_password_is_unique_usage"), False)
        user.UtilisateurClePrivee = Coalesce(reader("cle_privee"), "0x0000000000000000000000000000000000000000000000000000000000000001") 'TODO: remove
        user.UtilisateurAddress = Coalesce(reader("cle_publique"), "0x7E5F4552091A69125d5DfCb7b8C2659029395Bdf") 'TODO: remove

        ' --- recuperation des fonctions correspondant au profil de l'utilisateur
        addFonctions(user)

        Return user
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="user"></param>
    Public Sub addFonctions(user As Utilisateur)
        Dim fonctionDao As New FonctionDao
        user.LstFonction = fonctionDao.GetList(False, user.UtilisateurProfilId)
    End Sub

    ''' <summary>
    ''' Controle de la cohérence du mot de passe
    ''' </summary>
    ''' <param name="user"></param>
    ''' <param name="password"></param>
    Private Sub controlPassword(user As Utilisateur, password As String)
        If user.Password = Utilisateur.CryptePwd(user.UtilisateurLogin, password) Then
            user.Password = password   ' on ne garde que le pasword crypté
            Return
        End If
        Throw New ArgumentException("Identifiant et/ou mot de passe erroné !")
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="isInactif"></param>
    ''' <returns></returns>
    Public Function GetTableUtilisateurForGrid(Optional isInactif As Boolean = False) As DataTable
        Dim SQLString As String
        'Console.WriteLine("----------> getTableSousEpisode")
        SQLString =
            "SELECT " & vbCrLf &
            "	  oa_utilisateur_id, " & vbCrLf &
            "     oa_utilisateur_profil_id, " & vbCrLf &
            "     oa_utilisateur_prenom, " & vbCrLf &
            "     oa_utilisateur_nom, " & vbCrLf &
            "     oa_utilisateur_login, " & vbCrLf &
            "     oa_utilisateur_date_entree, " & vbCrLf &
            "     oa_utilisateur_date_sortie, " & vbCrLf &
            "     oa_utilisateur_etat, " & vbCrLf &
            "	  oa_utilisateur_admin " & vbCrLf &
            "	 ,oa_r_profil_designation " & vbCrLf &
            "    ,oa_siege_description " & vbCrLf &
            "    ,oa_unite_sanitaire_description " & vbCrLf &
            "    ,oa_site_description " & vbCrLf


        SQLString += "FROM oasis.oa_utilisateur U " & vbCrLf &
                     "LEFT JOIN oasis.oa_r_profil P ON P.oa_r_profil_id = U.oa_utilisateur_profil_id " & vbCrLf &
                     "LEFT JOIN oasis.oa_siege S ON S.oa_siege_id = U.oa_utilisateur_siege_id " & vbCrLf &
                     "LEFT JOIN oasis.oa_unite_sanitaire US ON US.oa_unite_sanitaire_id = U.oa_utilisateur_unite_sanitaire_id " & vbCrLf &
                     "LEFT JOIN oasis.oa_site SI ON SI.oa_site_id = U.oa_utilisateur_site_id " & vbCrLf &
                     "WHERE 1=1 " & vbCrLf &
                     "AND U.oa_utilisateur_etat <> @etat " & vbCrLf &
                     "ORDER by U.oa_utilisateur_nom"


        'Console.WriteLine(SQLString)

        Using con As SqlConnection = GetConnection()

            Dim tacheDataAdapter As SqlDataAdapter = New SqlDataAdapter()
            Using tacheDataAdapter
                tacheDataAdapter.SelectCommand = New SqlCommand(SQLString, con)
                tacheDataAdapter.SelectCommand.Parameters.AddWithValue("@etat", If(isInactif, "I", "A"))
                Dim tacheDataTable As DataTable = New DataTable()
                Using tacheDataTable
                    Try
                        tacheDataAdapter.Fill(tacheDataTable)
                    Catch ex As Exception
                        Throw ex
                    End Try
                    Return tacheDataTable
                End Using
            End Using
        End Using
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="idUser"></param>
    ''' <param name="isInactivation"></param>
    Public Sub ActivationOuDesactivation(idUser As Integer, isInactivation As Boolean)
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim nbUpdate As Integer

        Dim SQLstring As String = "UPDATE oasis.oa_utilisateur SET" &
            " oa_utilisateur_etat = @etat" &
            ",oa_utilisateur_date_sortie = @oa_utilisateur_date_sortie" &
            " WHERE oa_utilisateur_id = @Id AND oa_utilisateur_etat<> @etat2"

        Using con As SqlConnection = GetConnection()
            Dim cmd As SqlCommand
            cmd = New SqlCommand(SQLstring, con)
            With cmd.Parameters
                .AddWithValue("@etat", If(isInactivation, "I", "A"))
                .AddWithValue("@oa_utilisateur_date_sortie", If(isInactivation, Date.Now, New Date(2999, 12, 31, 0, 0, 0)))
                .AddWithValue("@Id", idUser)
                .AddWithValue("@etat2", If(isInactivation, "I", "A"))
            End With

            da.UpdateCommand = cmd
            nbUpdate = da.UpdateCommand.ExecuteNonQuery()
            If nbUpdate <= 0 Then
                Throw New Exception("Collision , Etat Utilisateur déjà modifié par un autre utilisateur !")
            End If
        End Using

    End Sub
End Class
