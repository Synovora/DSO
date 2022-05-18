Imports System.Data.SqlClient

Public Class DrcActeParamedicalAssoDao
    Inherits StandardDao

    Public Function GetAllActeParamedicalAssoByProtocoleCollaboratifId(protocoleCollaboratifDrcId As Integer) As DataTable
        Dim SQLString As String
        SQLString =
            "SELECT * FROM oasis.oa_drc_acte_paramedical" &
            " WHERE drc_protocole_collaboratif_id = " & protocoleCollaboratifDrcId.ToString

        Dim ParcoursDataTable As DataTable = New DataTable()

        Using con As SqlConnection = GetConnection()
            Dim ActeParamedicalDataAdapter As SqlDataAdapter = New SqlDataAdapter()
            Using ActeParamedicalDataAdapter
                ActeParamedicalDataAdapter.SelectCommand = New SqlCommand(SQLString, con)
                'Using ParcoursDataTable
                Try
                    ActeParamedicalDataAdapter.Fill(ParcoursDataTable)
                    Dim command As SqlCommand = con.CreateCommand()
                Catch ex As Exception
                    Throw ex
                Finally

                End Try
            End Using
        End Using

        Return ParcoursDataTable
    End Function

    Public Function GetDrcActeParamedicalAssoById(Id As Integer) As DrcActeParamedicalAsso
        Dim drcActeParamedicalAsso As DrcActeParamedicalAsso
        Dim con As SqlConnection = GetConnection()

        Try
            Dim command As SqlCommand = con.CreateCommand()

            command.CommandText =
                "SELECT * FROM oasis.oa_drc_acte_paramedical WHERE oa_ror_id = @id"
            command.Parameters.AddWithValue("@id", Id)
            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    drcActeParamedicalAsso = BuildBean(reader)
                Else
                    Throw New ArgumentException("Association acte paramédical / protocole collaboratif inexistant !")
                End If
            End Using

        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try

        Return drcActeParamedicalAsso
    End Function

    Private Function BuildBean(reader As SqlDataReader) As DrcActeParamedicalAsso
        Dim drcActeParamedicalAsso As New DrcActeParamedicalAsso With {
            .Id = reader("id"),
            .ProtocleCollabaratifDrcId = Coalesce(reader("drc_protocole_collaboratif_id"), 0),
            .ActeParamedicalDrcId = Coalesce(reader("drc_acte_paramedical_id"), 0)
        }
        Return drcActeParamedicalAsso
    End Function

    Public Function CreateDrcActeParamedicalAsso(drcActeParamedicalAsso As DrcActeParamedicalAsso) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim NbCreate As Integer
        Dim con As SqlConnection
        con = GetConnection()

        Dim SQLstring As String =
        "IF NOT EXISTS (SELECT 1 FROM oasis.oa_drc_acte_paramedical WHERE drc_acte_paramedical_id = @acteParamedicalId AND drc_protocole_collaboratif_id = @protocoleCollaboratifId)" &
        "INSERT INTO oasis.oa_drc_acte_paramedical" &
        " (drc_protocole_collaboratif_id, drc_acte_paramedical_id)" &
        " VALUES (@protocoleCollaboratifId, @acteParamedicalId)"

        Dim cmd As New SqlCommand(SQLstring, con)
        With cmd.Parameters
            .AddWithValue("@protocoleCollaboratifId", drcActeParamedicalAsso.ProtocleCollabaratifDrcId)
            .AddWithValue("@acteParamedicalId", drcActeParamedicalAsso.ActeParamedicalDrcId)
        End With

        Try
            da.InsertCommand = cmd
            NbCreate = da.InsertCommand.ExecuteNonQuery()
            If NbCreate <= 0 Then
                Throw New ArgumentException("Collision: couple DRC Proctocole collaboratif / Acte médical existe déjà")
            End If
        Catch ex As Exception
            codeRetour = False
            Throw ex
        Finally
            con.Close()
        End Try

        Return codeRetour
    End Function

    Public Function SuppressionDrcActeParamedicalAsso(Id As Long) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim con As SqlConnection = GetConnection()

        Dim SQLstring As String = "DELETE oasis.oa_drc_acte_paramedical" &
        " WHERE id = @Id"

        Dim cmd As New SqlCommand(SQLstring, con)

        With cmd.Parameters
            .AddWithValue("@Id", Id)
        End With

        Try
            da.DeleteCommand = cmd
            da.DeleteCommand.ExecuteNonQuery()
        Catch ex As Exception
            Throw New Exception(ex.Message)
            codeRetour = False
        Finally
            con.Close()
        End Try

        Return codeRetour
    End Function
End Class
