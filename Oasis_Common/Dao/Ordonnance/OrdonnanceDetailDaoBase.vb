Imports System.Data.SqlClient

Public Class OrdonnanceDetailDaoBase
    Inherits StandardDao

    Public Structure EnumDelivrance
        Const A_DELIVRER = "A délivrer"
        Const NE_PAS_DELIVRER = "Ne pas délivrer"
    End Structure

    Public Function getOrdonnanceLigneById(OrdonnanceLigneId As Long) As OrdonnanceDetailBase
        Dim ordonnanceDetail As OrdonnanceDetailBase
        Dim con As SqlConnection
        con = GetConnection()

        Try
            Dim command As SqlCommand = con.CreateCommand()

            command.CommandText =
                "SELECT * FROM oasis.oa_patient_ordonnance_detail WHERE oa_ordonnance_ligne_id = @ordonnanceLigneId"
            command.Parameters.AddWithValue("@ordonnanceLigneId", OrdonnanceLigneId)
            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    ordonnanceDetail = BuildBean(reader)
                Else
                    Throw New ArgumentException("Ordonnance inexistante !")
                End If
            End Using

        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try

        Return ordonnanceDetail
    End Function

    Private Function BuildBean(reader As SqlDataReader) As OrdonnanceDetailBase
        Dim ordonnanceDetail As New OrdonnanceDetailBase With {
            .LigneId = reader("oa_ordonnance_ligne_id"),
            .Traitement = Coalesce(reader("oa_ordonnance_traitement"), False),
            .TraitementId = Coalesce(reader("oa_traitement_id"), 0),
            .OrdreAffichage = Coalesce(reader("oa_traitement_ordre_affichage"), 0),
            .Ald = Coalesce(reader("oa_traitement_ald"), False),
            .ADelivrer = Coalesce(reader("oa_traitement_a_delivrer"), False),
            .MedicamentCis = Coalesce(reader("oa_traitement_medicament_cis"), 0),
            .MedicamentDci = Coalesce(reader("oa_traitement_medicament_dci"), ""),
            .DateDebut = Coalesce(reader("oa_traitement_date_debut"), Nothing),
            .DateFin = Coalesce(reader("oa_traitement_date_fin"), Nothing),
            .Duree = Coalesce(reader("oa_traitement_duree"), 0),
            .Posologie = Coalesce(reader("oa_traitement_posologie"), ""),
            .PosologieBase = Coalesce(reader("oa_traitement_posologie_base"), ""),
            .PosologieRythme = Coalesce(reader("oa_traitement_posologie_rythme"), 0),
            .PosologieMatin = Coalesce(reader("oa_traitement_posologie_matin"), 0),
            .PosologieMidi = Coalesce(reader("oa_traitement_posologie_midi"), 0),
            .PosologieApresMidi = Coalesce(reader("oa_traitement_posologie_apres_midi"), 0),
            .PosologieSoir = Coalesce(reader("oa_traitement_posologie_soir"), 0),
            .FractionMatin = Coalesce(reader("oa_traitement_fraction_matin"), ""),
            .FractionMidi = Coalesce(reader("oa_traitement_fraction_midi"), ""),
            .FractionApresMidi = Coalesce(reader("oa_traitement_fraction_apres_midi"), ""),
            .FractionSoir = Coalesce(reader("oa_traitement_fraction_soir"), ""),
            .PosologieCommentaire = Coalesce(reader("oa_traitement_posologie_commentaire"), ""),
            .Commentaire = Coalesce(reader("oa_traitement_commentaire"), ""),
            .Fenetre = Coalesce(reader("oa_traitement_fenetre"), False),
            .FenetreDateDebut = Coalesce(reader("oa_traitement_fenetre_date_debut"), Nothing),
            .FenetreDateFin = Coalesce(reader("oa_traitement_fenetre_date_fin"), Nothing),
            .Inactif = Coalesce(reader("oa_traitement_inactif"), False)
        }

        Return ordonnanceDetail
    End Function

    Public Function GetOrdonnanceLigneByOrdonnanceId(ordonnanceId As Integer) As List(Of OrdonnanceDetailBase)
        Dim ordonnanceDetail As New List(Of OrdonnanceDetailBase)
        Dim con As SqlConnection = GetConnection()

        Try
            Dim command As SqlCommand = con.CreateCommand()
            command.CommandText =
                "SELECT * FROM oasis.oa_patient_ordonnance_detail WHERE oa_ordonnance_id = @ordonnanceId ORDER BY oa_traitement_ordre_affichage, oa_ordonnance_ligne_id"
            command.Parameters.AddWithValue("@ordonnanceId", ordonnanceId)
            Using reader As SqlDataReader = command.ExecuteReader()
                While (reader.Read())
                    ordonnanceDetail.Add(BuildBean(reader))
                End While
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try

        Return ordonnanceDetail
    End Function

    Public Function getAllOrdonnanceLigneByOrdonnanceId(ordonnanceId As Integer) As DataTable
        Dim SQLString As String = "SELECT * FROM oasis.oa_patient_ordonnance_detail WHERE oa_ordonnance_id = " & ordonnanceId.ToString &
                    " ORDER BY oa_traitement_ordre_affichage, oa_ordonnance_ligne_id"

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

    Public Function getAllOrdonnanceLigneSelectAldByOrdonnanceId(ordonnanceId As Integer, traitementAld As Boolean) As DataTable
        Dim SQLString As String = "SELECT * FROM oasis.oa_patient_ordonnance_detail" &
                    " WHERE oa_ordonnance_id = " & ordonnanceId.ToString &
                    " AND oa_traitement_ald = '" & traitementAld & "'" &
                    " ORDER BY oa_traitement_ordre_affichage, oa_ordonnance_ligne_id"

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
End Class
