Imports System.Data.SqlClient
Imports Oasis_Common
Public Class PpsDao
    Inherits StandardDao

    Public Enum EnumCategoriePPS
        OBJECTIF_SANTE = 1
        MESURE_PREVENTIVE = 2
        SUIVI_INTERVENANT = 3
        STRATEGIE = 4
    End Enum


    Public Function getAllPPSbyPatient(patientId As Integer) As DataTable
        Dim SQLString As String

        SQLString = "Select oa_r_pps_categorie_id, oa_r_pps_sous_categorie_id, oa_r_pps_sous_categorie_type," &
        " oa_pps_id, oa_pps_drc_id, oa_pps_commentaire, oa_pps_drc_id, oa_pps_date_debut, oa_pps_date_creation, oa_pps_date_modification," &
        " oa_pps_arret," &
        " oa_parcours_id, oa_parcours_specialite, oa_parcours_ror_id, oa_parcours_base, oa_parcours_rythme, oa_parcours_commentaire, oa_parcours_date_creation," &
        " oa_parcours_date_modification, oa_parcours_cacher" &
        " From oasis.oasis.oa_r_pps_sous_categorie" &
        " Left outer join oasis.oasis.oa_patient_pps On oa_r_pps_categorie_id = oa_pps_categorie And oa_r_pps_sous_categorie_id = oa_pps_sous_categorie" &
        " Left outer join oasis.oasis.oa_patient_parcours on oa_r_pps_categorie_id = oa_parcours_categorie_id And oa_r_pps_sous_categorie_id = oa_parcours_sous_categorie_id" &
        " Where((oa_pps_inactif = 0 Or oa_pps_inactif Is NULL) And oa_pps_patient_id = " & patientId.ToString & ") Or" &
        " ((oa_parcours_inactif = 0 Or oa_parcours_inactif Is NULL) And oa_parcours_patient_id = " & patientId.ToString & ")" &
        " order by oa_r_pps_sous_categorie_ordre_affichage, oa_pps_priorite"

        Using con As SqlConnection = GetConnection()
            Dim PPSDataAdapter As SqlDataAdapter = New SqlDataAdapter()
            Using PPSDataAdapter
                PPSDataAdapter.SelectCommand = New SqlCommand(SQLString, con)
                Dim PPSDataTable As DataTable = New DataTable()
                Using PPSDataTable
                    Try
                        PPSDataAdapter.Fill(PPSDataTable)
                        Dim command As SqlCommand = con.CreateCommand()
                    Catch ex As Exception
                        Throw ex
                    End Try
                    Return PPSDataTable
                End Using
            End Using
        End Using
    End Function

    Friend Function getPpsById(ppsId As Integer) As Pps
        Dim pps As Pps
        Dim con As SqlConnection

        con = GetConnection()

        Try
            Dim command As SqlCommand = con.CreateCommand()

            command.CommandText =
                "select * from oasis.oa_patient_pps where oa_pps_id = @id"
            command.Parameters.AddWithValue("@id", ppsId)
            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    pps = buildBean(reader)
                Else
                    Throw New ArgumentException("PPS inexistant !")
                End If
            End Using

        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try

        Return pps
    End Function

    Friend Function getPpsObjectifByPatientId(patientId As Integer) As Pps
        Dim pps As Pps
        Dim con As SqlConnection

        con = GetConnection()

        Try
            Dim command As SqlCommand = con.CreateCommand()

            command.CommandText =
                "select * from oasis.oa_patient_pps where oa_pps_categorie = 1 and oa_pps_sous_categorie = 1 and oa_pps_patient_id = @id"

            command.Parameters.AddWithValue("@id", patientId)
            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    pps = buildBean(reader)
                Else
                    Throw New ArgumentException("PPS inexistant !")
                End If
            End Using

        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try

        Return pps
    End Function

    Friend Function ExistPPSObjectifByPatientId(patientId As Integer) As Boolean
        Dim con As SqlConnection
        con = GetConnection()

        Try
            Dim command As SqlCommand = con.CreateCommand()

            command.CommandText =
                "SELECT oa_pps_id FROM oasis.oasis.oa_patient_pps" &
                " WHERE oa_pps_patient_id = @id and oa_pps_categorie = 1 and oa_pps_sous_categorie = 1 and (oa_pps_inactif is NULL or oa_pps_inactif = 0)"

            command.Parameters.AddWithValue("@id", patientId)
            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    Return True
                End If
            End Using

        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try

        Return False
    End Function

    Private Function buildBean(reader As SqlDataReader) As Pps
        Dim pps As New Pps
        pps.Id = reader("oa_pps_id")
        pps.PatientId = Coalesce(reader("oa_pps_patient_id"), 0)
        pps.CategorieId = Coalesce(reader("oa_pps_categorie"), 0)
        pps.SousCategorieId = Coalesce(reader("oa_pps_sous_categorie"), 0)
        pps.Priorite = Coalesce(reader("oa_pps_priorite"), 0)
        pps.DrcId = Coalesce(reader("oa_pps_drc_id"), 0)
        pps.AffichageSynthese = Coalesce(reader("oa_pps_affichage_synthese"), False)
        pps.Commentaire = Coalesce(reader("oa_pps_commentaire"), "")
        pps.DateDebut = Coalesce(reader("oa_pps_date_debut"), Nothing)
        pps.Arret = Coalesce(reader("oa_pps_arret"), False)
        pps.ArretCommentaire = Coalesce(reader("oa_pps_commentaire_arret"), "")
        pps.UserCreation = Coalesce(reader("oa_pps_utilisateur_creation"), 0)
        pps.DateCreation = Coalesce(reader("oa_pps_date_creation"), Nothing)
        pps.UserModification = Coalesce(reader("oa_pps_utilisateur_modification"), 0)
        pps.DateModification = Coalesce(reader("oa_pps_date_modification"), Nothing)
        Return pps
    End Function

    Public Function getAllPPSStrategiePatient(patientId As Integer) As DataTable
        Dim SQLString As String

        SQLString = "select * from oasis.oa_patient_pps where (oa_pps_inactif is Null or oa_pps_inactif = 0) and" &
        " oa_pps_categorie = 4 And oa_pps_patient_id = " & patientId.ToString & " order by oa_pps_priorite;"

        Using con As SqlConnection = GetConnection()
            Dim PPSDataAdapter As SqlDataAdapter = New SqlDataAdapter()
            Using PPSDataAdapter
                PPSDataAdapter.SelectCommand = New SqlCommand(SQLString, con)
                Dim PPSDataTable As DataTable = New DataTable()
                Using PPSDataTable
                    Try
                        PPSDataAdapter.Fill(PPSDataTable)
                        Dim command As SqlCommand = con.CreateCommand()
                    Catch ex As Exception
                        Throw ex
                    End Try
                    Return PPSDataTable
                End Using
            End Using
        End Using
    End Function

    Public Function getAllPPSSuivibyPatient(patientId As Integer) As DataTable
        Dim SQLString As String

        SQLString = "select * from oasis.oa_patient_pps where (oa_pps_inactif is Null or oa_pps_inactif = 0)" &
            " And (oa_pps_affichage_synthese Is Null Or oa_pps_affichage_synthese = 1)" &
            " And oa_pps_categorie = 2 And oa_pps_sous_categorie <> 2" &
            " And oa_pps_patient_id = " & patientId.ToString & " order by oa_pps_sous_categorie"

        Using con As SqlConnection = GetConnection()
            Dim PPSDataAdapter As SqlDataAdapter = New SqlDataAdapter()
            Using PPSDataAdapter
                PPSDataAdapter.SelectCommand = New SqlCommand(SQLString, con)
                Dim PPSDataTable As DataTable = New DataTable()
                Using PPSDataTable
                    Try
                        PPSDataAdapter.Fill(PPSDataTable)
                        Dim command As SqlCommand = con.CreateCommand()
                    Catch ex As Exception
                        Throw ex
                    End Try
                    Return PPSDataTable
                End Using
            End Using
        End Using
    End Function

    Public Function getAllPPSPreventionbyPatient(patientId As Integer) As DataTable
        Dim SQLString As String

        SQLString = "select * from oasis.oa_patient_pps where (oa_pps_inactif is Null or oa_pps_inactif = 0)" &
        " and oa_pps_categorie = 2 and oa_pps_sous_categorie = 2 and oa_pps_patient_id = " & patientId.ToString &
        " order by oa_pps_priorite"

        Using con As SqlConnection = GetConnection()
            Dim PPSDataAdapter As SqlDataAdapter = New SqlDataAdapter()
            Using PPSDataAdapter
                PPSDataAdapter.SelectCommand = New SqlCommand(SQLString, con)
                Dim PPSDataTable As DataTable = New DataTable()
                Using PPSDataTable
                    Try
                        PPSDataAdapter.Fill(PPSDataTable)
                        Dim command As SqlCommand = con.CreateCommand()
                    Catch ex As Exception
                        Throw ex
                    End Try
                    Return PPSDataTable
                End Using
            End Using
        End Using
    End Function

End Class
