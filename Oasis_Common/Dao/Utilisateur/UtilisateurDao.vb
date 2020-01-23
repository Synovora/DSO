Imports System.Data.SqlClient
Imports System.Windows.Forms

Public Module UtilisateurDao
    'Initialisation des propriétés d'une instance de utilisateur depuis la BDD
    Public Function SetUtilisateur(instanceUtilisateur As Utilisateur, utilisateurId As Integer) As Boolean
        Dim CodeRetour As Boolean = True
        Dim conxn As New SqlConnection(GetConnectionStringOasis())
        Dim SQLString As String = "select * from oasis.oa_utilisateur where oa_utilisateur_id = @utilisateurId"
        Dim utilisateurDataReader As SqlDataReader
        Dim cmd As New SqlCommand(SQLString, conxn)

        cmd.Parameters.AddWithValue("@utilisateurId", utilisateurId.ToString)

        Try
            conxn.Open()
            utilisateurDataReader = cmd.ExecuteReader()
            BuilBean(instanceUtilisateur, utilisateurDataReader)
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            Return CodeRetour = False
        Finally
            conxn.Close()
            cmd.Dispose()
        End Try

        Return CodeRetour

    End Function

    Private Sub BuilBean(instanceutilisateur As Utilisateur, utilisateurDataReader As SqlDataReader)
        If utilisateurDataReader.Read() Then
            instanceutilisateur.UtilisateurId = Convert.ToInt64(utilisateurDataReader("oa_utilisateur_id"))
            instanceutilisateur.UtilisateurNom = Coalesce(utilisateurDataReader("oa_utilisateur_nom"), "")
            instanceutilisateur.UtilisateurPrenom = Coalesce(utilisateurDataReader("oa_utilisateur_prenom"), "")
            instanceutilisateur.UtilisateurProfilId = Coalesce(utilisateurDataReader("oa_utilisateur_profil_id"), "")
            instanceutilisateur.UtilisateurAdmin = Coalesce(utilisateurDataReader("oa_utilisateur_admin"), 0)
            instanceutilisateur.UtilisateurLogin = Coalesce(utilisateurDataReader("oa_utilisateur_login"), "")
            instanceutilisateur.UtilisateurSiteId = Coalesce(utilisateurDataReader("oa_utilisateur_site_id"), 0)
            instanceutilisateur.UtilisateurUniteSanitaireId = Coalesce(utilisateurDataReader("oa_utilisateur_unite_sanitaire_id"), 0)
            If instanceutilisateur.UtilisateurProfilId <> "" Then
                Dim profilDao As New ProfilDao
                Dim profil As Profil
                profil = profilDao.getProfilById(instanceutilisateur.UtilisateurProfilId.Trim)
                instanceutilisateur.UtilisateurNiveauAcces = Coalesce(profil.NiveauAcces, 0)
                instanceutilisateur.TypeProfil = Coalesce(profil.Type, "")
            Else
                instanceutilisateur.UtilisateurNiveauAcces = 0
            End If
        End If
    End Sub
End Module
