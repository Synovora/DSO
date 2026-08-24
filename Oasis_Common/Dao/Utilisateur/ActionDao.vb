Imports System.Data.SqlClient
Imports System.Windows.Forms

Public Class ActionDao
    Inherits StandardDao

    Public Structure EnumFonctionCode
        Const SYNTHESE = "SYNTHESE"
        Const EPISODE = "EPISODE"
    End Structure

    Public Function getAllActionByUser(userId As Long) As DataTable
        Dim SQLString As String = "SELECT horodatage, action, oa_patient_prenom, oa_patient_nom FROM oasis.oa_action" &
            " LEFT JOIN oasis.oa_patient ON oa_patient_id = patient_id" &
            " WHERE utilisateur_id = " + userId.ToString &
            " ORDER BY horodatage DESC"

        Using con As SqlConnection = GetConnection()
            Dim da As SqlDataAdapter = New SqlDataAdapter()
            Using da
                da.SelectCommand = New SqlCommand(SQLString, con)
                Dim dt As DataTable = New DataTable()
                Using dt
                    Try
                        da.Fill(dt)
                        Dim command As SqlCommand = con.CreateCommand()
                    Catch ex As Exception
                        Throw ex
                    End Try
                    Return dt
                End Using
            End Using
        End Using
    End Function

    Public Function getAllActionByUserAndDate(userId As Long, dateSelection As Date) As DataTable
        Dim jour As Integer = dateSelection.Day
        Dim mois As Integer = dateSelection.Month
        Dim An As Integer = dateSelection.Year
        Dim dateSelectionDebut As Date = New Date(An, mois, jour, 0, 0, 0)
        Dim dateSelectionFin As Date = New Date(An, mois, jour, 23, 59, 59)

        Dim SQLString As String = "SELECT horodatage, action, oa_patient_prenom, oa_patient_nom, patient_id, fonction, fonction_id FROM oasis.oa_action" &
            " LEFT JOIN oasis.oa_patient ON oa_patient_id = patient_id" &
            " WHERE utilisateur_id = " + userId.ToString &
            " AND horodatage >= '" & dateSelectionDebut.ToString("yyyy-MM-dd HH:mm:ss") & "'" &
            " AND horodatage <= '" & dateSelectionFin.ToString("yyyy-MM-dd HH:mm:ss") & "'" &
            " ORDER BY horodatage DESC"

        Using con As SqlConnection = GetConnection()
            Dim da As SqlDataAdapter = New SqlDataAdapter()
            Using da
                da.SelectCommand = New SqlCommand(SQLString, con)
                Dim dt As DataTable = New DataTable()
                Using dt
                    Try
                        da.Fill(dt)
                        Dim command As SqlCommand = con.CreateCommand()
                    Catch ex As Exception
                        Throw ex
                    End Try
                    Return dt
                End Using
            End Using
        End Using
    End Function

    Public Function getTraitementById(actionId As Long) As Action
        Dim action As Action
        Dim con As SqlConnection

        con = GetConnection()

        Try
            Dim command As SqlCommand = con.CreateCommand()

            command.CommandText =
                "select * from oasis.oa_action where oa_action_id = @id"
            command.Parameters.AddWithValue("@id", actionId)
            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    action = buildBean(reader)
                Else
                    Throw New ArgumentException("Note patient inexistante !")
                End If
            End Using

        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try

        Return action
    End Function

    ''' <summary>
    ''' Lit une ligne telle quelle, sans toucher à la base. IDataRecord plutôt que
    ''' SqlDataReader pour qu'un test puisse fournir la ligne.
    ''' </summary>
    Public Shared Function buildBean(record As System.Data.IDataRecord) As Action
        Dim action As New Action

        action.ActionId = record("action_id")
        action.PatientId = Coalesce(record("patient_id"), 0)
        action.UtilisateurId = Coalesce(record("utilisateur_id"), 0)
        action.Horodatage = Coalesce(record("horodatage"), Nothing)
        action.Action = Coalesce(record("action"), "")
        action.Fonction = Coalesce(record("fonction"), "")
        action.FonctionId = Coalesce(record("fonction_id"), 0)
        Return action
    End Function

    Public Function CreationAction(action As Action) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim con As SqlConnection
        con = GetConnection()

        Dim SQLstring As String = "INSERT into oasis.oa_action" &
        " (utilisateur_id, patient_id, horodatage, action, fonction, fonction_id)" &
        " VALUES (@utilisateurId, @patientId, @horodatage, @action, @fonction, @fonctionId)"

        Dim cmd As New SqlCommand(SQLstring, con)
        With cmd.Parameters
            .AddWithValue("@utilisateurId", action.UtilisateurId)
            .AddWithValue("@patientId", action.PatientId)
            .AddWithValue("@horodatage", Date.Now())
            .AddWithValue("@action", action.Action)
            .AddWithValue("@fonction", action.Fonction)
            .AddWithValue("@fonctionId", action.FonctionId)
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

End Class
