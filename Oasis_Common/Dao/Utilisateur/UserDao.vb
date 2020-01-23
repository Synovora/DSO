Imports System.Data.SqlClient

Public Class UserDao
    Inherits StandardDao

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
                   "inner join oasis.oa_r_profil p on p.oa_r_profil_id = oa_utilisateur_profil_id AND COALESCE(oa_r_profil_inactif,'false')='false' " &
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
        user.UtilisateurProfilId = Coalesce(reader("oa_utilisateur_profil_id"))
        user.UtilisateurAdmin = Coalesce(reader("oa_utilisateur_admin"), False)
        user.UtilisateurLogin = Coalesce(reader("oa_utilisateur_login"), "")
        user.UtilisateurSiteId = Coalesce(reader("oa_utilisateur_site_id"), 0)
        user.UtilisateurUniteSanitaireId = Coalesce(reader("oa_utilisateur_unite_sanitaire_id"), 0)
        user.Password = Trim(Coalesce(reader("oa_password"), ""))
        user.UtilisateurProfilId = Coalesce(reader("oa_r_profil_id"), "ADMINISTRATIF")
        user.FonctionParDefautId = Coalesce(reader("oa_r_profil_fonction_id_defaut"), 0)
        user.UtilisateurNiveauAcces = Coalesce(reader("oa_r_profil_niveau_acces"), 3)
        user.TypeProfil = Coalesce(reader("oa_r_profil_type"), "")

        ' --- recuperation des fonctions correspondant au profil de l'utilisateur
        addFonctions(user)

        Return user
    End Function

    Public Sub addFonctions(user As Utilisateur)
        Dim fonctionDao As New FonctionDao
        user.LstFonction = fonctionDao.getList(False, user.UtilisateurProfilId)
    End Sub

    ''' <summary>
    ''' Controle de la cohérence du mot de passe
    ''' </summary>
    ''' <param name="user"></param>
    ''' <param name="password"></param>
    Private Sub controlPassword(user As Utilisateur, password As String)
        If user.Password = Utilisateur.cryptePwd(user.UtilisateurLogin, password) Then
            user.Password = password   ' on ne garde que le pasword crypté
            Return
        End If
        Throw New ArgumentException("Identifiant et/ou mot de passe erroné !")
    End Sub




End Class
