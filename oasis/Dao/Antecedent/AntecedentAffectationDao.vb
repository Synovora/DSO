Imports System.Data.SqlClient
Imports Oasis_Common

Public Class AntecedentAffectationDao
    Inherits StandardDao


    Friend Sub AntecedentModificationNiveau(antecedentId As Long, antecedentIdPere As Long, niveauActuel As Integer, niveauCible As Integer, antecedentId1 As Long, antecedentId2 As Long, ordre1 As Integer, ordre2 As Integer, ordre3 As Integer, SelectedPatient As Patient, Cacher As String)
        UpdateAntecedentaAffecter(antecedentId, niveauCible, antecedentId1, antecedentId2, ordre1, ordre2, ordre3)
        AntecedentReorganisationOrdre(antecedentId, niveauCible, SelectedPatient.patientId, Cacher)

        Select Case niveauActuel 'Niveau actuel de l'antécédent à affecter
            Case 1
                Select Case niveauCible  'Nouveau niveau affecté à l'antécédent
                    Case 1 'Antécédent de niveau 1 reste niveau 1 : <-- sur niveau 1, !!! sans objet !!!
                        'Cas sans objet : Un antécédent de niveau 1 n'ayant pas de père ne peut pas réaffecté
                    Case 2 'Antécédent niveau 1 devient niveau 2 : --> sur niveau 1 => devient le fils de l'antécédent précédent de niveau 1
                        '(1) Réaffecter les antécédents liés de niveau 2 en niveau 3
                        AffectationAntecedenetsLies(1, antecedentId, antecedentIdPere, ordre1, SelectedPatient.patientId, Cacher)
                        AntecedentReorganisationOrdre(0, 1, SelectedPatient.patientId, Cacher)
                        AntecedentReorganisationOrdre(antecedentIdPere, 2, SelectedPatient.patientId, Cacher)
                        AntecedentReorganisationOrdre(antecedentId, 3, SelectedPatient.patientId, Cacher)
                        '(5) Occulter les antécédents liés de niveau 3
                        AffectationAntecedenetsLies(5, antecedentId, 0, 0, SelectedPatient.patientId, Cacher)
                    Case 3 'Antécédent niveau 1 devient niveau 3 : !!! Sans objet !!!
                        AntecedentReorganisationOrdre(antecedentIdPere, 3, SelectedPatient.patientId, Cacher)
                        '(4) Occulter les antécédents liés de niveau 2
                        AffectationAntecedenetsLies(4, antecedentId, 0, 0, SelectedPatient.patientId, Cacher)
                        '(5) Occulter les antécédents liés de niveau 3
                        AffectationAntecedenetsLies(5, antecedentId, 0, 0, SelectedPatient.patientId, Cacher)
                End Select
            Case 2
                Select Case niveauCible
                    Case 1 'Antécédent niveau 2 devient niveau 1 (Majeur) : <-- sur niveau 2 => devient majeur
                        '(3) Réaffecter les antécédents liés de niveau 3 en niveau 2
                        AffectationAntecedenetsLies(3, antecedentId, 0, 990, SelectedPatient.patientId, Cacher)
                        AntecedentReorganisationOrdre(antecedentId, niveauCible, SelectedPatient.patientId, Cacher)
                        AntecedentReorganisationOrdre(antecedentId, 2, SelectedPatient.patientId, Cacher)
                    Case 2 'Antécédent niveau 2 devient reste niveau 2, mais change de père : !!! Sans objet !!!
                        '(2) Réaffecter les antécédents liés de niveau 3
                        AffectationAntecedenetsLies(2, antecedentId, antecedentIdPere, ordre1, SelectedPatient.patientId, Cacher)
                        AntecedentReorganisationOrdre(antecedentIdPere, 2, SelectedPatient.patientId, Cacher)
                        AntecedentReorganisationOrdre(antecedentId, 3, SelectedPatient.patientId, Cacher)
                    Case 3 'Antécédent niveau 2 devient niveau 3 : --> sur niveau 2 => devient fils du précédent, si existe précédent de niveau 2
                        '(6) Occulter les antécédents liés de niveau 3
                        AntecedentReorganisationOrdre(antecedentIdPere, 3, SelectedPatient.patientId, Cacher)
                        AffectationAntecedenetsLies(6, antecedentId, 0, 0, SelectedPatient.patientId, Cacher)
                End Select
            Case 3
                Select Case niveauCible
                    Case 1 'Antécédent niveau 3 devient niveau 1 (Majeur) : !!! Sans objet !!!
                        AntecedentReorganisationOrdre(0, 1, SelectedPatient.patientId, Cacher)
                        'AntecedentReorganisationOrdre(AntecedentIdaAffecter, 3)
                    Case 2 'Antécédent niveau 3 devient niveau 2 : <-- sur niveau 3 => récupère le père du précédent niveau 2
                        AntecedentReorganisationOrdre(antecedentIdPere, 2, SelectedPatient.patientId, Cacher)
                        'AntecedentReorganisationOrdre(AntecedentIdaAffecter, 3)
                    Case 3 'Antécédent niveau 3 reste niveau 3, mais change de père
                        AntecedentReorganisationOrdre(antecedentIdPere, 3, SelectedPatient.patientId, Cacher)
                End Select
        End Select
    End Sub

    Friend Function UpdateAntecedentaAffecter(antecedentId As Integer, Niveau As Integer, AntecedentId1 As Integer, AntecedentId2 As Integer, ordre1 As Integer, ordre2 As Integer, ordre3 As Integer) As Boolean
        Dim con As SqlConnection
        con = GetConnection()

        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim CodeRetour As Boolean = True

        Dim dateModification As Date = Date.Now.Date
        Dim SQLstring As String

        SQLstring = "UPDATE oasis.oa_antecedent SET" &
                    " oa_antecedent_niveau = @niveau," &
                    " oa_antecedent_id_niveau1 = @antecedentId1," &
                    " oa_antecedent_id_niveau2 = @antecedentId2," &
                    " oa_antecedent_ordre_affichage1 = @ordre1," &
                    " oa_antecedent_ordre_affichage2 = @ordre2," &
                    " oa_antecedent_ordre_affichage3 = @ordre3" &
                    " WHERE oa_antecedent_id = @antecedentId"

        Dim cmd As New SqlCommand(SQLstring, con)

        With cmd.Parameters
            .AddWithValue("@niveau", Niveau.ToString)
            .AddWithValue("@antecedentId1", AntecedentId1.ToString)
            .AddWithValue("@antecedentId2", AntecedentId2.ToString)
            .AddWithValue("@ordre1", ordre1.ToString)
            .AddWithValue("@ordre2", ordre2.ToString)
            .AddWithValue("@ordre3", ordre3.ToString)
            .AddWithValue("@antecedentId", antecedentId.ToString)
        End With

        Try
            da.UpdateCommand = cmd
            da.UpdateCommand.ExecuteNonQuery()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            CodeRetour = False
        Finally
            con.Close()
        End Try

        Return CodeRetour
    End Function

    'Mise à jour de l'ordre des antécédents en réorganisant l'ordre sur un pas de 20
    Friend Function AntecedentReorganisationOrdre(AntecedentId As Integer, niveau As Integer, SelectedPatientId As Long, Cacher As String) As Boolean
        'Déclaration des données de connexion
        Dim con As SqlConnection
        con = GetConnection()
        Dim CodeRetour As Boolean = False

        Dim antecedentDataAdapter As SqlDataAdapter = New SqlDataAdapter()
        Dim antecedentDataTable As DataTable = New DataTable()
        Dim SQLString As String

        Select Case niveau
            Case 1
                SQLString = "SELECT * FROM oasis.oa_antecedent" &
                            " WHERE oa_antecedent_type = 'A'" &
                            " AND (oa_antecedent_statut_affichage = 'P' OR oa_antecedent_statut_affichage = '" & Cacher & "')" &
                            " AND (oa_antecedent_inactif = '0' or oa_antecedent_inactif is Null)" &
                            " AND oa_antecedent_patient_id = " & SelectedPatientId.ToString &
                            " AND oa_antecedent_niveau = 1" &
                            " ORDER BY oa_antecedent_ordre_affichage1;"
            Case 2
                SQLString = "SELECT * FROM oasis.oa_antecedent" &
                            " WHERE oa_antecedent_type = 'A'" &
                            " AND (oa_antecedent_statut_affichage = 'P' OR oa_antecedent_statut_affichage = '" & Cacher & "')" &
                            " AND (oa_antecedent_inactif = '0' or oa_antecedent_inactif is Null)" &
                            " AND oa_antecedent_patient_id = " & SelectedPatientId.ToString &
                            " AND oa_antecedent_id_niveau1 = " & AntecedentId.ToString &
                            " AND oa_antecedent_niveau = 2" &
                            " ORDER BY oa_antecedent_ordre_affichage2;"
            Case 3
                SQLString = "SELECT * FROM oasis.oa_antecedent" &
                            " WHERE oa_antecedent_type = 'A'" &
                            " AND (oa_antecedent_statut_affichage = 'P' OR oa_antecedent_statut_affichage = '" & Cacher & "')" &
                            " AND (oa_antecedent_inactif = '0' or oa_antecedent_inactif is Null)" &
                            " AND oa_antecedent_patient_id = " & SelectedPatientId.ToString &
                            " AND oa_antecedent_id_niveau2 = " & AntecedentId.ToString &
                            " AND oa_antecedent_niveau = 3" &
                            " ORDER BY oa_antecedent_ordre_affichage3;"
            Case Else
                Return False
        End Select

        'Lecture des données en base
        antecedentDataAdapter.SelectCommand = New SqlCommand(SQLString, con)
        antecedentDataAdapter.Fill(antecedentDataTable)
        'con.Open()
        CodeRetour = True

        'Déclaration des variables pour réaliser le parcours du DataTable pour alimenter le DataGridView
        Dim i As Integer
        Dim rowCount As Integer = antecedentDataTable.Rows.Count - 1
        Dim ordreAffichage As Integer = 0

        'Parcours du DataTable pour alimenter le DataGridView
        For i = 0 To rowCount Step 1
            'Ajout d'une ligne au DataGridView
            ordreAffichage += 20
            Dim AntecedentIdAModifier As Integer = CInt(antecedentDataTable.Rows(i)("oa_antecedent_id"))
            UpdateOrdreAffichageAntecedent(AntecedentIdAModifier, niveau, ordreAffichage)
            AffectationOrdreAntecedenetsLies(AntecedentIdAModifier, niveau, ordreAffichage, SelectedPatientId, Cacher)
        Next

        con.Close()

        Return CodeRetour
    End Function

    Friend Function AffectationOrdreAntecedenetsLies(antecedentIdRef As Integer, niveau As Integer, OrdreAffichageRef As Integer, selectedPatientId As Long, Cacher As String) As Boolean
        'Déclaration des données de connexion
        Dim con As SqlConnection
        con = GetConnection()

        Dim CodeRetour As Boolean = False

        Dim antecedentDataAdapter As SqlDataAdapter = New SqlDataAdapter()
        Dim antecedentDataTable As DataTable = New DataTable()
        Dim SQLString As String

        Select Case niveau
            Case 1
                SQLString = "SELECT * FROM oasis.oa_antecedent" &
                            " WHERE oa_antecedent_type = 'A'" &
                            " AND (oa_antecedent_statut_affichage = 'P' OR oa_antecedent_statut_affichage = '" & Cacher & "')" &
                            " AND (oa_antecedent_inactif = '0' or oa_antecedent_inactif is Null)" &
                            " AND oa_antecedent_patient_id = " & selectedPatientId.ToString &
                            " AND (oa_antecedent_niveau = 2 Or oa_antecedent_niveau = 3)" &
                            " AND oa_antecedent_id_niveau1 = " & antecedentIdRef.ToString + ";"
            Case 2
                SQLString = "SELECT * FROM oasis.oa_antecedent" &
                            " WHERE oa_antecedent_type = 'A'" &
                            " AND (oa_antecedent_statut_affichage = 'P' OR oa_antecedent_statut_affichage = '" & Cacher & "')" &
                            " AND (oa_antecedent_inactif = '0' or oa_antecedent_inactif is Null)" &
                            " AND oa_antecedent_patient_id = " & selectedPatientId.ToString &
                            " AND oa_antecedent_niveau = 3" &
                            " AND oa_antecedent_id_niveau2 = " & antecedentIdRef.ToString + ";"
            Case Else
                Return False
        End Select

        'Lecture des données en base
        antecedentDataAdapter.SelectCommand = New SqlCommand(SQLString, con)
        antecedentDataAdapter.Fill(antecedentDataTable)
        CodeRetour = True
        'conxn3.Open()


        'Déclaration des variables pour réaliser le parcours du DataTable pour alimenter le DataGridView
        Dim i As Integer
        Dim rowCount As Integer = antecedentDataTable.Rows.Count - 1
        'Parcours du DataTable pour mettre à jour l'ordre d'affichage
        For i = 0 To rowCount Step 1
            Dim AntecedentIdaTraiter As Integer = CInt(antecedentDataTable.Rows(i)("oa_antecedent_id"))
            UpdateOrdreAffichageAntecedent(AntecedentIdaTraiter, niveau, OrdreAffichageRef)
        Next

        con.Close()

        Return CodeRetour
    End Function

    Private Function UpdateOrdreAffichageAntecedent(antecedentId As Integer, niveau As Integer, OrdreAffichage As Integer) As Boolean
        'Dim conxn2 As New SqlConnection(getConnectionString())

        Dim con As SqlConnection
        con = GetConnection()

        Dim CodeRetour As Boolean = False
        Dim da As SqlDataAdapter = New SqlDataAdapter()

        Dim dateModification As Date = Date.Now.Date
        Dim SQLstring As String

        Select Case niveau
            Case 1
                SQLstring = "UPDATE oasis.oa_antecedent" &
                            " SET oa_antecedent_ordre_affichage1 = @ordreAffichage" &
                            " WHERE oa_antecedent_id = @antecedentId"
            Case 2
                SQLstring = "UPDATE oasis.oa_antecedent" &
                            " SET oa_antecedent_ordre_affichage2 = @ordreAffichage" &
                            " WHERE oa_antecedent_id = @antecedentId"
            Case 3
                SQLstring = "UPDATE oasis.oa_antecedent" &
                            " SET oa_antecedent_ordre_affichage3 = @ordreAffichage" &
                            " WHERE oa_antecedent_id = @antecedentId"
            Case Else
                Return False
        End Select

        Dim cmd As New SqlCommand(SQLstring, con)

        With cmd.Parameters
            .AddWithValue("@ordreAffichage", OrdreAffichage.ToString)
            .AddWithValue("@antecedentId", antecedentId.ToString)
        End With

        Try
            'conxn2.Open()
            da.UpdateCommand = cmd
            da.UpdateCommand.ExecuteNonQuery()
        Catch ex As Exception
            'PgbMiseAJour.Hide()
            MessageBox.Show(ex.Message)
            CodeRetour = False
        Finally
            con.Close()
        End Try

        Return CodeRetour
    End Function

    Friend Function AffectationAntecedenetsLies(Traitement As Integer, antecedentIdaAffecter As Integer, antecedentIdCible As Integer, Ordre1 As Integer, selectedPatientId As Long, Cacher As String) As Boolean
        'Déclaration des données de connexion
        Dim con As SqlConnection
        con = GetConnection()

        Dim CodeRetour As Boolean = False

        'Dim conxn3 As New SqlConnection(getConnectionString())
        Dim antecedentDataAdapter As SqlDataAdapter = New SqlDataAdapter()
        Dim antecedentDataTable As DataTable = New DataTable()
        Dim SQLString As String

        Select Case Traitement
            Case 1, 4
                SQLString = "SELECT * FROM oasis.oa_antecedent" &
                            " WHERE oa_antecedent_type = 'A'" &
                            " AND (oa_antecedent_statut_affichage = 'P' OR oa_antecedent_statut_affichage = '" & Cacher & "')" &
                            " AND (oa_antecedent_inactif = '0' OR oa_antecedent_inactif is Null)" &
                            " AND oa_antecedent_patient_id = " & selectedPatientId.ToString &
                            " AND oa_antecedent_niveau = 2" &
                            " AND oa_antecedent_id_niveau1 = " & antecedentIdaAffecter.ToString & ";"
            Case 2, 3, 6
                SQLString = "SELECT * FROM oasis.oa_antecedent" &
                            " WHERE oa_antecedent_type = 'A'" &
                            " AND (oa_antecedent_statut_affichage = 'P' OR oa_antecedent_statut_affichage = '" & Cacher & "')" &
                            " AND (oa_antecedent_inactif = '0' OR oa_antecedent_inactif is Null)" &
                            " AND oa_antecedent_patient_id = " & selectedPatientId.ToString &
                            " AND oa_antecedent_niveau = 3" &
                            " AND oa_antecedent_id_niveau2 = " & antecedentIdaAffecter.ToString & ";"
            Case 5
                SQLString = "SELECT * FROM oasis.oa_antecedent" &
                            " WHERE oa_antecedent_type = 'A'" &
                            " AND (oa_antecedent_statut_affichage = 'P' OR oa_antecedent_statut_affichage = '" & Cacher & "')" &
                            " AND (oa_antecedent_inactif = '0' OR oa_antecedent_inactif is Null)" &
                            " AND oa_antecedent_patient_id = " & selectedPatientId.ToString &
                            " AND oa_antecedent_niveau = 3" &
                            " AND oa_antecedent_id_niveau1 = " & antecedentIdaAffecter.ToString & ";"
            Case Else
                Return False
        End Select

        'Lecture des données en base
        antecedentDataAdapter.SelectCommand = New SqlCommand(SQLString, con)
        antecedentDataAdapter.Fill(antecedentDataTable)
        'conxn3.Open()


        'Déclaration des variables pour réaliser le parcours du DataTable pour alimenter le DataGridView
        Dim i As Integer
        Dim rowCount As Integer = antecedentDataTable.Rows.Count - 1
        'Parcours du DataTable pour mettre à jour l'ordre d'affichage
        For i = 0 To rowCount Step 1
            Dim AntecedentIdaTraiter As Integer = CInt(antecedentDataTable.Rows(i)("oa_antecedent_id"))
            Select Case Traitement
                Case 1, 2
                    UpdateAntecedentaAffecter(AntecedentIdaTraiter, 3, antecedentIdCible, antecedentIdaAffecter, Ordre1, 990, 990)
                Case 3
                    UpdateAntecedentaAffecter(AntecedentIdaTraiter, 2, antecedentIdaAffecter, 0, 990, 990, 0)
                Case 4, 5, 6
                    UpdateOccultationAntecedent(AntecedentIdaTraiter)
            End Select
        Next

        con.Close()

        Return CodeRetour
    End Function

    Private Function UpdateOccultationAntecedent(antecedentId As Integer) As Boolean
        'Dim conxn2 As New SqlConnection(getConnectionString())
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim con As SqlConnection
        con = GetConnection()

        Dim CodeRetour As Boolean = True

        Dim dateModification As Date = Date.Now.Date
        Dim SQLstring As String

        SQLstring = "UPDATE oasis.oa_antecedent SET" &
                    " oa_antecedent_niveau = 1," &
                    " oa_antecedent_id_niveau1 = 0," &
                    " oa_antecedent_id_niveau2 = 0," &
                    " oa_antecedent_ordre_affichage1 = 0," &
                    " oa_antecedent_ordre_affichage2 = 0," &
                    " oa_antecedent_ordre_affichage3 = 0," &
                    " oa_antecedent_statut_affichage = 'O'" &
                    " WHERE oa_antecedent_id = @antecedentId"

        Dim cmd As New SqlCommand(SQLstring, con)

        With cmd.Parameters
            .AddWithValue("@antecedentId", antecedentId.ToString)
        End With

        Try
            'conxn2.Open()
            da.UpdateCommand = cmd
            da.UpdateCommand.ExecuteNonQuery()
        Catch ex As Exception
            'PgbMiseAJour.Hide()
            MessageBox.Show(ex.Message)
            CodeRetour = False
        Finally
            con.Close()
        End Try

        Return CodeRetour
    End Function

End Class
