Imports System.Data.SqlClient

Public Class PatientNoteMedicaleDao
    Inherits StandardDao

    Friend Function getAllNoteMedicalebyPatient(patientId As Integer) As DataTable
        Dim SQLString As String = "select * from oasis.oa_patient_note_medicale where" &
        " (oa_patient_note_invalide = '0' or oa_patient_note_invalide is Null) and" &
        " oa_patient_id = " + patientId.ToString + " order by oa_patient_note_date_creation desc;"

        Using con As SqlConnection = GetConnection()
            Dim NoteMedicaleDataAdapter As SqlDataAdapter = New SqlDataAdapter()
            Using NoteMedicaleDataAdapter
                NoteMedicaleDataAdapter.SelectCommand = New SqlCommand(SQLString, con)
                Dim NoteMedicaleDataTable As DataTable = New DataTable()
                Using NoteMedicaleDataTable
                    Try
                        NoteMedicaleDataAdapter.Fill(NoteMedicaleDataTable)
                        Dim command As SqlCommand = con.CreateCommand()
                    Catch ex As Exception
                        Throw ex
                    End Try
                    Return NoteMedicaleDataTable
                End Using
            End Using
        End Using
    End Function

    Friend Function getTraitementById(traitementId As Integer) As PatientNote
        Dim patientNote As PatientNote
        Dim con As SqlConnection

        con = GetConnection()

        Try
            Dim command As SqlCommand = con.CreateCommand()

            command.CommandText =
                "select * from oasis.oa_patient_note_medicale where oa_patient_note_id = @id"
            command.Parameters.AddWithValue("@id", traitementId)
            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    patientNote = buildBean(reader)
                Else
                    Throw New ArgumentException("Note médicale patient inexistante !")
                End If
            End Using

        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try

        Return patientNote
    End Function

    Private Function buildBean(reader As SqlDataReader) As PatientNote
        Dim patientNote As New PatientNote

        patientNote.NoteId = reader("oa_patient_note_id")
        patientNote.PatientId = Coalesce(reader("oa_patient_id"), 0)
        patientNote.PatientNote = Coalesce(reader("oa_patient_note"), "")
        patientNote.UserCreation = Coalesce(reader("oa_patient_note_utilisateur_creation"), 0)
        patientNote.DateCreation = Coalesce(reader("oa_patient_note_date_creation"), Nothing)
        patientNote.UserModification = Coalesce(reader("oa_patient_note_utilisateur_modification"), 0)
        patientNote.DateModification = Coalesce(reader("oa_patient_note_date_modification"), Nothing)
        patientNote.Invalide = Coalesce(reader("oa_patient_note_invalide"), False)
        Return patientNote
    End Function

    Friend Function CreationNote(patientNote As PatientNote) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim con As SqlConnection
        con = GetConnection()


        Dim dateCreation As Date = Date.Now.Date

        Dim SQLstring As String = "insert into oasis.oa_patient_note_medicale" &
        " (oa_patient_id, oa_patient_note, oa_patient_note_utilisateur_creation, oa_patient_note_date_creation)" &
        " VALUES (@patientId, @note, @utilisateurCreation, @dateCreation)"

        Dim cmd As New SqlCommand(SQLstring, con)
        With cmd.Parameters
            .AddWithValue("@patientId", patientNote.PatientId.ToString)
            .AddWithValue("@dateCreation", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            .AddWithValue("@utilisateurCreation", patientNote.UserCreation.ToString)
            .AddWithValue("@note", patientNote.PatientNote)
        End With

        Try
            'con.Open()
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

    Friend Function ModificationNote(patientNote As PatientNote) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim con As SqlConnection
        con = GetConnection()

        Dim dateModification As Date = Date.Now.Date

        Dim SQLstring As String = "update oasis.oa_patient_note_medicale set" &
        " oa_patient_note_date_modification = @dateModification, oa_patient_note_utilisateur_modification = @utilisateurModification," &
        " oa_patient_note = @note where oa_patient_note_id = @noteId"

        Dim cmd As New SqlCommand(SQLstring, con)

        With cmd.Parameters
            .AddWithValue("@utilisateurModification", patientNote.UserModification.ToString)
            .AddWithValue("@dateModification", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            .AddWithValue("@note", patientNote.PatientNote)
            .AddWithValue("@noteId", patientNote.NoteId)
        End With

        Try
            'con.Open()
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

    Friend Function AnnulationNote(patientNote As PatientNote) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim con As SqlConnection
        con = GetConnection()

        Dim dateModification As Date = Date.Now.Date

        Dim SQLstring As String = "update oasis.oa_patient_note_medicale set oa_patient_note_date_modification = @dateModification," &
        " oa_patient_note_utilisateur_modification = @utilisateurModification, oa_patient_note_invalide = @invalide where oa_patient_note_id = @noteId"

        Dim cmd As New SqlCommand(SQLstring, con)

        With cmd.Parameters
            .AddWithValue("@utilisateurModification", patientNote.UserModification.ToString)
            .AddWithValue("@dateModification", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            .AddWithValue("@invalide", 1)
            .AddWithValue("@noteId", patientNote.NoteId)
        End With

        Try
            'con.Open()
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

End Class
