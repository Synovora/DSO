Imports System.Data.SqlClient
Module AldCim10Dao

    Public Function GetAldCim10ById(AldCim10Id As Integer) As AldCim10
        Dim aldCim10 As AldCim10
        Dim con As New SqlConnection(getConnectionString())

        Try
            Dim command As SqlCommand = con.CreateCommand()

            command.CommandText =
                "select * from oasis.oa_ald_cim10 where oa_ald_cim10_id = @id"
            command.Parameters.AddWithValue("@id", AldCim10Id)
            con.Open()
            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    aldCim10 = BuildBean(reader)
                Else
                    Throw New ArgumentException("ALD Cim10 inexistante !")
                End If
            End Using

        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try

        Return aldCim10
    End Function

    Private Function BuildBean(reader As SqlDataReader) As AldCim10
        Dim aldCim10 As New AldCim10
        aldCim10.AldCim10Id = Convert.ToInt64(reader("oa_ald_cim10_id"))
        aldCim10.AldCim10AldId = Coalesce(reader("oa_ald_cim10_ald_id"), 0)
        aldCim10.AldCim10AldCode = Coalesce(reader("oa_ald_cim10_ald_code"), "")
        aldCim10.AldCim10Code = Coalesce(reader("oa_ald_cim10_code"), "")
        aldCim10.AldCim10Description = Coalesce(reader("oa_ald_cim10_description"), "")
        Return aldCim10
    End Function

    'Initialisation des propriétés d'une instance de Drc depuis la BDD
    Public Function SetAldCim10(instanceAldCim10 As AldCim10, AldCim10Id As Integer) As Boolean
        Dim CodeRetour As Boolean = True
        Dim conxn As New SqlConnection(getConnectionString())
        Dim SQLString As String = "select * from oasis.oa_ald_cim10 where oa_ald_cim10_id = @aldCim10Id"
        Dim AldCim10DataReader As SqlDataReader
        Dim cmd As New SqlCommand(SQLString, conxn)

        cmd.Parameters.AddWithValue("@aldCim10Id", AldCim10Id.ToString)

        Try
            conxn.Open()
            AldCim10DataReader = cmd.ExecuteReader()
            setAldCim10Properties(instanceAldCim10, AldCim10DataReader)
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            Return CodeRetour = False
        Finally
            conxn.Close()
            cmd.Dispose()
        End Try

        Return CodeRetour

    End Function


    Private Sub setAldCim10Properties(instanceAldCim10 As AldCim10, drcDataReader As SqlDataReader)
        If drcDataReader.Read() Then
            instanceAldCim10.AldCim10Id = Convert.ToInt64(drcDataReader("oa_ald_cim10_id"))
            instanceAldCim10.AldCim10AldId = drcDataReader("oa_ald_cim10_ald_id")
            instanceAldCim10.AldCim10AldCode = drcDataReader("oa_ald_cim10_ald_code")
            instanceAldCim10.AldCim10Code = drcDataReader("oa_ald_cim10_code")
            instanceAldCim10.AldCim10Description = drcDataReader("oa_ald_cim10_description")
        Else
            instanceAldCim10.AldCim10Id = 0
            instanceAldCim10.AldCim10AldId = 0
            instanceAldCim10.AldCim10AldCode = ""
            instanceAldCim10.AldCim10Code = ""
            instanceAldCim10.AldCim10Description = ""
        End If
    End Sub
End Module
