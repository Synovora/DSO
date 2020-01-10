Imports System.Data.SqlClient

Public Class ParcoursConsigneDao
    Inherits StandardDao

    Friend Function getParcoursConsigneById(ConsigneId As Integer) As ParcoursConsigne
        Dim parcoursConsigne As ParcoursConsigne
        Dim con As SqlConnection = GetConnection()

        Try
            Dim command As SqlCommand = con.CreateCommand()
            command.CommandText =
                "select * from oasis.oa_patient_parcours_consigne where oa_parcours_consigne_id = @id"
            command.Parameters.AddWithValue("@id", ConsigneId)
            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    parcoursConsigne = buildBean(reader)
                Else
                    Throw New ArgumentException("parcours consigne inexistant !")
                End If
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try

        Return parcoursConsigne
    End Function

    Private Function buildBean(reader As SqlDataReader) As ParcoursConsigne
        Dim parcoursConsigne As New ParcoursConsigne

        parcoursConsigne.Id = reader("oa_parcours_consigne_id")
        parcoursConsigne.PatientId = Coalesce(reader("oa_parcours_consigne_patient_id"), 0)
        parcoursConsigne.ParcoursId = Coalesce(reader("oa_parcours_id"), 0)
        parcoursConsigne.DrcId = Coalesce(reader("oa_parcours_consigne_drc_id"), 0)
        parcoursConsigne.TypeEpisode = Coalesce(reader("activite_type_episode"), "")
        parcoursConsigne.Commentaire = Coalesce(reader("oa_parcours_consigne_commentaire"), "")
        parcoursConsigne.Ordre = Coalesce(reader("oa_parcours_consigne_ordre"), 0)
        parcoursConsigne.AgeMin = Coalesce(reader("oa_parcours_age_min"), 0)
        parcoursConsigne.AgeMax = Coalesce(reader("oa_parcours_age_max"), 0)
        parcoursConsigne.AgeUnite = Coalesce(reader("oa_parcours_age_unite"), "")
        parcoursConsigne.DateDebut = Coalesce(reader("oa_parcours_consigne_date_debut"), Nothing)
        parcoursConsigne.DateFin = Coalesce(reader("oa_parcours_consigne_date_fin"), Nothing)
        parcoursConsigne.Inactif = Coalesce(reader("oa_parcours_consigne_inactif"), False)

        Return parcoursConsigne
    End Function

    Friend Function getConsigneParamedicalebyParcoursId(parcoursId As Integer) As DataTable
        Dim SQLString As String = "SELECT oa_parcours_consigne_id, oa_parcours_id, oa_parcours_consigne_patient_id, oa_parcours_consigne_drc_id," &
            " activite_type_episode, oa_parcours_consigne_commentaire, oa_parcours_consigne_ordre, oa_parcours_consigne_date_debut," &
            " oa_parcours_consigne_date_fin, oa_drc_libelle FROM oasis.oa_patient_parcours_consigne" &
            " LEFT JOIN oasis.oa_drc ON oa_parcours_consigne_drc_id = oa_drc_id" &
            " WHERE oa_parcours_id = " + parcoursId.ToString &
            " AND oa_drc_oasis_categorie = " & DrcDao.EnumCategorieOasisCode.ActeParamedical &
            " AND (oa_parcours_consigne_inactif Is Null Or oa_parcours_consigne_inactif = 'False')" &
            " AND (oa_parcours_consigne_date_fin is Null OR oa_parcours_consigne_date_fin >= GETDATE())" &
            " ORDER BY oa_parcours_consigne_ordre"

        Using con As SqlConnection = GetConnection()
            Dim ParcoursConsigneDataAdapter As SqlDataAdapter = New SqlDataAdapter()
            Using ParcoursConsigneDataAdapter
                ParcoursConsigneDataAdapter.SelectCommand = New SqlCommand(SQLString, con)
                Dim ParcoursConsigneDataTable As DataTable = New DataTable()
                Using ParcoursConsigneDataTable
                    Try
                        ParcoursConsigneDataAdapter.Fill(ParcoursConsigneDataTable)
                        Dim command As SqlCommand = con.CreateCommand()
                    Catch ex As Exception
                        Throw ex
                    End Try
                    Return ParcoursConsigneDataTable
                End Using
            End Using
        End Using
    End Function

    Friend Function getAllConsignebyParcoursId(parcoursId As Integer) As DataTable
        Dim SQLString As String = "SELECT oa_parcours_consigne_id, oa_parcours_id, oa_parcours_consigne_patient_id, oa_parcours_consigne_drc_id," &
            " activite_type_episode, oa_parcours_consigne_commentaire, oa_parcours_consigne_ordre, oa_parcours_consigne_date_debut," &
            " oa_parcours_consigne_date_fin, oa_drc_libelle FROM oasis.oa_patient_parcours_consigne" &
            " LEFT JOIN oasis.oa_drc ON oa_parcours_consigne_drc_id = oa_drc_id" &
            " WHERE oa_parcours_id = " + parcoursId.ToString &
            " AND (oa_parcours_consigne_inactif Is Null Or oa_parcours_consigne_inactif = 'False')" &
            " AND (oa_parcours_consigne_date_fin is Null OR oa_parcours_consigne_date_fin >= GETDATE())" &
            " ORDER BY oa_parcours_consigne_ordre"

        Using con As SqlConnection = GetConnection()
            Dim ParcoursConsigneDataAdapter As SqlDataAdapter = New SqlDataAdapter()
            Using ParcoursConsigneDataAdapter
                ParcoursConsigneDataAdapter.SelectCommand = New SqlCommand(SQLString, con)
                Dim ParcoursConsigneDataTable As DataTable = New DataTable()
                Using ParcoursConsigneDataTable
                    Try
                        ParcoursConsigneDataAdapter.Fill(ParcoursConsigneDataTable)
                        Dim command As SqlCommand = con.CreateCommand()
                    Catch ex As Exception
                        Throw ex
                    End Try
                    Return ParcoursConsigneDataTable
                End Using
            End Using
        End Using
    End Function

    Friend Function getDrcConsigneByActiviteEtPatientId(typeActivite As String, patientId As Long) As DataTable
        Dim SQLString As String = "SELECT oa_parcours_consigne_drc_id, oa_drc_oasis_categorie" &
            " LEFT JOIN oasis.oa_drc ON oa_parcours_consigne_drc_id = oa_drc_id" &
            " WHERE oa_parcours_consigne_patient_id = " + patientId.ToString &
            " AND activite_type_episode = '" & typeActivite & "'" &
            " AND (oa_parcours_consigne_inactif Is Null Or oa_parcours_consigne_inactif = 'False')" &
            " AND (oa_parcours_consigne_date_fin is Null OR oa_parcours_consigne_date_fin >= GETDATE())" &
            " ORDER BY oa_parcours_consigne_ordre"

        Using con As SqlConnection = GetConnection()
            Dim ParcoursConsigneDataAdapter As SqlDataAdapter = New SqlDataAdapter()
            Using ParcoursConsigneDataAdapter
                ParcoursConsigneDataAdapter.SelectCommand = New SqlCommand(SQLString, con)
                Dim ParcoursConsigneDataTable As DataTable = New DataTable()
                Using ParcoursConsigneDataTable
                    Try
                        ParcoursConsigneDataAdapter.Fill(ParcoursConsigneDataTable)
                        Dim command As SqlCommand = con.CreateCommand()
                    Catch ex As Exception
                        Throw ex
                    End Try
                    Return ParcoursConsigneDataTable
                End Using
            End Using
        End Using
    End Function

    Friend Function GetDrcPatientByTypeActiviteEtPatient(typeActivite As String, patientId As Long) As DataTable
        Dim SQLString As String

        SQLString =
            "SELECT " & vbCrLf &
            "	  oa_parcours_consigne_id," & vbCrLf &
            "	  activite_type_episode," & vbCrLf &
            "	  D.oa_drc_oasis_categorie," & vbCrLf &
            "	  oa_parcours_consigne_drc_id," & vbCrLf &
            "	  oa_parcours_age_min," & vbCrLf &
            "	  oa_parcours_age_max," & vbCrLf &
            "	  oa_parcours_consigne_date_debut," & vbCrLf &
            "	  oa_parcours_consigne_date_fin" & vbCrLf &
            " FROM oasis.oa_patient_parcours_consigne" & vbCrLf &
            " LEFT JOIN oasis.oa_drc D ON D.oa_drc_id = oa_parcours_consigne_drc_id" &
            " WHERE activite_type_episode = @typeActivite" & vbCrLf &
            " AND oa_parcours_consigne_patient_id = @patientId" & vbCrLf &
            " AND (oa_parcours_consigne_inactif = 'False' OR oa_parcours_consigne_inactif is Null)" & vbCrLf

        'Console.WriteLine(SQLString)

        Using con As SqlConnection = GetConnection()

            Dim drcDataAdapter As SqlDataAdapter = New SqlDataAdapter()
            Using drcDataAdapter
                drcDataAdapter.SelectCommand = New SqlCommand(SQLString, con)
                drcDataAdapter.SelectCommand.Parameters.AddWithValue("@typeActivite", typeActivite)
                drcDataAdapter.SelectCommand.Parameters.AddWithValue("@patientId", patientId)
                Dim dt As DataTable = New DataTable()
                Using dt
                    Try
                        drcDataAdapter.Fill(dt)
                    Catch ex As Exception
                        Throw ex
                    End Try
                    Return dt
                End Using
            End Using
        End Using
    End Function

    Friend Function CreateParcoursConsigne(parcoursconsigne As ParcoursConsigne) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim con As SqlConnection
        con = GetConnection()

        Dim SQLstring As String = "insert into oasis.oa_patient_parcours_consigne" &
        " (oa_parcours_id, oa_parcours_consigne_patient_id, oa_parcours_consigne_drc_id, activite_type_episode, oa_parcours_consigne_commentaire, oa_parcours_consigne_ordre," &
        " oa_parcours_age_min, oa_parcours_age_max, oa_parcours_age_unite, oa_parcours_consigne_date_debut, oa_parcours_consigne_date_fin, oa_parcours_consigne_inactif)" &
        " VALUES (@parcoursId, @patientId, @drcId, @typeEpisode, @commentaire, @ordre," &
        " @ageMin, @ageMax, @ageUnite, @dateDebut, @dateFin, @inactif)"

        Dim cmd As New SqlCommand(SQLstring, con)
        With cmd.Parameters
            .AddWithValue("@parcoursId", parcoursconsigne.ParcoursId)
            .AddWithValue("@patientId", parcoursconsigne.PatientId)
            .AddWithValue("@drcId", parcoursconsigne.DrcId)
            .AddWithValue("@typeEpisode", parcoursconsigne.TypeEpisode)
            .AddWithValue("@commentaire", parcoursconsigne.Commentaire)
            .AddWithValue("@ordre", parcoursconsigne.Ordre)
            .AddWithValue("@ageMin", parcoursconsigne.AgeMin)
            .AddWithValue("@ageMax", parcoursconsigne.AgeMax)
            .AddWithValue("@ageUnite", parcoursconsigne.AgeUnite)
            .AddWithValue("@dateDebut", parcoursconsigne.DateDebut)
            .AddWithValue("@dateFin", parcoursconsigne.DateFin)
            .AddWithValue("@inactif", parcoursconsigne.Inactif)
        End With

        Try
            da.InsertCommand = cmd
            da.InsertCommand.ExecuteNonQuery()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            codeRetour = False
        Finally
            con.Close()
        End Try

        Return codeRetour
    End Function

    Friend Function ModificationParcoursConsigne(parcoursConsigne As ParcoursConsigne) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim con As SqlConnection = GetConnection()

        Dim SQLstring As String = "UPDATE oasis.oa_patient_parcours_consigne SET" &
        " oa_parcours_id = @parcoursId, oa_parcours_consigne_patient_id = @patientId, oa_parcours_consigne_drc_id = @drcId," &
        " activite_type_episode = @typeEpisode, oa_parcours_consigne_commentaire = @commentaire," &
        " oa_parcours_consigne_ordre = @ordre, oa_parcours_age_min = @ageMin, oa_parcours_age_max = @ageMax, oa_parcours_age_unite = @ageUnite," &
        " oa_parcours_consigne_date_debut = @dateDebut, oa_parcours_consigne_date_fin = @dateFin, oa_parcours_consigne_inactif = @inactif" &
        " WHERE oa_parcours_consigne_id = @Id"

        Dim cmd As New SqlCommand(SQLstring, con)

        With cmd.Parameters
            .AddWithValue("@Id", parcoursConsigne.Id)
            .AddWithValue("@parcoursId", parcoursConsigne.ParcoursId)
            .AddWithValue("@patientId", parcoursConsigne.PatientId)
            .AddWithValue("@drcId", parcoursConsigne.DrcId)
            .AddWithValue("@typeEpisode", parcoursConsigne.TypeEpisode)
            .AddWithValue("@commentaire", parcoursConsigne.Commentaire)
            .AddWithValue("@ordre", parcoursConsigne.Ordre)
            .AddWithValue("@ageMin", parcoursConsigne.AgeMin)
            .AddWithValue("@ageMax", parcoursConsigne.AgeMax)
            .AddWithValue("@ageUnite", parcoursConsigne.AgeUnite)
            .AddWithValue("@dateDebut", parcoursConsigne.DateDebut)
            .AddWithValue("@dateFin", parcoursConsigne.DateFin)
            .AddWithValue("@inactif", parcoursConsigne.Inactif)
        End With

        Try
            da.UpdateCommand = cmd
            da.UpdateCommand.ExecuteNonQuery()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            codeRetour = False
        Finally
            con.Close()
        End Try

        Return codeRetour
    End Function

    Friend Function AnnulationParcoursConsigne(parcoursConsigne As ParcoursConsigne) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim con As SqlConnection = GetConnection()

        Dim SQLstring As String = "UPDATE oasis.oa_patient_parcours_consigne SET oa_parcours_consigne_inactif = @inactif" &
        " WHERE oa_parcours_consigne_id = @Id"

        Dim cmd As New SqlCommand(SQLstring, con)

        With cmd.Parameters
            .AddWithValue("@Id", parcoursConsigne.Id)
            .AddWithValue("@inactif", True)
        End With

        Try
            da.UpdateCommand = cmd
            da.UpdateCommand.ExecuteNonQuery()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            codeRetour = False
        Finally
            con.Close()
        End Try

        Return codeRetour
    End Function

    Friend Function ExisteParcoursConsigne(parcoursId As Long) As Boolean
        Dim con As SqlConnection
        Dim Existe As Boolean = False
        con = GetConnection()

        Try
            Dim command As SqlCommand = con.CreateCommand()

            command.CommandText =
                "SELECT TOP (1) * FROM oasis.oasis.oa_patient_parcours_consigne WHERE oa_parcours_id = @parcoursId"

            With command.Parameters
                .AddWithValue("@parcoursId", parcoursId)
            End With

            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    Existe = True
                End If
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try

        Return Existe
    End Function

End Class
