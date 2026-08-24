Imports System.Data.SqlClient

Public Class InternauteDao
    Inherits StandardDao

    Public Function Create(internaute As Internaute) As Long
        Dim da As New SqlDataAdapter()
        Dim con As SqlConnection = GetConnection()
        Dim transaction As SqlClient.SqlTransaction = con.BeginTransaction
        Dim Id As Long

        Try
            ' Le mot de passe était haché puis jamais inséré : les comptes créés
            ' repartaient avec une colonne password NULL.
            Dim SQLstring As String = "INSERT INTO oasis.oa_internaute (" & vbCrLf &
                                     " username, email, password, recovery, code)" & vbCrLf &
                                     " VALUES (" & vbCrLf &
                                     " @username, @email, @password, @recovery, @code);" & vbCrLf &
                                     " SELECT SCOPE_IDENTITY()"

            Dim cmd As New SqlCommand(SQLstring, con, transaction)
            internaute.CryptePwd()
            With cmd.Parameters
                .AddWithValue("@username", internaute.Username)
                .AddWithValue("@email", internaute.Email)
                .AddWithValue("@password", If(internaute.Password, CObj(DBNull.Value)))
                .AddWithValue("@recovery", If(internaute.Recovery, CObj(DBNull.Value)))
                .AddWithValue("@code", If(internaute.Code, CObj(DBNull.Value)))
            End With

            da.InsertCommand = cmd
            Id = da.InsertCommand.ExecuteScalar()

            transaction.Commit()

        Catch ex As Exception
            transaction.Rollback()
            Throw New Exception(ex.Message)
        Finally
            transaction.Dispose()
            con.Close()
        End Try
        Return Id
    End Function

    Public Function GetInternauteByLoginPassword(email As String, password As String) As Internaute
        Dim user As Internaute = Nothing

        Using con As SqlConnection = GetConnection()
            Dim command As SqlCommand = con.CreateCommand()
            Try
                command.CommandText = "SELECT * FROM oasis.oa_internaute WHERE email = @email;"
                command.Parameters.AddWithValue("@email", email)
                Using reader As SqlDataReader = command.ExecuteReader()
                    If reader.Read() Then
                        user = New Internaute(reader)
                        ControlPassword(user, password)
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

    Public Function GetInternauteByRecoveryKey(recovery As String) As Internaute
        Dim user As Internaute = Nothing

        Using con As SqlConnection = GetConnection()
            Dim command As SqlCommand = con.CreateCommand()
            Try
                ' recovery <> '' : sinon une clé vide renvoie le premier compte ayant
                ' déjà terminé une réinitialisation.
                command.CommandText = "SELECT * FROM oasis.oa_internaute WHERE recovery = @recovery AND recovery <> '';"
                command.Parameters.AddWithValue("@recovery", If(recovery, ""))
                Using reader As SqlDataReader = command.ExecuteReader()
                    If reader.Read() Then
                        user = New Internaute(reader)
                    End If
                End Using
            Catch ex As Exception
                Throw ex
            End Try
        End Using
        Return user
    End Function

    Public Function GetInternauteById(id As Long) As Internaute
        Dim user As Internaute = Nothing

        Using con As SqlConnection = GetConnection()
            Dim command As SqlCommand = con.CreateCommand()
            Try
                command.CommandText = "SELECT * FROM oasis.oa_internaute WHERE id=@id;"
                command.Parameters.AddWithValue("@id", id)
                Using reader As SqlDataReader = command.ExecuteReader()
                    If reader.Read() Then
                        user = New Internaute(reader)
                    End If
                End Using
            Catch ex As Exception
                Throw ex
            End Try
        End Using
        Return user
    End Function

    Public Function Update(internaute As Internaute) As Long
        Dim internauteId As Long

        Dim SQLstring As String =
            "UPDATE oasis.oa_internaute SET password=@password, recovery=@recovery, code=@code," & vbCrLf &
            " recovery_expiration=@recovery_expiration WHERE id=@id;"

        Using con As SqlConnection = GetConnection()
            Using cmd As New SqlCommand(SQLstring, con)
                With cmd.Parameters
                    .AddWithValue("@id", internaute.Id)
                    .AddWithValue("@password", If(internaute.Password, CObj(DBNull.Value)))
                    .AddWithValue("@recovery", If(internaute.Recovery, CObj(DBNull.Value)))
                    .AddWithValue("@code", If(internaute.Code, CObj(DBNull.Value)))
                    .AddWithValue("@recovery_expiration", If(internaute.RecoveryExpiration.HasValue, CObj(internaute.RecoveryExpiration.Value), CObj(DBNull.Value)))
                End With
                cmd.ExecuteNonQuery()
                internauteId = internaute.Id
            End Using
        End Using

        Return internauteId
    End Function

    ''' <summary>
    ''' Enregistre une demande de récupération de mot de passe SANS toucher au mot
    ''' de passe existant. Effacer le mot de passe avant toute preuve de possession
    ''' de la boîte mail permettait de bloquer n'importe quel compte à distance.
    ''' </summary>
    Public Function UpdateRecovery(internauteId As Integer, recovery As String, expiration As Date, code As String) As Long
        Dim SQLstring As String =
            "UPDATE oasis.oa_internaute SET recovery=@recovery, recovery_expiration=@expiration, code=@code WHERE id=@id;"

        Using con As SqlConnection = GetConnection()
            Using cmd As New SqlCommand(SQLstring, con)
                With cmd.Parameters
                    .AddWithValue("@id", internauteId)
                    .AddWithValue("@recovery", recovery)
                    .AddWithValue("@expiration", expiration)
                    .AddWithValue("@code", If(code, CObj(DBNull.Value)))
                End With
                Return cmd.ExecuteNonQuery()
            End Using
        End Using
    End Function

    ''' <summary>
    ''' Compteur de verrouillage côté serveur : le client ne peut pas le contourner,
    ''' contrairement au compteur stocké dans la base de registre du poste.
    ''' </summary>
    Public Sub EnregistrerEchec(internauteId As Integer, seuil As Integer, dureeVerrouMinutes As Integer)
        Dim SQLstring As String =
            "UPDATE oasis.oa_internaute SET tentatives = COALESCE(tentatives, 0) + 1," & vbCrLf &
            " verrou_jusqua = CASE WHEN COALESCE(tentatives, 0) + 1 >= @seuil" & vbCrLf &
            "                      THEN DATEADD(minute, @duree, SYSDATETIME()) ELSE verrou_jusqua END" & vbCrLf &
            " WHERE id=@id;"

        Using con As SqlConnection = GetConnection()
            Using cmd As New SqlCommand(SQLstring, con)
                cmd.Parameters.AddWithValue("@id", internauteId)
                cmd.Parameters.AddWithValue("@seuil", seuil)
                cmd.Parameters.AddWithValue("@duree", dureeVerrouMinutes)
                cmd.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    ''' <summary>Remet le compteur d'échecs à zéro après une authentification réussie.</summary>
    Public Sub ReinitialiserEchecs(internauteId As Integer)
        Using con As SqlConnection = GetConnection()
            Using cmd As New SqlCommand("UPDATE oasis.oa_internaute SET tentatives = 0, verrou_jusqua = NULL WHERE id=@id;", con)
                cmd.Parameters.AddWithValue("@id", internauteId)
                cmd.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    Public Function GetInternauteByNIR(nir As String) As Internaute
        Dim user As Internaute = Nothing

        Using con As SqlConnection = GetConnection()
            Dim command As SqlCommand = con.CreateCommand()
            Try
                command.CommandText = "SELECT * FROM oasis.oa_internaute WHERE nir = @nir;"
                command.Parameters.AddWithValue("@nir", nir)
                Using reader As SqlDataReader = command.ExecuteReader()
                    If reader.Read() Then
                        user = New Internaute(reader)
                    End If
                End Using
            Catch ex As Exception
                Throw ex
            End Try
        End Using
        Return user
    End Function

    ''' <summary>
    ''' Vrai si un compte portail existe déjà pour cette adresse.
    '''
    ''' Le client lourd posait la question en chargeant le compte entier, ce qui
    ''' l'obligeait à lire l'empreinte du mot de passe et la clé de récupération
    ''' du patient. La base lui refuse désormais ces deux colonnes : il ne lui
    ''' faut ici qu'une réponse par oui ou par non.
    ''' </summary>
    Public Function ExisteInternautePourEmail(email As String) As Boolean
        If String.IsNullOrWhiteSpace(email) Then Return False

        Using con As SqlConnection = GetConnection()
            Using cmd As New SqlCommand(
                "SELECT COUNT(1) FROM oasis.oa_internaute WHERE email = @email;", con)
                cmd.Parameters.AddWithValue("@email", email)
                Return CInt(cmd.ExecuteScalar()) > 0
            End Using
        End Using
    End Function

    ''' <summary>
    ''' Charge un compte portail complet, secrets compris. Réservé au serveur :
    ''' la base refuse au compte du client lourd la lecture de password et recovery.
    ''' </summary>
    Public Function GetInternauteByEmail(email As String) As Internaute
        Dim user As Internaute = Nothing

        Using con As SqlConnection = GetConnection()
            Dim command As SqlCommand = con.CreateCommand()
            Try
                command.CommandText = "SELECT * FROM oasis.oa_internaute WHERE email = @email;"
                command.Parameters.AddWithValue("@email", email)
                Using reader As SqlDataReader = command.ExecuteReader()
                    If reader.Read() Then
                        user = New Internaute(reader)
                    End If
                End Using
            Catch ex As Exception
                Throw ex
            End Try
        End Using
        Return user
    End Function

    Private Const SeuilVerrou As Integer = 5
    Private Const DureeVerrouMinutes As Integer = 15

    Private Sub ControlPassword(user As Internaute, password As String)
        ' Verrouillage côté serveur : le portail n'avait aucune limite au nombre
        ' d'essais.
        If user.VerrouJusqua.HasValue AndAlso user.VerrouJusqua.Value > DateTime.Now Then
            Throw New ArgumentException("Compte temporairement verrouillé suite à plusieurs échecs. Réessayez plus tard.")
        End If

        Dim doitEtreRehache As Boolean
        Dim correct = MotDePasse.VerifierAvecMigration(
            password, user.Password,
            Internaute.CryptePwd(If(user.Email, "").ToString(), password), doitEtreRehache)

        If Not correct Then
            Try
                EnregistrerEchec(user.Id, SeuilVerrou, DureeVerrouMinutes)
            Catch ex As Exception
            End Try
            Throw New ArgumentException("Identifiant et/ou mot de passe erroné !")
        End If

        ' Migration transparente vers PBKDF2 à la première connexion réussie.
        If doitEtreRehache Then
            Try
                UpdateEmpreinteMotDePasse(user.Id, MotDePasse.Hacher(password))
            Catch ex As Exception
            End Try
        End If

        Try
            ReinitialiserEchecs(user.Id)
        Catch ex As Exception
        End Try

        ' Le mot de passe en clair ne doit pas rester sur le bean.
        user.Password = Nothing
    End Sub

    ''' <summary>Réenregistre l'empreinte du mot de passe, sans rien changer d'autre.</summary>
    Public Sub UpdateEmpreinteMotDePasse(internauteId As Integer, empreinte As String)
        Using con As SqlConnection = GetConnection()
            Using cmd As New SqlCommand("UPDATE oasis.oa_internaute SET password = @password WHERE id = @id;", con)
                cmd.Parameters.AddWithValue("@password", empreinte)
                cmd.Parameters.AddWithValue("@id", internauteId)
                cmd.ExecuteNonQuery()
            End Using
        End Using
    End Sub

End Class
