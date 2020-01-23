Imports System.Data.SqlClient
Imports Oasis_Common

Public Class DrcStandardDao
    Inherits StandardDao

    Friend Function GetDrcStandardById(drcStandardId As Integer) As DrcStandard
        Dim drcStandard As DrcStandard
        Dim con As SqlConnection

        con = GetConnection()

        Try
            Dim command As SqlCommand = con.CreateCommand()

            command.CommandText =
                "SELECT * FROM oasis.oa_drc_standard WHERE id = @id"
            command.Parameters.AddWithValue("@id", drcStandardId)
            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    drcStandard = BuildBean(reader)
                Else
                    Throw New ArgumentException("DRC Standard inexistante !")
                End If
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try

        Return drcStandard
    End Function

    Private Function BuildBean(reader As SqlDataReader) As DrcStandard
        Dim drcStandard As New DrcStandard

        drcStandard.Id = reader("id")
        drcStandard.TypeActivite = Coalesce(reader("type_activite_episode"), "")
        drcStandard.DrcId = Coalesce(reader("drc_id"), 0)
        drcStandard.CategorieOasis = Coalesce(reader("categorie_oasis"), 0)
        drcStandard.AgeMin = Coalesce(reader("age_min"), 0)
        drcStandard.AgeMax = Coalesce(reader("age_max"), 0)
        drcStandard.DateModification = Coalesce(reader("date_modification"), Nothing)
        drcStandard.Inactif = Coalesce(reader("inactif"), False)

        Return drcStandard
    End Function

    Public Function getAllDrcByTypeActivite(TypeActivite As String) As DataTable
        Dim SQLString As String
        SQLString =
            "SELECT * FROM oasis.oa_drc_standard" &
            " WHERE type_activite_episode = '" & TypeActivite & "'" &
            " AND (inactif = 'False' or inactif is Null)"

        Dim ParcoursDataTable As DataTable = New DataTable()

        Using con As SqlConnection = GetConnection()
            Dim DrcDataAdapter As SqlDataAdapter = New SqlDataAdapter()
            Using DrcDataAdapter
                DrcDataAdapter.SelectCommand = New SqlCommand(SQLString, con)
                'Using ParcoursDataTable
                Try
                    DrcDataAdapter.Fill(ParcoursDataTable)
                    Dim command As SqlCommand = con.CreateCommand()
                Catch ex As Exception
                    Throw ex
                Finally

                End Try
            End Using
        End Using

        Return ParcoursDataTable
    End Function

    Friend Function GetDrcStandardByTypeActivite(typeActivite As String) As DataTable
        Dim SQLString As String

        SQLString =
            "SELECT " & vbCrLf &
            "	  id," & vbCrLf &
            "	  type_activite_episode," & vbCrLf &
            "	  drc_id," & vbCrLf &
            "	  categorie_oasis," & vbCrLf &
            "	  age_min," & vbCrLf &
            "	  age_max" & vbCrLf &
            " FROM oasis.oa_drc_standard" & vbCrLf &
            " WHERE type_activite_episode = @typeActivite" & vbCrLf &
            " AND (inactif = 'False' OR inactif is Null)" & vbCrLf

        'Console.WriteLine(SQLString)

        Using con As SqlConnection = GetConnection()

            Dim drcDataAdapter As SqlDataAdapter = New SqlDataAdapter()
            Using drcDataAdapter
                drcDataAdapter.SelectCommand = New SqlCommand(SQLString, con)
                drcDataAdapter.SelectCommand.Parameters.AddWithValue("@typeActivite", typeActivite)
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

    Friend Function CreationDrcStandard(drcStandard As DrcStandard) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim NbCreate As Integer
        Dim con As SqlConnection
        con = GetConnection()

        Dim SQLstring As String =
        "IF NOT EXISTS (SELECT 1 FROM oasis.oa_drc_standard" &
        " WHERE type_activite_episode = @typeActivite" &
        " AND drc_id = @drcId" &
        " AND (inactif = 'False' or inactif is Null))" &
        "INSERT INTO oasis.oa_drc_standard" &
        " (type_activite_episode, drc_id, categorie_oasis, age_min, age_max)" &
        " VALUES (@typeActivite, @drcId, @categorieOasis, @ageMin, @ageMax)"

        Dim cmd As New SqlCommand(SQLstring, con)
        With cmd.Parameters
            .AddWithValue("@typeActivite", drcStandard.TypeActivite)
            .AddWithValue("@drcId", drcStandard.DrcId)
            .AddWithValue("@categorieOasis", drcStandard.CategorieOasis)
            .AddWithValue("@ageMin", drcStandard.AgeMin)
            .AddWithValue("@ageMax", drcStandard.AgeMax)
        End With

        Try
            da.InsertCommand = cmd
            NbCreate = da.InsertCommand.ExecuteNonQuery()
            If NbCreate <= 0 Then
                Throw New ArgumentException("Collision: la DRC sélectionnée existe déjà pour ce type d'activité d'épisode : " & drcStandard.TypeActivite)
            End If
        Catch ex As Exception
            codeRetour = False
            Throw ex
        Finally
            con.Close()
        End Try

        Return codeRetour
    End Function

    Friend Function GetDrcStandardCreated(drcStandard As DrcStandard) As Long
        Dim DrcStandardId As Long = 0
        Dim con As SqlConnection

        con = GetConnection()

        Try
            Dim command As SqlCommand = con.CreateCommand()

            command.CommandText =
                "SELECT MAX(id) FROM oasis.oa_drc_standard WHERE type_activite_episode = '" & drcStandard.TypeActivite & "'" &
                " AND drc_id = " & drcStandard.DrcId &
                " AND (inactif = 'False' or inactif is Null)"
            command.Parameters.AddWithValue("@id", DrcStandardId)
            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.HasRows Then
                    reader.Read()
                    'Récupération de la clé de l'enregistrement créé
                    DrcStandardId = reader(0)
                Else
                    Throw New ArgumentException("Pasc de DRC Standard !")
                End If
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try

        Return DrcStandardId
    End Function

    Friend Function SuppressionDrcStandard(Id As Long) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim con As SqlConnection = GetConnection()

        Dim SQLstring As String = "DELETE oasis.oa_drc_standard" &
        " WHERE id = @Id"

        Dim cmd As New SqlCommand(SQLstring, con)

        With cmd.Parameters
            .AddWithValue("@Id", Id)
        End With

        Try
            da.DeleteCommand = cmd
            da.DeleteCommand.ExecuteNonQuery()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            codeRetour = False
        Finally
            con.Close()
        End Try

        Return codeRetour
    End Function

    Friend Function ModificationDrcStandard(drcStandard As DrcStandard) As Boolean
        Dim NbUpdate As Integer
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim con As SqlConnection = GetConnection()

        Dim SQLstring As String = "UPDATE oasis.oa_drc_standard SET" &
        " age_min = @ageMin, age_max = @ageMax, date_modification = @dateModification" &
        " WHERE id = @Id"

        Dim cmd As New SqlCommand(SQLstring, con)

        With cmd.Parameters
            .AddWithValue("@Id", drcStandard.Id)
            .AddWithValue("@ageMin", drcStandard.AgeMin)
            .AddWithValue("@ageMax", drcStandard.AgeMax)
            .AddWithValue("@dateModification", Date.Now().ToString("yyyy-MM-dd HH:mm:ss"))
        End With

        Try
            da.UpdateCommand = cmd
            NbUpdate = da.UpdateCommand.ExecuteNonQuery()
            If NbUpdate <= 0 Then
                Dim anomalie As String = "La modification de la DRC standard n'a pas abouti - Id : " & drcStandard.Id.ToString & " DRC N° : " & drcStandard.DrcId.ToString
                MessageBox.Show(anomalie)
                Throw New Exception(anomalie)
                CreateLog(anomalie, "DrcStandardDao", LogDao.EnumTypeLog.ERREUR.ToString)
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            codeRetour = False
        Finally
            con.Close()
        End Try

        Return codeRetour
    End Function

    Friend Function AnnulationDrcStandard(Id As Long) As Boolean
        Dim NbUpdate As Integer
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim con As SqlConnection = GetConnection()

        Dim SQLstring As String = "UPDATE oasis.oa_drc_standard SET" &
        " date_modification = @dateModification, inactif = @inactif" &
        " WHERE id = @Id"

        Dim cmd As New SqlCommand(SQLstring, con)

        With cmd.Parameters
            .AddWithValue("@Id", Id)
            .AddWithValue("@inactif", True)
            .AddWithValue("@dateModification", Date.Now().ToString("yyyy-MM-dd HH:mm:ss"))
        End With

        Try
            da.UpdateCommand = cmd
            NbUpdate = da.UpdateCommand.ExecuteNonQuery()
            If NbUpdate <= 0 Then
                Dim anomalie As String = "L'annulation de la DRC standard n'a pas abouti - Id : " & Id.ToString
                MessageBox.Show(anomalie)
                Throw New Exception(anomalie)
                CreateLog(anomalie, "DrcStandardDao", LogDao.EnumTypeLog.ERREUR.ToString)
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            codeRetour = False
        Finally
            con.Close()
        End Try

        Return codeRetour
    End Function

End Class
