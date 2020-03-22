Imports System.Collections.Specialized
Imports System.Data.SqlClient
Imports Oasis_Common
Public Class TheriaqueDao
    Inherits StandardDao

    Public Enum EnumGetSpecialite
        VIRTUEL_NE_PAS_UTILISER
        ID_THERIAQUE
        CODE_UCD
        CODE_CIP
        LABO_TITULAIRE_AMM
        LABO_EXPLOITANT
        CODE_FICHE_INDICATION
        SUBSTANCE_ACTIVE
        EXCIPIENT
        CLASSE_EPHMRA
        CLASSE_ATC
        CLASSE_PHARMACO_THERAPEUTIQUE
        CLASSE_GESTION_AP_HP
        GENERIQUE_THERIAQUE
        GENERIQUE_AFSSAPS
        NOM_SPECIALITE
        CODE_UCD13
        CODE_CIP13
    End Enum

    Public Enum EnumMonoVir
        CLASSIQUE
        VIRTUEL
        NULL
    End Enum

    Public Enum EnumTypeEffetIndesirable
        CLINIQUE = 1
        PARA_CLINIQUE = 2
        CLINIQUE_SURDOSAGE = 3
        PARA_CLINIQUE_SURDOSAGE = 4
    End Enum

    Friend Function GetMedicamentById(medicamentCis As Integer) As Medicament
        Dim medicament As Medicament
        Dim con As SqlConnection

        con = GetConnection()

        Try
            Dim command As SqlCommand = con.CreateCommand()

            command.CommandText =
                "SELECT * FROM oasis.oa_r_medicament WHERE oa_medicament_cis = @Id"
            command.Parameters.AddWithValue("@id", medicamentCis)
            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    medicament = BuildBean(reader)
                Else
                    Throw New ArgumentException("Médicament inexistant !")
                End If
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try

        Return medicament
    End Function

    Private Function BuildBean(reader As SqlDataReader) As Medicament
        Dim medicament As New Medicament

        medicament.MedicamentCis = reader("oa_medicament_cis")
        medicament.MedicamentDci = Coalesce(reader("oa_medicament_dci"), "")
        medicament.Forme = Coalesce(reader("oa_medicament_forme"), "")
        medicament.Titulaire = Coalesce(reader("oa_medicament_titulaire"), "")
        medicament.VoieAdministration = Coalesce(reader("oa_medicament_voie_administration"), "")

        Return medicament
    End Function

    Public Function GetAllATC() As DataTable
        Dim SQLString As String
        SQLString = "SELECT CATC_CODE_PK, CATC_NOMF FROM [Theriak].[theriaque].[CATC_CLASSEATC]" &
                    " WHERE (CATC_CATC_CODE_FK is Null)" &
                    " ORDER BY CATC_CODE_PK"

        Dim dt As DataTable = New DataTable()

        Using con As SqlConnection = GetConnection()
            Dim da As SqlDataAdapter = New SqlDataAdapter()
            Using da
                da.SelectCommand = New SqlCommand(SQLString, con)
                Try
                    da.Fill(dt)
                    Dim command As SqlCommand = con.CreateCommand()
                Catch ex As Exception
                    Throw ex
                Finally
                    con.Close()
                End Try
            End Using
        End Using

        Return dt
    End Function

    Friend Function getATCByATC(CodeATC As String) As DataTable
        Dim dt As New DataTable
        Dim ds As New DataSet

        Using con As SqlConnection = GetConnection()
            Try
                Dim command As New SqlCommand("theriaque.GET_THE_ATC_ATC", con)
                command.CommandType = CommandType.StoredProcedure
                command.Connection.ChangeDatabase("Theriak")
                'command.CommandText = "theriaque.GET_THE_ATC_ATC"
                command.Parameters.AddWithValue("@codeId", CodeATC)

                Dim da As New SqlDataAdapter(command)
                da.Fill(dt)
            Catch ex As Exception
                Throw ex
            Finally
                con.Close()
            End Try
        End Using

        Return dt
    End Function

    Friend Function getSpecialiteByArgument(CodeId As String, VarTyp As EnumGetSpecialite, Monovir As Integer) As DataTable
        Dim dt As New DataTable
        Dim ds As New DataSet

        Using con As SqlConnection = GetConnection()
            Try
                Dim command As New SqlCommand("theriaque.GET_THE_SPECIALITE", con)
                command.CommandType = CommandType.StoredProcedure
                command.Connection.ChangeDatabase("Theriak")
                'command.CommandText = "theriaque.GET_THE_SPECIALITE"
                command.Parameters.AddWithValue("@codeId", CodeId)
                command.Parameters.AddWithValue("@VarTyp", VarTyp)
                If Monovir = TheriaqueDao.EnumMonoVir.NULL Then
                    command.Parameters.AddWithValue("@MonoVir", DBNull.Value)
                Else
                    command.Parameters.AddWithValue("@MonoVir", Monovir)
                End If

                Dim da As New SqlDataAdapter(command)
                da.Fill(dt)
            Catch ex As Exception
                Throw ex
            Finally
                con.Close()
            End Try
        End Using

        Return dt
    End Function

    Friend Function GetPharmacoCinetqueBySpecialite(CodeId As String) As String
        Dim dt As New DataTable
        Dim ds As New DataSet
        Dim Pharmacotext As String = ""
        Dim PremierPassage As Boolean = False

        Using con As SqlConnection = GetConnection()
            Try
                Dim command As New SqlCommand("theriaque.GET_THE_CINETIQUE_SPE", con)
                command.CommandType = CommandType.StoredProcedure
                command.Connection.ChangeDatabase("Theriak")
                command.Parameters.AddWithValue("@codeId", CodeId)

                Dim da As New SqlDataAdapter(command)
                da.Fill(dt)

                Dim rowCount As Integer = dt.Rows.Count - 1

                For i = 0 To rowCount Step 1
                    If PremierPassage = False Then
                        PremierPassage = True
                        Pharmacotext = dt.Rows(i)("PHARMACOTEXT")
                    Else
                        Pharmacotext += vbCrLf & dt.Rows(i)("PHARMACOTEXT")
                    End If
                Next
            Catch ex As Exception
                Throw ex
            Finally
                con.Close()
            End Try
        End Using

        Return Pharmacotext
    End Function

    Friend Function GetEffetIndesirableBySpecialite(CodeId As String, Type As EnumTypeEffetIndesirable) As StringCollection
        Dim dt As New DataTable
        Dim ds As New DataSet
        Dim sc As New StringCollection

        Using con As SqlConnection = GetConnection()
            Try
                Dim command As New SqlCommand("theriaque.GET_THE_EFFIND_SPE", con)
                command.CommandType = CommandType.StoredProcedure
                command.Connection.ChangeDatabase("Theriak")
                command.Parameters.AddWithValue("@codeId", CodeId)
                command.Parameters.AddWithValue("@type", Type)

                Dim da As New SqlDataAdapter(command)
                da.Fill(dt)

                Dim rowCount As Integer = dt.Rows.Count - 1

                For i = 0 To rowCount Step 1
                    sc.Add(dt.Rows(i)("TEXTEFFET"))
                Next
            Catch ex As Exception
                Throw ex
            Finally
                con.Close()
            End Try
        End Using

        Return sc
    End Function

    Friend Function GetPharmacoDynamiqueBySpecialite(CodeId As String) As String
        Dim dt As New DataTable
        Dim ds As New DataSet
        Dim PharmacoDynamique As String = ""
        Dim PremierPassage As Boolean = False


        Using con As SqlConnection = GetConnection()
            Try
                Dim command As New SqlCommand("theriaque.GET_THE_DET_PHDYNA", con)
                command.CommandType = CommandType.StoredProcedure
                command.Connection.ChangeDatabase("Theriak")
                command.Parameters.AddWithValue("@codeId", CodeId)
                command.Parameters.AddWithValue("@typId", 1)

                Dim da As New SqlDataAdapter(command)
                da.Fill(dt)

                Dim rowCount As Integer = dt.Rows.Count - 1

                For i = 0 To rowCount Step 1
                    If PremierPassage = False Then
                        PremierPassage = True
                        PharmacoDynamique = dt.Rows(i)("TEXTEPH")
                    Else
                        PharmacoDynamique += vbCrLf & dt.Rows(i)("TEXTEPH")
                    End If
                Next
            Catch ex As Exception
                Throw ex
            Finally
                con.Close()
            End Try
        End Using

        Return PharmacoDynamique
    End Function

End Class
