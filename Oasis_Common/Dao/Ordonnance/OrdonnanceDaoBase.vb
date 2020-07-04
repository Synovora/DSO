Imports System.Data.SqlClient

Public MustInherit Class OrdonnanceDaoBase
    Inherits StandardDao

    Public Function GetOrdonnaceById(OrdonnanceId As Integer) As OrdonnanceBase
        Dim ordonnance As OrdonnanceBase
        Dim con As SqlConnection = GetConnection()

        Try
            Dim command As SqlCommand = con.CreateCommand()

            command.CommandText =
                "SELECT * FROM oasis.oa_patient_ordonnance WHERE oa_ordonnance_id = @ordonnanceId"
            command.Parameters.AddWithValue("@ordonnanceId", OrdonnanceId)
            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    ordonnance = BuildBean(reader)
                Else
                    Throw New ArgumentException("Ordonnance inexistante !")
                End If
            End Using

        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try

        Return ordonnance
    End Function

    Private Function BuildBean(reader As SqlDataReader) As OrdonnanceBase
        Dim ordonnance As New OrdonnanceBase With {
            .Id = reader("oa_ordonnance_id"),
            .PatientId = Coalesce(reader("oa_ordonnance_patient_id"), 0),
            .EpisodeId = Coalesce(reader("oa_ordonnance_episode_id"), 0),
            .UtilisateurCreation = Coalesce(reader("oa_ordonnance_utilisateur_creation"), 0),
            .DateCreation = Coalesce(reader("oa_ordonnance_date_creation"), Nothing),
            .DateValidation = Coalesce(reader("oa_ordonnance_date_validation"), Nothing),
            .UserValidation = Coalesce(reader("oa_ordonnance_user_validation"), 0),
            .DateEdition = Coalesce(reader("oa_ordonnance_date_edition"), Nothing),
            .Commentaire = Coalesce(reader("oa_ordonnance_commentaire"), ""),
            .Renouvellement = Coalesce(reader("oa_ordonnance_renouvellement"), 0),
            .Inactif = Coalesce(reader("oa_ordonnance_inactif"), False),
            .Signature = Coalesce(reader("oa_ordonnance_signature"), "")
        }
        Return ordonnance
    End Function

    Public Function GetAllOrdonnancebyPatient(patientId As Integer) As DataTable
        Dim SQLString As String
        SQLString = "SELECT * FROM oasis.oa_patient_ordonnance" &
                    " WHERE oa_ordonnance_patient_id = " & patientId.ToString &
                    " ORDER BY oa_ordonnance_id DESC"

        Using con As SqlConnection = GetConnection()
            Dim OrdonnanceDataAdapter As SqlDataAdapter = New SqlDataAdapter()
            Using OrdonnanceDataAdapter
                OrdonnanceDataAdapter.SelectCommand = New SqlCommand(SQLString, con)
                Dim OrdonnanceDataTable As DataTable = New DataTable()
                Using OrdonnanceDataTable
                    Try
                        OrdonnanceDataAdapter.Fill(OrdonnanceDataTable)
                        Dim command As SqlCommand = con.CreateCommand()
                    Catch ex As Exception
                        Throw ex
                    End Try
                    Return OrdonnanceDataTable
                End Using
            End Using
        End Using
    End Function

    Public Function GetOrdonnanceValidebyPatient(patientId As Integer, episodeId As Long) As DataTable
        Dim SQLString As String
        SQLString = "SELECT * FROM oasis.oa_patient_ordonnance" &
                    " WHERE oa_ordonnance_patient_id = " & patientId.ToString &
                    " AND oa_ordonnance_episode_id = " & episodeId.ToString &
                    " AND (oa_ordonnance_inactif = 'False' OR oa_ordonnance_inactif is Null)"

        Using con As SqlConnection = GetConnection()
            Dim da As SqlDataAdapter = New SqlDataAdapter()
            Using da
                da.SelectCommand = New SqlCommand(SQLString, con)
                Dim dt As DataTable = New DataTable()
                Using dt
                    Try
                        da.Fill(dt)
                        Dim command As SqlCommand = con.CreateCommand()
                        If dt.Rows.Count > 0 Then

                        End If
                    Catch ex As Exception
                        Throw ex
                    End Try
                    Return dt
                End Using
            End Using
        End Using
    End Function

End Class
