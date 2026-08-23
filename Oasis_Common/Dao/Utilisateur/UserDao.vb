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
                                     ",oa_utilisateur_mail, oa_utilisateur_rpps " &
                                     ",oa_utilisateur_password_is_unique_usage, cle_privee, cle_publique)" & vbCrLf &
                                     " VALUES (" & vbCrLf &
                                     " @oa_utilisateur_prenom, @oa_utilisateur_nom, @oa_utilisateur_profil_id, @oa_utilisateur_login,@oa_utilisateur_siege_id " &
                                     ",@oa_utilisateur_unite_sanitaire_id, @oa_utilisateur_site_id, @oa_utilisateur_date_entree, @oa_utilisateur_date_sortie " &
                                     ",@oa_utilisateur_etat, @oa_password, @oa_utilisateur_admin, @oa_utilisateur_telephone, @oa_utilisateur_fax " &
                                     ",@oa_utilisateur_mail, @oa_utilisateur_rpps " &
                                     ",@oa_utilisateur_password_is_unique_usage, @oa_utilisateur_cle_privee, @oa_utilisateur_cle_publique);" & vbCrLf &
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
            Throw New Exception(ex.Message)
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
            ' Pour les clés de signature, une valeur vide signifie « ne pas toucher » :
            ' les fiches chargées sans clé privée ne doivent pas l'effacer en base.
            Dim SQLstring As String = "UPDATE oasis.oa_utilisateur SET " & vbCrLf &
                                     " oa_utilisateur_prenom=@oa_utilisateur_prenom, oa_utilisateur_nom=@oa_utilisateur_nom " & vbCrLf &
                                     ",oa_utilisateur_profil_id=@oa_utilisateur_profil_id, oa_utilisateur_login=@oa_utilisateur_login" & vbCrLf &
                                     ",oa_utilisateur_siege_id=@oa_utilisateur_siege_id, oa_utilisateur_unite_sanitaire_id=@oa_utilisateur_unite_sanitaire_id" & vbCrLf &
                                     ", oa_utilisateur_site_id=@oa_utilisateur_site_id" & vbCrLf &
                                     ", oa_utilisateur_admin=@oa_utilisateur_admin, oa_utilisateur_telephone=@oa_utilisateur_telephone" & vbCrLf &
                                     ", oa_utilisateur_fax=@oa_utilisateur_fax, oa_utilisateur_mail=@oa_utilisateur_mail, oa_utilisateur_rpps=@oa_utilisateur_rpps" & vbCrLf &
                                     ", oa_password=@oa_password, oa_utilisateur_password_is_unique_usage=@oa_utilisateur_password_is_unique_usage" & vbCrLf &
                                     ", cle_privee = CASE WHEN @oa_utilisateur_cle_privee = '' THEN cle_privee ELSE @oa_utilisateur_cle_privee END" & vbCrLf &
                                     ", cle_publique = CASE WHEN @oa_utilisateur_cle_publique = '' THEN cle_publique ELSE @oa_utilisateur_cle_publique END" & vbCrLf &
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
                ' Les clés de signature n'étaient jamais écrites par cette requête :
                ' un utilisateur créé sans clé ne pouvait plus jamais en obtenir.
                .AddWithValue("@oa_utilisateur_cle_privee", If(utilisateur.UtilisateurClePrivee, ""))
                .AddWithValue("@oa_utilisateur_cle_publique", If(utilisateur.UtilisateurAddress, ""))
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
            Throw New Exception(ex.Message)
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
                        ' Seul l'utilisateur qui s'authentifie repart avec sa clé de signature.
                        user = buildBean(reader, inclureClePrivee:=True)
                    Else
                        Throw New ArgumentException("Identifiant et/ou mot de passe erroné !")
                    End If
                End Using
            Catch ex As Exception
                Throw ex
            End Try
        End Using

        ' Verrouillage côté serveur : le compteur du poste (base de registre) est
        ' effaçable par l'utilisateur, celui-ci ne l'est pas.
        If user.VerrouJusqua.HasValue AndAlso user.VerrouJusqua.Value > DateTime.Now Then
            Throw New ArgumentException("Compte temporairement verrouillé suite à plusieurs échecs. Réessayez plus tard.")
        End If

        controlPassword(user, password)
        Return user
    End Function

    ''' <summary>
    ''' retrouve un utilisateur par son id
    ''' </summary>
    ''' <param name="userId"></param>
    ''' <returns></returns>
    Public Function GetUserById(userId As Integer) As Utilisateur
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
    ''' <summary>
    ''' Construit un Utilisateur à partir d'une ligne lue.
    '''
    ''' La clé privée de signature n'est chargée que pour l'utilisateur qui vient
    ''' de s'authentifier, car lui seul en a besoin (pour signer ses ordonnances).
    ''' Elle était auparavant chargée pour tout utilisateur lu, y compris dans les
    ''' listes et côté portail, où elle a déjà fini dans une vue.
    ''' </summary>
    Public Function buildBean(reader As SqlDataReader, Optional inclureClePrivee As Boolean = False) As Utilisateur
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
        ' Aucune valeur de repli ici. Les versions précédentes retombaient sur la clé
        ' privée 0x...01 et son adresse dérivée, toutes deux publiquement connues, ce qui
        ' rendait falsifiable toute signature émise par un utilisateur sans clé en base.
        ' Une clé absente doit rester absente : Utilisateur.Sign lève alors une erreur.
        If inclureClePrivee Then
            user.UtilisateurClePrivee = Coalesce(reader("cle_privee"), "")
        Else
            user.UtilisateurClePrivee = ""
        End If
        user.UtilisateurAddress = Coalesce(reader("cle_publique"), "")
        If HasColumn(reader, "oa_utilisateur_tentatives") Then
            user.Tentatives = Coalesce(reader("oa_utilisateur_tentatives"), 0)
        End If
        If HasColumn(reader, "oa_utilisateur_verrou_jusqua") Then
            Dim verrou = Coalesce(reader("oa_utilisateur_verrou_jusqua"), Nothing)
            user.VerrouJusqua = If(verrou Is Nothing, CType(Nothing, Date?), CDate(verrou))
        End If

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
        Dim doitEtreRehache As Boolean
        Dim correct = MotDePasse.VerifierAvecMigration(
            password, user.Password,
            Utilisateur.CryptePwd(user.UtilisateurLogin, password), doitEtreRehache)

        If Not correct Then
            EnregistrerEchec(user.UtilisateurId)
            Throw New ArgumentException("Identifiant et/ou mot de passe erroné !")
        End If

        ' Migration transparente : le compte passe au format courant dès que son
        ' propriétaire se connecte, sans lui demander de changer de mot de passe.
        If doitEtreRehache Then
            Try
                UpdateEmpreinteMotDePasse(user.UtilisateurId, MotDePasse.Hacher(password))
            Catch ex As Exception
                ' Une migration ratée ne doit pas empêcher la connexion : elle sera
                ' retentée au prochain accès.
            End Try
        End If

        ReinitialiserEchecs(user.UtilisateurId)

        ' Le mot de passe en clair ne doit pas rester sur le bean : il vivait
        ' jusqu'ici dans le global userLog pour toute la durée de la session.
        user.Password = Nothing
    End Sub

    ''' <summary>Réenregistre l'empreinte du mot de passe, sans rien changer d'autre.</summary>
    Public Sub UpdateEmpreinteMotDePasse(userId As Integer, empreinte As String)
        Using con As SqlConnection = GetConnection()
            Using cmd As New SqlCommand(
                "UPDATE oasis.oa_utilisateur SET oa_password = @password WHERE oa_utilisateur_id = @id;", con)
                cmd.Parameters.AddWithValue("@password", empreinte)
                cmd.Parameters.AddWithValue("@id", userId)
                cmd.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    ''' <summary>Incrémente le compteur d'échecs et verrouille au-delà du seuil.</summary>
    Private Sub EnregistrerEchec(userId As Integer)
        Try
            Using con As SqlConnection = GetConnection()
                Using cmd As New SqlCommand(
                    "UPDATE oasis.oa_utilisateur" & vbCrLf &
                    " SET oa_utilisateur_tentatives = COALESCE(oa_utilisateur_tentatives, 0) + 1," & vbCrLf &
                    "     oa_utilisateur_verrou_jusqua = CASE WHEN COALESCE(oa_utilisateur_tentatives, 0) + 1 >= @seuil" & vbCrLf &
                    "                                        THEN DATEADD(minute, @duree, SYSDATETIME())" & vbCrLf &
                    "                                        ELSE oa_utilisateur_verrou_jusqua END" & vbCrLf &
                    " WHERE oa_utilisateur_id = @id;", con)
                    cmd.Parameters.AddWithValue("@id", userId)
                    cmd.Parameters.AddWithValue("@seuil", MAX_TRY)
                    cmd.Parameters.AddWithValue("@duree", DureeVerrouMinutes)
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        Catch ex As Exception
            ' Le comptage ne doit pas masquer l'échec d'authentification lui-même.
        End Try
    End Sub

    ''' <summary>Remet le compteur d'échecs à zéro après une authentification réussie.</summary>
    Private Sub ReinitialiserEchecs(userId As Integer)
        Try
            Using con As SqlConnection = GetConnection()
                Using cmd As New SqlCommand(
                    "UPDATE oasis.oa_utilisateur SET oa_utilisateur_tentatives = 0," &
                    " oa_utilisateur_verrou_jusqua = NULL WHERE oa_utilisateur_id = @id;", con)
                    cmd.Parameters.AddWithValue("@id", userId)
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        Catch ex As Exception
        End Try
    End Sub

    Private Const DureeVerrouMinutes As Integer = 15

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

    ''' <summary>
    ''' Clé privée de signature d'un utilisateur. Accès délibérément isolé : aucun
    ''' chargement de fiche ne doit ramener cette valeur en mémoire.
    ''' </summary>
    Public Function GetCleSignature(userId As Integer) As String
        Using con As SqlConnection = GetConnection()
            Using cmd As New SqlCommand("SELECT cle_privee FROM oasis.oa_utilisateur WHERE oa_utilisateur_id = @id;", con)
                cmd.Parameters.AddWithValue("@id", userId)
                Dim valeur = cmd.ExecuteScalar()
                If valeur Is Nothing OrElse valeur Is DBNull.Value Then Return ""
                Return CStr(valeur)
            End Using
        End Using
    End Function

    ''' <summary>Vrai si l'utilisateur possède déjà une clé de signature.</summary>
    Public Function ACleSignature(userId As Integer) As Boolean
        Return Not String.IsNullOrWhiteSpace(GetCleSignature(userId))
    End Function
End Class
