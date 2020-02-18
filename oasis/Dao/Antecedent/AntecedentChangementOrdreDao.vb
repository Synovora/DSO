Imports System.Data.SqlClient
Imports Oasis_WF
Imports Oasis_Common
Public Class AntecedentChangementOrdreDao
    Inherits StandardDao

    'Mise à jour de l'ordre des antécédents en réorganisant l'ordre sur un pas de 20
    Friend Function AntecedentReorganisationOrdre(niveau As Integer, selectedPatientId As Long, AntecedentIdPere As Long, NiveauAntecedentAOrdonner As Integer) As Boolean
        'Déclaration des données de connexion
        Dim con As SqlConnection = GetConnection()

        Dim antecedentDataAdapter As SqlDataAdapter = New SqlDataAdapter()
        Dim antecedentDataTable As DataTable = New DataTable()
        Dim SQLString As String
        Dim CodeRetour As Boolean = True

        Select Case niveau
            Case 1
                SQLString = "select * from oasis.oa_antecedent where oa_antecedent_type = 'A'" &
                            " And oa_antecedent_statut_affichage = 'P'" &
                            " And (oa_antecedent_inactif = '0' or oa_antecedent_inactif is Null)" &
                            " And oa_antecedent_patient_id = " & selectedPatientId.ToString &
                            " And oa_antecedent_niveau = 1 order by oa_antecedent_ordre_affichage1;"
            Case 2
                SQLString = "select * from oasis.oa_antecedent where oa_antecedent_type = 'A'" &
                            " And oa_antecedent_statut_affichage = 'P'" &
                            " And (oa_antecedent_inactif = '0' or oa_antecedent_inactif is Null)" &
                            " And oa_antecedent_patient_id = " & selectedPatientId.ToString &
                            " And oa_antecedent_id_niveau1 = " & AntecedentIdPere.ToString &
                            " And oa_antecedent_niveau = 2 order by oa_antecedent_ordre_affichage2;"
            Case 3
                SQLString = "select * from oasis.oa_antecedent where oa_antecedent_type = 'A'" &
                            " And oa_antecedent_statut_affichage = 'P'" &
                            " And (oa_antecedent_inactif = '0' or oa_antecedent_inactif is Null)" &
                            " And oa_antecedent_patient_id = " & selectedPatientId.ToString &
                            " And oa_antecedent_id_niveau2 = " & AntecedentIdPere.ToString &
                            " And oa_antecedent_niveau = 3 order by oa_antecedent_ordre_affichage3;"
            Case Else
                SQLString = ""
                CodeRetour = False
        End Select

        'Lecture des données en base
        antecedentDataAdapter.SelectCommand = New SqlCommand(SQLString, con)
        antecedentDataAdapter.Fill(antecedentDataTable)
        'conxn.Open()

        'Déclaration des variables pour réaliser le parcours du DataTable pour alimenter le DataGridView
        Dim i As Integer
        Dim rowCount As Integer = antecedentDataTable.Rows.Count - 1
        Dim ordreAffichage As Integer = 0

        'Parcours du DataTable pour alimenter le DataGridView
        For i = 0 To rowCount Step 1
            'Ajout d'une ligne au DataGridView
            ordreAffichage += 20
            Dim AntecedentId As Integer = CInt(antecedentDataTable.Rows(i)("oa_antecedent_id"))
            UpdateAntecedent(AntecedentId, ordreAffichage, NiveauAntecedentAOrdonner)
            AffectationOrdreAntecedenetsLies(AntecedentId, niveau, ordreAffichage, selectedPatientId, NiveauAntecedentAOrdonner)
        Next

        con.Close()

        Return CodeRetour
    End Function

    'Affectation aux antécédents fils, de l'ordre d'affichage attribué à l'antécédent père
    Friend Function AffectationOrdreAntecedenetsLies(antecedentIdRef As Integer, niveau As Integer, OrdreAffichageRef As Integer, selectedPatientId As Long, NiveauAntecedentAOrdonner As Integer)
        'Déclaration des données de connexion
        Dim con3 As SqlConnection = GetConnection()
        Dim antecedentDataAdapter As SqlDataAdapter = New SqlDataAdapter()
        Dim antecedentDataTable As DataTable = New DataTable()
        Dim SQLString As String

        Select Case niveau
            Case 1
                SQLString = "select * from oasis.oa_antecedent where oa_antecedent_type = 'A'" &
                            " And oa_antecedent_statut_affichage = 'P'" &
                            " And (oa_antecedent_inactif = '0' or oa_antecedent_inactif is Null)" &
                            " And oa_antecedent_patient_id = " & selectedPatientId.ToString &
                            " And (oa_antecedent_niveau = 2 Or oa_antecedent_niveau = 3)" &
                            " And oa_antecedent_id_niveau1 = " + antecedentIdRef.ToString + ";"
            Case 2
                SQLString = "select * from oasis.oa_antecedent where oa_antecedent_type = 'A'" &
                            " And oa_antecedent_statut_affichage = 'P'" &
                            " And (oa_antecedent_inactif = '0' or oa_antecedent_inactif is Null)" &
                            " And oa_antecedent_patient_id = " & selectedPatientId.ToString &
                            " And oa_antecedent_niveau = 3" &
                            " And oa_antecedent_id_niveau2 = " & antecedentIdRef.ToString + ";"
            Case Else
                Return False
        End Select

        'Lecture des données en base
        antecedentDataAdapter.SelectCommand = New SqlCommand(SQLString, con3)
        antecedentDataAdapter.Fill(antecedentDataTable)

        'Déclaration des variables pour réaliser le parcours du DataTable pour alimenter le DataGridView
        Dim i As Integer
        Dim rowCount As Integer = antecedentDataTable.Rows.Count - 1
        'Parcours du DataTable pour mettre à jour l'ordre d'affichage
        For i = 0 To rowCount Step 1
            Dim AntecedentIdaTraiter As Integer = CInt(antecedentDataTable.Rows(i)("oa_antecedent_id"))
            UpdateAntecedent(AntecedentIdaTraiter, OrdreAffichageRef, NiveauAntecedentAOrdonner)
        Next

        con3.Close()

    End Function

    Friend Function UpdateAntecedent(antecedentId As Integer, ordreAffichage As Integer, NiveauAntecedentAOrdonner As Integer) As Boolean
        Dim con2 As SqlConnection = GetConnection()
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim CodeRetour As Boolean = True

        Dim dateModification As Date = Date.Now.Date
        Dim SQLstring As String

        Select Case NiveauAntecedentAOrdonner
            Case 1
                SQLstring = "update oasis.oa_antecedent set oa_antecedent_ordre_affichage1 = @ordreAffichage where oa_antecedent_id = @antecedentId"
            Case 2
                SQLstring = "update oasis.oa_antecedent set oa_antecedent_ordre_affichage2 = @ordreAffichage where oa_antecedent_id = @antecedentId"
            Case 3
                SQLstring = "update oasis.oa_antecedent set oa_antecedent_ordre_affichage3 = @ordreAffichage where oa_antecedent_id = @antecedentId"
            Case Else
                Return False
        End Select

        Dim cmd As New SqlCommand(SQLstring, con2)

        With cmd.Parameters
            .AddWithValue("@ordreAffichage", ordreAffichage.ToString)
            .AddWithValue("@antecedentId", antecedentId.ToString)
        End With

        Try
            'con2.Open()
            da.UpdateCommand = cmd
            da.UpdateCommand.ExecuteNonQuery()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            CodeRetour = False
        Finally
            con2.Close()
        End Try

        Return CodeRetour
    End Function

End Class
